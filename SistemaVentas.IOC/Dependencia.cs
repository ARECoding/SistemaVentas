using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVentas.DAL.DBContext;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.DAL.Repositorios.Contrato;
using SistemaVentas.DAL.Repositorios;
using SistemaVentas.Utility;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.BLL.Servicios;

namespace SistemaVentas.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<VentasContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("cadenaSQL")));

            service.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            service.AddScoped<IVentaRepository, VentaRepositorio>();
            service.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            service.AddScoped<IRolService, RolService>();
            service.AddScoped<IUsuarioService, UsuarioService>();
            service.AddScoped<ICategoriaService, CategoriaService>();
            service.AddScoped<IProductoService, ProductoService>();
            service.AddScoped<IVentaService, VentaService>();
            service.AddScoped<IDashboardService, DashboardService>();
            service.AddScoped<IMenuService, MenuService>();
            
        }


    }
}
