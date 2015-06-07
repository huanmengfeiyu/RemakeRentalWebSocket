using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalWebSocket
{
    public class Device
    {
        public OperateModel om = new OperateModel();
        public SocketMessageModel smm = new SocketMessageModel();
        public TdrPlatform tdrPlatForm { get; set; }

        public Device(TdrPlatform platform)
        {
            this.tdrPlatForm = platform;
        }
        public void SendCommand(OperateModel operateModel)
        {
            tdrPlatForm.SendOperateCommand(operateModel);
        }
    }
    public class DeviceList : List<Device>
    {

    }
}
