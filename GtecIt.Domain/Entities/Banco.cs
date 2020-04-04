using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public class Banco
    {
        public Banco()
        {
            this.fintitrcs = new List<Titulo>();
            this.fintitpgs= new List<Tituloapagar>();
        }

        public int id_Fincdbanco { get; set; }
        public string cd_banco { get; set; }
        public string desc_banco { get; set; }
        public Nullable<int> cd_usuario { get; set; }
        public virtual ICollection<Titulo> fintitrcs { get; set; }

        public virtual ICollection<Tituloapagar> fintitpgs { get; set; }
    }
}
