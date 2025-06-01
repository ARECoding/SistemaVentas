using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.DTO;
using SistemaVentas.API.Utility;
using SistemaVentas.BLL.Servicios.Contrato;
using System.Security.Cryptography;
using System.Xml;

namespace SistemaVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var rsp = new Response<List<UsuarioDTO>>();
            try 
            {
                rsp.status = true;
                rsp.value = await _usuarioService.Lista();
                
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.message = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpPost]
        [Route("iniciarSesion")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login) 
        {
            var rsp = new Response<SesionDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _usuarioService.ValidarCredenciales(login.Correo, login.Clave);
            }
            catch (Exception ex) 
            {
                rsp.status = false;
                rsp.message = ex.Message;
            }
            return rsp.status ? Ok(rsp) : Unauthorized(rsp);
        }

        [HttpPost]
        [Route("guardar")]
        public async Task<IActionResult> SaveUser([FromBody] UsuarioDTO usuario)
        {
            var rsp = new Response<UsuarioDTO>();
            try 
            {
                rsp.status = true;
                rsp.value = await _usuarioService.Crear(usuario);
            }
            catch(Exception ex)
            {
                rsp.status = false;
                rsp.message = ex.Message;
            }
            return rsp.status ? Created() : BadRequest(rsp);
        }

        [HttpPut]
        [Route("editar")]
        public async Task<IActionResult> EditUser([FromBody] UsuarioDTO usuario)
        {
            var rsp = new Response<bool>();
            try
            {
                rsp.status = true;
                rsp.value = await _usuarioService.Editar(usuario);
            }
            catch (Exception ex)
            { 
                rsp.status = false;
                rsp.message = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpDelete]
        [Route("eliminar/{idUsuario:int}")]
        public async Task<IActionResult> DeleteUser(int idUsuario)
        { 
            var rsp = new Response<bool>();
            try
            {
                rsp.status = true;
                rsp.value = await _usuarioService.Eliminar(idUsuario);
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
