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
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace SistemaVentas.BLL.Servicios
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IGenericRepository<DetalleVenta> _detailRepository;
        private readonly IMapper _mapper;

        public VentaService(IGenericRepository<DetalleVenta> detailRepository,IVentaRepository ventaRepository ,IMapper mapper)
        {
            _detailRepository = detailRepository;
            _ventaRepository = ventaRepository;
            _mapper = mapper;
        }


        public async Task<VentaDTO> Registrar(VentaDTO modelo)
        {
            try
            {
                var generatedVenta = await _ventaRepository.Registrar(_mapper.Map<Venta>(modelo));
                if (generatedVenta == null)
                    throw new TaskCanceledException("Error al registrar la venta");
                return _mapper.Map<VentaDTO>(generatedVenta);
            }
            catch 
            {
                throw;
            }
        }


        public async Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {
            IQueryable<Venta> query = await _ventaRepository.Consultar();
            var resultList = new List<Venta>();
            try
            {
                if (buscarPor == "fecha")
                {
                    DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-AR"));
                    DateTime fech_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-AR"));
                    resultList = await query.Where(v =>
                    v.FechaRegistro.Value.Date >= fech_inicio.Date &&
                    v.FechaRegistro.Value.Date <= fech_fin.Date)
                        .Include(dv => dv.DetalleVenta)
                        .ThenInclude(p => p.IdProductoNavigation)
                        .ToListAsync();   
                }
                else 
                {
                    resultList = await query.Where(v =>
                    v.NumeroFactura == numeroVenta)
                        .Include(dv => dv.DetalleVenta)
                        .ThenInclude(p => p.IdProductoNavigation)
                        .ToListAsync();
                }
                
            }
            catch 
            {
                throw;
            }

            return _mapper.Map<List<VentaDTO>>(resultList);
        }

        public async Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFin)
        {
            IQueryable<DetalleVenta> query = await _detailRepository.Consultar();
            var listResult = new List<DetalleVenta>();
            try
            {
                DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-AR"));
                DateTime fech_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-AR"));
                listResult = await query
                    .Include(p => p.IdProductoNavigation)
                .Include(v => v.IdVentaNavigation)
                .Where(dv =>
                        dv.IdVentaNavigation.FechaRegistro.Value.Date >= fech_inicio.Date &&
                        dv.IdVentaNavigation.FechaRegistro.Value.Date <= fech_fin.Date)
                        .ToListAsync();
            }
            catch 
            {
                throw;
            }
            return _mapper.Map<List<ReporteDTO>>(listResult);

        }
    }
}
