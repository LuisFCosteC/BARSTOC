using BARSTOC_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_BLL.Servicios.Contrato
{
    public interface IPedidosService
    {
        Task<DTO_Pedidos> Register(DTO_Pedidos modelo);
        Task<List<DTO_Pedidos>> History(string buscarPor, string numeroPedido, string fechaInicio, string fechaFin);
        Task<List<DTO_Pedidos>> GetActiveBySede(int idSede);
        Task<DTO_Pedidos> Get(int idPedido);
        Task<bool> UpdateEstado(int idPedido, string nuevoEstado);
        Task<bool> CancelarPedido(int idPedido, string metodoPago);
        Task<bool> ClosePedido(int idPedido, string metodoPago);
    }
}