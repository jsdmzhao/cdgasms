using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NHibernate;

namespace PoliceSMS.Web.SMSWcf
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IWorkTypeService”。
    [ServiceContract]
    public interface IWorkTypeService
    {
        /// <summary>
        /// Nhibernate的Session工厂
        /// </summary>
        ISessionFactory HbmSessionFactory { get; set; }
        /// <summary>
        /// 保存实体对象
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        [OperationContract]
        string SaveOrUpdate(string json);
        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        string DeleteById(int id);
        /// <summary>
        /// 通过id返回一个实体类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        string GetById(int id);
        /// <summary>
        /// 通过一个hql返回单个对象
        /// </summary>
        /// <param name="hql"></param>
        /// <returns></returns>
        [OperationContract]
        string GetByHql(string hql);
        /// <summary>
        /// 通过一个hql查询获得一个实体类集合
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="hql"></param>
        /// <returns></returns>
        [OperationContract]
        string GetListByHQL(string hqlQuery);
        /// <summary>
        /// 通过一个sql查询获得一个object[]集合
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [OperationContract]
        string GetListBySQL(string sql);
        /// <summary>
        /// 通过一个hql获得一个分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [OperationContract]
        string GetListByHQLWithPaging(string query);
        /// <summary>
        /// 通过属性组合获得一个实体类集合
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        [OperationContract]
        string GetListByProperties(string properties);
    }
}
