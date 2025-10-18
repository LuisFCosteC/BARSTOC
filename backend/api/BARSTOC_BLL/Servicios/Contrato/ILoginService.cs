using BARSTOC_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_BLL.Servicios.Contrato
{
    public interface ILoginService
    {
        Task<DTO_LoginResponse> ValidateCredentials(string usuarioLogin, string password);
        Task<bool> ChangePassword(int idUsuario, string nuevaPassword);
    }
}