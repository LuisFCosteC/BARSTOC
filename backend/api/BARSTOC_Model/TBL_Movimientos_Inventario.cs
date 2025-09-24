using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class TBL_Movimientos_Inventario
{
    public int IdMovimientoInventario { get; set; }

    public int IdSede { get; set; }

    public int IdProducto { get; set; }

    public string? TipoMovimiento { get; set; }

    public int Cantidad { get; set; }

    public int CantidadAnterior { get; set; }

    public int CantidadNueva { get; set; }

    public int IdUsuario { get; set; }

    public DateTime? FechaMovimiento { get; set; }

    public string? Observaciones { get; set; }

    public virtual TBL_Producto IdProductoNavigation { get; set; } = null!;

    public virtual TBL_Sedes IdSedeNavigation { get; set; } = null!;

    public virtual TBL_Usuarios IdUsuarioNavigation { get; set; } = null!;
}
