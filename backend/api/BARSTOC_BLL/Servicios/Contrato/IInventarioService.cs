using BARSTOC_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_BLL.Servicios.Contrato
{
    public interface IInventarioService
    {
        Task<List<DTO_Inventario>> List();
        Task<List<DTO_Inventario>> ListBySede(int idSede);
        Task<DTO_Inventario> GetByProductoSede(int idProducto, int idSede);
        Task<DTO_Inventario> Create(DTO_Inventario modelo);
        Task<bool> UpdateStock(int idInventario, int nuevaCantidad);
    }
}
