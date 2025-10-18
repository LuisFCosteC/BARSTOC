using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_DTO
{
    public class DTO_Usuarios
    {
        public int IdUsuario { get; set; }
        public string NumeroDocumento { get; set; } = null!;
        public int IdSede { get; set; }
        public string? NombreSede { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string ApellidoUsuario { get; set; } = null!;
        public string correo { get; set; } = null!;
        public int IdRol { get; set; }
        public string? DescripcionRol { get; set; }
        public string UsuarioLogin { get; set; } = null!;
        public string? Estado { get; set; }
        public string? PasswordTemporal { get; set; }
        public string? PasswordPersonalizada { get; set; }
    }
}