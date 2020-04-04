using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using GtecIt.Infra.Data.Core;


namespace GtecIt.Infra.Data.Persistencia
{

    public class Repositorio<TEntity> : IRepositorio<TEntity> where TEntity : class
    {

        protected readonly DbContext Context;

        public Repositorio(DbContext context)
        {
            Context = context;
        }

        public TEntity ObterPorId(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public IQueryable<TEntity> ObterTodos()
        {
            return Context.Set<TEntity>();
            //return Context.Set<TEntity>().AsNoTracking();
        }

        public IQueryable<TEntity> ObterComFiltro(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
            //return Context.Set<TEntity>().AsNoTracking().Where(predicate);
        }

        public TEntity ObterUnico(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public bool VerificarExistencia(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().AsNoTracking().Any(predicate);
        }

        //public void SalvarOuAtualizar(TEntity entity)
        //{
        //    Context.Set<TEntity>().AddOrUpdate(entity);
        //}

        public void Salvar(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void Atualizar(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void SalvarLista(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public int SalvarRetornarId(TEntity obj)
        {
            Context.Set<TEntity>().Add(obj);

            var key = obj.GetType().GetProperties().AsQueryable().ToArray();

            Context.SaveChanges();

            return Convert.ToInt32(key[0].GetValue(obj));
        }

        public void Remover(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoverPorId(int id)
        {
            var entity = Context.Set<TEntity>().Find(id);
            if (entity != null)
            {
                if (Context.Entry<TEntity>(entity).State == EntityState.Detached)
                {
                    Context.Set<TEntity>().Attach(entity);
                }
                Context.Set<TEntity>().Remove(entity);
                Context.SaveChanges();
            }

        }

        public void RemoverLista(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }
    }
}


    