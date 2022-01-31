using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class PrecoPlanoEditViewModel 
    {
        public PrecoPlanoEditViewModel()
        {
            DropdownConvenio = new List<SelectListItem>();
        }
        public int idgrlplanosprecos { get; set; }
        [Required(ErrorMessage = "Selecione a Tabela de Planos!")]
        public int? idGrlplanos { get; set; }
        [Required(ErrorMessage = "Selecione o Plano!")]
        public int? id_stqcdprd { get; set; }
        [Required(ErrorMessage = "Informe a Vigência!")]
        public DateTime? vigencia { get; set; }
        [Required(ErrorMessage = "Informe Preço!")]
        [Range(0.00, Double.PositiveInfinity)]
        //[Range(typeof(Decimal), "0.0", "1000000000000000000")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public decimal? preco { get; set; }
        public int? cd_usuario { get; set; }
        [Required(ErrorMessage = "Informe a quantidade de aulas!")]
        public int qtd_aulas { get; set; }

        [Required(ErrorMessage = "Informe a quantidade de dias do plano!")]
        public int qtd_dias_plano { get; set; }
        public decimal? valor2 { get; set; }
        public virtual PlanoEditViewModel planos { get; set; }
        public virtual ProdutoEditViewModel produtos { get; set; }
        public List<SelectListItem> DropdownConvenio { get; set; }
    }
}