using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GtecIt.Domain.Entities;
using GtecIt.Infra.Data.Core;

namespace GtecIt.Controllers.Api
{
    [RoutePrefix("Api/Banco")]
    public class BancoapiController : ApiController
    {
        private readonly IUnitOfWork _uoW;

        public BancoapiController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public IQueryable<Banco> GetPacientes()
        {
            var model = _uoW.Bancos.ObterTodos().OrderBy(x => x.desc_banco);
            return model;
            //string teste = "";
        }
    }
}
