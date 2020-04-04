using System;
using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class TituloapagarIndexViewModel
    {
        public TituloapagarIndexViewModel()
        {
            Grid = new List<TituloapagarGridViewModel>();
        }

        public bool ConsultaTodos { get; set; }

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
        
    }
}