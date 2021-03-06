﻿using System;
using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class NotaEntradaCreateViewModel
    {
        public NotaEntradaCreateViewModel()
        {
            this.titulosapagar = new List<TituloapagarEditViewModel>();
            this.itens_entrada = new List<NotaEntradaItemEditViewModel>();
        }

        public int Id_stqnoten { get; set; }
        public int? Id_stqtpent { get; set; }
        public DateTime? dt_entrada { get; set; }
        public int? Id_grlfornecedor { get; set; }

        public string CodigoFornecedor { get; set; }
        public string NomeFornecedor { get; set; }

        public int? Id_stqalmox { get; set; }
        public int? Id_grlcccust { get; set; }
        public int? num_nf { get; set; }
        public string serie_nf { get; set; }
        public DateTime? dt_emissao_nf { get; set; }
        public int? Id_stqtpnot { get; set; }
        public string historico_nf { get; set; }
        public int? Id_grlcdusu { get; set; }

        public  decimal valor { get; set; }
        public string status { get; set; }
        public virtual ICollection<TituloapagarEditViewModel> titulosapagar { get; set; }
        public virtual CentrodeCustoEditViewModel CentrodeCustos { get; set; }
        //public virtual Usuario usuarios { get; set; }
        public virtual FornecedorEditViewModel grlfornecedor { get; set; }
        //public virtual stqalmox stqalmox { get; set; }
        public virtual ICollection<NotaEntradaItemEditViewModel> itens_entrada { get; set; }
        //public virtual TipoEntrada TipoEntrada { get; set; }
        //public virtual TipoNota TipoNotas { get; set; }
    }
}