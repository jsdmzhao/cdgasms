using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoliceSMS.Lib.Core;

namespace PoliceSMS.Lib.Organization
{
    public class Organization:Item,INode<Organization>
    {

        public virtual Organization Parent { get; set; }


        public virtual IList<Organization> Childs { get; set; }


        public virtual int SMSUnitType { get; set; }
    }
}
