using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Core.MISAAttributes
{
    /// <summary>
    /// @author: Vũ Quang Phong (26/01/2022)
    /// @desc: Marking the Properties that are not allowed Empty
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotEmpty : Attribute
    {
    }

    /// <summary>
    /// @author: Vũ Quang Phong (26/01/2022)
    /// @desc: Marking the Properties that are not allowed Duplicated
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotDuplicated : Attribute
    {
    }

    /// <summary>
    /// @author: Vũ Quang Phong (26/01/2022)
    /// @desc: Marking the Vietnamese Name of Properties
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropsName : Attribute
    {
        public string Name { get; set; }

        public PropsName(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// @author: Vũ Quang Phong (11/02/2022)
    /// @desc: Prop for Format Code
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FormatCode : Attribute
    {
    }

    /// <summary>
    /// @author: Vũ Quang Phong (11/02/2022)
    /// @desc: Prop for Format Email
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FormatEmail : Attribute
    {
    }

    /// <summary>
    /// @author: Vũ Quang Phong (11/02/2022)
    /// @desc: Prop for Format Date
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FormatDate : Attribute
    {
    }
}
