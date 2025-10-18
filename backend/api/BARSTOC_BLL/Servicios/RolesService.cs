using AutoMapper;
using BARSTOC_BLL.Servicios.Contrato;
using BARSTOC_DAL.Repositorios.Contrato;
using BARSTOC_DTO;
using BARSTOC_Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_BLL.Servicios
{
    public class RolesService : IRolesService
    {
        private readonly IGenericRepository<TBL_Roles> _rolesRepository;
        private readonly IGenericRepository<TBL_Usuarios> _usuariosRepository;
        private readonly IMapper _mapper;

        public RolesService(
            IGenericRepository<TBL_Roles> rolesRepository,
            IGenericRepository<TBL_Usuarios> usuariosRepository,
            IMapper mapper)
        {
            _rolesRepository = rolesRepository;
            _usuariosRepository = usuariosRepository;
            _mapper = mapper;
        }

        public async Task<List<DTO_Roles>> List()
        {
            try
            {
                var queryRoles = await _rolesRepository.Consultar();
                var listaRoles = queryRoles.ToList();
                return _mapper.Map<List<DTO_Roles>>(listaRoles);
            }
            catch
            {
                throw;
            }
        }

        public async Task<DTO_Roles> Get(int id)
        {
            try
            {
                var rol = await _rolesRepository.Obtener(r => r.IdRol == id);

                if (rol == null)
                    throw new TaskCanceledException("El rol no existe");

                return _mapper.Map<DTO_Roles>(rol);
            }
            catch
            {
                throw;
            }
        }

        public async Task<DTO_Roles> Create(DTO_Roles modelo)
        {
            try
            {
                // Verificar si ya existe un rol con el mismo nombre
                var rolExistente = await _rolesRepository.Obtener(r =>
                    r.NombreRol.ToLower() == modelo.NombreRol.ToLower());

                if (rolExistente != null)
                    throw new TaskCanceledException("Ya existe un rol con ese nombre");

                var rol = _mapper.Map<TBL_Roles>(modelo);
                rol.FechaCreacion = DateTime.Now;

                var rolCreado = await _rolesRepository.Crear(rol);

                if (rolCreado.IdRol == 0)
                    throw new TaskCanceledException("No se pudo crear el rol");

                return _mapper.Map<DTO_Roles>(rolCreado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Edit(DTO_Roles modelo)
        {
            try
            {
                var rolModelo = _mapper.Map<TBL_Roles>(modelo);
                var rolEncontrado = await _rolesRepository.Obtener(r => r.IdRol == rolModelo.IdRol);

                if (rolEncontrado == null)
                    throw new TaskCanceledException("El rol no existe");

                // Verificar si el nuevo nombre ya existe en otro rol
                var rolConMismoNombre = await _rolesRepository.Obtener(r =>
                    r.NombreRol.ToLower() == rolModelo.NombreRol.ToLower() &&
                    r.IdRol != rolModelo.IdRol);

                if (rolConMismoNombre != null)
                    throw new TaskCanceledException("Ya existe otro rol con ese nombre");

                rolEncontrado.NombreRol = rolModelo.NombreRol;

                bool respuesta = await _rolesRepository.Editar(rolEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar el rol");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var rolEncontrado = await _rolesRepository.Obtener(r => r.IdRol == id);

                if (rolEncontrado == null)
                    throw new TaskCanceledException("El rol no existe");

                // Verificar si hay usuarios asociados a este rol
                var usuariosConRol = await _usuariosRepository.Consultar(u => u.IdRol == id);
                if (usuariosConRol.Any())
                    throw new TaskCanceledException("No se puede eliminar el rol porque tiene usuarios asociados");

                bool respuesta = await _rolesRepository.Eliminar(rolEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo eliminar el rol");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}