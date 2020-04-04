using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class ProfissaoEditViewModel 
    {
        public int Id_grlprofi { get; set; }
        
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "*")]
        public string descricao { get; set; }
        public List<ProfissaoEditViewModel> ProfissaoList { get; set; }
    }
}