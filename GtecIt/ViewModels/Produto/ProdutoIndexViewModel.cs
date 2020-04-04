using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class ProdutoIndexViewModel 
    {
        public ProdutoIndexViewModel()
        {
            Grid = new List<ProdutoGridViewModel>();
        }

        public bool ConsultaTodos { get; set; }
        public int Id_stqcdprd { get; set; }
        public int? cd_produto { get; set; }
        public string desc_produto { get; set; }
        public int? Id_stqcdgrp { get; set; }

        public string nome_grupo { get; set; }

        public int? Id_stqsbgrp { get; set; }
        public string nome_subgrupo { get; set; }
       
        public List<ProdutoGridViewModel> Grid { get; set; }

       
    }
}