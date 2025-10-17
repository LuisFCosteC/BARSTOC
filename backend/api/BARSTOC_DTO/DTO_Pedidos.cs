using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_DTO
{
    public class DTO_Pedidos
    {
        public int IdPedido { get; set; }

        public int IdMesa { get; set; }

        public int IdUsuarioMesero { get; set; }

        public int IdSede { get; set; }

        public string NumeroPedido { get; set; } = null!;

        public string? EstadoPedido { get; set; }

        public DateTime? FechaApertura { get; set; }

        public DateTime? FechaCierre { get; set; }

        public decimal? TotalPedido { get; set; }

        public string? MetodoPago { get; set; }

        public string? Observaciones { get; set; }
    }
}
