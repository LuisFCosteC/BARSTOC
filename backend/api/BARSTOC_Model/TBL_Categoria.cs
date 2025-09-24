using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class TBL_Categoria
{
    public int IdCategoria { get; set; }

    public string NombreCategoria { get; set; } = null!;

    public string? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<TBL_Producto> TblProductos { get; set; } = new List<TBL_Producto>();
}
