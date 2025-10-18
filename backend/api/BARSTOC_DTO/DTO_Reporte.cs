using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_DTO
{
    public class DTO_Reporte
    {
        public string? Fecha { get; set; }
        public string NumeroPedido { get; set; } = null!;
        public string Sede { get; set; } = null!;
        public string NombreProducto { get; set; } = null!;
        public string NombreCategoria { get; set; } = null!;
        public int Cantidad { get; set; }
        public string? CostoTexto { get; set; }
        public string? PrecioVentaTexto { get; set; }
        public string? MargenGananciaTexto { get; set; }
        public string? GananciaTotalTexto { get; set; }
        public string? SubtotalTexto { get; set; }
        public string? TotalPedidoTexto { get; set; }
        public string Mesero { get; set; } = null!;
    }
}