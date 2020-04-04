using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public class PrecoPlano
    {
        public int idgrlplanosprecos { get; set; }
        public int? idGrlplanos { get; set; }
        public int? id_stqcdprd { get; set; }
        public DateTime? vigencia { get; set; }
        public decimal? preco { get; set; }
        public int? cd_usuario { get; set; }
        public int qtd_aulas { get; set; } 
        public int qtd_dias_plano { get; set; }
        public virtual Plano Planos { get; set; }
        public virtual Produto produtos { get; set; }
    }
}
