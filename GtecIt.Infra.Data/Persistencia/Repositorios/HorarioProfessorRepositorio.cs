﻿using System.Data.Entity;
using GtecIt.Domain.Entities;
using GtecIt.Infra.Data.Core.Interfaces;

namespace GtecIt.Infra.Data.Persistencia.Repositorios
{
    public class HorarioProfessorRepositorio : Repositorio<HorarioProfessor>, IHorarioProfessorRepositorio
    {
        public HorarioProfessorRepositorio(DbContext context) : base(context)
        {
        }

        public GtecContext GtecContext
        {
            get { return Context as GtecContext; }
        }


       
    }
}
