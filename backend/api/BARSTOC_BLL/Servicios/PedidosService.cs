using AutoMapper;
using BARSTOC_BLL.Servicios.Contrato;
using BARSTOC_DAL.Repositorios.Contrato;
using BARSTOC_DTO;
using BARSTOC_Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_BLL.Servicios
{
    public class PedidosService : IPedidosService
    {
        private readonly IPedidoRepository _pedidosRepository;
        private readonly IGenericRepository<TBL_Detalle_Pedidos> _detallePedidosRepository;
        private readonly IGenericRepository<TBL_Mesas> _mesasRepository;
        private readonly IGenericRepository<TBL_Inventario> _inventarioRepository;
        private readonly IMapper _mapper;

        public PedidosService(
            IPedidoRepository pedidosRepository,
            IGenericRepository<TBL_Detalle_Pedidos> detallePedidosRepository,
            IGenericRepository<TBL_Mesas> mesasRepository,
            IGenericRepository<TBL_Inventario> inventarioRepository,
            IMapper mapper)
        {
            _pedidosRepository = pedidosRepository;
            _detallePedidosRepository = detallePedidosRepository;
            _mesasRepository = mesasRepository;
            _inventarioRepository = inventarioRepository;
            _mapper = mapper;
        }

        public async Task<DTO_Pedidos> Register(DTO_Pedidos modelo)
        {
            try
            {
                // ✅ CORREGIDO: Usar Registro en lugar de Register
                var pedidoGenerado = await _pedidosRepository.Registro(_mapper.Map<TBL_Pedidos>(modelo));

                if (pedidoGenerado.IdPedido == 0)
                    throw new TaskCanceledException("No se pudo crear el pedido");

                return _mapper.Map<DTO_Pedidos>(pedidoGenerado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CancelarPedido(int idPedido, string metodoPago)
        {
            try
            {
                // ✅ CORREGIDO: Usar Obtener en lugar de Get
                var pedidoEncontrado = await _pedidosRepository.Obtener(p => p.IdPedido == idPedido);

                if (pedidoEncontrado == null)
                    throw new TaskCanceledException("El pedido no existe");

                pedidoEncontrado.EstadoPedido = "Cancelado";
                pedidoEncontrado.MetodoPago = metodoPago;
                pedidoEncontrado.FechaCierre = DateTime.Now;

                // ✅ CORREGIDO: Usar Editar en lugar de Edit
                bool respuesta = await _pedidosRepository.Editar(pedidoEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo cancelar el pedido");

                // Actualizar estado de la mesa a "Libre"
                await ActualizarEstadoMesa(pedidoEncontrado.IdMesa, "Libre");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ClosePedido(int idPedido, string metodoPago)
        {
            try
            {
                // ✅ CORREGIDO: Usar Obtener en lugar de Get
                var pedidoEncontrado = await _pedidosRepository.Obtener(p => p.IdPedido == idPedido);

                if (pedidoEncontrado == null)
                    throw new TaskCanceledException("El pedido no existe");

                pedidoEncontrado.EstadoPedido = "Cerrado";
                pedidoEncontrado.MetodoPago = metodoPago;
                pedidoEncontrado.FechaCierre = DateTime.Now;

                // ✅ CORREGIDO: Usar Editar en lugar de Edit
                bool respuesta = await _pedidosRepository.Editar(pedidoEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo cerrar el pedido");

                // Actualizar estado de la mesa a "Libre"
                await ActualizarEstadoMesa(pedidoEncontrado.IdMesa, "Libre");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        private async Task<string> GenerarNumeroPedido()
        {
            // ✅ CORREGIDO: Ahora el método existe en el repositorio
            var ultimoNumero = await _pedidosRepository.GetUltimoNumeroPedido();
            return $"PED-{ultimoNumero + 1:000000}";
        }

        private async Task ActualizarEstadoMesa(int idMesa, string estado)
        {
            // ✅ CORREGIDO: Usar Obtener y Editar
            var mesaEncontrada = await _mesasRepository.Obtener(m => m.IdMesa == idMesa);
            if (mesaEncontrada != null)
            {
                mesaEncontrada.Estado = estado;
                await _mesasRepository.Editar(mesaEncontrada);
            }
        }

        private async Task ActualizarInventario(int idPedido)
        {
            // ✅ CORREGIDO: Usar Consultar, Obtener y Editar
            var detalles = await _detallePedidosRepository.Consultar(d => d.IdPedido == idPedido);

            foreach (var detalle in detalles)
            {
                var inventario = await _inventarioRepository.Obtener(i =>
                    i.IdProducto == detalle.IdProducto &&
                    i.IdSede == detalle.IdPedidoNavigation.IdSede);

                if (inventario != null)
                {
                    inventario.CantidadDisponible -= detalle.Cantidad;
                    inventario.UltimaActualizacion = DateTime.Now;
                    await _inventarioRepository.Editar(inventario);
                }
            }
        }

        // ✅ CORREGIDO: Implementar métodos faltantes
        public async Task<List<DTO_Pedidos>> History(string buscarPor, string numeroPedido, string fechaInicio, string fechaFin)
        {
            try
            {
                IQueryable<TBL_Pedidos> query = await _pedidosRepository.Consultar();
                var listaResultado = new List<TBL_Pedidos>();

                if (buscarPor == "fecha")
                {
                    DateTime fecha_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-CO"));
                    DateTime fecha_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-CO"));

                    listaResultado = await query
                        .Where(v =>
                            v.FechaApertura.Value.Date >= fecha_Inicio.Date &&
                            v.FechaApertura.Value.Date <= fecha_Fin.Date)
                        .Include(ds => ds.TblDetallePedidos)
                        .ThenInclude(p => p.IdProductoNavigation)
                        .ToListAsync();
                }
                else
                {
                    listaResultado = await query
                        .Where(v => v.NumeroPedido == numeroPedido)
                        .Include(ds => ds.TblDetallePedidos)
                        .ThenInclude(p => p.IdProductoNavigation)
                        .ToListAsync();
                }

                return _mapper.Map<List<DTO_Pedidos>>(listaResultado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<DTO_Pedidos>> GetActiveBySede(int idSede)
        {
            try
            {
                var query = await _pedidosRepository.Consultar(p =>
                    p.IdSede == idSede &&
                    p.EstadoPedido == "activo");

                var listaPedidos = query
                    .Include(p => p.TblDetallePedidos)
                    .ThenInclude(d => d.IdProductoNavigation)
                    .Include(p => p.IdMesaNavigation)
                    .Include(p => p.IdUsuarioMeseroNavigation)
                    .ToList();

                return _mapper.Map<List<DTO_Pedidos>>(listaPedidos);
            }
            catch
            {
                throw;
            }
        }

        public async Task<DTO_Pedidos> Get(int idPedido)
        {
            try
            {
                var pedido = await _pedidosRepository.Obtener(p => p.IdPedido == idPedido);

                if (pedido == null)
                    throw new TaskCanceledException("El pedido no existe");

                var query = await _pedidosRepository.Consultar(p => p.IdPedido == idPedido);
                pedido = query
                    .Include(p => p.TblDetallePedidos)
                    .ThenInclude(d => d.IdProductoNavigation)
                    .Include(p => p.IdMesaNavigation)
                    .Include(p => p.IdUsuarioMeseroNavigation)
                    .FirstOrDefault();

                return _mapper.Map<DTO_Pedidos>(pedido);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateEstado(int idPedido, string nuevoEstado)
        {
            try
            {
                var pedidoEncontrado = await _pedidosRepository.Obtener(p => p.IdPedido == idPedido);

                if (pedidoEncontrado == null)
                    throw new TaskCanceledException("El pedido no existe");

                pedidoEncontrado.EstadoPedido = nuevoEstado;

                bool respuesta = await _pedidosRepository.Editar(pedidoEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo actualizar el estado del pedido");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}