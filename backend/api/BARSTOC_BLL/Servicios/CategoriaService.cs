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
    public class CategoriaService : ICategoriaService
    {
        private readonly IGenericRepository<TBL_Categoria> _categoriaRepository;
        private readonly IMapper _mapper;

        public CategoriaService(IGenericRepository<TBL_Categoria> categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<List<DTO_Categoria>> List()
        {
            try
            {
                // ✅ CORREGIDO: Consultar en lugar de Consult
                var queryCategoria = await _categoriaRepository.Consultar();
                var listCategorias = queryCategoria.ToList();
                return _mapper.Map<List<DTO_Categoria>>(listCategorias);
            }
            catch
            {
                throw;
            }
        }

        public async Task<DTO_Categoria> Create(DTO_Categoria modelo)
        {
            try
            {
                // ✅ CORREGIDO: Crear en lugar de Create
                var categoriaCreada = await _categoriaRepository.Crear(_mapper.Map<TBL_Categoria>(modelo));

                if (categoriaCreada.IdCategoria == 0)
                    throw new TaskCanceledException("No se pudo crear la categoría");

                return _mapper.Map<DTO_Categoria>(categoriaCreada);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Edit(DTO_Categoria modelo)
        {
            try
            {
                var categoriaModelo = _mapper.Map<TBL_Categoria>(modelo);
                // ✅ CORREGIDO: Obtener en lugar de Get
                var categoriaEncontrada = await _categoriaRepository.Obtener(c =>
                    c.IdCategoria == categoriaModelo.IdCategoria);

                if (categoriaEncontrada == null)
                    throw new TaskCanceledException("La categoría no existe");

                categoriaEncontrada.NombreCategoria = categoriaModelo.NombreCategoria;
                categoriaEncontrada.Estado = categoriaModelo.Estado;

                // ✅ CORREGIDO: Editar en lugar de Edit
                bool respuesta = await _categoriaRepository.Editar(categoriaEncontrada);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar la categoría");

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
                // ✅ CORREGIDO: Obtener en lugar de Get
                var categoriaEncontrada = await _categoriaRepository.Obtener(c => c.IdCategoria == id);

                if (categoriaEncontrada == null)
                    throw new TaskCanceledException("La categoría no existe");

                // Cambiar estado en lugar de eliminar (según requerimientos)
                categoriaEncontrada.Estado = "Inactivo";

                // ✅ CORREGIDO: Editar en lugar de Edit
                bool respuesta = await _categoriaRepository.Editar(categoriaEncontrada);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo desactivar la categoría");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}