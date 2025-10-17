using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_DTO
{
    public class DTO_Sedes
    {
        public int IdSede { get; set; }

        public string NombreSede { get; set; } = null!;

        public string? Direccion { get; set; }

        public string? Telefono { get; set; }

        public string? Estado { get; set; }
    }
}
