using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoliceSMS.Lib.Core
{
    public abstract class Item:IItem
    {
        #region IItem 成员

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Number { get; set; }

        public virtual Guid GUID { get; set; }

        public virtual string Description { get; set; }

        public virtual bool IsUsed { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType())
                return false;

            IItem item = obj as Item;

            if (item.Id == Id)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            string str = GetType().Name + Id + Name;

            return str.GetHashCode();
        }

        #endregion
    }
}
