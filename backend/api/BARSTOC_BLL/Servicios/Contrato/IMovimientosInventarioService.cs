using BARSTOC_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_BLL.Servicios.Contrato
{
    public interface IMovimientosInventarioService
    {
        Task<List<DTO_Movimientos_Inventario>> List();
        Task<List<DTO_Movimientos_Inventario>> ListBySede(int idSede);
        Task<DTO_Movimientos_Inventario> Create(DTO_Movimientos_Inventario modelo);
        Task<List<DTO_Movimientos_Inventario>> GetByProducto(int idProducto);
    }
}
