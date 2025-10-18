using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BARSTOC_BLL.Servicios.Contrato;
using BARSTOC_DTO;
using BARSTOC_API.Utility;

namespace BARSTOC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SedesController : ControllerBase
    {
        private readonly ISedesService _sedesService;

        public SedesController(ISedesService sedesService)
        {
            _sedesService = sedesService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var respuesta = new Response<List<DTO_Sedes>>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _sedesService.List();
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
            var respuesta = new Response<DTO_Sedes>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _sedesService.Get(id);
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
        public async Task<IActionResult> Registrar([FromBody] DTO_Sedes sede)
        {
            var respuesta = new Response<DTO_Sedes>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _sedesService.Create(sede);
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
        public async Task<IActionResult> Editar([FromBody] DTO_Sedes sede)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _sedesService.Edit(sede);
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
                respuesta.Value = await _sedesService.ChangeEstado(id, "inactiva");
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
                respuesta.Value = await _sedesService.ChangeEstado(id, "activa");
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