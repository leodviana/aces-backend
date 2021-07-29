using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class DentistaCreateViewModel 
    {
        public DentistaCreateViewModel()
        {
            //this.Orcamentos = new List<OrcamentoEditViewModel>();
            Dropdownnome = new List<SelectListItem>();
        }

        public long orcamentoid { get; set; }

        #region boelanos para navegacao 

        public bool  dentista_em_cadastro { get; set;}
        

        #endregion

        public int id_grldentista { get; set; }

        [Required(ErrorMessage = "Selecione o Nome do Vendedor")]
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
        public virtual PessoaViewModel Idgrlbasic { get; set; }
        public string NomeDentista { get; set; }
      //  public List<OrcamentoEditViewModel> Orcamentos { get; set; }
        public List<SelectListItem> Dropdownnome { get; set; }
        public string origem { get; set; }
        //  public virtual ICollection<CefalometriaItemEditViewModel> cefalometriaItem { get; set; }
    }
}