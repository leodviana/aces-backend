using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Infra.Data.Persistencia.Dto
{
    public class OrcamentoDto
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
        public int? id_grlconvenio { get; set; }
        public string nome_aluno { get; set; }
        public string nome_professor { get; set; }
        public DateTime? dt_renovacao { get; set; }
       

    }
}
