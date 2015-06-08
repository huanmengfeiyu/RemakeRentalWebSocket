using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperWebSocket;
using RentalModel;
using RentalDal;
using RentalUtity;
using System.Collections.Concurrent;

namespace RentalWebSocket
{
    public class RentalServer : WebSocketServer<RentalSession>
    {
        Random random = new Random();
        public static ConcurrentQueue<OperateModel> operateList = new ConcurrentQueue<OperateModel>();
        public static ConcurrentQueue<SocketMessageModel> replyList = new ConcurrentQueue<SocketMessageModel>();
        public static List<OperateModel> processedList = new List<OperateModel>();
        public TdrPlatform platform;
        public RentalServer()
        {
            platform = new TdrPlatform(GlobalConfig.appId, GlobalConfig.appKey, GlobalConfig.serverPlatIP, GlobalConfig.serverPlatPort, GlobalConfig.minDeviceId, GlobalConfig.maxDeviceId);
            Task task = new Task(ReplyClients);//回复客户端
            Task task1 = new Task(BangDevices);//绑定设备下发
            Task task2 = new Task(BangYYKs);//绑定有源卡
            //task.Start();
            //task1.Start();
            //task2.Start();
        }

        private void ReplyClients()
        {
            while (true)
            {
                try
                {
                    if (replyList.Count > 0)
                    {

                    }
                    if (operateList.Count>0)
                    {
                        OperateModel operate = new OperateModel();
                        operateList.TryDequeue(out operate);
                        processedList.Add(operate);
                        platform.SendOperateCommand(operate);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(300);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());                    
                }
            }
        }
        public void BangDevices()
        {
            while (true)
            {
                try
                {
                    List<DeviceInfo> stationList = new List<DeviceInfo>();
                    stationList = BindingDeviceDal.GetV2Devices();
                    if (stationList.Count > 0)
                    {
                        foreach (var item in stationList)
                        {
                            List<byte> bytelist = new List<byte>();
                            bytelist.AddRange(ConvertHelpers.intToBytes2(Convert.ToUInt32(item.ROOMNO)));
                            bytelist.AddRange(ConvertHelpers.IntToByteTwoByHignFirst(item.DeviceType));
                            bytelist.AddRange(ConvertHelpers.intToBytes2(item.DeviceCode));
                            OperateModel operate = new OperateModel();
                            //operate.comid=;
                            operate.commandID = 0x0201;
                            operate.deviceId = item.HostID;
                            operate.sessionId = "-1";//绑定下发
                            operate.Sn = Convert.ToUInt16(random.Next(1, 100));
                            operate.Data = bytelist.ToArray();
                            operateList.Enqueue(operate);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                }
                System.Threading.Thread.Sleep(1800000);
             }
        }
        public void BangYYKs()
        {
            while (true)
            {
                try
                {
                    List<YYKInfo> yykList = new List<YYKInfo>();
                    yykList = BindingDeviceDal.GetV2YYKs();
                    foreach (var item in yykList)
                    {
                        List<byte> bytelist = new List<byte>();
                        bytelist.AddRange(ConvertHelpers.intToBytes2(Convert.ToUInt32(item.roomno)));
                        bytelist.AddRange(ConvertHelpers.intToBytes2(item.yykID));
                        bytelist.AddRange(ConvertHelpers.intToBytes2(Convert.ToUInt64(item.IDENTITYCARDID)));
                        OperateModel operate = new OperateModel();
                        //operate.comid=;
                        operate.commandID = 0x0201;
                        operate.deviceId = item.HostID;
                        operate.sessionId = "-2";//绑定下发
                        operate.Sn = Convert.ToUInt16(random.Next(1, 100));
                        operate.Data = bytelist.ToArray();
                        operateList.Enqueue(operate);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                }
                System.Threading.Thread.Sleep(1800000);
            }
        }
    }
}
