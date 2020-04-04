using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class CentrodeCustoEditViewModel 
    {
        public CentrodeCustoEditViewModel()
        {
            this.NotasEntradas = new List<NotaEntradaEditViewModel>();
        }

        public int Id_grlccust { get; set; }
        public string desc_ccusto { get; set; }
        public int? Id_grlcdusu { get; set; }
        public string Ativo { get; set; }

        public IEnumerable<SelectListItem> Dropdownativo
        {
            get
            {
                var lst = new List<SelectListItem>
                {
                    new SelectListItem {Text = "SIM", Value = "S"},
                    new SelectListItem {Text = "Não", Value = "N"}
                };
                return lst;
            }
        }
        //public virtual Usuario grlcdusu { get; set; }
        public virtual List<NotaEntradaEditViewModel> NotasEntradas { get; set; }
    }
}