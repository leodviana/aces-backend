using System.Collections.Generic;
using System.Web.Mvc;
namespace GtecIt.ViewModels
{
    public class DentistaEditViewModelx 
    {
        public DentistaEditViewModelx()
        {
            this.Orcamentos = new List<OrcamentoEditViewModel>();
            this.cefalometriaItem = new List<CefalometriaItemEditViewModel>();
        }

        public int id_grldentista { get; set; }
        public int? Id_grlbasico { get; set; }
        //public string cd_usuario { get; set; }
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
        public virtual PessoaEditViewModel Idgrlbasic { get; set; }

        public virtual ICollection<OrcamentoEditViewModel> Orcamentos { get; set; }

        public virtual ICollection<CefalometriaItemEditViewModel> cefalometriaItem { get; set; }


    }
}