using Microsoft.Practices.EnterpriseLibrary.Data;
using RentalModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalDal
{
    public class BindingDeviceDal
    {
        public static List<DeviceInfo> GetDevices()
        {
            var db = new DatabaseProviderFactory().Create("OracleDB");
            string sql = "select a.DEVICECODE,a.DEVICETYPE,b.DEVICECODE HostID,b.STATIONNO StationNo from TB_DEVICEINFO  a LEFT join TB_DEVICEINFO b on  a.roomid=b.roomid where a.devicetype> 258 and a.devicetype < 512 and b.devicetype>256  and  b.devicetype<259 and a.ISBUNG =0 and b.STATIONNO is not null";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            DataSet ds = db.ExecuteDataSet(cmd);
            DataTable dt = ds.Tables[0];
            List<DeviceInfo> DeviceList = new List<DeviceInfo>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DeviceInfo deviceInfo = new DeviceInfo();
                    deviceInfo.DeviceCode = Convert.ToUInt32(dt.Rows[i]["DEVICECODE"].ToString());
                    deviceInfo.DeviceType = Convert.ToUInt16(dt.Rows[i]["DEVICETYPE"].ToString());
                    deviceInfo.HostID = Convert.ToUInt32(dt.Rows[i]["HOSTID"]);
                    deviceInfo.StationNo = Convert.ToUInt32(dt.Rows[0]["STATIONNO"]);
                    DeviceList.Add(deviceInfo);
                }
            }
            return DeviceList;
        }
        /// <summary>
        /// 获取出租房二期设备列表
        /// </summary>
        /// <returns></returns>
        public static List<DeviceInfo> GetV2Devices()
        {
            var db = new DatabaseProviderFactory().Create("OracleDB");
            string sql = "select c.roomno,a.DEVICECODE,a.DEVICETYPE,b.DEVICECODE HostID from TB_DEVICEINFO a,TB_DEVICEINFO b,tb_roominfo c where a.houseid=b.houseid and a.ROOMID=c.roomid and a.parentid=b.deviceid and a.devicetype> 1023 and a.devicetype < 1088 and b.devicetype>1023 and b.devicetype<1088 and a.ISBUNG =0 and a.STATUS=1 and b.STATUS=1 ";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            DataSet ds = db.ExecuteDataSet(cmd);
            DataTable dt = ds.Tables[0];
            List<DeviceInfo> DeviceList = new List<DeviceInfo>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DeviceInfo deviceInfo = new DeviceInfo();
                    deviceInfo.ROOMNO = dt.Rows[i]["ROOMNO"].ToString();
                    deviceInfo.DeviceCode = Convert.ToUInt32(dt.Rows[i]["DEVICECODE"].ToString());
                    deviceInfo.DeviceType = Convert.ToUInt16(dt.Rows[i]["DEVICETYPE"].ToString());
                    deviceInfo.HostID = Convert.ToUInt32(dt.Rows[i]["HOSTID"]);
                    //deviceInfo.StationNo = Convert.ToUInt32(dt.Rows[0]["STATIONNO"]);
                    DeviceList.Add(deviceInfo);
                }
            }
            return DeviceList;
        }
        /// <summary>
        /// 获取有源卡列表
        /// </summary>
        /// <returns></returns>
        public static List<YYKInfo> GetV2YYKs()
        {
            var db = new DatabaseProviderFactory().Create("OracleDB");
            string sql = "select c.roomno,b.devicecode,a.IDENTITYCARDID,d.devicecode HostID  from TB_EMPOWER a, tb_deviceinfo b,tb_roominfo c,tb_deviceinfo d where a.deviceid=b.deviceid and b.DEVICETYPE='32769' and b.roomid=c.roomid and b.STATUS=1 and c.managedeviceid=d.deviceid";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            DataSet ds = db.ExecuteDataSet(cmd);
            DataTable dt = ds.Tables[0];
            List<YYKInfo> YYKList = new List<YYKInfo>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    YYKInfo yykInfo = new YYKInfo();
                    yykInfo.roomno = dt.Rows[i]["roomno"].ToString();
                    yykInfo.yykID = Convert.ToUInt32(dt.Rows[i]["DEVICECODE"]);
                    yykInfo.IDENTITYCARDID = dt.Rows[i]["IDENTITYCARDID"].ToString();
                    yykInfo.HostID =Convert.ToUInt32( dt.Rows[i]["HostID"]);
                }
            }
            return YYKList;
        }
        public static bool BangResult(int value, int devicetype, uint devicecode)
        {
            var db = new DatabaseProviderFactory().Create("OracleDB");
            string sql = "UPDATE TB_DEVICEINFO SET ISBUNG=:ISBUNG WHERE DEVICETYPE=:DEVICETYPE and DEVICECODE=:DEVICECODE";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, ":ISBUNG", DbType.String, value);
            db.AddInParameter(cmd, ":DEVICETYPE", DbType.String, devicetype);
            db.AddInParameter(cmd, ":DEVICECODE", DbType.String, devicecode);
            return db.ExecuteNonQuery(cmd) > 0;
        }
        public static bool SaveSwitchStatus(string value, string deviceCode, string deviceType)
        {
            var db = new DatabaseProviderFactory().Create("OracleDB");
            string sql = "update TB_DEVICEHEARTBEAT set PARAM4=:PARAM4 where DEVICEID=(select DEVICEID from TB_DEVICEINFO where DEVICECODE=:DEVICECODE and DEVICETYPE=:DEVICETYPE) and DEVICETYPE=:DEVICETYPE;";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, ":PARAM4", DbType.String, value);
            db.AddInParameter(cmd, ":DEVICECODE", DbType.String, deviceCode);
            db.AddInParameter(cmd, ":DEVICETYPE", DbType.String, deviceType);
            db.AddInParameter(cmd, ":DEVICETYPE", DbType.String, deviceType);
            return db.ExecuteNonQuery(cmd) > 0;
        }
        /// <summary>
        /// 获取身份认证信息
        /// </summary>
        /// <returns></returns>
        public static List<DeviceInfo> AuthenticationDal()
        {
            var db = new DatabaseProviderFactory().Create("OracleDB");
            string sql = "select DEVICECODE,DEVICETYPE,STATIONNO from TB_DEVICEINFO a,TB_CERTIFY b where a.DEVICEID=b.DEVICEID";
            DbCommand cmd = db.GetSqlStringCommand(sql);

            List<DeviceInfo> deviceList = new List<DeviceInfo>();
            DataSet ds = db.ExecuteDataSet(cmd);
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DeviceInfo deviceInfo = new DeviceInfo();
                deviceInfo.DeviceCode = Convert.ToUInt32(dt.Rows[i]["DEVICECODE"]);
                deviceInfo.DeviceType = Convert.ToUInt16(dt.Rows[i]["DEVICETYPE"]);
                deviceInfo.StationNo = Convert.ToUInt32(dt.Rows[i]["STATIONNO"]);
                deviceList.Add(deviceInfo);
            }
            return deviceList;
        }
        /// <summary>
        /// 身份认证结果处理
        /// </summary>
        /// <returns></returns>
        public static bool AuthenticationResult(int pushstatus, string pushreason)
        {
            var db = new DatabaseProviderFactory().Create("OracleDB");
            string sql = "update TB_CERTIFY set PUSHSTATUS=:PUSHSTATUS,PUSHREASON=:PUSHREASON,PUSHTIME=SYSDATE() where CERTIFYTYPE=1 and COMPARE=1 and PUSHSTATUS=0";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, ":PUSHSTATUS", DbType.String, pushstatus);
            db.AddInParameter(cmd, ":PUSHREASON", DbType.String, pushreason);
            return db.ExecuteNonQuery(cmd) > 0;
        }
        /// <summary>
        /// 获取未上线设备
        /// </summary>
        public static void SelectNotLineEquipment()
        {
            //TODO:获取未上线设备
        }
    }
}
