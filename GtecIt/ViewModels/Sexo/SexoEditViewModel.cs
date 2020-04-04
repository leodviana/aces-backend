using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class SexoEditViewModel 
    {
        public int Id_gercdsexo { get; set; }
        
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "*")]
        public string descricao { get; set; }
    }
}