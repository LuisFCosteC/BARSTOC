using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_DTO
{
    public class DTO_Reporte
    {
        public string? NumeroPedido { get; set; }

        public string? TipoPago { get; set; }

        public string? FechaRegistro { get; set; }

        public string? PedidosTotales { get; set; }

        public string? Producto { get; set; }

        public int? Cantidad { get; set; }

        public string? Precio { get; set; }

        public string? Total { get; set; }
    }
}
