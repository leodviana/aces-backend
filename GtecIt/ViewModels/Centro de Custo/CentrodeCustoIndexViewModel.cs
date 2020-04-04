using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class CentrodeCustoIndexViewModel
    {
        public CentrodeCustoIndexViewModel()
        {
            Grid = new List<CentrodeCustoGridViewModel>();
        }

        public int Id_grlccust { get; set; }
        public string desc_ccusto { get; set; }
        public int? Id_grlcdusu { get; set; }
        public string Ativo { get; set; }
        public bool ConsultaTodos { get; set; }
        public List<CentrodeCustoGridViewModel> Grid { get; set; }
    }
}