using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public class Ranking
    {
        public int id_ranking { get; set; }
        public int id_grlbasico { get; set; }
        public string categoria { get; set; }
        public double pontos { get; set; }
        public int id_grlbasico_dupla { get; set; }
        public int posicao { get; set; }

    }
}
