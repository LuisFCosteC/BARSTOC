using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BARSTOC_Model;

namespace BARSTOC_DAL.Repositorios.Contrato
{
    public interface IPedidoRepository : IGenericRepository<TBL_Pedidos>
    {
        Task<TBL_Pedidos> Registro(TBL_Pedidos modelo);
        Task<int> GetUltimoNumeroPedido();
    }
}