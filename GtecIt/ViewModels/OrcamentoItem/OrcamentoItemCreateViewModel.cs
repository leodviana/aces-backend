using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class OrcamentoItemCreateViewModel 
    {

         public OrcamentoItemCreateViewModel()
        {
            
            DropdownProduto = new List<SelectListItem>();
            
        }
        public int id_stqitorcamento { get; set; }
        public int? id_stqporcamento { get; set; }
        public int? id_stqcdprd { get; set; }
        public decimal? qtd { get; set; }
        public decimal? Vl_unitario { get; set; }

        public decimal? desconto { get; set; }
        public decimal? descontoperc { get; set; }
        public string status { get; set; }
        public string cd_usuario { get; set; }

        public decimal? Valor_total
        {
            get
            {
                
                decimal? valor_final;
                decimal valor_total = (Convert.ToDecimal((qtd) * Convert.ToDecimal(Vl_unitario))) - Convert.ToDecimal(desconto);
                if (descontoperc != 0)
                {
                    valor_total = valor_total - (valor_total * Convert.ToDecimal(descontoperc) / 100);
                    valor_final = valor_total;
                }
                else
                {
                    valor_final = valor_total;
                }
               

                return valor_final;
            }

        }

        public int? id_grlconvenio { get; set; }

        public virtual ProdutoEditViewModel produtos { get; set; }
        // melhorar a performance public virtual OrcamentoEditViewModel orcamentos { get; set; }
        public List<SelectListItem> DropdownProduto { get; set; }

        


    }
}