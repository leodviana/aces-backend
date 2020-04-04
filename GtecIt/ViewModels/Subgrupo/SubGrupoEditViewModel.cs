using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class SubGrupoEditViewModel 
    {
        public int Id_stqsbgrp { get; set; }
        public string desc_subgrupo { get; set; }
        public int? Id_grlcdusu { get; set; }
        
        /*public Nullable<int> Id_grlcdusu { get; set; }
        public virtual grlcdusu grlcdusu { get; set; }
        public virtual ICollection<stqcdprd> stqcdprds { get; set; }*/
        //public List<EstadoCivilEditViewModel> EstadosList { get; set; }
    }
}