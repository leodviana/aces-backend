using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GtecIt.Infra.Data.Core
{
    public interface IRepositorio<TEntity> where TEntity : class
    {
        TEntity ObterPorId(int id);
        IQueryable<TEntity> ObterTodos();
        IQueryable<TEntity> ObterComFiltro(Expression<Func<TEntity, bool>> predicate);

        TEntity ObterUnico(Expression<Func<TEntity, bool>> predicate);

        bool VerificarExistencia(Expression<Func<TEntity, bool>> predicate);

        //void SalvarOuAtualizar(TEntity entity);
        void Salvar(TEntity entity);
        void Atualizar(TEntity entity);


        void SalvarLista(IEnumerable<TEntity> entities);

        int SalvarRetornarId(TEntity entity);

        void Remover(TEntity entity);
        void RemoverPorId(int id);
        void RemoverLista(IEnumerable<TEntity> entities);
    }
}