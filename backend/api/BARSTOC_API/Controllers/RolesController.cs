using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BARSTOC_BLL.Servicios.Contrato;
using BARSTOC_DTO;
using BARSTOC_API.Utility;

namespace BARSTOC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var respuesta = new Response<List<DTO_Roles>>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _rolesService.List();
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
            var respuesta = new Response<DTO_Roles>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _rolesService.Get(id);
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
        public async Task<IActionResult> Registrar([FromBody] DTO_Roles rol)
        {
            var respuesta = new Response<DTO_Roles>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _rolesService.Create(rol);
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
        public async Task<IActionResult> Editar([FromBody] DTO_Roles rol)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _rolesService.Edit(rol);
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _rolesService.Delete(id);
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }
    }
}