using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class EstadoCivilIndexViewModel 
    {
        public EstadoCivilIndexViewModel()
        {
            Grid = new List<EstadoCivilGridViewModel>();
        }

        public int Id_grlcivil { get; set; }
        
        [Display(Name = "Descrição")]
        public string descricao { get; set; }
        public bool ConsultaTodos { get; set; }
        public List<EstadoCivilGridViewModel> Grid { get; set; }
    }
}