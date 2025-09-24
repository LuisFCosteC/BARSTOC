using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class TBL_Mesas
{
    public int IdMesa { get; set; }

    public int IdSede { get; set; }

    public string NumeroMesa { get; set; } = null!;

    public int? Capacidad { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual TBL_Sedes IdSedeNavigation { get; set; } = null!;

    public virtual ICollection<TBL_Pedidos> TblPedidos { get; set; } = new List<TBL_Pedidos>();
}
