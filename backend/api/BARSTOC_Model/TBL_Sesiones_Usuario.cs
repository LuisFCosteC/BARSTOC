using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class TBL_Sesiones_Usuario
{
    public string IdSesionUsuario { get; set; } = null!;

    public int IdUsuario { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaUltimaActividad { get; set; }

    public string? UserAgent { get; set; }

    public string? Estado { get; set; }

    public virtual TBL_Usuarios IdUsuarioNavigation { get; set; } = null!;
}
