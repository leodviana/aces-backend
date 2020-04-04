using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class TipoTituloIndexViewModel 
    {
        public TipoTituloIndexViewModel()
        {
            Grid = new List<TipoTituloGridViewModel>();
        }

        public int Codigo { get; set; }
        
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        public bool ConsultaTodos { get; set; }
        public List<TipoTituloGridViewModel> Grid { get; set; }
    }
}