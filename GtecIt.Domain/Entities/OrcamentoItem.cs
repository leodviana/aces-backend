using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public class OrcamentoItem
    {
        public int id_stqitorcamento { get; set; }
        public int? id_stqporcamento { get; set; }
        public int? id_stqcdprd { get; set; }
        public decimal? qtd { get; set; }
        public decimal? Vl_unitario { get; set; }
        public decimal? desconto { get; set; }
        public decimal? descontoperc { get; set; }
        public string status { get; set; }
        public string cd_usuario { get; set; }
        public virtual Produto produtos { get; set; }
        public virtual Orcamento orcamentos { get; set; }
    }
}
