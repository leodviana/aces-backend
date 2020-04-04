using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public class Aulas
    {
        public int idGercdaulas { get; set; }
        public int? id_Stqcporcamento { get; set; }
        public DateTime? inicio { get; set; }
        public DateTime? final { get; set; }
        public string dia_semana { get; set; }
        public string status { get; set; }
        public virtual Orcamento orcamentos { get; set; }
    }
}
