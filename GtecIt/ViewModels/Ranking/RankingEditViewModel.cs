using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class RankingEditViewModel
    {
        public int id_ranking { get; set; }
        public int id_grlbasico { get; set; }
        public int id_grlbasico_dupla { get; set; }
        public string categoria { get; set; }
        public double pontos { get; set; }
        public int posicao { get; set; }
    }
}