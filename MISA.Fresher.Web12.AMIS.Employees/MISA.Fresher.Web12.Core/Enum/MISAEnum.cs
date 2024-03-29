﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Core.Enum
{
    /// <summary>
    /// @desc: Enum Gender
    /// @author: VQPhong (28/01/2022)
    /// </summary>
    public enum Gender
    {
        Female = 0,
        Male = 1,
    }

    /// <summary>
    /// @desc: Enum Marital Status
    /// @author: VQPhong (28/01/2022)
    /// </summary>
    public enum MaritalStatus
    {
        Single = 0,
        Married = 1,
        Divorced = 2,
    }

    /// <summary>
    /// @desc: Enum Work Status
    /// @author: VQPhong (28/01/2022)
    /// </summary>
    public enum WorkStatus
    {
        Working = 0,
        Stopping = 1,
    }

    /// <summary>
    /// @desc: Enum Customer or Supplier
    /// @author: VQPhong (28/01/2022)
    /// </summary>
    public enum CustomerOrSupplier
    {
        NotBoth = 0,
        Customer = 1,
        Supplier = 2,
        Both = 3,
    }

    /// <summary>
    /// @desc: Enum Self-defined Status Code
    /// @author: VQPhong (08/06/2022)
    /// </summary>
    public enum CustomizeStatusCode
    {
        GetOkay = 10,
        Created = 11,
        Updated = 12,
        Deleted = 13,
        NoContent = 14,
        BadRequest = 40,
        NormalException = 55,
    }
}
