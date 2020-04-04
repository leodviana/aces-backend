using System;
using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class PedidoEditViewModel 
    {
        public int num_saida { get; set; }
        public int? tp_saida { get; set; }
        public DateTime? dt_saida { get; set; }
        public int? cd_cliente { get; set; }
        public int? cd_almox { get; set; }
        public int? cd_vendedor { get; set; }
        public int? cd_trans { get; set; }
        public string obs { get; set; }
        public int? num_nf { get; set; }
        public string serie_nf { get; set; }
        public DateTime? dt_emissao_nf { get; set; }
        public int? cd_prazo { get; set; }
        public string status_pedido { get; set; }
        public int? cd_usuario { get; set; }
        public List<PedidoItemEditViewModel> PedidoItems { get; set; }
    }
}