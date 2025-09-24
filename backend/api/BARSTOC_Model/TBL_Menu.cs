using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class TBL_Menu
{
    public int IdMenu { get; set; }

    public string NombreMenu { get; set; } = null!;

    public string? Icono { get; set; }

    public string? Url { get; set; }

    public virtual ICollection<TBL_MenuRol> TblMenuRols { get; set; } = new List<TBL_MenuRol>();
}
