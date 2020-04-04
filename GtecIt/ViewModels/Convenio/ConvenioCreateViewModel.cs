using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class ConvenioCreateViewModel 
    {
        public ConvenioCreateViewModel()
        {
            //this.Orcamentos = new List<OrcamentoEditViewModel>();
        }

        public int id_grlconvenio { get; set; }
        public int? id_grlbasico { get; set; }
        public string Guia { get; set; }
        public int? cd_usuario { get; set; }
        public string Ativo { get; set; }
        public virtual PessoaViewModel grlbasic { get; set; }
        public IEnumerable<SelectListItem> Dropdownguia
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

      //  public virtual ICollection<OrcamentoEditViewModel> Orcamentos { get; set; }
    }
}