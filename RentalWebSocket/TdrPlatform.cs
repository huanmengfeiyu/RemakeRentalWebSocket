using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalWebSocket
{
    public class TdrPlatform
    {
        public Tendency.ConnToTDR.TendencyConnection conn;
        private uint _appID = 0;
        private string _appKey = string.Empty;
        private uint _userID = 0;
        private string _token = string.Empty;
        private ushort _version = 1;
        private string _ip = string.Empty;
        private ushort _port = 0;
        private uint minDeviceId = 0;
        private uint maxDeviceId = 0;

        public TdrPlatform(uint appID, string appKey, string platformIP, ushort platformPort, uint minDeviceId, uint maxDeviceId, ushort version = 0)
        {
            this._appID = appID;
            this._appKey = appKey;
            this._ip = platformIP;
            this._port = platformPort;
            this.minDeviceId = minDeviceId;
            this.maxDeviceId = maxDeviceId;

            conn = new Tendency.ConnToTDR.TendencyConnection(_version, _appID, _appKey);
            conn.ConnectComplete += conn_ConnectComplete;
            conn.LostConnection += conn_LostConnection;
            conn.ReplyUserLogin += conn_ReplyUserLogin;

            conn.ReplyRegionDevice += conn_ReplyRegionDevice;
            conn.ReplyRegionDeviceData += conn_ReplyRegionDeviceData;
            conn.ReplySendedCommand += conn_ReplySendedCommand;
            conn.ReplySendedCommandResult += conn_ReplySendedCommandResult;
            conn.ReplySendedCommandStandardResult += conn_ReplySendedCommandStandardResult;
            conn.ReceiveDeviceOnLine += conn_ReceiveDeviceOnLine;
            conn.ReceiveDeviceOffLine += conn_ReceiveDeviceOffLine;
            conn.ReceiveDeviceData += conn_ReceiveDeviceData;
            conn.WriteLog = true;
            conn.ReceiveUnknownData += conn_ReceiveUnknownData;
            PlatForm_Connec();
        }
        private void PlatForm_Connec()
        {
            if (conn.Connect(_ip, _port))
            {
                Log.Warn("正在平台连接");
            }
            else
            {
                Log.Warn("打开平台连接00000");
            }
        }
        /// <summary>
        /// standard result Reply
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void conn_ReplySendedCommandStandardResult(object sender, Tendency.ConnToTDR.TendencyConnection.CDeviceStandardDataReceivedArgs e)
        {
            try
            {
                ProtocolDataList Datas = new ProtocolDataList();
                Datas = JsonConvert.DeserializeObject<ProtocolDataList>(e.CDeviceStandardData.m_Data);
                foreach (var Data in Datas)
                {
                    OperateModel om = new OperateModel();
                    SocketMessageModel smm = new SocketMessageModel();
                    int res;

                    switch (Data.key)
                    {
                        case "F005_DATA_CON"://命令F005数据下发

                            break;
                        case "C0201_ADDDRIVCE_CON"://添加设备下发
                            res = Convert.ToInt32(Data.val);
                            if (res == 1)
                            {
                                //success
                                Log.Info("添加设备下发成功");
                            }
                            else
                            {
                                Log.Info("添加设备下发失败");
                            }
                            break;
                        case "C0202_DELDRIVCE_CON"://删除设备下发
                            res = Convert.ToInt32(Data.val);
                            if (res == 1)
                            {
                                //success
                                Log.Info("删除设备下发成功");
                            }
                            else
                            {
                                Log.Info("删除设备下发失败");
                            }
                            break;
                        case "C0203_ADDCARD_CON"://添加有源卡下发
                            res = Convert.ToInt32(Data.val);
                            if (res == 1)
                            {
                                //success
                                Log.Info("添加有源卡下发成功");
                            }
                            else
                            {
                                Log.Info("添加有源卡下发失败");
                            }
                            break;
                        case "C0204_DELCARD_CON"://删除有源卡下发
                            res = Convert.ToInt32(Data.val);
                            if (res == 1)
                            {
                                //success
                                Log.Info("删除有源卡下发成功");
                            }
                            else
                            {
                                Log.Info("删除有源卡下发失败");
                            }
                            break;
                        case "C0205_CBF_CON"://手动撤布防下发
                            break;
                        default:
                            break;
                    }

                    if (Data.key == "DEVICE_CONTROL_REPLY_CON")
                    {
                        int year = Convert.ToInt32(Data.val.Substring(0, 2), 16);
                        int mouth = Convert.ToInt32(Data.val.Substring(2, 2), 16);
                        int day = Convert.ToInt32(Data.val.Substring(4, 2), 16);
                        int hour = Convert.ToInt32(Data.val.Substring(6, 2), 16);
                        int min = Convert.ToInt32(Data.val.Substring(8, 2), 16);
                        int sern = Convert.ToInt32(Data.val.Substring(10, 2), 16);
                        int deviceType = Convert.ToInt32(Data.val.Substring(14, 4), 16);
                        int deviID = Convert.ToInt32(Data.val.Substring(18, 8), 16);
                        int commID = Convert.ToInt32(Data.val.Substring(26, 2), 16);
                        int result = Convert.ToInt32(Data.val.Substring(28, 2), 16);
                        if (Data.val.Length > 30)
                        {
                            int swithType = Convert.ToInt32(Data.val.Substring(30, 2), 16);
                            Log.Warn("开关状态:" + swithType);
                        }
                        if (deviceType == 0x0101)
                        {
                            if (commID == 0x06)//设备绑定
                            {
                                #region 远程开关继电器
                                om = RentalServer.processedList.Find(x => x.guid == e.CDeviceStandardData.commondGuid);
                                if (result == 1)
                                {
                                    smm.SessionId = om.sessionId;
                                    smm.stationId = om.deviceList[0].StationNo.ToString();
                                    smm.deviceId = om.deviceList[0].HostID.ToString();
                                    smm.commId = om.comid;
                                    smm.DeviceType = om.deviceList[0].DeviceType.ToString();
                                    smm.result = "00001";
                                    Log.Warn("远程开关继电器控制成功");
                                    RentalServer.replyList.Enqueue(smm);
                                }
                                else
                                {
                                    smm.SessionId = om.sessionId;
                                    smm.stationId = om.deviceId.ToString();
                                    smm.deviceId = om.deviceList[0].HostID.ToString();
                                    smm.DeviceType = om.deviceList[0].DeviceType.ToString();
                                    smm.commId = om.comid;
                                    smm.result = "00000";
                                    Log.Warn("远程开关继电器控制失败");
                                    RentalServer.replyList.Enqueue(smm);
                                }
                                #endregion
                                #region 设备绑定
                                //om = RentalServer.ProcessedList.Find(x => x.guid == e.CDeviceStandardData.commondGuid);
                                //if (result == 1)
                                //{
                                //    for (int i = 0; i < om.deviceList.Count; i++)
                                //    {
                                //        if (BindingDeviceDal.BangResult(1, om.deviceList[i].DeviceType, om.deviceList[i].DeviceCode))
                                //        {
                                //            Log.Warn("绑定00001，修改数据库00001");
                                //        }
                                //        else
                                //        {
                                //            Log.Error("绑定下发00001，修改数据库00000");
                                //        }
                                //    }
                                //}
                                //else
                                //{
                                //    for (int i = 0; i < om.deviceList.Count; i++)
                                //    {
                                //        if (BindingDeviceDal.BangResult(2, om.deviceList[i].DeviceType, om.deviceList[i].DeviceCode))
                                //        {
                                //            Log.Warn("绑定下发00000，修改数据库00001");
                                //        }
                                //        else
                                //        {
                                //            Log.Error("绑定下发00000，修改数据库00000");
                                //        }
                                //    }
                                //}
                                #endregion
                            }
                            else if (commID == 0x08)//解除绑定
                            {
                                //om = RentalServer.ProcessedList.Find(x => x.guid == e.CDeviceStandardData.commondGuid);
                                //if (result == 1)
                                //{
                                //    for (int i = 0; i < om.deviceList.Count; i++)
                                //    {
                                //        if (BindingDeviceDal.BangResult(0, om.deviceList[i].DeviceType, om.deviceList[i].DeviceCode))
                                //        {
                                //            Log.Warn("解除绑定00001，修改数据库00001");
                                //        }
                                //        else
                                //        {
                                //            Log.Error("解除绑定下发00001，修改数据库00000");
                                //        }
                                //    }
                                //}
                                //else
                                //{
                                //    for (int i = 0; i < om.deviceList.Count; i++)
                                //    {
                                //        if (BindingDeviceDal.BangResult(2, om.deviceList[i].DeviceType, om.deviceList[i].DeviceCode))
                                //        {
                                //            Log.Warn("解除绑定下发00000，修改数据库00001");
                                //        }
                                //        else
                                //        {
                                //            Log.Error("解除绑定下发00000，修改数据库00000");
                                //        }
                                //    }
                                //}
                            }
                            else if (commID == 0x0A)//远程开关继电器
                            {
                                #region 远程开关继电器
                                om = RentalServer.processedList.Find(x => x.guid == e.CDeviceStandardData.commondGuid);
                                if (result == 1)
                                {
                                    smm.SessionId = om.sessionId;
                                    smm.stationId = om.deviceId.ToString();
                                    smm.deviceId = om.deviceId.ToString();
                                    smm.result = "00001";

                                    RentalServer.replyList.Enqueue(smm);
                                }
                                else
                                {
                                    smm.SessionId = om.sessionId;
                                    smm.stationId = om.deviceId.ToString();
                                    smm.deviceId = om.deviceId.ToString();
                                    smm.result = "00000";

                                    RentalServer.replyList.Enqueue(smm);
                                }
                                #endregion
                            }
                            else if (commID == 0x0C)//设定阀值
                            {
                                //om = RentalServer.ProcessedList.Find(x => x.guid == e.CDeviceStandardData.commondGuid);
                                //if (result == 1)
                                //{
                                //    smm.SessionId = om.sessionId;
                                //    smm.stationId = om.deviceId.ToString();
                                //    smm.deviceId = om.deviceList[0].DeviceCode.ToString();
                                //    smm.result = "00001";

                                //    RentalServer.smessageModelList.Enqueue(smm);
                                //}
                                //else
                                //{
                                //    smm.SessionId = om.sessionId;
                                //    smm.stationId = om.deviceId.ToString();
                                //    smm.deviceId = om.deviceList[0].DeviceCode.ToString();
                                //    smm.result = "00000";

                                //    RentalServer.smessageModelList.Enqueue(smm);
                                //}
                            }
                            else if (commID == 0x10)//撤防
                            {
                                #region 撤防
                                om = RentalServer.processedList.Find(x => x.guid == e.CDeviceStandardData.commondGuid);
                                if (result == 1)
                                {
                                    smm.SessionId = om.sessionId;
                                    smm.stationId = om.deviceId.ToString();
                                    smm.deviceId = om.deviceList[0].HostID.ToString();
                                    smm.DeviceType = om.deviceList[0].DeviceType.ToString();
                                    smm.commId = "010110";
                                    smm.result = "00001";
                                    Log.Warn("撤防控制成功");
                                    RentalServer.replyList.Enqueue(smm);
                                }
                                else
                                {
                                    smm.SessionId = om.sessionId;
                                    smm.stationId = om.deviceId.ToString();
                                    smm.deviceId = om.deviceList[0].HostID.ToString();
                                    smm.DeviceType = om.deviceList[0].DeviceType.ToString();
                                    smm.commId = "010110";
                                    smm.result = "00000";
                                    Log.Warn("撤防控制失败");
                                    RentalServer.replyList.Enqueue(smm);
                                }
                                #endregion
                            }
                            else if (commID == 0x12)//布防
                            {
                                #region 布防
                                om = RentalServer.processedList.Find(x => x.guid == e.CDeviceStandardData.commondGuid);
                                if (result == 1)
                                {
                                    smm.SessionId = om.sessionId;
                                    smm.stationId = om.deviceId.ToString();
                                    smm.deviceId = om.deviceList[0].HostID.ToString();
                                    smm.DeviceType = om.deviceList[0].DeviceType.ToString();
                                    smm.commId = "010112";
                                    smm.result = "00001";
                                    Log.Warn("布防控制成功");
                                    RentalServer.replyList.Enqueue(smm);
                                }
                                else
                                {
                                    smm.SessionId = om.sessionId;
                                    smm.stationId = om.deviceId.ToString();
                                    smm.deviceId = om.deviceList[0].HostID.ToString();
                                    smm.DeviceType = om.deviceList[0].DeviceType.ToString();
                                    smm.commId = "010112";
                                    smm.result = "00000";
                                    Log.Warn("布防控制失败");
                                    RentalServer.replyList.Enqueue(smm);
                                }
                                #endregion
                            }
                            else
                            {
                                Log.Warn("未知解析结果");
                            }
                        }
                        else if (deviceType == 0x0108)
                        {
                            if (commID == 0x06)//定时开关
                            {
                                //om = RentalServer.ProcessedList.Find(x => x.guid == e.CDeviceStandardData.commondGuid);
                                //if (result == 1)
                                //{
                                //    smm.SessionId = om.sessionId;
                                //    smm.stationId = om.deviceId.ToString();
                                //    smm.deviceId = om.deviceList[0].DeviceCode.ToString();
                                //    smm.result = "00001";

                                //    RentalServer.smessageModelList.Enqueue(smm);
                                //}
                                //else
                                //{
                                //    smm.SessionId = om.sessionId;
                                //    smm.stationId = om.deviceId.ToString();
                                //    smm.deviceId = om.deviceList[0].DeviceCode.ToString();
                                //    smm.result = "00000";

                                //    RentalServer.smessageModelList.Enqueue(smm);
                                //}
                            }
                            else
                            {
                                Log.Warn("未知解析结果");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        private void conn_ReceiveUnknownData(object sender, Tendency.ConnToTDR.TendencyConnection.UnknownDataReceivedArgs e)
        {
            switch (e.m_usErrorCode)
            {
                case 0:
                    return;

                case 20003:
                    Log.Warn(string.Format("设备[ID:{0}]不在线", e.m_unDevice));
                    break;

                case 20004:
                    Log.Warn("控制超时");
                    break;

                case 20005:
                    Log.Warn(string.Format("未注册设备[ID:{0}]", e.m_unDevice));
                    break;

                case 20009:
                    Log.Warn(string.Format("用户无设备状态权限", e.m_unDevice));
                    break;

                case 20012:
                    Log.Warn(string.Format("用户无设备数据权限", e.m_unDevice));
                    break;

                default:
                    Log.Warn(string.Format("未知错误[Code:{0}]", e.m_usErrorCode));
                    break;
            }
        }

        private void conn_ReceiveDeviceData(object sender, Tendency.ConnToTDR.TendencyConnection.CDeviceDataReceivedArgs e)
        {
            //Log.Debug(e.CDeviceData.ToString());
        }

        private void conn_ReceiveDeviceOffLine(object sender, Tendency.ConnToTDR.TendencyConnection.CBaseInfoReceivedArgs e)
        {
            Log.Warn(string.Format("设备[ID:{0}]下线", e.CBaseInfo.m_unDevice));
        }

        private void conn_ReceiveDeviceOnLine(object sender, Tendency.ConnToTDR.TendencyConnection.CBaseInfoReceivedArgs e)
        {
            Log.Warn(string.Format("设备[ID:{0}]上线", e.CBaseInfo.m_unDevice));
        }

        private void conn_ReplySendedCommandResult(object sender, Tendency.ConnToTDR.TendencyConnection.CDeviceDataReceivedArgs e)
        {
            //Log.Debug(e.CDeviceData.ToString());
        }

        private void conn_ReplySendedCommand(object sender, Tendency.ConnToTDR.TendencyConnection.CDeviceCommandReceivedArgs e)
        {
            OperateModel om = new OperateModel();
            SocketMessageModel smm = new SocketMessageModel();
            switch (e.CDeviceCommand.m_usErrorCode)
            {
                case 0:
                    Log.Warn(string.Format("命令已00001送达至基站[ID:{0}]", e.CDeviceCommand.m_unDevice));
                    om = RentalServer.processedList.Find(x => x.guid == e.CDeviceCommand.m_GUID);
                    smm.SessionId = om.sessionId;
                    smm.stationId = om.deviceId.ToString();
                    smm.deviceId = om.deviceId.ToString();
                    smm.commId = om.comid;
                    smm.DeviceType = om.deviceList[0].DeviceType.ToString();
                    smm.result = "00002";

                    RentalServer.replyList.Enqueue(smm);
                    break;

                case 20003:
                    Log.Warn(string.Format("设备[ID:{0}]不在线", e.CDeviceCommand.m_unDevice));
                    om = RentalServer.processedList.Find(x => x.guid == e.CDeviceCommand.m_GUID);
                    smm.SessionId = om.sessionId;
                    smm.stationId = om.deviceId.ToString();
                    smm.deviceId = om.deviceId.ToString();
                    smm.commId = om.comid;
                    smm.DeviceType = om.deviceList[0].DeviceType.ToString();
                    smm.result = "00005";

                    RentalServer.replyList.Enqueue(smm);
                    break;

                case 20004:
                    Log.Warn(string.Format("命令发送到至基站[ID:{0}]超时", e.CDeviceCommand.m_unDevice));
                    om = RentalServer.processedList.Find(x => x.guid == e.CDeviceCommand.m_GUID);
                    smm.SessionId = om.sessionId;
                    smm.stationId = om.deviceId.ToString();
                    smm.deviceId = om.deviceId.ToString();
                    smm.commId = om.comid;
                    smm.DeviceType = om.deviceList[0].DeviceType.ToString();
                    smm.result = "00004";

                    RentalServer.replyList.Enqueue(smm);
                    break;

                case 20005:
                    Log.Warn(string.Format("未登记设备[ID:{0}]", e.CDeviceCommand.m_unDevice));
                    om = RentalServer.processedList.Find(x => x.guid == e.CDeviceCommand.m_GUID);
                    smm.SessionId = om.sessionId;
                    smm.stationId = om.deviceId.ToString();
                    smm.deviceId = om.deviceId.ToString();
                    smm.commId = om.comid;
                    smm.DeviceType = om.deviceList[0].DeviceType.ToString();
                    smm.result = "00006";

                    RentalServer.replyList.Enqueue(smm);
                    break;

                case 20009:
                    Log.Warn(string.Format("没有权限", e.CDeviceCommand.m_unDevice));
                    om = RentalServer.processedList.Find(x => x.guid == e.CDeviceCommand.m_GUID);
                    smm.SessionId = om.sessionId;
                    smm.stationId = om.deviceId.ToString();
                    smm.deviceId = om.deviceId.ToString();
                    smm.commId = om.comid;
                    smm.DeviceType = om.deviceList[0].DeviceType.ToString();
                    smm.result = "00007";

                    RentalServer.replyList.Enqueue(smm);
                    break;

                default:
                    Log.Warn(string.Format("未知错误[Code:{0}]", e.CDeviceCommand.m_usErrorCode));
                    om = RentalServer.processedList.Find(x => x.guid == e.CDeviceCommand.m_GUID);
                    smm.SessionId = om.sessionId;
                    smm.stationId = om.deviceId.ToString();
                    smm.deviceId = om.deviceId.ToString();
                    smm.commId = om.comid;
                    smm.DeviceType = om.deviceList[0].DeviceType.ToString();
                    smm.result = "10000";

                    RentalServer.replyList.Enqueue(smm);
                    break;
            }
        }

        private void conn_ReplyRegionDeviceData(object sender, Tendency.ConnToTDR.TendencyConnection.CDeviceStateReceivedArgs e)
        {
            foreach (Tendency.ConnToTDR.AppTcpClient.DeviceItem state in e.CDeviceState.m_vctItem)
            {
                Log.Warn(string.Format("基站设备[ID:{0}]{1}", state.unDevice, state.btState == 1 ? "在线" : "离线"));
            }

            Log.Warn(string.Format("注册设备00001[{0}]", e.CDeviceState.m_usErrorCode));
        }

        private void conn_ReplyRegionDevice(object sender, Tendency.ConnToTDR.TendencyConnection.CDeviceStateReceivedArgs e)
        {
            if (e.CDeviceState.m_usErrorCode != 0)
                Log.Warn(string.Format("注册设备出错[{0}]", e.CDeviceState.m_usErrorCode));
        }

        private void conn_ReplyUserLogin(object sender, Tendency.ConnToTDR.TendencyConnection.CBaseInfoReceivedArgs e)
        {
            switch (e.CBaseInfo.m_usErrorCode)
            {
                case 0:

                    Log.Warn("用户登录00001");
                    RegDevice();
                    break;

                case 20009:
                    Log.Warn("没有权限");
                    break;

                default:
                    Log.Warn(string.Format("用户登录00000,未知错误[Code:{0}]", e.CBaseInfo.m_usErrorCode));
                    break;
            }
        }

        private void RegDevice()
        {
            uint cnt = (maxDeviceId - minDeviceId + 1) / 10000;
            uint yusu = (maxDeviceId - minDeviceId + 1) % 10000;
            uint[] tol = new uint[maxDeviceId - minDeviceId + 1];
            for (uint i = 0; i < maxDeviceId - minDeviceId + 1; i++)
            {
                tol[i] = minDeviceId + i;
            }
            uint[] num = new uint[10000];
            for (uint i = 0; i < cnt + 1; i++)
            {
                if (i == cnt)
                {
                    num = new uint[yusu];
                    Array.Copy(tol, 10000 * i, num, 0, yusu);
                }
                else
                {
                    num = new uint[10000];
                    Array.Copy(tol, i * 10000, num, 0, 10000);
                }
                conn.RegionDevice(_userID, num);
                conn.RegionDeviceTransData(_userID, num);
            }
        }

        private void conn_LostConnection(object sender, Tendency.ConnToTDR.TendencyConnection.CBaseInfoReceivedArgs e)
        {
            Log.Warn(string.Format("与平台断开连接,错误码[{0}]", e.CBaseInfo.m_usErrorCode));

            if (conn.Connect(_ip, _port))
            {
                Log.Warn("正在平台连接");
            }
            else
            {
                Log.Warn("打开平台连接00000");
            }
        }

        private void conn_ConnectComplete(object sender, EventArgs e)
        {
            Log.Warn("打开平台连接00001,正在注册用户");
            conn.UserLogin(_userID, _token);
        }

        public void SendOperateCommand(OperateModel om)
        {
            if (conn.ControlDevice(0, om.Sn, 0, om.deviceId, 0, 0, 0, om.guid, om.commandID, om.Data))
            {
                Log.Warn("命令发送成功");
            }
            else
            {
                Log.Error("命令发送失败");

                SocketMessageModel smm = new SocketMessageModel();
                smm.SessionId = om.sessionId;
                smm.stationId = om.deviceId.ToString();
                smm.deviceId = om.deviceId.ToString();
                smm.commId = om.comid;
                smm.DeviceType = om.deviceList[0].DeviceType.ToString();
                smm.result = "00008";

                RentalServer.replyList.Enqueue(smm);
            }
        }
    }
}

