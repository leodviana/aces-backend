using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class TipoTituloCreateViewModel 
    {
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "*")]
        public string Descricao { get; set; }
    }
}