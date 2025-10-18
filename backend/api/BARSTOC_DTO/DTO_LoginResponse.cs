using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_DTO
{
    public class DTO_LoginResponse
    {
        public DTO_Sesion Sesion { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime Expiracion { get; set; }
    }
}