using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class PlanoCreateViewModel 
    {
        public int idGrlplanos { get; set; }
        public string desc_plano { get; set; }
        public int? cd_usuario { get; set; }
    }
}