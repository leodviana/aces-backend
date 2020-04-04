using System;

namespace GtecIt.ViewModels
{
    public class PedidoItemCreateViewModel 
    {
        public int Id_saida { get; set; }
        public int num_saida { get; set; }
        public int num_item { get; set; }
        public int tp_saida { get; set; }
        public int? cd_almox { get; set; }
        public DateTime? dt_saida { get; set; }
        public int? cd_produto { get; set; }
        public decimal? qtd_saida { get; set; }
        public decimal? valor_unitario { get; set; }
        public decimal? valor_total { get; set; }
        public decimal? valor_desconto { get; set; }
        public decimal? valor_ipi { get; set; }
        public decimal? qtd_saida_opc { get; set; }
        public string und_opc { get; set; }
        public decimal? valor_total_opc { get; set; }
        public string status_saida { get; set; }
        public long? cd_usuario { get; set; }
        public decimal? vl_desc_perc { get; set; }
        public  PedidoEditViewModel Pedido { get; set; }
    }
}