using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoliceSMS.Lib.Core
{
    public interface IItem
    {
        int Id { get; set; }

        string Name { get; set; }

        string Number { get; set; }

        Guid GUID { get; set; }

        string Description { get; set; }

        bool IsUsed { get; set; }


    }
}
