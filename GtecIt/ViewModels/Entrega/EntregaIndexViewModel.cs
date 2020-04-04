using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class EntregaIndexViewModel 
    {
        public EntregaIndexViewModel()
        {
            Grid = new List<EntregaGridViewModel>();
        }

        public int idEntrega { get; set; }
        public string Desc_entrega { get; set; }
        public bool ConsultaTodos { get; set; }
        public List<EntregaGridViewModel> Grid { get; set; }
    }
}