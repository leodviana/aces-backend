using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class TituloCreateViewModel 
    {

         public TituloCreateViewModel()
        {
           
            
            
        }
         public int id_fintitrc { get; set; }
         public DateTime? dt_emissao { get; set; }
         public int? id_stqcporcamento { get; set; }
         public string num_titulo { get; set; }
         public DateTime? dt_vencimento { get; set; }
         public decimal? Valor { get; set; }
         public decimal? Valor_pago { get; set; }
         public DateTime? dt_pagamento { get; set; }
         public int? id_fintipotitulo { get; set; }
         public int? id_fintipopagamento { get; set; }
         public string doc_pagamento { get; set; }
         public string agencia { get; set; }
         public int? cd_banco { get; set; }
         public string cd_usuario { get; set; }
         public string num_conta { get; set; }
         public string status { get; set; }
         public int parcelas { get; set; }
         public List<TituloGridViewModel> Grid { get; set; }
         public virtual OrcamentoEditViewModel orcamento { get; set; }
         public virtual TipoPagamentoEditViewModel tp_pagamento { get; set; }

        public string Mensagem { get; set; }

        public string Icone { get; set; }

        public string alerta { get; set; }

        public bool resultado { get; set; }


    }
}