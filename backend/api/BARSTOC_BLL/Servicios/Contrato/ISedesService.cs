using BARSTOC_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_BLL.Servicios.Contrato
{
    public interface ISedesService
    {
        Task<List<DTO_Sedes>> List();
        Task<DTO_Sedes> Get(int id);
        Task<DTO_Sedes> Create(DTO_Sedes modelo);
        Task<bool> Edit(DTO_Sedes modelo);
        Task<bool> ChangeEstado(int idSede, string nuevoEstado);
    }
}