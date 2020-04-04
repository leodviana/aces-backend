using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class AulaIndexViewModel 
    {
        public AulaIndexViewModel()
        {
            Grid = new List<AulaGridViewModel>();
        }

        public int idGercdaulas { get; set; }
        public int? id_Stqcporcamento { get; set; }
        public DateTime? inicio { get; set; }
        public DateTime? fim { get; set; }
        public string status { get; set; }
        public string dia_semana { get; set; }
        public bool ConsultaTodos { get; set; }
        public List<AulaGridViewModel> Grid { get; set; }
    }
}