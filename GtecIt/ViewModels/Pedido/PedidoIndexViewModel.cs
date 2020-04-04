using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class PedidoIndexViewModel 
    {
        public PedidoIndexViewModel()
        {
            Grid = new List<PedidoGridViewModel>();
        }

        public bool ConsultaTodos { get; set; }
        public int Numsaida { get; set; }
        public List<PedidoGridViewModel> Grid { get; set; }
    }
}