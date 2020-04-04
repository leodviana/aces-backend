using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class SubGrupoIndexViewModel 
    {
        public SubGrupoIndexViewModel()
        {
            Grid = new List<SubGrupoGridViewModel>();
        }

        public int Id_stqsbgrp { get; set; }
        public string desc_subgrupo { get; set; }
        public int? Id_grlcdusu { get; set; }
        
        public bool ConsultaTodos { get; set; }
        public List<SubGrupoGridViewModel> Grid { get; set; }
    }
}