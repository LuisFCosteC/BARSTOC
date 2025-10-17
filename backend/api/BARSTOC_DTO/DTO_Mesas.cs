using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_DTO
{
    public class DTO_Mesas
    {
        public int IdMesa { get; set; }

        public int IdSede { get; set; }

        public string NumeroMesa { get; set; } = null!;

        public int? Capacidad { get; set; }

        public string? Estado { get; set; }
    }
}
