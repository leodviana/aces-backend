﻿

using System.Collections.Generic;

namespace GtecIt.Domain.Entities
{
    public  class EstadoCivil
    {
        
        public int Id_grlcivil { get; set; }
        public string descricao { get; set; }
        public int? Id_grlcdusu { get; set; }
       /* public virtual grlcdusu grlcdusu { get; set; }*/
        public  ICollection<Pessoa> Pessoas { get; set; }
        
    }
}
