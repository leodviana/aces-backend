
using System.Collections.Generic;

namespace GtecIt.Domain.Entities
{
     public  class TipoTelefone
    {
        
        public int id_grlidtel { get; set; }
        public string descricao { get; set; }
        public int? id_grlcdusu { get; set; }

        public ICollection<Pessoa> Pessoas { get; set; }

        public  ICollection<Pessoa> Pessoas1 { get; set; }
        public ICollection<Endereco> Enderecos { get; set; }
       /* public virtual grlcdusu grlcdusu { get; set; }*/
    }
}
