using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class TBL_Roles
{
    public int IdRol { get; set; }

    public string NombreRol { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<TBL_MenuRol> TblMenuRols { get; set; } = new List<TBL_MenuRol>();

    public virtual ICollection<TBL_Usuarios> TblUsuarios { get; set; } = new List<TBL_Usuarios>();
}
