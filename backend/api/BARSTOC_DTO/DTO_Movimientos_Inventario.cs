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
        public string? NombreSede { get; set; }
        public int IdProducto { get; set; }
        public string? NombreProducto { get; set; }
        public string? TipoMovimiento { get; set; }
        public int Cantidad { get; set; }
        public int CantidadAnterior { get; set; }
        public int CantidadNueva { get; set; }
        public int IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public DateTime? FechaMovimiento { get; set; }
        public string? Observaciones { get; set; }
    }
}