using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class OrcamentoCreateViewModel 
    {
        public OrcamentoCreateViewModel()
        {
            Dropdownconvenio = new List<SelectListItem>();
        }

        public int id_Stqcporcamento { get; set; }
        [Required(ErrorMessage = "Informe a Emissão!")]
        public DateTime? Dt_orcamento { get; set; }

        public int? id_grlcliente { get; set; }
        public string CodigoCliente { get; set; }
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "Informe o Aluno!")]
        public string NomeCliente { get; set; }


        [Display(Name = "Professor")]
        [Required(ErrorMessage = "Informe o Professor!")]
        public string NomeDentista { get; set; }

        public string  NomeConvenio { get; set; }

        public int? Id_grldentista { get; set; }
        [MaxLength(45, ErrorMessage = "Tamanho Excedido")]
        public string Obs { get; set; }
        public int? cd_usuario { get; set; }
        public string status { get; set; }
        [Required(ErrorMessage = "Informe o Plano!")]
        public int? id_grlconvenio { get; set; }
        public int? Id_grlcccust { get; set; }
        public int? id_Grltpatendimento { get; set; }
        public DateTime? dt_renovacao { get; set; }
       
        public List<SelectListItem> Dropdownconvenio { get; set; }

        public decimal valor { get; set; }
        
    }
}