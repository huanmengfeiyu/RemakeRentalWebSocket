using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalWebSocket
{
    public class RentalSocketList : IRequestInfo
    {
        public string Key { get; set; }//SN 流水号，ushort
        public string StationNo { get; set; }//基站ID
        public string HostID { get; set; }//主机ID
        public List<RentalSocketInfo> RentalList { get; set; }
        public int type { get; set; }//状态
        public uint Delay { get; set; }//延时时间
        public int OverValue { get; set; }//功率过大阀值
        public int JumpValue { get; set; }//功率跳变阀值
        public int DeviceType { get; set; }
    }
    public class RentalSocketInfo
    {
        public int DeviceType { get; set; }//设备类型
        public uint DeviceCode { get; set; }//设备id
        public RentalSocketInfo(uint DeviceCode, int DeviceType)
        {
            this.DeviceCode = DeviceCode;
            this.DeviceType = DeviceType;
        }

    }
}
