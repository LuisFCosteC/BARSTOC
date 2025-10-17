using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_DTO
{
    public class DTO_Sesiones_Usuario
    {
        public string IdSesionUsuario { get; set; } = null!;

        public int IdUsuario { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaUltimaActividad { get; set; }

        public string? UserAgent { get; set; }

        public string? Estado { get; set; }
    }
}
