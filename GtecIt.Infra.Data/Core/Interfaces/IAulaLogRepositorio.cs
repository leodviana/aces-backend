using GtecIt.Domain.Entities;
using GtecIt.Infra.Data.Persistencia.Dto;
using System;
using System.Linq;

namespace GtecIt.Infra.Data.Core.Interfaces
{
    public interface IAulaLogRepositorio : IRepositorio<AulasLog>
    {
        IQueryable<AulaslogDto> ObterAulas(int id, DateTime inicio, DateTime fim,int contrato);
    }
}
