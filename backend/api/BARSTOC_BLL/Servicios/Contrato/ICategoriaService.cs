using BARSTOC_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_BLL.Servicios.Contrato
{
    public interface ICategoriaService
    {
        Task<List<DTO_Categoria>> List();
        Task<DTO_Categoria> Create(DTO_Categoria modelo);
        Task<bool> Edit(DTO_Categoria modelo);
        Task<bool> Delete(int id);
    }
}
