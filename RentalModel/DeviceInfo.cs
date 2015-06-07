using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalModel
{
    public class DeviceInfo
    {
        public uint StationNo { get; set; }//基站ID

        public uint HostID { get; set; }//主机ID
        public ushort DeviceType { get; set; }//设备类型
        public uint DeviceCode { get; set; }//设备id
        public string ROOMNO { get; set; }//房间号
    }
}
