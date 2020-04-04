using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GtecIt.Reports.Entidades
{
    public class Rel001
    {
        public int id_Stqcporcamento { get; set; }
        public DateTime? Dt_orcamento { get; set; }
        public int? id_grlcliente { get; set; }
        public int? Id_grldentista { get; set; }
        public string Obs { get; set; }
        public int? cd_usuario { get; set; }
        public string status { get; set; }

    }
}