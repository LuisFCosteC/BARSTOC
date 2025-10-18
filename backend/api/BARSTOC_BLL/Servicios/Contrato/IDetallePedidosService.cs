using BARSTOC_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_BLL.Servicios.Contrato
{
    public interface IDetallePedidosService
    {
        Task<List<DTO_Detalle_Pedidos>> List();
        Task<DTO_Detalle_Pedidos> Create(DTO_Detalle_Pedidos modelo);
        Task<bool> Edit(DTO_Detalle_Pedidos modelo);
        Task<bool> Delete(int id);
    }
}
