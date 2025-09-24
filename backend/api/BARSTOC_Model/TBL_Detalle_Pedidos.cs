using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class TBL_Detalle_Pedidos
{
    public int IdDetallePedido { get; set; }

    public int IdPedido { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual TBL_Pedidos IdPedidoNavigation { get; set; } = null!;

    public virtual TBL_Producto IdProductoNavigation { get; set; } = null!;
}
