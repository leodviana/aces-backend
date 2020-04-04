using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;


namespace GtecIt.ViewModels
{
    public class TipoPagamentoEditViewModel 
    {
      

        public int Codigo { get; set; }
        
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "*")]
        public string Descricao { get; set; }
        public virtual TituloEditViewModel titulos { get; set; }

       
    }
}