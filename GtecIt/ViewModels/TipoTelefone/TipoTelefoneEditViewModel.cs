using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class TipoTelefoneEditViewModel 
    {
        public int id_grlidtel { get; set; }
        
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "*")]
        public string Descricao { get; set; }
    }
}