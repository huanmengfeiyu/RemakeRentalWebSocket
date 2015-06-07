using SuperWebSocket.SubProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalWebSocket.Command
{
    public class ArmStatusUpload : JsonSubCommand<RentalSession, RentalSocketList>
    {
        public override string Name
        {
            get
            {
                return "0x0206";
            }
        }
        protected override void ExecuteJsonCommand(RentalSession session, RentalSocketList commandList)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
    }
}
