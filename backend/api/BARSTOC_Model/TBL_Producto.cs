using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class TBL_Producto
{
    public int IdProducto { get; set; }

    public string NombreProducto { get; set; } = null!;

    public int IdCategoria { get; set; }

    public decimal Costo { get; set; }

    public decimal PrecioVenta { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual TBL_Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<TBL_Detalle_Pedidos> TblDetallePedidos { get; set; } = new List<TBL_Detalle_Pedidos>();

    public virtual ICollection<TBL_Inventario> TblInventarios { get; set; } = new List<TBL_Inventario>();

    public virtual ICollection<TBL_Movimientos_Inventario> TblMovimientosInventarios { get; set; } = new List<TBL_Movimientos_Inventario>();
}
