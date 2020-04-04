using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class ProfissaoCreateViewModel 
    {
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "*")]
        public string descricao { get; set; }
    }
}