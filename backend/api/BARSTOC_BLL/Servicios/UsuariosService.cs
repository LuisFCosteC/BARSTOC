using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BARSTOC_BLL.Servicios.Contrato;
using BARSTOC_DAL.Repositorios.Contrato;
using BARSTOC_DTO;
using BARSTOC_Model;
using Microsoft.EntityFrameworkCore;

namespace BARSTOC_BLL.Servicios
{
    public class UsuariosService : IUsuariosService
    {
        private readonly IGenericRepository<TBL_Usuarios> _usuariosRepository;
        private readonly IMapper _mapper;

        public UsuariosService(IGenericRepository<TBL_Usuarios> usuariosRepository, IMapper mapper)
        {
            _usuariosRepository = usuariosRepository;
            _mapper = mapper;
        }

        public async Task<List<DTO_Usuarios>> List()
        {
            try
            {
                var queryUsuarios = await _usuariosRepository.Consultar();
                var listaUsuarios = queryUsuarios
                    .Include(u => u.IdRolNavigation)
                    .Include(u => u.IdSedeNavigation)
                    .Where(u => u.Estado == "Activo")
                    .ToList();

                return _mapper.Map<List<DTO_Usuarios>>(listaUsuarios);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<DTO_Usuarios>> ListBySede(int idSede)
        {
            try
            {
                var queryUsuarios = await _usuariosRepository.Consultar(u =>
                    u.IdSede == idSede && u.Estado == "Activo");

                var listaUsuarios = queryUsuarios
                    .Include(u => u.IdRolNavigation)
                    .Include(u => u.IdSedeNavigation)
                    .ToList();

                return _mapper.Map<List<DTO_Usuarios>>(listaUsuarios);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<DTO_Usuarios>> ListByRol(int idRol)
        {
            try
            {
                var queryUsuarios = await _usuariosRepository.Consultar(u =>
                    u.IdRol == idRol && u.Estado == "Activo");

                var listaUsuarios = queryUsuarios
                    .Include(u => u.IdRolNavigation)
                    .Include(u => u.IdSedeNavigation)
                    .ToList();

                return _mapper.Map<List<DTO_Usuarios>>(listaUsuarios);
            }
            catch
            {
                throw;
            }
        }

        public async Task<DTO_Usuarios> Get(int id)
        {
            try
            {
                var usuario = await _usuariosRepository.Obtener(u => u.IdUsuario == id);

                if (usuario == null)
                    throw new TaskCanceledException("El usuario no existe");

                var query = await _usuariosRepository.Consultar(u => u.IdUsuario == id);
                usuario = query
                    .Include(u => u.IdRolNavigation)
                    .Include(u => u.IdSedeNavigation)
                    .FirstOrDefault();

                return _mapper.Map<DTO_Usuarios>(usuario);
            }
            catch
            {
                throw;
            }
        }

        public async Task<DTO_Usuarios> Create(DTO_Usuarios modelo)
        {
            try
            {
                // Verificar si el usuario ya existe
                var usuarioExistente = await _usuariosRepository.Obtener(u =>
                    u.UsuarioLogin == modelo.UsuarioLogin || u.correo == modelo.correo);

                if (usuarioExistente != null)
                    throw new TaskCanceledException("El nombre de usuario o correo ya existe");

                // ✅ MODIFICADO: Usar contraseña personalizada si se proporciona, sino generar una segura
                string passwordSegura;
                bool esPasswordPersonalizada = false;

                if (!string.IsNullOrWhiteSpace(modelo.PasswordPersonalizada))
                {
                    if (!PasswordService.IsPasswordStrong(modelo.PasswordPersonalizada))
                        throw new TaskCanceledException("La contraseña personalizada no cumple con los estándares de seguridad");

                    passwordSegura = modelo.PasswordPersonalizada;
                    esPasswordPersonalizada = true;
                }
                else
                {
                    passwordSegura = PasswordService.GenerateSecurePassword();
                }

                var usuario = _mapper.Map<TBL_Usuarios>(modelo);
                usuario.PasswordHash = PasswordService.HashPassword(passwordSegura);
                usuario.Estado = "Activo";
                usuario.FechaCreacion = DateTime.Now;

                var usuarioCreado = await _usuariosRepository.Crear(usuario);

                if (usuarioCreado.IdUsuario == 0)
                    throw new TaskCanceledException("No se pudo crear el usuario");

                // Obtener usuario completo con relaciones
                var query = await _usuariosRepository.Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);
                usuarioCreado = query
                    .Include(u => u.IdRolNavigation)
                    .Include(u => u.IdSedeNavigation)
                    .FirstOrDefault();

                var dtoResult = _mapper.Map<DTO_Usuarios>(usuarioCreado);

                // ✅ MODIFICADO: Solo retornar password temporal si NO es personalizada
                if (!esPasswordPersonalizada)
                {
                    dtoResult.PasswordTemporal = passwordSegura;
                }

                return dtoResult;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Edit(DTO_Usuarios modelo)
        {
            try
            {
                var usuarioModelo = _mapper.Map<TBL_Usuarios>(modelo);
                var usuarioEncontrado = await _usuariosRepository.Obtener(u =>
                    u.IdUsuario == usuarioModelo.IdUsuario);

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                // Actualizar campos permitidos
                usuarioEncontrado.NumeroDocumento = usuarioModelo.NumeroDocumento;
                usuarioEncontrado.IdSede = usuarioModelo.IdSede;
                usuarioEncontrado.NombreUsuario = usuarioModelo.NombreUsuario;
                usuarioEncontrado.ApellidoUsuario = usuarioModelo.ApellidoUsuario;
                usuarioEncontrado.correo = usuarioModelo.correo;
                usuarioEncontrado.IdRol = usuarioModelo.IdRol;
                usuarioEncontrado.UsuarioLogin = usuarioModelo.UsuarioLogin;
                usuarioEncontrado.FechaUltimoLogin = usuarioModelo.FechaUltimoLogin;

                bool respuesta = await _usuariosRepository.Editar(usuarioEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar el usuario");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ChangeEstado(int idUsuario, string nuevoEstado)
        {
            try
            {
                var usuarioEncontrado = await _usuariosRepository.Obtener(u => u.IdUsuario == idUsuario);

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                // No eliminar, solo cambiar estado (cumple requerimiento)
                usuarioEncontrado.Estado = nuevoEstado;

                bool respuesta = await _usuariosRepository.Editar(usuarioEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo cambiar el estado del usuario");

                return respuesta;
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
                if (!PasswordService.IsPasswordStrong(nuevaPassword))
                    throw new TaskCanceledException("La contraseña no cumple con los estándares de seguridad");

                var usuarioEncontrado = await _usuariosRepository.Obtener(u => u.IdUsuario == idUsuario);

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                usuarioEncontrado.PasswordHash = PasswordService.HashPassword(nuevaPassword);

                bool respuesta = await _usuariosRepository.Editar(usuarioEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo cambiar la contraseña");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ResetPassword(int idUsuario)
        {
            try
            {
                var usuarioEncontrado = await _usuariosRepository.Obtener(u => u.IdUsuario == idUsuario);

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                // Generar nueva contraseña segura
                var nuevaPassword = PasswordService.GenerateSecurePassword();
                usuarioEncontrado.PasswordHash = PasswordService.HashPassword(nuevaPassword);

                bool respuesta = await _usuariosRepository.Editar(usuarioEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo resetear la contraseña");

                // En un sistema real, enviar la nueva contraseña por email
                // Por ahora solo retornamos éxito

                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}