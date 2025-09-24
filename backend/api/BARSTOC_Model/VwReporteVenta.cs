using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class VwReporteVenta
{
    public DateTime? Fecha { get; set; }

    public string NumeroPedido { get; set; } = null!;

    public string Sede { get; set; } = null!;

    public string NombreProducto { get; set; } = null!;

    public string NombreCategoria { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal Costo { get; set; }

    public decimal PrecioVenta { get; set; }

    public decimal? MargenGanancia { get; set; }

    public decimal? GananciaTotal { get; set; }

    public decimal Subtotal { get; set; }

    public decimal? TotalPedido { get; set; }

    public string Mesero { get; set; } = null!;
}
