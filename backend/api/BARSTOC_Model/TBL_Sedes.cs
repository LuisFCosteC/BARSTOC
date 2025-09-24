using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class TBL_Sedes
{
    public int IdSede { get; set; }

    public string NombreSede { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<TBL_Inventario> TblInventarios { get; set; } = new List<TBL_Inventario>();

    public virtual ICollection<TBL_Mesas> TblMesas { get; set; } = new List<TBL_Mesas>();

    public virtual ICollection<TBL_Movimientos_Inventario> TblMovimientosInventarios { get; set; } = new List<TBL_Movimientos_Inventario>();

    public virtual ICollection<TBL_Pedidos> TblPedidos { get; set; } = new List<TBL_Pedidos>();

    public virtual ICollection<TBL_Usuarios> TblUsuarios { get; set; } = new List<TBL_Usuarios>();
}
