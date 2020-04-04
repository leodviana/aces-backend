using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class TipoTelefoneIndexViewModel 
    {
        public TipoTelefoneIndexViewModel()
        {
            Grid = new List<TipoTelefoneGridViewModel>();
        }

        public int id_grlidtel { get; set; }
        
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        public bool ConsultaTodos { get; set; }
        public List<TipoTelefoneGridViewModel> Grid { get; set; }
    }
}