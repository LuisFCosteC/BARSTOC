using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class TBL_Inventario
{
    public int IdInventario { get; set; }

    public int IdSede { get; set; }

    public int IdProducto { get; set; }

    public int CantidadDisponible { get; set; }

    public DateTime? UltimaActualizacion { get; set; }

    public virtual TBL_Producto IdProductoNavigation { get; set; } = null!;

    public virtual TBL_Sedes IdSedeNavigation { get; set; } = null!;
}
