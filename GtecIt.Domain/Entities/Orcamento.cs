using System;
using System.Collections.Generic;

namespace GtecIt.Domain.Entities
{
    public class Orcamento
    {
        public int id_Stqcporcamento { get; set; }
        public DateTime? Dt_orcamento { get; set; }
        public int? id_grlcliente { get; set; }
        public int? Id_grldentista { get; set; }

        public int? Id_grlcccust { get; set; }
        public int? id_Grltpatendimento { get; set; }
        public int? id_grlconvenio { get; set; }
        public string Obs { get; set; }
        public int? cd_usuario { get; set; }
        public string status { get; set; }
        public virtual Cliente grlcliente { get; set; }
        public virtual Dentista grldentista { get; set; }
        public int? id_entrega { get; set; }
        public DateTime? dt_entrega { get; set; }

        public DateTime? dt_renovacao { get; set; }

        public virtual Convenio Convenios { get; set; }

        public virtual ICollection<OrcamentoItem> itemorcamentos { get; set; }
        public virtual ICollection<Titulo> Titulos { get; set; }
        // public virtual ICollection<Aulas> Aulas { get; set; }
        //public virtual HorarioProfessor Aulas { get; set; }
        public virtual CentrodeCusto grlccust { get; set; }

        public virtual TipoAtendimento tipoatendimentos { get; set; }
        public virtual ICollection<HorarioProfessor> aulas { get; set; }
        public virtual ICollection<Aulas> horarios { get; set; }
    }
}
