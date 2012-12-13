using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using NHibernate;
using PoliceSMS.Web.Comm;
using PoliceSMS.Web.Serializable;
using PoliceSMS.Lib.SMS;
using PoliceSMS.Lib.Query;
using Newtonsoft.Json;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace PoliceSMS.Web.SMSWcf
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“SMSRecordService”。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class SMSRecordService : ISMSRecordService
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
        /// <summary>
        /// 保存或修改
        /// 保存的工作每个业务对象都不相同，有可能重写，请注意！
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual string SaveOrUpdate(string json)
        {
            using (ISession sess = HbmSessionFactory.OpenSession())
            {
                ITransaction tx = null;

                SMSRecord entity = JsonSerializerHelper.JsonToEntity<SMSRecord>(json);
                if (entity.Id == 0)
                {
                    //判断重复录入
                    string hql = string.Format(" from SMSRecord as r where convert(varchar(50),CreatTime,112) = '{0}' and PersonName = '{1}' and WorkType.Id = {2} "
                        , DateTime.Now.ToString("yyyyMMdd"), entity.PersonName, entity.WorkType.Id);
                    var existList = sess.CreateQuery(hql).List<SMSRecord>();
                    if (existList != null && existList.Count > 0)
                        return PackJsonResult("false", "0", "此案件已经录入！");

                    if (string.IsNullOrEmpty(entity.PersonMobile))
                        entity.PersonMobile = defaultMobile;

                    entity.WorkDate = DateTime.Now;
                    entity.YearMonth = (DateTime.Now.Year * 100 + DateTime.Now.Month).ToString();
                }

                try
                {
                    tx = sess.BeginTransaction();

                    sess.SaveOrUpdate(entity);
                    tx.Commit();
                    //有点sb了，凑合补救一下
                    //正确的做法是定义个包含Id的interface，所有要持久化的类都实现这个interface
                    dynamic d = entity;
                    return PackJsonResult("true", d.Id.ToString(), string.Empty);
                }
                catch (Exception ex)
                {
                    if (tx != null && tx.IsActive)
                        tx.Rollback();

                    return PackJsonResult("false", "0", ex.Message);
                }
            }
        }

        private static readonly string defaultMobile = ConfigurationManager.AppSettings["DefaultMobile"];

        public virtual string SaveList(string json, int cnt)
        {
            using (var sess = HbmSessionFactory.OpenStatelessSession())
            {
                try
                {
                    SMSRecord entity = JsonSerializerHelper.JsonToEntity<SMSRecord>(json);
                    if (entity.WorkDate == null)
                        entity.WorkDate = DateTime.Now;
                    entity.YearMonth = (entity.WorkDate.Value.Year * 100 + entity.WorkDate.Value.Month).ToString();
                    DataTable table = new DataTable();
                    table.Columns.Add(new DataColumn("Id"));
                    table.Columns.Add(new DataColumn("YearMonth"));
                    table.Columns.Add(new DataColumn("Name"));
                    table.Columns.Add(new DataColumn("CreateTime"));
                    table.Columns.Add(new DataColumn("TelPhone"));
                    table.Columns.Add(new DataColumn("Address"));
                    table.Columns.Add(new DataColumn("PersonIdCard"));
                    table.Columns.Add(new DataColumn("Sex"));
                    table.Columns.Add(new DataColumn("WorkTypeId"));
                    table.Columns.Add(new DataColumn("WorkDetail"));
                    table.Columns.Add(new DataColumn("PoliceManId"));
                    table.Columns.Add(new DataColumn("ValuationId"));
                    table.Columns.Add(new DataColumn("IsValuation"));
                    table.Columns.Add(new DataColumn("UnitId"));
                    table.Columns.Add(new DataColumn("Valuation"));
                    table.Columns.Add(new DataColumn("IsSend"));
                    table.Columns.Add(new DataColumn("Receiver"));
                    table.Columns.Add(new DataColumn("Number"));
                    table.Columns.Add(new DataColumn("WorkOfficerId"));
                    table.Columns.Add(new DataColumn("LeaderId"));

                    for (int i = 0; i < cnt; i++)
                    {
                        DataRow row = table.NewRow();
                        row[0] = 0;
                        row[1] = entity.YearMonth;
                        row[3] = entity.WorkDate;
                        row[4] = entity.PersonMobile;
                        row[8] = entity.WorkType.Id;
                        row[10] = entity.LoginOfficer.Id;
                        row[11] = entity.GradeType.Id;
                        row[12] = entity.IsResponse;
                        row[13] = entity.Organization.Id;
                        row[15] = entity.IsSend;
                        row[18] = entity.WorkOfficer.Id;
                        row[19] = entity.Leader.Id;
                        table.Rows.Add(row);
                    }

                    //只有sql server才能使用
                    if (!(sess.Connection is SqlConnection))
                        throw new Exception("目前只支持sql server数据库！");
                    insertBulkTable(sess.Connection as SqlConnection, "SMS_Work", table);
                    return PackJsonResult("true", "0", string.Empty);
                }
                catch (Exception ex)
                {
                    return PackJsonResult("false", "0", ex.Message);
                }
            }
        }

        private static void insertBulkTable(SqlConnection conn, string tableName, DataTable data)
        {
            SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);
            bulkCopy.DestinationTableName = tableName;
            bulkCopy.BatchSize = data.Rows.Count;

            if (conn.State != ConnectionState.Open)
                conn.Open();
            if (data != null && data.Rows.Count != 0)
                bulkCopy.WriteToServer(data);

            bulkCopy.Close();

        }

        /// <summary>
        /// 通过id删除一个实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string DeleteById(int id)
        {
            using (ISession sess = HbmSessionFactory.OpenSession())
            {
                ITransaction tx = sess.BeginTransaction();
                try
                {
                    SMSRecord entity = sess.Load<SMSRecord>(id);
                    sess.Delete(entity);
                    tx.Commit();
                    return PackJsonResult("true", "true", string.Empty);
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    return PackJsonResult("false", "false", ex.Message);
                }
            }
        }

        /// <summary>
        /// 通过id获得一个实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string GetById(int id)
        {
            using (ISession sess = HbmSessionFactory.OpenSession())
            {
                try
                {
                    SMSRecord entity = sess.Get<SMSRecord>(id);

                    string s = JsonSerializerHelper.EntityToJson(entity);

                    return PackJsonResult("true", s, string.Empty);
                }
                catch (Exception ex)
                {
                    return PackJsonResult("false", string.Empty, ex.Message);
                }
            }
        }

        public virtual string GetByHql(string hql)
        {
            using (ISession sess = HbmSessionFactory.OpenSession())
            {
                try
                {
                    SMSRecord entity = sess.CreateQuery(hql).UniqueResult<SMSRecord>();

                    string s = JsonSerializerHelper.EntityToJson(entity);

                    return PackJsonResult("true", s, string.Empty);

                }
                catch (Exception ex)
                {
                    return PackJsonResult("true", string.Empty, ex.Message);
                }
            }
        }

        /// <summary>
        /// 读取一个列表
        /// </summary>
        /// <param name="hql"></param>
        /// <returns></returns>
        public virtual string GetListByHQL(string hql)
        {

            using (ISession sess = HbmSessionFactory.OpenSession())
            {
                try
                {
                    IList<SMSRecord> ls = sess.CreateQuery(hql).List<SMSRecord>();

                    if (ls.Count > 0 && ls[0] == null)
                        ls.RemoveAt(0);


                    string json = JsonSerializerHelper.EntityToJson(ls);

                    string s = PackJsonListResult("true", json, string.Empty, ls.Count);

                    return s;
                }
                catch (Exception ex)
                {
                    return PackJsonListResult("false", "[]", ex.Message, 0);
                }
            }
        }

        public virtual string GetListBySQL(string sql)
        {
            using (ISession sess = HbmSessionFactory.OpenSession())
            {
                try
                {
                    ISQLQuery q = sess.CreateSQLQuery(sql);

                    IList<object[]> ls = q.List<object[]>();

                    //object o = ls[0][3];

                    //Type t = o.GetType();

                    //double dd = (double)o;

                    if (ls.Count > 0 && ls[0] == null)
                        ls.RemoveAt(0);


                    string json = JsonSerializerHelper.EntityToJson(ls);

                    string s = PackJsonListResultForArray("true", json, string.Empty, ls.Count);

                    return s;
                }
                catch (Exception ex)
                {
                    return PackJsonListResultForArray("false", "[]", ex.Message, 0);
                }
            }

        }

        /// <summary>
        /// 通过hql查询一个分页数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public virtual string GetListByHQLWithPaging(string condition)
        {
            ISession sess = SessionFactory.Session;

            try
            {
                //将查询参数Json转换成对象
                QueryCondition qc = JsonConvert.DeserializeObject<QueryCondition>(condition);
                //查询行记录数综合
                long total = sess.CreateQuery(qc.TotalHQL).UniqueResult<long>();
                //查询
                //qc.HQL = "from Case";
                IQuery q = sess.CreateQuery(qc.HQL);
                //设置分页
                IList<SMSRecord> ls = q.SetFirstResult(qc.FirstResult).SetMaxResults(qc.MaxResults).List<SMSRecord>();
                //IList<T> ls = q.List<T>();

                string json = JsonSerializerHelper.EntityToJson(ls);

                return PackJsonListResult("true", json, string.Empty, total);

            }
            catch (Exception ex)
            {
                return PackJsonListResult("false", "[]", ex.Message, 0);
            }
            finally
            {
                sess.Close();
            }
        }

        public virtual string Export(string query)
        {
            using (ISession sess = HbmSessionFactory.OpenSession())
            {
                try
                {
                    IList<SMSRecord> ls = sess.CreateQuery(query).List<SMSRecord>();
                    const int max = 40000;
                    if (ls.Count > max)
                        return string.Format("错误：条数超出上限{0},请重新选择条件", max);
                    ////解决导出excel字符串转换成数字
                    //foreach (var item in ls)
                    //{
                    //    item.PersonName = string.Format(@"=T(""{0}"")", item.PersonName);
                    //    item.WorkContent = string.Format(@"=T(""{0}"")", item.WorkContent);
                    //    item.WorkNo = string.Format(@"=T(""{0}"")", item.WorkNo);
                    //    item.PersonMobile = string.Format(@"=T(""{0}"")", item.PersonMobile);
                    //    item.Address = string.Format(@"=T(""{0}"")", item.Address);
                    //}

                    var obj = (from item in ls
                               select new
                                   {
                                       PersonName = item.PersonName,
                                       WorkTypeName = item.WorkType == null ? "" : item.WorkType.Name,
                                       WorkContent = item.WorkContent,
                                       WorkOfficerName = item.WorkOfficer == null ? "" : item.WorkOfficer.Name,
                                       LeaderName = item.Leader == null ? "" : item.Leader.Name,
                                       WorkDate = item.WorkDate,
                                       WorkNo = item.WorkNo,
                                       OrganizationName = item.Organization == null ? "" : item.Organization.Name,
                                       GradeTypeScore = item.GradeType == null ? 0 : item.GradeType.Score,
                                       IsResponseStr = item.IsResponseStr,
                                       PersonMobile = item.PersonMobile,
                                       Address = item.Address
                                   }).ToList();

                    GridView gv = new GridView();
                    gv.AutoGenerateColumns = false;
                    gv.Columns.Add(new BoundField() { HeaderText = "姓名", DataField = "PersonName" });
                    gv.Columns.Add(new BoundField() { HeaderText = "办事内容", DataField = "WorkTypeName" });
                    gv.Columns.Add(new BoundField() { HeaderText = "办事详情", DataField = "WorkContent" });
                    gv.Columns.Add(new BoundField() { HeaderText = "受理人", DataField = "WorkOfficerName" });
                    gv.Columns.Add(new BoundField() { HeaderText = "值班领导", DataField = "LeaderName" });
                    gv.Columns.Add(new BoundField() { HeaderText = "登记时间", DataField = "WorkDate" });
                    gv.Columns.Add(new BoundField() { HeaderText = "登记流水号", DataField = "WorkNo" });
                    gv.Columns.Add(new BoundField() { HeaderText = "单位", DataField = "OrganizationName" });
                    gv.Columns.Add(new BoundField() { HeaderText = "评价得分", DataField = "GradeTypeScore" });
                    gv.Columns.Add(new BoundField() { HeaderText = "是否评价", DataField = "IsResponseStr" });
                    gv.Columns.Add(new BoundField() { HeaderText = "办事人电话", DataField = "PersonMobile" });
                    gv.Columns.Add(new BoundField() { HeaderText = "办事人地址", DataField = "Address" });

                    gv.DataSource = obj;
                    gv.DataBind();

                    var request = System.Web.HttpContext.Current.Request;
                    string fileName = Guid.NewGuid().ToString() + ".xls";
                    string url = string.Format("http://{0}/ClientBin/Files/{1}", request.Url.Authority, fileName);
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                        {
                            gv.RenderControl(htw);
                            string physicalAddress = string.Format("{0}ClientBin/Files/{1}", System.Web.Hosting.HostingEnvironment.MapPath("~"), fileName);
                            FileInfo fi = new FileInfo(physicalAddress);
                            if (!fi.Directory.Exists)
                                Directory.CreateDirectory(fi.Directory.FullName);
                            File.WriteAllText(physicalAddress, sw.ToString());
                        }
                    }

                    return url;
                }
                catch (Exception ex)
                {
                    return PackJsonListResult("false", "[]", ex.Message, 0);
                }
            }

        }

        /// <summary>
        /// 暂不使用
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public virtual string GetListByProperties(string properties)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 打包Json返回格式
        /// </summary>
        /// <param name="success"></param>
        /// <param name="json"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected string PackJsonResult(string success, string json, string message)
        {
            return string.Format("{0} 'Success': '{1}', 'Data': {2},'Message': '{3}'{4}", "{", success, json, message, "}");
        }
        /// <summary>
        /// 打包Json返回列表格式
        /// </summary>
        /// <param name="success"></param>
        /// <param name="json"></param>
        /// <param name="message"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        protected string PackJsonListResult(string success, string json, string message, long total)
        {
            //"{ 'Success': {0}, 'Data': {1},'Message':{2},'Total':{3}}"
            return string.Format("{0}'Success':'{1}','Data':{2},'Message':'{3}','Total':{4}{5}", "{", success, json, message, total.ToString(), "}");
        }

        protected string PackJsonListResultForArray(string success, string json, string message, long total)
        {
            //"{ 'Success': {0}, 'Data': {1},'Message':{2},'Total':{3}}"
            return string.Format("{0}'Success':'{1}','Datas':{2},'Message':'{3}','Total':{4}{5}", "{", success, json, message, total.ToString(), "}");
        }

        #endregion
    }
}
