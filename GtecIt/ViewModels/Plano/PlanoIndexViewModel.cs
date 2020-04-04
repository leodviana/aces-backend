using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class PlanoIndexViewModel 
    {
        public PlanoIndexViewModel()
        {
            Grid = new List<PlanoGridViewModel>();
        }

        public int idGrlplanos { get; set; }
        public string desc_plano { get; set; }
        public int? cd_usuario { get; set; }
        public bool ConsultaTodos { get; set; }
        public List<PlanoGridViewModel> Grid { get; set; }
    }
}