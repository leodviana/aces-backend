using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class SexoIndexViewModel 
    {
        public SexoIndexViewModel()
        {
            Grid = new List<SexoGridViewModel>();
        }

        public int Id_gercdsexo { get; set; }
        
        [Display(Name = "Descrição")]
        public string descricao { get; set; }
        public bool ConsultaTodos { get; set; }
        public List<SexoGridViewModel> Grid { get; set; }
    }
}