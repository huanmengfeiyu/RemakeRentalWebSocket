using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalUtity
{
    public class GlobalConfig
    {
        public static string serverPlatIP = ConfigurationManager.AppSettings["serverPlatIp"];
        public static ushort serverPlatPort = Convert.ToUInt16(ConfigurationManager.AppSettings["serverPlatPort"]);
        public static uint appId = Convert.ToUInt32(ConfigurationManager.AppSettings["appId"]);
        public static string appKey = ConfigurationManager.AppSettings["appKey"];
        public static uint minDeviceId = Convert.ToUInt32(ConfigurationManager.AppSettings["minDeviceId"]);
        public static uint maxDeviceId = Convert.ToUInt32(ConfigurationManager.AppSettings["maxDeviceId"]);
    }
}
