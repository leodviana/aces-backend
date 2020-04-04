using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.WebPages.Html;

namespace GtecIt.ViewModels
{
    public class PessoaIndexViewModelx 
    {
        public PessoaIndexViewModelx()
        {
            Grid = new List<PessoaGridViewModel>();
        }

        public bool ConsultaTodos { get; set; }
        public int Id_grlbasico { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "*")]
        public string nome { get; set; }

        public string razao_social { get; set; }
        
        
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? dt_nascimento { get; set; }
        
        public string contato { get; set; }
        public string ddd_telefone { get; set; }
        public string ddd_telefone2 { get; set; }
        public string email { get; set; }
        public string Email2 { get; set; }
        
        public List<PessoaGridViewModel> Grid { get; set; }

        public string tipo_pessoa { get; set; }

        public string status { get; set; }
        
        public List<SelectListItem> DropDownTipoPessoa
        {
            get
            {
                var lst = new List<SelectListItem>
                {
                    new SelectListItem {Text = "Física", Value = "F"},
                    new SelectListItem {Text = "Jurídica", Value = "J"}
                };
                return lst;
            }
        }
    }
}