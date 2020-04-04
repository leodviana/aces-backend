using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public class HorarioProfessor
    {
        public int idgercdhorarioProf { get; set; }
        public int? id_grldentista { get; set; }
        public string horario { get; set; }
       
        public string status  { get; set; }
        public string Dia { get; set; }
        public virtual Dentista dentistas { get; set; }
        public int? id_Stqcporcamento { get; set; }
        public int? id_Stqcporcamento_dupla { get; set; }
        public virtual Orcamento orcamentos { get; set; }
             

       
    }
}
