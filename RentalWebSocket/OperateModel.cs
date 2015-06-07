using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalWebSocket
{
    public class OperateModel
    {
        public uint deviceId { get; set; }

        public DateTime time { get; set; }

        public ushort Sn { get; set; }

        public Guid guid { get; set; }

        public byte[] Data { get; set; }

        public ushort commandID { get; set; }
        public string comid { get; set; }
        public string sessionId { get; set; }

        public int Oragin { get; set; }

        public int UserId { get; set; }

        public List<RentalSocketList> deviceList { get; set; }
        public OperateModel()
        {
            guid = Guid.NewGuid();
            deviceList = new List<RentalSocketList>();
        }
    }
}
