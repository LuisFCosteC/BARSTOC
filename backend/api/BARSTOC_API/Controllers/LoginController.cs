using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BARSTOC_BLL.Servicios.Contrato;
using BARSTOC_DTO;
using BARSTOC_API.Utility;

namespace BARSTOC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        [Route("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] DTO_Login login)
        {
            var respuesta = new Response<DTO_LoginResponse>();

            try
            {
                respuesta.Status = true;
                respuesta.Value = await _loginService.ValidateCredentials(login.UsuarioLogin, login.PasswordHash);
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
                respuesta.Value = await _loginService.ChangePassword(id, modelo.NuevaPassword);
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