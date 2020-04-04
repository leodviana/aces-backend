using System;
using System.Collections.Generic;

using GtecIt.Domain.Entities;

namespace GtecIt.ViewModels
{
    public class OrcamentoGridViewModel 
    {
        public int id_Stqcporcamento { get; set; }
        public DateTime? Dt_orcamento { get; set; }
        public int? id_grlcliente { get; set; }
        public int? Id_grldentista { get; set; }
        public string Obs { get; set; }
        public int? cd_usuario { get; set; }
        public string status { get; set; }
        public int? Id_grlcccust { get; set; }
        public int? id_Grltpatendimento { get; set; }
        public int? id_entrega { get; set; }
        public DateTime? dt_entrega { get; set; }

        public string nome_aluno { get; set; }
        public string nome_professor { get; set; }
        public DateTime? dt_renovacao { get; set; }
        public virtual ClienteEditViewModel grlcliente { get; set; }
        public virtual DentistaEditViewModel grldentista { get; set; }

     //   public virtual ICollection<Titulo> Titulos { get; set; }
      //  public virtual ConvenioEditViewModel Convenios { get; set; }
        //public virtual CentrodeCustoIndexViewModel grlccust { get; set; }

    }
}