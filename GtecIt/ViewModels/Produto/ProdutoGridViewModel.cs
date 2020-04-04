using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class ProdutoGridViewModel 
    {
        public ProdutoGridViewModel()
        {
            //this.Orcamentos = new List<OrcamentoEditViewModel>();
        }
        public int Id_stqcdprd { get; set; }
        public int? cd_produto { get; set; }
        public string desc_produto { get; set; }
        public int? Id_stqcdgrp { get; set; }
        public int? Id_stqsbgrp { get; set; }
        
        public virtual GrupoEditViewModel grupos { get; set; }
        //public virtual ICollection<stqitorcamento> stqitorcamentoes { get; set; }
        public virtual SubGrupoEditViewModel subgrupos { get; set; }
        
        
    }
}