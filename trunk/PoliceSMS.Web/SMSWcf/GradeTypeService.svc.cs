using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NHibernate;
using PoliceSMS.Web.Comm;
using Newtonsoft.Json;
using PoliceSMS.Lib.Query;
using PoliceSMS.Web.Serializable;
using PoliceSMS.Lib.SMS;
using System.ServiceModel.Activation;

namespace PoliceSMS.Web.SMSWcf
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“GradeTypeService”。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class GradeTypeService : IGradeTypeService
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

                //DunLibrary.User.User u = sess.Get<DunLibrary.User.User>(2);

                //DunLibrary.Dun.VisitRecord vr = entity as DunLibrary.Dun.VisitRecord;

                //vr.Visitors.Add(u);

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
                    GradeType entity = sess.Get<GradeType>(id);

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
                    GradeType entity = sess.CreateQuery(hql).UniqueResult<GradeType>();

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
                    IList<GradeType> ls = sess.CreateQuery(hql).List<GradeType>();

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
                IList<GradeType> ls = q.SetFirstResult(qc.FirstResult).SetMaxResults(qc.MaxResults).List<GradeType>();
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
