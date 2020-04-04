using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public class Plano
    {
        public int idGrlplanos { get; set; }
        public string desc_plano { get; set; }
        public int? cd_usuario { get; set; }

        public virtual ICollection<PrecoPlano> Precosplano { get; set; }
    }
}
