using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class TipoPagamentoIndexViewModel 
    {
        public TipoPagamentoIndexViewModel()
        {
            Grid = new List<TipoPagamentoGridViewModel>();
        }

        public int Codigo { get; set; }
        
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        public bool ConsultaTodos { get; set; }
        public List<TipoPagamentoGridViewModel>  Grid { get; set; }
    }
}