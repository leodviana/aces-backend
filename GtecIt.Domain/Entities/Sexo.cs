
using System.Collections.Generic;

namespace GtecIt.Domain.Entities
{
    public  class Sexo
    {
        
        public int Id_gercdsexo { get; set; }
        public string descricao { get; set; }
        public int? Id_grlcdusu { get; set; }
       /* public virtual grlcdusu grlcdusu { get; set; }*/
        public  ICollection<Pessoa> Pessoas { get; set; }
        
    }
}
