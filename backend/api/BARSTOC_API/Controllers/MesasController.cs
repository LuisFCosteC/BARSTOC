using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BARSTOC_BLL.Servicios.Contrato;
using BARSTOC_DTO;
using BARSTOC_API.Utility;

namespace BARSTOC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MesasController : ControllerBase
    {
        private readonly IMesasService _mesasService;

        public MesasController(IMesasService mesasService)
        {
            _mesasService = mesasService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var respuesta = new Response<List<DTO_Mesas>>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _mesasService.List();
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
            var respuesta = new Response<DTO_Mesas>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _mesasService.Get(id);
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
            var respuesta = new Response<List<DTO_Mesas>>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _mesasService.ListBySede(idSede);
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
        public async Task<IActionResult> Registrar([FromBody] DTO_Mesas mesa)
        {
            var respuesta = new Response<DTO_Mesas>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _mesasService.Create(mesa);
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
        public async Task<IActionResult> Editar([FromBody] DTO_Mesas mesa)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _mesasService.Edit(mesa);
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpPut]
        [Route("CambiarEstado/{id:int}")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoDTO modelo)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _mesasService.ChangeEstado(id, modelo.NuevoEstado);
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpPut]
        [Route("Liberar/{id:int}")]
        public async Task<IActionResult> Liberar(int id)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _mesasService.ChangeEstado(id, "libre");
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpPut]
        [Route("Ocupar/{id:int}")]
        public async Task<IActionResult> Ocupar(int id)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _mesasService.ChangeEstado(id, "ocupada");
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpPut]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _mesasService.ChangeEstado(id, "eliminada");
            }
            catch (Exception ex)
            {
                respuesta.Status = false;
                respuesta.Msg = ex.Message;
            }

            return Ok(respuesta);
        }
    }

    public class CambiarEstadoDTO
    {
        public string NuevoEstado { get; set; } = null!;
    }
}