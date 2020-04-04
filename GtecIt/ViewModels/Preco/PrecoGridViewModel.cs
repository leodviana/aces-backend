using System;

namespace GtecIt.ViewModels
{
    public class PrecoGridViewModel 
    {
        public int id_grlproceprecos { get; set; }
        public int? id_grlconvenio { get; set; }
        public int? id_stqcdprd { get; set; }
        public DateTime? vigencia { get; set; }
        public decimal? preco { get; set; }
        public int? cd_usuario { get; set; }
    }
}