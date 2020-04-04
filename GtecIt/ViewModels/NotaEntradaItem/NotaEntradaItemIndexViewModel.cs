using System;
using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class NotaEntradaItemIndexViewModel
    {
        public NotaEntradaItemIndexViewModel()
        {
            Grid = new List<NotaEntradaItemGridViewModel>();
        }

        public int Id_stqentra { get; set; }
        public int id_stqnoten { get; set; }
        public int num_item { get; set; }
        public int? cd_almox { get; set; }
        public int tp_entrada { get; set; }
        public DateTime? dt_entrada { get; set; }
        public int? cd_produto { get; set; }
        public decimal? qtd_entrada { get; set; }
        public decimal? valor_total { get; set; }
        public string status_entrada { get; set; }
        public int? Id_grlcdusu { get; set; }
        //public virtual Usuario grlcdusu { get; set; }
        public virtual ProdutoEditViewModel stqcdprd { get; set; }
        public virtual NotaEntradaEditViewModel NotasEntradas { get; set; }
        public bool ConsultaTodos { get; set; }
        
        public List<NotaEntradaItemGridViewModel> Grid { get; set; }
    }
}