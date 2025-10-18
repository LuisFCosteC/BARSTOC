using AutoMapper;
using BARSTOC_DTO;
using BARSTOC_Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            var culture = new CultureInfo("es-CO");

            // --------------------------------------------------------------------------
            // ------------------------------ Roles Mapping -----------------------------
            // --------------------------------------------------------------------------
            #region Roles
            CreateMap<TBL_Roles, DTO_Roles>().ReverseMap();
            #endregion

            // --------------------------------------------------------------------------
            // ------------------------------ Menu Mapping ------------------------------
            // --------------------------------------------------------------------------
            #region Menu
            CreateMap<TBL_Menu, DTO_Menu>().ReverseMap();
            #endregion

            // ---------------------------------------------------------------------------
            // ------------------------------ Usuarios Mapping ---------------------------
            // ---------------------------------------------------------------------------
            #region Usuarios
            CreateMap<TBL_Usuarios, DTO_Usuarios>()
                .ForMember(dest => dest.DescripcionRol,
                    opt => opt.MapFrom(src => src.IdRolNavigation.NombreRol))
                .ForMember(dest => dest.NombreSede,
                    opt => opt.MapFrom(src => src.IdSedeNavigation.NombreSede))
                .ForMember(dest => dest.PasswordTemporal, opt => opt.Ignore());

            CreateMap<DTO_Usuarios, TBL_Usuarios>()
                .ForMember(dest => dest.IdRolNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdSedeNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Se maneja en el servicio
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaUltimoLogin, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.Ignore());
            #endregion

            // ------------------------------------------------------------------------------
            // ------------------------------ Categoria Mapping ----------------------------
            // ------------------------------------------------------------------------------
            #region Categoria
            CreateMap<TBL_Categoria, DTO_Categoria>().ReverseMap();
            #endregion

            // -----------------------------------------------------------------------------
            // ------------------------------ Producto Mapping ----------------------------
            // -----------------------------------------------------------------------------
            #region Producto
            CreateMap<TBL_Producto, DTO_Producto>()
                .ForMember(dest => dest.DescripcionCategoria,
                    opt => opt.MapFrom(src => src.IdCategoriaNavigation.NombreCategoria))
                .ForMember(dest => dest.CostoTexto,
                    opt => opt.MapFrom(src => src.Costo.ToString("C", culture)))
                .ForMember(dest => dest.PrecioVentaTexto,
                    opt => opt.MapFrom(src => src.PrecioVenta.ToString("C", culture)));

            CreateMap<DTO_Producto, TBL_Producto>()
                .ForMember(dest => dest.IdCategoriaNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Costo,
                    opt => opt.MapFrom(src => decimal.Parse(src.CostoTexto.Replace("$", "").Replace(",", ""), culture)))
                .ForMember(dest => dest.PrecioVenta,
                    opt => opt.MapFrom(src => decimal.Parse(src.PrecioVentaTexto.Replace("$", "").Replace(",", ""), culture)));
            #endregion

            // ---------------------------------------------------------------------------
            // ------------------------------ Mesas Mapping -----------------------------
            // ---------------------------------------------------------------------------
            #region Mesas
            CreateMap<TBL_Mesas, DTO_Mesas>()
                .ForMember(dest => dest.NombreSede,
                    opt => opt.MapFrom(src => src.IdSedeNavigation.NombreSede));

            CreateMap<DTO_Mesas, TBL_Mesas>()
                .ForMember(dest => dest.IdSedeNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore());
            #endregion

            // ---------------------------------------------------------------------------
            // ------------------------------ Sedes Mapping -----------------------------
            // ---------------------------------------------------------------------------
            #region Sedes
            CreateMap<TBL_Sedes, DTO_Sedes>().ReverseMap();
            #endregion

            // ---------------------------------------------------------------------------
            // ------------------------------ Pedidos Mapping ---------------------------
            // ---------------------------------------------------------------------------
            #region Pedidos
            CreateMap<TBL_Pedidos, DTO_Pedidos>()
                .ForMember(dest => dest.NumeroMesa,
                    opt => opt.MapFrom(src => src.IdMesaNavigation.NumeroMesa))
                .ForMember(dest => dest.NombreMesero,
                    opt => opt.MapFrom(src => $"{src.IdUsuarioMeseroNavigation.NombreUsuario} {src.IdUsuarioMeseroNavigation.ApellidoUsuario}"))
                .ForMember(dest => dest.NombreSede,
                    opt => opt.MapFrom(src => src.IdSedeNavigation.NombreSede))
                .ForMember(dest => dest.FechaAperturaTexto,
                    opt => opt.MapFrom(src => src.FechaApertura.Value.ToString("dd/MM/yyyy HH:mm")))
                .ForMember(dest => dest.FechaCierreTexto,
                    opt => opt.MapFrom(src => src.FechaCierre.HasValue ? src.FechaCierre.Value.ToString("dd/MM/yyyy HH:mm") : null))
                .ForMember(dest => dest.TotalPedidoTexto,
                    opt => opt.MapFrom(src => src.TotalPedido.HasValue ? src.TotalPedido.Value.ToString("C", culture) : "0"));

            CreateMap<DTO_Pedidos, TBL_Pedidos>()
                .ForMember(dest => dest.IdMesaNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuarioMeseroNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdSedeNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.TblDetallePedidos, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoPedido, opt => opt.Ignore())
                .ForMember(dest => dest.FechaApertura, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCierre, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPedido, opt => opt.Ignore())
                .ForMember(dest => dest.MetodoPago, opt => opt.Ignore());
            #endregion

            // ---------------------------------------------------------------------------------
            // ------------------------------ Detalle Pedidos Mapping -------------------------
            // ---------------------------------------------------------------------------------
            #region DetallePedidos
            CreateMap<TBL_Detalle_Pedidos, DTO_Detalle_Pedidos>()
                .ForMember(dest => dest.DescripcionProducto,
                    opt => opt.MapFrom(src => src.IdProductoNavigation.NombreProducto))
                .ForMember(dest => dest.PrecioUnitarioTexto,
                    opt => opt.MapFrom(src => src.PrecioUnitario.ToString("C", culture)))
                .ForMember(dest => dest.SubtotalTexto,
                    opt => opt.MapFrom(src => src.Subtotal.ToString("C", culture)));

            CreateMap<DTO_Detalle_Pedidos, TBL_Detalle_Pedidos>()
                .ForMember(dest => dest.IdProductoNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdPedidoNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.PrecioUnitario,
                    opt => opt.MapFrom(src => decimal.Parse(src.PrecioUnitarioTexto.Replace("$", "").Replace(",", ""), culture)))
                .ForMember(dest => dest.Subtotal,
                    opt => opt.MapFrom(src => decimal.Parse(src.SubtotalTexto.Replace("$", "").Replace(",", ""), culture)))
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore());
            #endregion

            // ----------------------------------------------------------------------------
            // ------------------------------ Inventario Mapping --------------------------
            // ----------------------------------------------------------------------------
            #region Inventario
            CreateMap<TBL_Inventario, DTO_Inventario>()
                .ForMember(dest => dest.NombreSede,
                    opt => opt.MapFrom(src => src.IdSedeNavigation.NombreSede))
                .ForMember(dest => dest.NombreProducto,
                    opt => opt.MapFrom(src => src.IdProductoNavigation.NombreProducto));

            CreateMap<DTO_Inventario, TBL_Inventario>()
                .ForMember(dest => dest.IdSedeNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdProductoNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.UltimaActualizacion, opt => opt.Ignore());
            #endregion

            // ----------------------------------------------------------------------------
            // ------------------------------ Movimientos Inventario Mapping --------------
            // ----------------------------------------------------------------------------
            #region MovimientosInventario
            CreateMap<TBL_Movimientos_Inventario, DTO_Movimientos_Inventario>()
                .ForMember(dest => dest.NombreSede,
                    opt => opt.MapFrom(src => src.IdSedeNavigation.NombreSede))
                .ForMember(dest => dest.NombreProducto,
                    opt => opt.MapFrom(src => src.IdProductoNavigation.NombreProducto))
                .ForMember(dest => dest.NombreUsuario,
                    opt => opt.MapFrom(src => $"{src.IdUsuarioNavigation.NombreUsuario} {src.IdUsuarioNavigation.ApellidoUsuario}"));

            CreateMap<DTO_Movimientos_Inventario, TBL_Movimientos_Inventario>()
                .ForMember(dest => dest.IdSedeNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdProductoNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuarioNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.FechaMovimiento, opt => opt.Ignore());
            #endregion

            // ----------------------------------------------------------------------------
            // ------------------------------ Reporte Mapping ----------------------------
            // ----------------------------------------------------------------------------
            #region Reporte
            CreateMap<VwReporteVenta, DTO_Reporte>()
                .ForMember(dest => dest.Fecha,
                    opt => opt.MapFrom(src => src.Fecha.HasValue ? src.Fecha.Value.ToString("dd/MM/yyyy") : null))
                .ForMember(dest => dest.CostoTexto,
                    opt => opt.MapFrom(src => src.Costo.ToString("C", culture)))
                .ForMember(dest => dest.PrecioVentaTexto,
                    opt => opt.MapFrom(src => src.PrecioVenta.ToString("C", culture)))
                .ForMember(dest => dest.MargenGananciaTexto,
                    opt => opt.MapFrom(src => src.MargenGanancia.HasValue ? src.MargenGanancia.Value.ToString("C", culture) : "0"))
                .ForMember(dest => dest.GananciaTotalTexto,
                    opt => opt.MapFrom(src => src.GananciaTotal.HasValue ? src.GananciaTotal.Value.ToString("C", culture) : "0"))
                .ForMember(dest => dest.SubtotalTexto,
                    opt => opt.MapFrom(src => src.Subtotal.ToString("C", culture)))
                .ForMember(dest => dest.TotalPedidoTexto,
                    opt => opt.MapFrom(src => src.TotalPedido.HasValue ? src.TotalPedido.Value.ToString("C", culture) : "0"));
            #endregion

            // ----------------------------------------------------------------------------
            // ------------------------------ Vistas Mapping -----------------------------
            // ----------------------------------------------------------------------------
            #region Vistas
            CreateMap<VwInventarioActual, DTO_Inventario>()
                .ForMember(dest => dest.NombreSede,
                    opt => opt.MapFrom(src => src.NombreSede))
                .ForMember(dest => dest.NombreProducto,
                    opt => opt.MapFrom(src => src.NombreProducto));

            CreateMap<VwProductosPreciosFormateado, DTO_Producto>()
                .ForMember(dest => dest.NombreProducto,
                    opt => opt.MapFrom(src => src.NombreProducto))
                .ForMember(dest => dest.DescripcionCategoria,
                    opt => opt.MapFrom(src => src.NombreCategoria))
                .ForMember(dest => dest.CostoTexto,
                    opt => opt.MapFrom(src => src.CostoFormateado))
                .ForMember(dest => dest.PrecioVentaTexto,
                    opt => opt.MapFrom(src => src.PrecioVentaFormateado));
            #endregion

            // ---------------------------------------------------------------------------
            // ------------------------------ Sesion Mapping -----------------------------
            // ---------------------------------------------------------------------------
            #region Sesion
            CreateMap<TBL_Usuarios, DTO_Sesion>()
                .ForMember(dest => dest.DescripcionRol,
                    opt => opt.MapFrom(src => src.IdRolNavigation.NombreRol))
                .ForMember(dest => dest.IdSede,
                    opt => opt.MapFrom(src => src.IdSede))
                .ForMember(dest => dest.NombreSede,
                    opt => opt.MapFrom(src => src.IdSedeNavigation.NombreSede));
            #endregion
        }
    }
}