using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVentas.DTO;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.API.Utility;

namespace SistemaVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }
        [HttpGet]
        [Route("lista")]
        public async Task<IActionResult> Lista(int idUsuario)
        {
            var rsp = new Response<List<MenuDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _menuService.Lista(idUsuario);
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
