using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.DTO;
using SistemaVentas.API.Utility;

namespace SistemaVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    { 
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var rsp = new Response<List<RolDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _rolService.Lista();

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
