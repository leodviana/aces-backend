

namespace GtecIt.ViewModels
{
    public class OrcamentoItemGridViewModel 
    {

        public int id_stqitorcamento { get; set; }
        public int? id_stqporcamento { get; set; }
        public int? id_stqcdprd { get; set; }
        public decimal? qtd { get; set; }
        public decimal? Vl_unitario { get; set; }
        public decimal? desconto { get; set; }
        public decimal? descontoperc { get; set; }
        public string status { get; set; }
        public string cd_usuario { get; set; }
        public virtual ProdutoEditViewModel produtos { get; set; }
        //melhora public virtual OrcamentoEditViewModel orcamentos { get; set; }
       }
}