using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PoliceSMS.Web.Report
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IReportWcf”。
    [ServiceContract]
    public interface IReportWcf
    {
        [OperationContract]
        string LoadStationReportResult(int UnitType, DateTime beginTime1, DateTime endTime1, DateTime beginTime2, DateTime endTime2);

        [OperationContract]
        string LoadOfficerReportResult(int UnitId, DateTime beginTime1, DateTime endTime1, DateTime beginTime2, DateTime endTime2);

    }
}
