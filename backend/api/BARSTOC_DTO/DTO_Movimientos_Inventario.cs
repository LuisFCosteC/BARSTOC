using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_DTO
{
    public class DTO_Movimientos_Inventario
    {
        public int IdMovimientoInventario { get; set; }

        public int IdSede { get; set; }

        public int IdProducto { get; set; }

        public string? TipoMovimiento { get; set; }

        public int Cantidad { get; set; }

        public int CantidadAnterior { get; set; }

        public int CantidadNueva { get; set; }

        public int IdUsuario { get; set; }

    }
}
