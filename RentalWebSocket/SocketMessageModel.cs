using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalWebSocket
{
    public class SocketMessageModel
    {
        [NonSerialized()]
        public string SessionId;
        public string stationId { get; set; }
        public string hostId { get; set; }
        public string deviceId { get; set; }
        public string DeviceType { get; set; }
        public string result { get; set; }
        public string commId { get; set; }
    }
}
