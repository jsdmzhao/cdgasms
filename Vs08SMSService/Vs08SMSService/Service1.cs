using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;

namespace Vs08SMSService
{
    public partial class Service1 : ServiceBase
    {
        Log log = new Log(ConfigurationManager.AppSettings["Path"].ToString());
        string OutTime = ConfigurationManager.AppSettings["OutTime"].ToString();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Thread thd = new Thread(Run);
                thd.Start();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private void Dataprocessing()
        {
            SendMessage();

            List<WorkPersonView> works = Get(Convert.ToInt32(ConfigurationManager.AppSettings["OutTime"]));

            Update(works);
        }

        /// <summary>
        /// 获得未评价的信息的评价
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private List<WorkPersonView> Get(int p)
        {
            List<WorkPersonView> works = new List<WorkPersonView>();
            try
            {
                string connectionStr = ConfigurationManager.AppSettings["Case"];
                SqlConnection con = new SqlConnection(connectionStr);
                con.Open();
                string sql = "select SmsIndex,SmsContent,SendNumber from sms2.dbo.RecvSmsTable where [Used] = 0 and SendNumber in  (select w.TelPhone from  [case].dbo.SMS_Work as w where w.IsValuation =0 and dateadd(hour," + p + ", w.CreatTime) > getdate())";

                SqlCommand command = new SqlCommand(sql, con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    works.Add(new WorkPersonView()
                    {
                        SMSId = Convert.ToInt32(reader[0].ToString()),
                        Value = reader[1].ToString(),
                        TelNumber = reader[2].ToString()
                    });
                }

                con.Close();
            }
            catch (Exception ex)
            {
                log.WriteLog(DateTime.Now + ":" + ex.Message);
            }
            return works;
        }

        /// <summary>
        /// 修改评价表和回复表
        /// </summary>
        /// <param name="works"></param>
        private void Update(List<WorkPersonView> works)
        {
            for (int i = 0; i < works.Count; i++)
            {
                int valuationId = GetIdByCode(works[i].Value);
                string sql = "update [case].[dbo].[SMS_Work] set Valuation = '" + works[i].Value + "' , ValuationId = " + valuationId + ", IsValuation = 1 where IsValuation =0 and TelPhone = '" + works[i].TelNumber + "'";

                if (SqlNoQuery(sql))
                    UpdateSmsRecv(works[i].SMSId);
            }
        }

        /// <summary>
        /// 更新SmsRecv表
        /// </summary>
        /// <param name="smID"></param>
        /// <returns></returns>
        private bool UpdateSmsRecv(int smID)
        {
            string sql = "update sms2.dbo.RecvSmsTable set Used = 1 where SmsIndex = " + smID;

            return SqlNoQuery(sql);
        }

        private int GetIdByCode(string code)
        {
            int id = 1;
            try
            {
                string connectionStr = ConfigurationManager.AppSettings["Case"];
                SqlConnection con = new SqlConnection(connectionStr);
                con.Open();
                string sql = "select Id from [case].dbo.SMS_WorkValuationType where code = '" + code + "'";

                SqlCommand command = new SqlCommand(sql, con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    id = Convert.ToInt32(reader[0].ToString());
                }
            }
            catch (Exception ex)
            {
                log.WriteLog(DateTime.Now + ":" + ex.Message);
            }
            return id;
        }
        /// <summary>
        /// 执行没有查询的sql语句
        /// </summary>
        /// <param name="sql"></param>
        private bool SqlNoQuery(string sql)
        {
            try
            {
                string connectionStr = ConfigurationManager.AppSettings["Case"];
                SqlConnection con = new SqlConnection(connectionStr);
                con.Open();

                SqlCommand command = new SqlCommand(sql, con);
                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                log.WriteLog(DateTime.Now + ":" + ex.Message);
                return false;
            }
        }

        private static readonly string message = ConfigurationManager.AppSettings["Message"];
        //"您好！请您对到派出所办事时接待民警的工作进行评价。请回复数字1-5，1表示非常满意，2表示基本满意，3表示一般，4表示不满意—态度不好，5表示不满意—业务不精！谢谢。成都市公安局青羊分局";
        /// <summary>
        /// 发送短信
        /// </summary>
        private bool SendMessage()
        {
            
            string sql = "insert [sms2].[dbo].[SendingSmsTable](PhoneNumber,SmsContent,NewFlag) select TelPhone, '" + message + "',1 from [case].[dbo].[SMS_Work] as w inner join  dbo.SMS_WorkType as t on w.WorkTypeId =t.id where IsSend = 0";
            sql += "update [case].[dbo].[SMS_Work] set IsSend = 1 where IsSend = 0";
            return SqlNoQuery(sql);
        }

        public void Run()
        {
            while (true)
            {
                if (DateTime.Now.Hour > 9 && DateTime.Now.Hour < 21)
                {
                    Dataprocessing();
                    Thread.Sleep(30000);
                }
            }
        }

        protected override void OnStop()
        {
        }
    }
    public class WorkPersonView
    {
        public int Id { get; set; }
        public string TelNumber { get; set; }
        public string Value { get; set; }
        public bool IsValuation { get; set; }

        public int SMSId { get; set; }
    }
}
