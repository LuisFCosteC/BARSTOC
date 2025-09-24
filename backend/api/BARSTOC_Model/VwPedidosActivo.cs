using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class VwPedidosActivo
{
    public int IdPedido { get; set; }

    public string NumeroPedido { get; set; } = null!;

    public string NumeroMesa { get; set; } = null!;

    public string NombreSede { get; set; } = null!;

    public string Mesero { get; set; } = null!;

    public DateTime? FechaApertura { get; set; }

    public decimal? TotalPedido { get; set; }

    public string? Observaciones { get; set; }
}
