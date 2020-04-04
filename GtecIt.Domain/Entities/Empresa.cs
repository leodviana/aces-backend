using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public partial class Empresa
    {
        public Empresa()
        {
            this.grlrelatorios = new List<Relatorio>();
        }

        public int id_Grlempresa { get; set; }
        public string descricao { get; set; }
        public string endereco { get; set; }
        public string logo { get; set; }

        public string altura_logo { get; set; }
        public string comprimentro_logo { get; set; }
        public string Status { get; set; }
        public virtual ICollection<Relatorio> grlrelatorios { get; set; }
    }
}
