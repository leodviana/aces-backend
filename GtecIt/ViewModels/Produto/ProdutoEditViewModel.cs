using System;
using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class ProdutoEditViewModel 
    {
        public ProdutoEditViewModel()
        {
            //this.Orcamentos = new List<OrcamentoEditViewModel>();
        }

        public int Id_stqcdprd { get; set; }
        public int? cd_produto { get; set; }
        public string desc_produto { get; set; }
        public int? Id_stqcdgrp { get; set; }

        public string nome_grupo { get; set; }

        public int? Id_stqsbgrp { get; set; }
        public string nome_subgrupo { get; set; }

        //  public int? Id_stqvlunmed_con { get; set; }


        private int? _Id_stqvlunmed_con = 1;


        public int? Id_stqvlunmed_con
        {
            get { return _Id_stqvlunmed_con; }
            set { _Id_stqvlunmed_con = value; }
        }

        private int? _Id_stqvlunmed_est = 1;

        public int? Id_stqvlunmed_est
        {
            get { return _Id_stqvlunmed_est; }
            set { _Id_stqvlunmed_est = value; }
        }
        public virtual ICollection<PrecoPlanoEditViewModel> precosplano { get; set; }
    }
}