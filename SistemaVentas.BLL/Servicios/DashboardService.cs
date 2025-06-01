using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using SistemaVentas.DTO;
using SistemaVentas.Model;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DAL.Repositorios.Contrato;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Metadata;


namespace SistemaVentas.BLL.Servicios
{
    public class DashboardService : IDashboardService
    {
        private readonly IGenericRepository<Producto> _productRepository;
        private readonly IVentaRepository _ventaRepository;
        private readonly IMapper _mapper;

        public DashboardService(IGenericRepository<Producto> productRepository, IVentaRepository ventaRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _ventaRepository = ventaRepository;
            _mapper = mapper;
        }

        private IQueryable<Venta> GetLastSalesFromNDaysBefore(IQueryable<Venta> salesTable, int numberOfDays)
        {
            DateTime? lastDate = salesTable.OrderByDescending(s => s.FechaRegistro).Select(v => v.FechaRegistro).First();
            lastDate = lastDate.Value.AddDays(-numberOfDays);
            return salesTable.Where(v => v.FechaRegistro.Value.Date >= lastDate.Value.Date);
        }

        private async Task<int> TotalSalesLastWeek()
        {
            int total = 0;
            IQueryable<Venta> _salesQuery = await _ventaRepository.Consultar();
            if(_salesQuery.Count() > 0)
            {
                var salesTable = GetLastSalesFromNDaysBefore(_salesQuery, 7);
                total = salesTable.Count();
            }
            return total;
        }

        private async Task<string> TotalIncomeLastWeek()
        {
            decimal result = 0;
            IQueryable<Venta> _salesQuery = await _ventaRepository.Consultar();
            if(_salesQuery.Count() > 0)
            {
                var salesTable = GetLastSalesFromNDaysBefore(_salesQuery, 7);
                result = salesTable.Select(v => v.Total).Sum() ?? 0;

            }
            return Convert.ToString(result, new CultureInfo("es-AR"));
        }

        private async Task<int> TotalProducts()
        {
            IQueryable<Producto> productQuery = await _productRepository.Consultar();
            int total = productQuery.Count();
            return total;
        }

        private async Task<Dictionary<string, int>> SalesLastWeek() 
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            IQueryable<Venta> _salesQuery = await _ventaRepository.Consultar();
            if (_salesQuery.Count() > 0)
            {
                var resultTable = GetLastSalesFromNDaysBefore(_salesQuery, 7)
                    .GroupBy(v => v.FechaRegistro.Value.Date)
                    .OrderBy(g => g.Key).Select(dv => new
                    {
                        fecha = dv.Key.ToString("dd/MM/yyyy"),
                        total = dv.Count()

                    }).ToDictionary(keySelector : r => r.fecha, elementSelector: r => r.total);
            }
            return result;

        }

        public async Task<DashboardDTO> Resumen()
        {
            DashboardDTO vmDashboard = new DashboardDTO();
            try
            {
                vmDashboard.TotalVentas = await TotalSalesLastWeek();
                vmDashboard.TotalIngresos = await TotalIncomeLastWeek();
                vmDashboard.TotalProducts = await TotalProducts();
                List<VentaSemanaDTO> listventasUltimaSemana = new List<VentaSemanaDTO>();

                foreach (KeyValuePair<string, int> item in await SalesLastWeek())
                {
                    listventasUltimaSemana.Add(new VentaSemanaDTO 
                    {
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }
                vmDashboard.ventasUltimaSemana = listventasUltimaSemana;

            }
            catch 
            {
                throw;
            }
            return vmDashboard;
        }
    }
}
