using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_DTO
{
    public class DTO_Menu
    {
        public int IdMenu { get; set; }

        public string NombreMenu { get; set; } = null!;

        public string? Icono { get; set; }

        public string? Url { get; set; }
    }
}
