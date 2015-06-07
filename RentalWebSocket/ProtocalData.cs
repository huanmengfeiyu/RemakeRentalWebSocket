using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalWebSocket
{
    public class ProtocolData
    {
        public string key { get; set; }
        public string type { get; set; }
        public string val { get; set; }
    }
    public class ProtocolDataList:List<ProtocolData>
    {
        
    }
}
