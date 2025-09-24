using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class VwProductosPreciosFormateado
{
    public int IdProducto { get; set; }

    public string NombreProducto { get; set; } = null!;

    public string NombreCategoria { get; set; } = null!;

    public decimal Costo { get; set; }

    public decimal PrecioVenta { get; set; }

    public string? CostoFormateado { get; set; }

    public string? PrecioVentaFormateado { get; set; }

    public decimal? Margen { get; set; }

    public string? MargenFormateado { get; set; }

    public string? Estado { get; set; }
}
