using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace BARSTOC_DAL.Repositorios.Contrato
{
    public interface IGenericRepository <TModel> where TModel : class
    {
        Task<TModel> Obtener(Expression<Func<TModel, bool>> filtro);

        Task<TModel> Crear(TModel model);

        Task<bool> Editar(TModel model);

        Task<bool> Eliminar(TModel model);

        Task<IQueryable<TModel>> Consultar(Expression<Func<TModel, bool>> filtro = null);
    }
}
