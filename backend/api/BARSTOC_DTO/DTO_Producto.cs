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
        public string? DescripcionCategoria { get; set; }
        public string? CostoTexto { get; set; }
        public string? PrecioVentaTexto { get; set; }
        public string? Estado { get; set; }
    }
}