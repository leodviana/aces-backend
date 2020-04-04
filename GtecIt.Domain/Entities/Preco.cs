using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public class Preco
    {
        public int id_grlproceprecos { get; set; }
        public int? id_grlconvenio { get; set; }
        public int? id_stqcdprd { get; set; }
        public DateTime? vigencia { get; set; }
        public decimal? preco { get; set; }
        public int? cd_usuario { get; set; }
        public virtual Convenio convenios { get; set; }
        public virtual Produto produtos { get; set; }
    }
}
