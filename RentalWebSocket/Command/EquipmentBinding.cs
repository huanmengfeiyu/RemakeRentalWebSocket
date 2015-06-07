using SuperWebSocket.SubProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalWebSocket
{
    public class EquipmentBinding : JsonSubCommand<RentalSession, RentalSocketList>
    {
        //public override string Name
        //{
        //    get
        //    {
        //        return "010106";
        //    }
        //}

        protected override void ExecuteJsonCommand(RentalSession session, RentalSocketList commandList)
        {
            OperateModel operate = new OperateModel();
            operate.commandID = 0xF003;
            operate.Sn = Convert.ToUInt16(commandList.Key);
            operate.sessionId = session.SessionID;
            List<DeviceInfo> stationList = new List<DeviceInfo>();
            stationList = BindingDeviceDal.GetDevices();
            while (stationList.Count > 0)
            {
                List<DeviceInfo> BangDeviceList = new List<DeviceInfo>();
                //stationList.GroupBy(x => x.StationNo);
                BangDeviceList = stationList.FindAll(x => x.StationNo == stationList[0].StationNo);
                stationList.RemoveAll(x => x.StationNo == stationList[0].StationNo);
                while (BangDeviceList.Count > 0)
                {
                    List<DeviceInfo> deviceList = BangDeviceList.FindAll(x => x.HostID == BangDeviceList[0].HostID);
                    BangDeviceList.RemoveAll(x => x.HostID == BangDeviceList[0].HostID);
                    operate.deviceId = Convert.ToUInt32(deviceList[0].StationNo);
                    List<byte> bytelist = new List<byte>();
                    DateTime dt = DateTime.Now;
                    bytelist.Add(Convert.ToByte(dt.Year.ToString().Substring(2, 2)));
                    bytelist.Add(Convert.ToByte(dt.Month));
                    bytelist.Add(Convert.ToByte(dt.Day));
                    bytelist.Add(Convert.ToByte(dt.Hour));
                    bytelist.Add(Convert.ToByte(dt.Minute));
                    bytelist.Add(Convert.ToByte(dt.Second));
                    bytelist.Add(0);//SN
                    bytelist.AddRange(ConvertHelpers.hexStrToByte("0x0101"));
                    bytelist.AddRange(ConvertHelpers.intToBytes2(deviceList[0].HostID));
                    bytelist.AddRange(ConvertHelpers.hexStrToByte("0x06"));
                    for (int i = 0; i < deviceList.Count; i++)
                    {
                        bytelist.AddRange(ConvertHelpers.IntToByteTwoByHignFirst(deviceList[i].DeviceType));
                        bytelist.AddRange(ConvertHelpers.intToBytes2(deviceList[i].DeviceCode));
                    }
                    operate.Data = bytelist.ToArray();
                    RentalServer.oprateModelList.Enqueue(operate);
                }
            }
        }
    }
}
