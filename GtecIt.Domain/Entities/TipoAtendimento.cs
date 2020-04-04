using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public class TipoAtendimento
    {
        public TipoAtendimento()
        {
            this.orcamentos = new List<Orcamento>();
        }

        public int id_Grltpatendimento { get; set; }
        public string desc_Grltpatendimento { get; set; }
        public string cd_usuario { get; set; }
        public virtual ICollection<Orcamento> orcamentos { get; set; }
    }
}
