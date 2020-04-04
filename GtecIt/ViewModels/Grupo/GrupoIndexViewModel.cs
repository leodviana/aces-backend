using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class GrupoIndexViewModel 
    {
        public GrupoIndexViewModel()
        {
            Grid = new List<GrupoGridViewModel>();
        }

        public int Id_stqcdgrp { get; set; }
        public string desc_grupo { get; set; }
        public string participa_rateio { get; set; }
        /*public Nullable<int> Id_grlcdusu { get; set; }
        public virtual grlcdusu grlcdusu { get; set; }
        public virtual ICollection<stqcdprd> stqcdprds { get; set; }*/
        public bool ConsultaTodos { get; set; }
        public List<GrupoGridViewModel> Grid { get; set; }
    }
}