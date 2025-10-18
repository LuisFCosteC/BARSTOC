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
    public class SedesService : ISedesService
    {
        private readonly IGenericRepository<TBL_Sedes> _sedesRepository;
        private readonly IMapper _mapper;

        public SedesService(IGenericRepository<TBL_Sedes> sedesRepository, IMapper mapper)
        {
            _sedesRepository = sedesRepository;
            _mapper = mapper;
        }

        public async Task<List<DTO_Sedes>> List()
        {
            try
            {
                var querySedes = await _sedesRepository.Consultar();
                var listaSedes = querySedes.Where(s => s.Estado == "activa").ToList();
                return _mapper.Map<List<DTO_Sedes>>(listaSedes);
            }
            catch
            {
                throw;
            }
        }

        public async Task<DTO_Sedes> Get(int id)
        {
            try
            {
                var sede = await _sedesRepository.Obtener(s => s.IdSede == id && s.Estado == "activa");

                if (sede == null)
                    throw new TaskCanceledException("La sede no existe o está inactiva");

                return _mapper.Map<DTO_Sedes>(sede);
            }
            catch
            {
                throw;
            }
        }

        public async Task<DTO_Sedes> Create(DTO_Sedes modelo)
        {
            try
            {
                // Verificar si ya existe una sede con el mismo nombre
                var sedeExistente = await _sedesRepository.Obtener(s =>
                    s.NombreSede == modelo.NombreSede && s.Estado == "activa");

                if (sedeExistente != null)
                    throw new TaskCanceledException("Ya existe una sede con ese nombre");

                var sede = _mapper.Map<TBL_Sedes>(modelo);
                sede.Estado = "activa";
                sede.FechaCreacion = DateTime.Now;

                var sedeCreada = await _sedesRepository.Crear(sede);

                if (sedeCreada.IdSede == 0)
                    throw new TaskCanceledException("No se pudo crear la sede");

                return _mapper.Map<DTO_Sedes>(sedeCreada);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Edit(DTO_Sedes modelo)
        {
            try
            {
                var sedeModelo = _mapper.Map<TBL_Sedes>(modelo);
                var sedeEncontrada = await _sedesRepository.Obtener(s =>
                    s.IdSede == sedeModelo.IdSede && s.Estado == "activa");

                if (sedeEncontrada == null)
                    throw new TaskCanceledException("La sede no existe");

                // Verificar si el nuevo nombre ya existe en otra sede
                var sedeConMismoNombre = await _sedesRepository.Obtener(s =>
                    s.NombreSede == sedeModelo.NombreSede &&
                    s.IdSede != sedeModelo.IdSede &&
                    s.Estado == "activa");

                if (sedeConMismoNombre != null)
                    throw new TaskCanceledException("Ya existe otra sede con ese nombre");

                sedeEncontrada.NombreSede = sedeModelo.NombreSede;
                sedeEncontrada.Direccion = sedeModelo.Direccion;
                sedeEncontrada.Telefono = sedeModelo.Telefono;

                bool respuesta = await _sedesRepository.Editar(sedeEncontrada);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar la sede");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ChangeEstado(int idSede, string nuevoEstado)
        {
            try
            {
                var sedeEncontrada = await _sedesRepository.Obtener(s => s.IdSede == idSede);

                if (sedeEncontrada == null)
                    throw new TaskCanceledException("La sede no existe");

                // No eliminar físicamente, solo cambiar estado
                sedeEncontrada.Estado = nuevoEstado;

                bool respuesta = await _sedesRepository.Editar(sedeEncontrada);

                if (!respuesta)
                    throw new TaskCanceledException($"No se pudo {nuevoEstado.ToLower()} la sede");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}