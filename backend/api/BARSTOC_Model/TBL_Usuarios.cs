using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class TBL_Usuarios
{
    public int IdUsuario { get; set; }

    public string NumeroDocumento { get; set; } = null!;

    public int IdSede { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string ApellidoUsuario { get; set; } = null!;

    public string correo { get; set; } = null!;

    public int IdRol { get; set; }

    public string UsuarioLogin { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaUltimoLogin { get; set; }

    public virtual TBL_Roles IdRolNavigation { get; set; } = null!;

    public virtual TBL_Sedes IdSedeNavigation { get; set; } = null!;

    public virtual ICollection<TBL_Movimientos_Inventario> TblMovimientosInventarios { get; set; } = new List<TBL_Movimientos_Inventario>();

    public virtual ICollection<TBL_Pedidos> TblPedidos { get; set; } = new List<TBL_Pedidos>();

    public virtual ICollection<TBL_Sesiones_Usuario> TblSesionesUsuarios { get; set; } = new List<TBL_Sesiones_Usuario>();
}
