using System.Data.Entity;
using GtecIt.Domain.Entities;
using GtecIt.Infra.Data.Core.Interfaces;


namespace GtecIt.Infra.Data.Persistencia.Repositorios
{
    public class TipoAtendimentoRepositorio : Repositorio<TipoAtendimento>, ITipoAtendimentoRepositorio
    {
         public TipoAtendimentoRepositorio(DbContext context)
            : base(context)
        {
        }

        public GtecContext GtecContext
        {
            get { return Context as GtecContext; }
        }

       
    }
}
