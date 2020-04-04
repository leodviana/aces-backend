using System.Collections.Generic;

namespace GtecIt.Domain.Entities
{
    public partial class TipoPagamento
    {
        public TipoPagamento()
        {
            this.Titulos = new List<Titulo>();
            this.titulosapagar = new List<Tituloapagar>();
        }

        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public virtual ICollection<Titulo> Titulos { get; set; }
        public virtual ICollection<Tituloapagar> titulosapagar { get; set; }
        
    }
}
