using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public class SubGrupo
    {
        public SubGrupo()
        {
            this.produtos = new List<Produto>();
        }

        public int Id_stqsbgrp { get; set; }
        public string desc_subgrupo { get; set; }
        public int? Id_grlcdusu { get; set; }
        
       // public virtual grlcdusu grlcdusu { get; set; }
        public virtual ICollection<Produto> produtos { get; set; }

    }
}
