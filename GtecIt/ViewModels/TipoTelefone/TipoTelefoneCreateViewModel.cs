using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class TipoTelefoneCreateViewModel 
    {
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "*")]
        public string Descricao { get; set; }
    }
}