using RentalUtity;
using SuperWebSocket.SubProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalWebSocket.Command
{
    public class ManuallyArm : JsonSubCommand<RentalSession, RentalSocketList>
    {
        public override string Name
        {
            get
            {
                return "0x0205";
            }
        }
        protected override void ExecuteJsonCommand(RentalSession session, RentalSocketList commandList)
        {
            try
            {
                OperateModel operate = new OperateModel();
                operate.commandID = 0x0205;
                operate.deviceId = Convert.ToUInt32(commandList.StationNo);
                List<byte> byteList = new List<byte>();
                string a = commandList.HostID;

                byteList.AddRange(Encoding.GetEncoding("gbk").GetBytes(a));
                byteList.Add(Convert.ToByte(commandList.type));
                operate.Data = byteList.ToArray();
                operate.sessionId = session.SessionID;
                operate.Sn = Convert.ToUInt16(commandList.Key);
                operate.comid = Name;

                RentalServer.operateList.Enqueue(operate);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}
