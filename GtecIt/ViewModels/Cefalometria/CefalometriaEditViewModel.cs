using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class CefalometriaEditViewModel 
    {
        public int id_GrlCefalometrias { get; set; }
        public string desc_cefalometria { get; set; }
        public int? cd_usuario { get; set; }
        
    }
}