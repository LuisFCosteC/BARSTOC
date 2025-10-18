using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BARSTOC_DAL.DBContext;
using BARSTOC_DAL.Repositorios.Contrato;
using BARSTOC_DAL.Repositorios;
using BARSTOC_BLL.Servicios.Contrato;
using BARSTOC_BLL.Servicios;
using BARSTOC_Utility;

namespace BARSTOC_IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbBarstocContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("StringSQlConnection"));
            });

            // Dependencia de Repositorios
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IPedidoRepository, PedidoRepository>();

            // Dependencia de AutoMapper 
            services.AddAutoMapper(typeof(AutoMapperProfile));

            // ✅ AGREGAR JwtService
            services.AddScoped<JwtService>();

            // Dependencia de Servicios BLL
            services.AddScoped<IUsuariosService, UsuariosService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<ISedesService, SedesService>();
            services.AddScoped<IMesasService, MesasService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IPedidosService, PedidosService>();
            // services.AddScoped<IInventarioService, InventarioService>();
            // services.AddScoped<IReporteService, ReporteService>();
            // services.AddScoped<IDashBoardService, DashBoardService>();
            // services.AddScoped<IMenuService, MenuService>();
        }
    }
}