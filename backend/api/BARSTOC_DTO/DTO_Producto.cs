using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_DTO
{
    public class DTO_Producto
    {
        public int IdProducto { get; set; }

        public string NombreProducto { get; set; } = null!;

        public int IdCategoria { get; set; }

        public decimal Costo { get; set; }

        public decimal PrecioVenta { get; set; }

        public string? Estado { get; set; }

        public DateTime? FechaCreacion { get; set; }
    }
}
