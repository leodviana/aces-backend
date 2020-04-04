using System.Data.Entity;
using GtecIt.Domain.Entities;
using GtecIt.Infra.Data.Core.Interfaces;
using GtecIt.Infra.Data.Persistencia;


namespace GtecIt.Infra.Data.Persistencia.Repositorios
{
    public class PedidoRepositorio :Repositorio<Pedido>, IPedidoRepositorio
    {
         public PedidoRepositorio(DbContext context)
            : base(context)
        {
        }

        public GtecContext GtecContext
        {
            get { return Context as GtecContext; }
        }

        
    }
}
