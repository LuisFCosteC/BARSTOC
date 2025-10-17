using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_DTO
{
    public class DTO_Inventario
    {
        public int IdInventario { get; set; }

        public int IdSede { get; set; }

        public int IdProducto { get; set; }

        public int CantidadDisponible { get; set; }
    }
}
