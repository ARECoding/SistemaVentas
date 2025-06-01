using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVentas.DTO;
using SistemaVentas.API.Utility;
using SistemaVentas.BLL.Servicios.Contrato;

namespace SistemaVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Listar()
        {
            var rsp = new Response<List<CategoriaDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _categoriaService.Lista();
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
