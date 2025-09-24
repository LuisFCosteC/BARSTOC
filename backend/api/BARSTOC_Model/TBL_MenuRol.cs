using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class TBL_MenuRol
{
    public int IdMenuRol { get; set; }

    public int IdMenu { get; set; }

    public int IdRol { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual TBL_Menu IdMenuNavigation { get; set; } = null!;

    public virtual TBL_Roles IdRolNavigation { get; set; } = null!;
}
