using SuperWebSocket.SubProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalWebSocket
{
    public class Unbind : JsonSubCommand<RentalSession, RentalSocketList>
    {
        public override string Name
        {
            get
            {
                return "010108";
            }
        }

        protected override void ExecuteJsonCommand(RentalSession session, RentalSocketList commandList)
        {
            try
            {
                OperateModel operate = new OperateModel();
                operate.commandID = 0xF003;
                operate.Sn = Convert.ToUInt16(commandList.Key);
                operate.deviceId = Convert.ToUInt32(commandList.StationNo);
                operate.sessionId = session.SessionID;
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
                bytelist.AddRange(ConvertHelpers.intToBytes2(Convert.ToUInt32(commandList.HostID)));
                bytelist.AddRange(ConvertHelpers.hexStrToByte("0x06"));

                foreach (var commandInfo in commandList.RentalList)
                {
                    bytelist.AddRange(ConvertHelpers.IntToByteTwoByHignFirst(commandInfo.DeviceType));
                    bytelist.AddRange(ConvertHelpers.intToBytes2(commandInfo.DeviceCode));
                }
                operate.Data = bytelist.ToArray();
                RentalServer.oprateModelList.Enqueue(operate);
            }
            catch (Exception ex)
            {

                Log.Error(ex.ToString());
            }
        }
    }
}