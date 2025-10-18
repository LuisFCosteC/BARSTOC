using BARSTOC_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_BLL.Servicios.Contrato
{
    public interface IMesasService
    {
        Task<List<DTO_Mesas>> List();
        Task<List<DTO_Mesas>> ListBySede(int idSede);
        Task<DTO_Mesas> Get(int id);
        Task<DTO_Mesas> Create(DTO_Mesas modelo);
        Task<bool> Edit(DTO_Mesas modelo);
        Task<bool> ChangeEstado(int idMesa, string nuevoEstado);
    }
}
