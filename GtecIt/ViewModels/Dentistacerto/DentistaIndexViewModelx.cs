using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class DentistaIndexViewModelx 
    {
        public DentistaIndexViewModelx()
        {
            Grid = new List<DentistaGridViewModelx>();
        }

        public bool ConsultaTodos { get; set; }
        public int id_grldentista { get; set; }
        public int? Id_grlbasico { get; set; }
        public string Ativo { get; set; }

        public string Nome { get; set; }
       public List<DentistaGridViewModelx> Grid { get; set; }

       


    }
}