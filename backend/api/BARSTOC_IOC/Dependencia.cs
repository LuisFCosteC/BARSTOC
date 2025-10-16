using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using BARSTOC_DAL.DBContext;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace BARSTOC_IOC
{
    public static class Dependencia
    {
        public static void InyectarDependecias(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbBarstocContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("StringSQlConnection"));
            });
        }
    }
}
