using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class TipoPagamentoCreateViewModel 
    {
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "*")]
        public string Descricao { get; set; }
        public virtual TituloEditViewModel titulos { get; set; }
    }
}