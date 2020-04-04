using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class CefalometriaIndexViewModel 
    {
        public CefalometriaIndexViewModel()
        {
           // cefalometriaItems = new List<CefalometriaItemEditViewModel>();
            Grid = new List<CefalometriaGridViewModel>();
        }

        public int id_GrlCefalometrias { get; set; }
        public string desc_cefalometria { get; set; }
        public int? cd_usuario { get; set; }
        public bool ConsultaTodos { get; set; }

       // public virtual ICollection<CefalometriaItemEditViewModel> cefalometriaItems { get; set; }
        public List<CefalometriaGridViewModel> Grid { get; set; }
    }
}