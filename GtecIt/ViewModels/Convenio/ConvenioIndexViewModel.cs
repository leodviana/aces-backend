using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class ConvenioIndexViewModel 
    {
        public ConvenioIndexViewModel()
        {
            Grid = new List<ConvenioGridViewModel>();
        }

        public bool ConsultaTodos { get; set; }
        public int id_grlconvenio { get; set; }
        public int? id_grlbasico { get; set; }
        public string Guia { get; set; }
        public int? cd_usuario { get; set; }

        public string Ativo { get; set; }
        public string Nome { get; set; }
        //public string cd_usuario { get; set; }

        public List<ConvenioGridViewModel> Grid { get; set; }

       
    }
}