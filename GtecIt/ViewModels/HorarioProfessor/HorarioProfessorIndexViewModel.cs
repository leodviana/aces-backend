using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class HorarioProfessorIndexViewModel 
    {
        public HorarioProfessorIndexViewModel()
        {
            Grid = new List<HorarioProfessorGridViewModel>();
        }

        public int idgercdhorarioProf { get; set; }
        public int id_grldentista { get; set; }
        public string horario { get; set; }
        public string segunda { get; set; }
        public string terca { get; set; }
        public string quarta { get; set; }
        public string quinta { get; set; }
        public string sexta { get; set; }
        public string sabado { get; set; }
        public string status { get; set; }
        public string Dia { get; set; }
        public bool ConsultaTodos { get; set; }
        public List<HorarioProfessorGridViewModel> Grid { get; set; }
    }
}