using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class ClienteEditViewModelx 
    {
        public ClienteEditViewModelx()
        {
            this.Orcamentos = new List<OrcamentoEditViewModel>();
        }

        public int id_Grlcliente { get; set; }
        public int? Id_grlbasico { get; set; }
        public string cd_usuario { get; set; }
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
        public virtual PessoaEditViewModel grlbasic { get; set; }
        public virtual ICollection<OrcamentoEditViewModel> Orcamentos { get; set; }
    }
}