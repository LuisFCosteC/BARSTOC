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
    public class MesasService : IMesasService
    {
        private readonly IGenericRepository<TBL_Mesas> _mesasRepository;
        private readonly IGenericRepository<TBL_Sedes> _sedesRepository;
        private readonly IMapper _mapper;

        public MesasService(
            IGenericRepository<TBL_Mesas> mesasRepository,
            IGenericRepository<TBL_Sedes> sedesRepository,
            IMapper mapper)
        {
            _mesasRepository = mesasRepository;
            _sedesRepository = sedesRepository;
            _mapper = mapper;
        }

        public async Task<List<DTO_Mesas>> List()
        {
            try
            {
                var queryMesas = await _mesasRepository.Consultar();
                var listaMesas = queryMesas
                    .Include(m => m.IdSedeNavigation)
                    .Where(m => m.Estado != "eliminada")
                    .ToList();

                return _mapper.Map<List<DTO_Mesas>>(listaMesas);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<DTO_Mesas>> ListBySede(int idSede)
        {
            try
            {
                var queryMesas = await _mesasRepository.Consultar(m =>
                    m.IdSede == idSede && m.Estado != "eliminada");

                var listaMesas = queryMesas
                    .Include(m => m.IdSedeNavigation)
                    .ToList();

                return _mapper.Map<List<DTO_Mesas>>(listaMesas);
            }
            catch
            {
                throw;
            }
        }

        public async Task<DTO_Mesas> Get(int id)
        {
            try
            {
                var mesa = await _mesasRepository.Obtener(m => m.IdMesa == id && m.Estado != "eliminada");

                if (mesa == null)
                    throw new TaskCanceledException("La mesa no existe");

                var query = await _mesasRepository.Consultar(m => m.IdMesa == id);
                mesa = query
                    .Include(m => m.IdSedeNavigation)
                    .FirstOrDefault();

                return _mapper.Map<DTO_Mesas>(mesa);
            }
            catch
            {
                throw;
            }
        }

        public async Task<DTO_Mesas> Create(DTO_Mesas modelo)
        {
            try
            {
                // Verificar si la sede existe y está activa
                var sede = await _sedesRepository.Obtener(s =>
                    s.IdSede == modelo.IdSede && s.Estado == "activa");

                if (sede == null)
                    throw new TaskCanceledException("La sede no existe o está inactiva");

                // Verificar si ya existe una mesa con el mismo número en la misma sede
                var mesaExistente = await _mesasRepository.Obtener(m =>
                    m.IdSede == modelo.IdSede &&
                    m.NumeroMesa == modelo.NumeroMesa &&
                    m.Estado != "eliminada");

                if (mesaExistente != null)
                    throw new TaskCanceledException("Ya existe una mesa con ese número en esta sede");

                var mesa = _mapper.Map<TBL_Mesas>(modelo);
                mesa.Estado = "libre"; // Estado inicial según requerimiento
                mesa.FechaCreacion = DateTime.Now;
                mesa.Capacidad = modelo.Capacidad ?? 4; // Valor por defecto

                var mesaCreada = await _mesasRepository.Crear(mesa);

                if (mesaCreada.IdMesa == 0)
                    throw new TaskCanceledException("No se pudo crear la mesa");

                // Obtener mesa completa con relaciones
                var query = await _mesasRepository.Consultar(m => m.IdMesa == mesaCreada.IdMesa);
                mesaCreada = query
                    .Include(m => m.IdSedeNavigation)
                    .FirstOrDefault();

                return _mapper.Map<DTO_Mesas>(mesaCreada);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Edit(DTO_Mesas modelo)
        {
            try
            {
                var mesaModelo = _mapper.Map<TBL_Mesas>(modelo);
                var mesaEncontrada = await _mesasRepository.Obtener(m =>
                    m.IdMesa == mesaModelo.IdMesa && m.Estado != "eliminada");

                if (mesaEncontrada == null)
                    throw new TaskCanceledException("La mesa no existe");

                // Verificar si el nuevo número de mesa ya existe en la misma sede
                var mesaConMismoNumero = await _mesasRepository.Obtener(m =>
                    m.IdSede == mesaModelo.IdSede &&
                    m.NumeroMesa == mesaModelo.NumeroMesa &&
                    m.IdMesa != mesaModelo.IdMesa &&
                    m.Estado != "eliminada");

                if (mesaConMismoNumero != null)
                    throw new TaskCanceledException("Ya existe otra mesa con ese número en esta sede");

                mesaEncontrada.NumeroMesa = mesaModelo.NumeroMesa;
                mesaEncontrada.Capacidad = mesaModelo.Capacidad;
                mesaEncontrada.IdSede = mesaModelo.IdSede;

                bool respuesta = await _mesasRepository.Editar(mesaEncontrada);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar la mesa");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ChangeEstado(int idMesa, string nuevoEstado)
        {
            try
            {
                var mesaEncontrada = await _mesasRepository.Obtener(m => m.IdMesa == idMesa);

                if (mesaEncontrada == null)
                    throw new TaskCanceledException("La mesa no existe");

                // Validar estados permitidos
                var estadosPermitidos = new List<string> { "libre", "ocupada", "reservada", "mantenimiento", "eliminada" };
                if (!estadosPermitidos.Contains(nuevoEstado.ToLower()))
                    throw new TaskCanceledException("Estado no válido");

                mesaEncontrada.Estado = nuevoEstado.ToLower();

                bool respuesta = await _mesasRepository.Editar(mesaEncontrada);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo cambiar el estado de la mesa");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}