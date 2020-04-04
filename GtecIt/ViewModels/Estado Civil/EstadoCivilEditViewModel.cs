using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class EstadoCivilEditViewModel 
    {
        public int Id_grlcivil { get; set; }
        
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "*")]
        public string descricao { get; set; }
        public List<EstadoCivilEditViewModel> EstadosList { get; set; }
    }
}