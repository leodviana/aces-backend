using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public class Grupo
    {
        public Grupo()
        {
            this.produtos = new List<Produto>();
        }

        public int Id_stqcdgrp { get; set; }
        public string desc_grupo { get; set; }
        public string participa_rateio { get; set; }
        public int? Id_grlcdusu { get; set; }
       // public virtual grlcdusu grlcdusu { get; set; }
        public virtual ICollection<Produto> produtos { get; set; }

    }
}
