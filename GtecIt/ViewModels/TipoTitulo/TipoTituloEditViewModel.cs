using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class TipoTituloEditViewModel 
    {
        public int Codigo { get; set; }
        
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "*")]
        public string Descricao { get; set; }
    }
}