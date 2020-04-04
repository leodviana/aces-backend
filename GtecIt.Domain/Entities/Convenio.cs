using System.Collections.Generic;

namespace GtecIt.Domain.Entities
{
    public class Convenio
    {
        public Convenio()
        {
            //this.stqcporcamentoes = new List<Orcamento>();
        }

        public int id_grlconvenio { get; set; }
        public int? id_grlbasico { get; set; }
        public string Guia { get; set; }

        public string Ativo { get; set; }

        public int? cd_usuario { get; set; }
        public virtual Pessoa grlbasic { get; set; }

        public virtual ICollection<Preco> Precos { get; set; }

        public virtual ICollection<Orcamento> Orcamentos { get; set; }
    }
}
