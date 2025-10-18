using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_DTO
{
    public class DTO_Dash_Board
    {
        public int? TotalPedidos { get; set; }
        public string? TotalIngresos { get; set; }
        public int TotalProductos { get; set; }
        public List<DTO_Pedidos_Semanales>? UltimasSemanasPedidos { get; set; }
    }
}