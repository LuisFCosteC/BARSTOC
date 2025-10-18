using BARSTOC_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_BLL.Servicios.Contrato
{
    public interface IUsuariosService
    {
        Task<List<DTO_Usuarios>> List();
        Task<List<DTO_Usuarios>> ListBySede(int idSede);
        Task<List<DTO_Usuarios>> ListByRol(int idRol);
        Task<DTO_Usuarios> Get(int id);
        Task<DTO_Usuarios> Create(DTO_Usuarios modelo);
        Task<bool> Edit(DTO_Usuarios modelo);
        Task<bool> ChangeEstado(int idUsuario, string nuevoEstado);
        Task<bool> ChangePassword(int idUsuario, string nuevaPassword);
        Task<bool> ResetPassword(int idUsuario);
    }
}