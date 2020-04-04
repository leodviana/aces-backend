using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public class CentrodeCusto
    {
        public CentrodeCusto()
        {
            this.NotasEntradas = new List<NotaEntrada>();
            this.orcamentos = new List<Orcamento>();
        }

        public int Id_grlccust { get; set; }
        public string desc_ccusto { get; set; }
        public int? Id_grlcdusu { get; set; }
        public string Ativo { get; set; }
       // public virtual Usuario grlcdusu { get; set; }
        public virtual ICollection<NotaEntrada> NotasEntradas { get; set; }
        public virtual ICollection<Orcamento> orcamentos { get; set; }
    }
}

