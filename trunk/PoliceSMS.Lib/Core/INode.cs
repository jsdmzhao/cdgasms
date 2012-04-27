using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoliceSMS.Lib.Core
{
    public interface INode<T>
    {
        /// <summary>
        /// 父节点
        /// </summary>
        T Parent { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        IList<T> Childs { get; set; }
    }
}
