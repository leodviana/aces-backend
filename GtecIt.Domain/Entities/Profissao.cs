using System.Collections.Generic;

namespace GtecIt.Domain.Entities
{
    public  class Profissao
    {
        
        public int Id_grlprofi { get; set; }
        public string descricao { get; set; }
        public int? Id_grlcdusu { get; set; }
       /* public virtual grlcdusu grlcdusu { get; set; }*/
        public  ICollection<Pessoa> Pessoas { get; set; }
        
    }
}
