using System;

namespace GtecIt.ViewModels
{
    public class PrecoPlanoGridViewModel 
    {
        public int idgrlplanosprecos { get; set; }
        public int? idGrlplanos { get; set; }
        public int? id_stqcdprd { get; set; }
        public DateTime? vigencia { get; set; }
        public decimal? preco { get; set; }
        public int qtd_aulas { get; set; }
        public int qtd_dias_plano { get; set; }
        public decimal? valor2 { get; set; }
        public int? cd_usuario { get; set; }
    }
}