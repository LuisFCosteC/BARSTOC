using BARSTOC_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_BLL.Servicios.Contrato
{
    public interface IProductoService
    {
        Task<List<DTO_Producto>> List();
        Task<List<DTO_Producto>> ListByCategoria(int idCategoria);
        Task<DTO_Producto> Get(int id);
        Task<DTO_Producto> Create(DTO_Producto modelo);
        Task<bool> Edit(DTO_Producto modelo);
        Task<bool> ChangeEstado(int idProducto, string nuevoEstado);
    }
}
