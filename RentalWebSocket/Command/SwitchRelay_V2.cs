using RentalUtity;
using SuperWebSocket.SubProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalWebSocket.Command
{
    public class SwitchRelay_V2 : JsonSubCommand<RentalSession, RentalSocketList>
    {
        public override string Name
        {
            get
            {
                return "0x040606";
            }
        }
        protected override void ExecuteJsonCommand(RentalSession session, RentalSocketList commandList)
        {
            try
            {
                OperateModel operate = new OperateModel();
                operate.commandID = 0xF005;
                operate.Sn = Convert.ToUInt16(commandList.Key);
                if (!string.IsNullOrWhiteSpace(commandList.StationNo))
                {
                    operate.deviceId = Convert.ToUInt32(commandList.StationNo);
                }
                if (operate.deviceId != 0)
                {
                    operate.sessionId = session.SessionID;
                    List<byte> byteList = new List<byte>();
                    DateTime dt = DateTime.Now;
                    byteList.Add(Convert.ToByte(dt.Year.ToString().Substring(2, 2)));
                    byteList.Add(Convert.ToByte(dt.Month));
                    byteList.Add(Convert.ToByte(dt.Day));
                    byteList.Add(Convert.ToByte(dt.Hour));
                    byteList.Add(Convert.ToByte(dt.Minute));
                    byteList.Add(Convert.ToByte(dt.Second));
                    byteList.Add(0);//SN
                    byteList.AddRange(ConvertHelpers.hexStrToByte("0x0406"));
                    byteList.AddRange(ConvertHelpers.intToBytes2(Convert.ToUInt32(commandList.HostID)));
                    byteList.AddRange(ConvertHelpers.hexStrToByte("0x06"));
                    byteList.Add(Convert.ToByte(commandList.type));

                    operate.deviceList.Add(commandList);
                    operate.Data = byteList.ToArray();
                    operate.comid = Name;
                    RentalServer.operateList.Enqueue(operate);
                    Log.Warn(session.SessionID + ",执行了远程开关继电器:" + commandList.type);
                    Log.Debug(session.SessionID + ",执行了远程开关继电器," + Newtonsoft.Json.JsonConvert.SerializeObject(commandList));
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}
