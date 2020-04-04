using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class CefalometriaCreateViewModel 
    {
        public int id_GrlCefalometrias { get; set; }
        public string desc_cefalometria { get; set; }
        public int? cd_usuario { get; set; }
    }
}