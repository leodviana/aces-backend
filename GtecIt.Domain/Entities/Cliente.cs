using System;
using System.Collections.Generic;

namespace GtecIt.Domain.Entities
{
    public  class Cliente
    {

        public Cliente()
        {
            this.Orcamentos = new List<Orcamento>();
        }

        public int id_Grlcliente { get; set; }
        public int? Id_grlbasico { get; set; }
        public string cd_usuario { get; set; }
        public string Ativo { get; set; }
        public virtual Pessoa grlbasic { get; set; }
        public virtual ICollection<Orcamento> Orcamentos { get; set; }

    }
}
