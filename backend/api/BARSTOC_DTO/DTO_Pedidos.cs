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
        public string? NumeroMesa { get; set; }
        public int IdUsuarioMesero { get; set; }
        public string? NombreMesero { get; set; }
        public int IdSede { get; set; }
        public string? NombreSede { get; set; }
        public string NumeroPedido { get; set; } = null!;
        public string? EstadoPedido { get; set; }
        public string? FechaAperturaTexto { get; set; }
        public string? FechaCierreTexto { get; set; }
        public string? TotalPedidoTexto { get; set; }
        public string? MetodoPago { get; set; }
        public string? Observaciones { get; set; }
        public virtual ICollection<DTO_Detalle_Pedidos>? DetallePedidos { get; set; }
    }
}