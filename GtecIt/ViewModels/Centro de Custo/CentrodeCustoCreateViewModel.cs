using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class CentrodeCustoCreateViewModel 
    {
        

        public int Id_grlccust { get; set; }
        public string desc_ccusto { get; set; }
        public int? Id_grlcdusu { get; set; }
        public string Ativo { get; set; }
        //public virtual Usuario grlcdusu { get; set; }
       // public virtual List<NotaEntradaEditViewModel> NotasEntradas { get; set; }
    }
}