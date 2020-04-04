using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class ProfissaoIndexViewModel 
    {
        public ProfissaoIndexViewModel()
        {
            Grid = new List<ProfissaoGridViewModel>();
        }

        public int Id_grlprofi { get; set; }
        
        [Display(Name = "Descrição")]
        public string descricao { get; set; }
        public bool ConsultaTodos { get; set; }
        public List<ProfissaoGridViewModel> Grid { get; set; }
    }
}