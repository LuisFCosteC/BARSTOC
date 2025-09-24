using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class VwInventarioActual
{
    public string NombreSede { get; set; } = null!;

    public string NombreProducto { get; set; } = null!;

    public string NombreCategoria { get; set; } = null!;

    public int CantidadDisponible { get; set; }

    public DateTime? UltimaActualizacion { get; set; }

    public decimal PrecioVenta { get; set; }

    public decimal Costo { get; set; }
}
