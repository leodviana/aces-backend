using System.Data.Entity;
using GtecIt.Domain.Entities;
using GtecIt.Infra.Data.Core.Interfaces;

namespace GtecIt.Infra.Data.Persistencia.Repositorios
{
    public class EventsRepositorio : Repositorio<Events>, IEventsRepositorio
    {
        public EventsRepositorio(DbContext context) : base(context)
        {
        }

        public GtecContext GtecContext
        {
            get { return Context as GtecContext; }
        }


       
    }
}
