using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class EstadoCivilCreateViewModel 
    {
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "*")]
        public string descricao { get; set; }
    }
}