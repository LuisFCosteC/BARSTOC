using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BARSTOC_BLL.Servicios.Contrato;
using BARSTOC_DTO;
using BARSTOC_API.Utility;

namespace BARSTOC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;

        public UsuariosController(IUsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var respuesta = new Response<List<DTO_Usuarios>>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _usuariosService.List();
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpGet]
        [Route("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var respuesta = new Response<DTO_Usuarios>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _usuariosService.Get(id);
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpGet]
        [Route("ListaPorSede/{idSede:int}")]
        public async Task<IActionResult> ListaPorSede(int idSede)
        {
            var respuesta = new Response<List<DTO_Usuarios>>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _usuariosService.ListBySede(idSede);
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpGet]
        [Route("ListaPorRol/{idRol:int}")]
        public async Task<IActionResult> ListaPorRol(int idRol)
        {
            var respuesta = new Response<List<DTO_Usuarios>>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _usuariosService.ListByRol(idRol);
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] DTO_Usuarios usuario)
        {
            var respuesta = new Response<DTO_Usuarios>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _usuariosService.Create(usuario);

                // En producción, eliminar la contraseña temporal de la respuesta
                // respuesta.Value.PasswordTemporal = null;
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpPost]
        [Route("RegistrarConPassword")]
        public async Task<IActionResult> RegistrarConPassword([FromBody] DTO_Usuarios usuario)
        {
            var respuesta = new Response<DTO_Usuarios>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _usuariosService.Create(usuario);

                // No retornar la contraseña en producción
                respuesta.Value.PasswordTemporal = null;
                respuesta.Value.PasswordPersonalizada = null;
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] DTO_Usuarios usuario)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _usuariosService.Edit(usuario);
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpPut]
        [Route("Desactivar/{id:int}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _usuariosService.ChangeEstado(id, "Inactivo");
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpPut]
        [Route("Activar/{id:int}")]
        public async Task<IActionResult> Activar(int id)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _usuariosService.ChangeEstado(id, "Activo");
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpPut]
        [Route("CambiarPassword/{id:int}")]
        public async Task<IActionResult> CambiarPassword(int id, [FromBody] CambiarPasswordDTO modelo)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _usuariosService.ChangePassword(id, modelo.NuevaPassword);
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpPut]
        [Route("ResetPassword/{id:int}")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _usuariosService.ResetPassword(id);
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }
    }

    public class CambiarPasswordDTO
    {
        public string NuevaPassword { get; set; } = null!;
    }
}