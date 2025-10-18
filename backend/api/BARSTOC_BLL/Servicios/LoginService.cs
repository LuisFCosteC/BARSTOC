using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BARSTOC_BLL.Servicios.Contrato;
using BARSTOC_DAL.Repositorios.Contrato;
using BARSTOC_DTO;
using BARSTOC_Model;
using Microsoft.EntityFrameworkCore;

namespace BARSTOC_BLL.Servicios
{
    public class LoginService : ILoginService
    {
        private readonly IGenericRepository<TBL_Usuarios> _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;

        public LoginService(
            IGenericRepository<TBL_Usuarios> usuarioRepository,
            IMapper mapper,
            JwtService jwtService)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<DTO_LoginResponse> ValidateCredentials(string usuarioLogin, string password)
        {
            try
            {
                var queryUsuario = await _usuarioRepository.Consultar(u =>
                    u.UsuarioLogin == usuarioLogin &&
                    u.Estado == "Activo");

                if (queryUsuario.FirstOrDefault() == null)
                    throw new TaskCanceledException("El usuario no existe o está inactivo");

                TBL_Usuarios usuario = queryUsuario
                    .Include(r => r.IdRolNavigation)
                    .Include(s => s.IdSedeNavigation)
                    .First();

                // Verificar contraseña
                if (!PasswordService.VerifyPassword(password, usuario.PasswordHash))
                    throw new TaskCanceledException("Credenciales incorrectas");

                // Actualizar último login
                usuario.FechaUltimoLogin = DateTime.Now;
                await _usuarioRepository.Editar(usuario);

                var sesion = _mapper.Map<DTO_Sesion>(usuario);
                var token = _jwtService.GenerateToken(sesion);

                return new DTO_LoginResponse
                {
                    Sesion = sesion,
                    Token = token,
                    Expiracion = DateTime.UtcNow.AddMinutes(3)
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ChangePassword(int idUsuario, string nuevaPassword)
        {
            try
            {
                var usuarioEncontrado = await _usuarioRepository.Obtener(u => u.IdUsuario == idUsuario);

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                if (!PasswordService.IsPasswordStrong(nuevaPassword))
                    throw new TaskCanceledException("La contraseña no cumple con los estándares de seguridad");

                usuarioEncontrado.PasswordHash = PasswordService.HashPassword(nuevaPassword);

                bool respuesta = await _usuarioRepository.Editar(usuarioEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo cambiar la contraseña");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}