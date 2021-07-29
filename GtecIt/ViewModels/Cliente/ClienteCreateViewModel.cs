using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using  System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class ClienteCreateViewModel 
    {
        public ClienteCreateViewModel()
        {
           //this.Orcamentos = new List<OrcamentoEditViewModel>();
            
        }
        
        public long orcamentoid { get; set; }
        public bool cliente_em_cadastro { get; set; }
        public bool cliente_em_cadastro_curso { get; set; }
        public string NomeCliente { get; set; }

        public int id_Grlcliente { get; set; }
        [Required(ErrorMessage = "Selecione o Nome do Aluno")]
        public int? Id_grlbasico { get; set; }
        //public int? Id_grlbasico { get; set; }
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
      //  public virtual ICollection<OrcamentoEditViewModel> Orcamentos { get; set; }
        public List<SelectListItem> Dropdownnome { get; set; }
        public string origem { get; set; }

    }
}