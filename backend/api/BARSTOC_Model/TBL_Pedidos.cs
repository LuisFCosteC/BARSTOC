using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class TBL_Pedidos
{
    public int IdPedido { get; set; }

    public int IdMesa { get; set; }

    public int IdUsuarioMesero { get; set; }

    public int IdSede { get; set; }

    public string NumeroPedido { get; set; } = null!;

    public string? EstadoPedido { get; set; }

    public DateTime? FechaApertura { get; set; }

    public DateTime? FechaCierre { get; set; }

    public decimal? TotalPedido { get; set; }

    public string? MetodoPago { get; set; }

    public string? Observaciones { get; set; }

    public virtual TBL_Mesas IdMesaNavigation { get; set; } = null!;

    public virtual TBL_Sedes IdSedeNavigation { get; set; } = null!;

    public virtual TBL_Usuarios IdUsuarioMeseroNavigation { get; set; } = null!;

    public virtual ICollection<TBL_Detalle_Pedidos> TblDetallePedidos { get; set; } = new List<TBL_Detalle_Pedidos>();
}
