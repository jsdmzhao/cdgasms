using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Vs08SMSService
{
    /// <summary>
    /// 实现写日志的类
    /// 时间：2008-3-17
    /// </summary>
    public class Log
    {
        private string log_file_path = null;
        /// <summary>
        /// 获取或设置日志文件的路径
        /// </summary>
        public string FilePath
        {
            get
            {
                return log_file_path;
            }
            set
            {
                log_file_path = value;
            }
        }
        public Log(string filePath)
        {
            log_file_path = filePath;
        }
        public Log()
        {
            log_file_path = null;
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public void WriteLog(string log)
        {
            StreamWriter sw = null;
            try
            {
                if (log_file_path != null)
                {
                    sw = new StreamWriter(log_file_path, true, System.Text.Encoding.Default);
                    sw.WriteLine(System.DateTime.Now.ToShortTimeString() + " : " + log);
                    sw.Close();
                    sw = null;
                }
            }
            catch (Exception) { }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }
    }

    public class ShortUnit
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
