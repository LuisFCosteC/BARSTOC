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
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<TBL_Producto> _productoRepository;
        private readonly IGenericRepository<TBL_Categoria> _categoriaRepository;
        private readonly IMapper _mapper;

        public ProductoService(
            IGenericRepository<TBL_Producto> productoRepository,
            IGenericRepository<TBL_Categoria> categoriaRepository,
            IMapper mapper)
        {
            _productoRepository = productoRepository;
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<List<DTO_Producto>> List()
        {
            try
            {
                var queryProducto = await _productoRepository.Consultar();
                var listaProductos = queryProducto
                    .Include(p => p.IdCategoriaNavigation)
                    .Where(p => p.Estado == "activo")
                    .ToList();

                return _mapper.Map<List<DTO_Producto>>(listaProductos);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<DTO_Producto>> ListByCategoria(int idCategoria)
        {
            try
            {
                var queryProducto = await _productoRepository.Consultar(p =>
                    p.IdCategoria == idCategoria && p.Estado == "activo");

                var listaProductos = queryProducto
                    .Include(p => p.IdCategoriaNavigation)
                    .ToList();

                return _mapper.Map<List<DTO_Producto>>(listaProductos);
            }
            catch
            {
                throw;
            }
        }

        public async Task<DTO_Producto> Get(int id)
        {
            try
            {
                var producto = await _productoRepository.Obtener(p => p.IdProducto == id && p.Estado == "activo");

                if (producto == null)
                    throw new TaskCanceledException("El producto no existe");

                var query = await _productoRepository.Consultar(p => p.IdProducto == id);
                producto = query
                    .Include(p => p.IdCategoriaNavigation)
                    .FirstOrDefault();

                return _mapper.Map<DTO_Producto>(producto);
            }
            catch
            {
                throw;
            }
        }

        public async Task<DTO_Producto> Create(DTO_Producto modelo)
        {
            try
            {
                // Verificar si la categoría existe y está activa
                var categoria = await _categoriaRepository.Obtener(c =>
                    c.IdCategoria == modelo.IdCategoria && c.Estado == "activa");

                if (categoria == null)
                    throw new TaskCanceledException("La categoría no existe o está inactiva");

                // Verificar si ya existe un producto con el mismo nombre
                var productoExistente = await _productoRepository.Obtener(p =>
                    p.NombreProducto == modelo.NombreProducto && p.Estado == "activo");

                if (productoExistente != null)
                    throw new TaskCanceledException("Ya existe un producto con ese nombre");

                var producto = _mapper.Map<TBL_Producto>(modelo);
                producto.Estado = "activo";
                producto.FechaCreacion = DateTime.Now;

                // Convertir los textos de precio a decimal
                var culture = new CultureInfo("es-CO");
                if (!string.IsNullOrEmpty(modelo.CostoTexto))
                {
                    producto.Costo = decimal.Parse(modelo.CostoTexto.Replace("$", "").Replace(",", ""), culture);
                }
                if (!string.IsNullOrEmpty(modelo.PrecioVentaTexto))
                {
                    producto.PrecioVenta = decimal.Parse(modelo.PrecioVentaTexto.Replace("$", "").Replace(",", ""), culture);
                }

                var productoCreado = await _productoRepository.Crear(producto);

                if (productoCreado.IdProducto == 0)
                    throw new TaskCanceledException("No se pudo crear el producto");

                // Obtener producto completo con relaciones
                var query = await _productoRepository.Consultar(p => p.IdProducto == productoCreado.IdProducto);
                productoCreado = query
                    .Include(p => p.IdCategoriaNavigation)
                    .FirstOrDefault();

                return _mapper.Map<DTO_Producto>(productoCreado);
            }
            catch (FormatException)
            {
                throw new TaskCanceledException("Formato de precio inválido");
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Edit(DTO_Producto modelo)
        {
            try
            {
                var productoModelo = _mapper.Map<TBL_Producto>(modelo);
                var productoEncontrado = await _productoRepository.Obtener(p =>
                    p.IdProducto == productoModelo.IdProducto && p.Estado == "activo");

                if (productoEncontrado == null)
                    throw new TaskCanceledException("El producto no existe");

                // Verificar si el nuevo nombre ya existe en otro producto
                var productoConMismoNombre = await _productoRepository.Obtener(p =>
                    p.NombreProducto == productoModelo.NombreProducto &&
                    p.IdProducto != productoModelo.IdProducto &&
                    p.Estado == "activo");

                if (productoConMismoNombre != null)
                    throw new TaskCanceledException("Ya existe otro producto con ese nombre");

                // Verificar si la categoría existe y está activa
                var categoria = await _categoriaRepository.Obtener(c =>
                    c.IdCategoria == productoModelo.IdCategoria && c.Estado == "activa");

                if (categoria == null)
                    throw new TaskCanceledException("La categoría no existe o está inactiva");

                productoEncontrado.NombreProducto = productoModelo.NombreProducto;
                productoEncontrado.IdCategoria = productoModelo.IdCategoria;

                // Convertir y actualizar precios
                var culture = new CultureInfo("es-CO");
                if (!string.IsNullOrEmpty(modelo.CostoTexto))
                {
                    productoEncontrado.Costo = decimal.Parse(modelo.CostoTexto.Replace("$", "").Replace(",", ""), culture);
                }
                if (!string.IsNullOrEmpty(modelo.PrecioVentaTexto))
                {
                    productoEncontrado.PrecioVenta = decimal.Parse(modelo.PrecioVentaTexto.Replace("$", "").Replace(",", ""), culture);
                }

                bool respuesta = await _productoRepository.Editar(productoEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar el producto");

                return respuesta;
            }
            catch (FormatException)
            {
                throw new TaskCanceledException("Formato de precio inválido");
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ChangeEstado(int idProducto, string nuevoEstado)
        {
            try
            {
                var productoEncontrado = await _productoRepository.Obtener(p => p.IdProducto == idProducto);

                if (productoEncontrado == null)
                    throw new TaskCanceledException("El producto no existe");

                // No eliminar físicamente, solo cambiar estado
                productoEncontrado.Estado = nuevoEstado;

                bool respuesta = await _productoRepository.Editar(productoEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException($"No se pudo {nuevoEstado.ToLower()} el producto");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}