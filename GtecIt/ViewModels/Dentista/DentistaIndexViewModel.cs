using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class DentistaIndexViewModel 
    {
        public DentistaIndexViewModel()
        {
            Grid = new List<DentistaGridViewModel>();
        }

        public bool ConsultaTodos { get; set; }
        public int id_grldentista { get; set; }
        public int? Id_grlbasico { get; set; }
        public string Ativo { get; set; }

        public string Nome { get; set; }
       public List<DentistaGridViewModel> Grid { get; set; }

       


    }
}