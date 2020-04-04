using System;
using System.Collections.Generic;
using System.Web.Mvc;
namespace GtecIt.ViewModels
{
    public class TituloapagarEditViewModel
    {
        
         public TituloapagarEditViewModel()
        {

            Dropdowntpagamento = new List<SelectListItem>();
            Dropdownbanco = new List<SelectListItem>();
            
        }
        public int id_fintitpg { get; set; }
        public DateTime? dt_emissao { get; set; }
        public int? Id_stqnoten { get; set; }
        public string num_titulo { get; set; }
        public DateTime? dt_vencimento { get; set; }
        public decimal? valor { get; set; }
        public decimal? Valor_pago { get; set; }
        public DateTime? dt_pagamento { get; set; }
        public int? id_fintipotitulo { get; set; }
        public int? id_fintipopagamento { get; set; }
        public string doc_pagamento { get; set; }
        public string agencia { get; set; }
        public int? cd_banco { get; set; }
        public int? cd_usuario { get; set; }
        public string num_conta { get; set; }
        public string Status { get; set; }
        public virtual BancoEditViewModel fincdbanco { get; set; }

        public virtual NotaEntradaEditViewModel NotaEntradas { get; set; }

        public List<TituloapagarGridViewModel> Grid { get; set; }
         
         public virtual TipoPagamentoEditViewModel tp_pagamento { get; set; }

         public virtual BancoEditViewModel banco { get; set; }

         public List<SelectListItem> Dropdowntpagamento { get; set; }
         public List<SelectListItem> Dropdownbanco { get; set; }
        public string Mensagem { get; set; }

        public string Icone { get; set; }

        public string alerta { get; set; }

        public bool resultado { get; set; }
    }
}