using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class CefalometriaItemIndexViewModel 
    {
        public CefalometriaItemIndexViewModel()
        {
           // cefalometriaItems = new List<CefalometriaItemEditViewModel>();
            Grid = new List<CefalometriaItemGridViewModel>();
        }

        public int id_Grlcefdent { get; set; }
        public int? id_grldentista { get; set; }
        public int? id_GrlCefalometrias { get; set; }
        public int? cd_usuario { get; set; }
        public bool ConsultaTodos { get; set; }

        public virtual CefalometriaEditViewModel grlcefalometria { get; set; }

        public virtual DentistaEditViewModel grldentista { get; set; }
        public List<CefalometriaItemGridViewModel> Grid { get; set; }
    }
}