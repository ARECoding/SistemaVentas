using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVentas.DTO;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.API.Utility;
using SistemaVentas.BLL.Servicios;

namespace SistemaVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaService;

        public VentaController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        [HttpPost]
        [Route("registrar")]
        public async Task<IActionResult> SaveProduct([FromBody] VentaDTO venta)
        {
            var rsp = new Response<VentaDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _ventaService.Registrar(venta);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.message = ex.Message;
            }
            return rsp.status ? Created() : BadRequest(rsp);
        }

        [HttpGet]
        [Route("historial")]
        public async Task<IActionResult> Lista(string buscarPor, string? numeroVenta, string? fechaInicio, string? fechaFin)
        {
            var rsp = new Response<List<VentaDTO>>();
            numeroVenta = numeroVenta ?? string.Empty;
            fechaInicio = fechaInicio ?? string.Empty;
            fechaFin = fechaFin ?? string.Empty;

            try
            {
                rsp.status = true;
                rsp.value = await _ventaService.Historial(buscarPor, numeroVenta, fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.message = ex.Message;
            }
            return Ok(rsp);
        }
    }
}
