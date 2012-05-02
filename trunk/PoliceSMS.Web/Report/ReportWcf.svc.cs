using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using NHibernate;
using PoliceSMS.Web.Comm;
using System.Data.SqlClient;
using System.Data;
using PoliceSMS.Lib.Report;
using PoliceSMS.Web.Serializable;

namespace PoliceSMS.Web.Report
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“ReportWcf”。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ReportWcf : IReportWcf
    {

        private ISessionFactory hbmSessionFactory;
        #region IBaseService 成员

        public ISessionFactory HbmSessionFactory
        {
            get
            {
                if (hbmSessionFactory == null)
                    hbmSessionFactory = SessionFactory.HbmSessionFactory;


                return hbmSessionFactory;
            }
            set
            {
                hbmSessionFactory = value;
            }
        }

        public string LoadStationReportResult(int UnitType, DateTime beginTime1, DateTime endTime1,DateTime beginTime2, DateTime endTime2)
        {
            ISession session=HbmSessionFactory.OpenSession();

            IDbCommand cmd = session.Connection.CreateCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            //exec (SMS_UnitReportWithCompare @UnitType,@BeginTim1,@EndTime1,@BeginTime2,@EndTime2)
            cmd.CommandText = "SMS_UnitReportWithCompare";

            SqlParameter pUnitType = new SqlParameter("@unitType", SqlDbType.Int);
            SqlParameter pBeginTime1 = new SqlParameter("@beginTime1", SqlDbType.DateTime);
            SqlParameter pEndTime1 = new SqlParameter("@endTime1", SqlDbType.DateTime);
            SqlParameter pBeginTime2 = new SqlParameter("@beginTime2", SqlDbType.DateTime);
            SqlParameter pEndTime2 = new SqlParameter("@endTime2", SqlDbType.DateTime);

            pUnitType.Value = UnitType;
            pBeginTime1.Value = beginTime1;
            pEndTime1.Value = endTime1;
            pBeginTime2.Value = beginTime2;
            pEndTime2.Value = endTime2;

            cmd.Parameters.Add(pUnitType);
            cmd.Parameters.Add(pBeginTime1);
            cmd.Parameters.Add(pEndTime1);
            cmd.Parameters.Add(pBeginTime2);
            cmd.Parameters.Add(pEndTime2);

            IDataReader reader = cmd.ExecuteReader();

            

            IList<StationReportResult> result = new List<StationReportResult>();

            while (reader.Read())
            {
                StationReportResult srr = new StationReportResult();

                srr.UpRank = reader.IsDBNull(0) ? null : (int?)reader.GetInt32(0);
                srr.UnitName = reader.GetString(1);
                srr.Rank = reader.GetInt32(2);
                srr.UnitId = reader.GetInt32(3);
                srr.TotalCount = reader.GetInt32(4);
                srr.StationRate = (double)reader.GetDecimal(5);
                srr.Score = (double)reader.GetDecimal(6);

                srr.G1Count = reader.GetInt32(7);
                srr.G1Rate = (double)reader.GetDecimal(8);

                srr.G2Count = reader.GetInt32(9);
                srr.G2Rate = (double)reader.GetDecimal(10);

                srr.G3Count = reader.GetInt32(11);
                srr.G3Rate = (double)reader.GetDecimal(12);

                srr.G4Count = reader.GetInt32(13);
                srr.G4Rate = (double)reader.GetDecimal(14);

                srr.G5Count = reader.GetInt32(15);
                srr.G5Rate = (double)reader.GetDecimal(16);

                result.Add(srr);
            }

            string json = JsonSerializerHelper.EntityToJson(result);

            return PackJsonListResult("true", json, string.Empty, result.Count);
        }

        protected string PackJsonListResult(string success, string json, string message, long total)
        {
            //"{ 'Success': {0}, 'Data': {1},'Message':{2},'Total':{3}}"
            return string.Format("{0}'Success':'{1}','Data':{2},'Message':'{3}','Total':{4}{5}", "{", success, json, message, total.ToString(), "}");
        }

        protected string PackJsonListResultForArray(string success, string json, string message, long total)
        {
            return string.Format("{0}'Success':'{1}','Datas':{2},'Message':'{3}','Total':{4}{5}", "{", success, json, message, total.ToString(), "}");
        }

        #endregion
    }
}
