using System;

namespace GtecIt.ViewModels
{
    public class RankingGridViewModel 
    {
        public int id_ranking { get; set; }
        public int id_grlbasico { get; set; }
        public int id_grlbasico_dupla { get; set; }
        public string nome_dupla { get; set; }
        public string nome { get; set; }
        public string categoria { get; set; }
        public double pontos { get; set; }
        public int posicao { get; set; }
    }
}