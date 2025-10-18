using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_DTO
{
    public class DTO_Categoria
    {
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; } = null!;
        public string? Estado { get; set; }
    }
}