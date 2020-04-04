using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class PessoaEditViewModel 
    {
        public PessoaEditViewModel()
        {
            Enderecos = new List<EnderecoEditViewModel>();
           
            /*  clientes = new List<ClienteEditViewModel>();
           // dentistas = new List<DentistaEditViewModel>();*/
            EstadoCivil = new List<EstadoCivilEditViewModel>();
            Tipo01 = new List<TipoTelefoneEditViewModel>();
            DropdownIdentifica = new List<SelectListItem>();
            DropdownSexo = new List<SelectListItem>();
            DropdownEstado = new List<SelectListItem>();
        }
        public int Id_grlbasico { get; set; }

        [Required(ErrorMessage = "Informe o Campo Nome")]
        public string nome { get; set; }

        public string razao_social { get; set; }
        public string Nome_fantasia { get; set; }
        public string cgc { get; set; }

        public string insc_municipal { get; set; }
        public string insc_estadual { get; set; }

        public int sexo { get; set; }
        [Display(Name = "Data Nascimento")]
        [DataType(DataType.Date)]
        // [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Required(ErrorMessage = "Informe a Data de Nascimento")]
        public DateTime? dt_nascimento { get; set; }
        public int? Id_grlprofi { get; set; }
        public string NomeProfissao { get; set; }
        public int? Id_grlcidad { get; set; }
        public int? Id_grlcivil { get; set; }
        public string cpf { get; set; }
        public string rg { get; set; }
        public string contato { get; set; }
        public string ddd_telefone { get; set; }
        public int? Id_grlidtel { get; set; }
        public string ddd_telefone2 { get; set; }
        public int? Id_grlidtel02 { get; set; }
        public string email { get; set; }
        public string Email2 { get; set; }
        public int? Id_grlcdusu { get; set; }

        public string Id_grltopessoa { get; set; }
        public string status { get; set; }
        public IEnumerable<SelectListItem> DropDownTipoPessoa
        {
            get
            {
                return  new List<SelectListItem>
                {
                    new SelectListItem {Text = "Física", Value = "F"},
                    new SelectListItem {Text = "Jurídica", Value = "J"}
                };
            }
        }
       public List<SelectListItem> DropdownIdentifica { get; set; }
        public List<SelectListItem> DropdownSexo { get; set; }
        public List<SelectListItem> DropdownEstado { get; set; }
        public List<EstadoCivilEditViewModel> EstadoCivil { get; set; }
        public List<TipoTelefoneEditViewModel> Tipo01 { get; set; }
        public List<EnderecoEditViewModel>Enderecos { get; set; }
        
       
        //public List<ClienteEditViewModel> clientes { get; set; }
        /*public List<DentistaEditViewModel> dentistas { get; set; }*/

    }
}