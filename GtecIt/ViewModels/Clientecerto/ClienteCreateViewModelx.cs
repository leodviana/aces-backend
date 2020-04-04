using System;
using System.Collections.Generic;
using  System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class ClienteCreateViewModelx 
    {
        public ClienteCreateViewModelx()
        {
            this.Orcamentos = new List<OrcamentoEditViewModel>();
        }


        public long orcamentoid { get; set; }
        public bool cliente_em_cadastro { get; set; }
        public bool cliente_em_cadastro_curso { get; set; }
        public string NomeCliente { get; set; }

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