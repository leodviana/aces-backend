using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class BancoIndexViewModel 
    {
        public BancoIndexViewModel()
        {
            Grid = new List<BancoGridViewModel>();
        }

        public int id_Fincdbanco { get; set; }
        public string cd_banco { get; set; }
        public string desc_banco { get; set; }
        public int? cd_usuario { get; set; }
        public bool ConsultaTodos { get; set; }
        public List<BancoGridViewModel> Grid { get; set; }
    }
}