using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BARSTOC_DAL.Repositorios.Contrato;
using BARSTOC_DAL.DBContext;
using BARSTOC_Model;
using Microsoft.EntityFrameworkCore;

namespace BARSTOC_DAL.Repositorios
{
    public class PedidoRepository : GenericRepository<TBL_Pedidos>, IPedidoRepository
    {
        private readonly DbBarstocContext _dbcontext;

        public PedidoRepository(DbBarstocContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<TBL_Pedidos> Registro(TBL_Pedidos modelo)
        {
            TBL_Pedidos pedidoGenerado = new TBL_Pedidos();

            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    // Validar disponibilidad de productos antes de registrar el pedido
                    foreach (var detalle in modelo.TblDetallePedidos)
                    {
                        // Buscar el inventario del producto en la sede correspondiente
                        var inventario = await _dbcontext.TblInventarios
                            .FirstOrDefaultAsync(i =>
                                i.IdSede == modelo.IdSede &&
                                i.IdProducto == detalle.IdProducto);

                        if (inventario == null)
                        {
                            throw new Exception($"Producto con ID {detalle.IdProducto} no encontrado en el inventario de la sede {modelo.IdSede}");
                        }

                        if (inventario.CantidadDisponible < detalle.Cantidad)
                        {
                            // Obtener nombre del producto para el mensaje de error
                            var producto = await _dbcontext.TblProductos
                                .FirstOrDefaultAsync(p => p.IdProducto == detalle.IdProducto);

                            throw new Exception($"Stock insuficiente para el producto '{producto?.NombreProducto}'. " +
                                              $"Disponible: {inventario.CantidadDisponible}, Solicitado: {detalle.Cantidad}");
                        }
                    }

                    // Actualizar inventario - reducir stock de productos
                    foreach (var detalle in modelo.TblDetallePedidos)
                    {
                        var inventario = await _dbcontext.TblInventarios
                            .FirstOrDefaultAsync(i =>
                                i.IdSede == modelo.IdSede &&
                                i.IdProducto == detalle.IdProducto);

                        if (inventario != null)
                        {
                            // Guardar cantidad anterior para el registro de movimiento
                            var cantidadAnterior = inventario.CantidadDisponible;
                            inventario.CantidadDisponible -= detalle.Cantidad;
                            inventario.UltimaActualizacion = DateTime.Now;

                            _dbcontext.TblInventarios.Update(inventario);

                            // Registrar movimiento de inventario
                            var movimiento = new TBL_Movimientos_Inventario
                            {
                                IdSede = modelo.IdSede,
                                IdProducto = detalle.IdProducto,
                                TipoMovimiento = "salida",
                                Cantidad = detalle.Cantidad,
                                CantidadAnterior = cantidadAnterior,
                                CantidadNueva = inventario.CantidadDisponible,
                                IdUsuario = modelo.IdUsuarioMesero,
                                Observaciones = $"Venta por pedido - Mesa: {modelo.IdMesa}",
                                FechaMovimiento = DateTime.Now
                            };

                            await _dbcontext.TblMovimientosInventarios.AddAsync(movimiento);
                        }
                    }

                    await _dbcontext.SaveChangesAsync();

                    // Obtener o crear registro de número de pedido
                    var numeroPedido = await _dbcontext.TblNumeroPedidos.FirstOrDefaultAsync();
                    if (numeroPedido == null)
                    {
                        numeroPedido = new TBL_Numero_Pedido
                        {
                            UltimoNumero = 0,
                            Prefijo = "PED-",
                            FechaCreacion = DateTime.Now
                        };
                        await _dbcontext.TblNumeroPedidos.AddAsync(numeroPedido);
                        await _dbcontext.SaveChangesAsync();
                    }

                    // Generar número de pedido
                    numeroPedido.UltimoNumero += 1;
                    numeroPedido.FechaCreacion = DateTime.Now;
                    _dbcontext.TblNumeroPedidos.Update(numeroPedido);
                    await _dbcontext.SaveChangesAsync();

                    // Formatear número de pedido
                    int numeroDigitos = 6;
                    string ceros = new string('0', numeroDigitos);
                    string numeroPedidoFormateado = ceros + numeroPedido.UltimoNumero.ToString();
                    numeroPedidoFormateado = numeroPedidoFormateado.Substring(numeroPedidoFormateado.Length - numeroDigitos, numeroDigitos);
                    modelo.NumeroPedido = $"{numeroPedido.Prefijo}{numeroPedidoFormateado}";

                    // Configurar valores por defecto del pedido
                    modelo.EstadoPedido = "activo";
                    modelo.FechaApertura = DateTime.Now;
                    modelo.TotalPedido = modelo.TblDetallePedidos.Sum(d => d.Subtotal);
                    modelo.MetodoPago = "efectivo"; // Valor por defecto según requerimientos

                    // Registrar el pedido
                    await _dbcontext.TblPedidos.AddAsync(modelo);
                    await _dbcontext.SaveChangesAsync();

                    // Actualizar estado de la mesa a "ocupada"
                    var mesa = await _dbcontext.TblMesas.FindAsync(modelo.IdMesa);
                    if (mesa != null)
                    {
                        mesa.Estado = "ocupada";
                        _dbcontext.TblMesas.Update(mesa);
                        await _dbcontext.SaveChangesAsync();
                    }

                    pedidoGenerado = modelo;

                    // Confirmar transacción
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Revertir transacción en caso de error
                    transaction.Rollback();

                    // Log del error (podrías implementar un sistema de logging aquí)
                    Console.WriteLine($"Error al registrar pedido: {ex.Message}");

                    // Relanzar la excepción para que el controlador la maneje
                    throw new Exception($"Error al registrar el pedido: {ex.Message}", ex);
                }
            }

            return pedidoGenerado;
        }
    }
}