using BARSTOC_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_BLL.Servicios.Contrato
{
    public interface IRolesService
    {
        Task<List<DTO_Roles>> List();
        Task<DTO_Roles> Get(int id);
        Task<DTO_Roles> Create(DTO_Roles modelo);
        Task<bool> Edit(DTO_Roles modelo);
        Task<bool> Delete(int id);
    }
}