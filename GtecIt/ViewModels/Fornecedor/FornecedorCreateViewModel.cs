using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class FornecedorCreateViewModel 
    {
        public FornecedorCreateViewModel()
        {
            this.NotasEntradas = new List<NotaEntradaEditViewModel>();
        }


        public long orcamentoid { get; set; }

        #region boelanos para navegacao 
        public bool fornecedor_em_cadastro { get; set; }
        #endregion

        public int Id_grlfornecedor { get; set; }
        public int? Id_grlbasic { get; set; }
        public int Id_grlcdusu { get; set; }
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

        public string NomeFornecedor { get; set; }
        public virtual PessoaViewModel grlbasico { get; set; }
       // public virtual Usuario grlcdusu { get; set; }
        public virtual List<NotaEntradaEditViewModel> NotasEntradas { get; set; }
    }
}