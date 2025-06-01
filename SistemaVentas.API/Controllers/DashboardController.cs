using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVentas.DTO;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.API.Utility;

namespace SistemaVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [Route("resumen")]
        public async Task<IActionResult> Summary()
        {
            var rsp = new Response<DashboardDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _dashboardService.Resumen();
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
