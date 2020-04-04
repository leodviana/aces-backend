using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GtecIt.Controllers.Api
{
    public class testeController : ApiController
    {
        // GET: api/teste
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/teste/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/teste
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/teste/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/teste/5
        public void Delete(int id)
        {
        }
    }
}
