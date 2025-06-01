using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVentas.DTO;
using SistemaVentas.BLL.Servicios.Contrato;
using SistemaVentas.API.Utility;
using System.Xml;
using SistemaVentas.BLL.Servicios;

namespace SistemaVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        [Route("lista")]
        public async Task<IActionResult> Lista()
        {
            var rsp = new Response<List<ProductoDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _productoService.Lista();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.message = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpPost]
        [Route("guardar")]
        public async Task<IActionResult> SaveProduct([FromBody] ProductoDTO producto)
        {
            var rsp = new Response<ProductoDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _productoService.Crear(producto);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.message = ex.Message;
            }
            return rsp.status ? Created() : BadRequest(rsp);
        }

        [HttpPut]
        [Route("editar")]
        public async Task<IActionResult> EditUser([FromBody] ProductoDTO producto)
        {
            var rsp = new Response<bool>();
            try
            {
                rsp.status = true;
                rsp.value = await _productoService.Editar(producto);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.message = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpDelete]
        [Route("eliminar/{idProducto:int}")]
        public async Task<IActionResult> DeleteUser(int idProducto)
        {
            var rsp = new Response<bool>();
            try
            {
                rsp.status = true;
                rsp.value = await _productoService.Eliminar(idProducto);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.message = ex.Message;
            }
            return rsp.status ? Ok(rsp) : BadRequest(rsp);
        }
    }
}
