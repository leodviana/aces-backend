using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public partial class Relatorio
    {
        public int id_Grlrelatorios { get; set; }
        public Nullable<int> Empresa { get; set; }
        public string relatorio { get; set; }
        public virtual Empresa grlempresa { get; set; }
    }
}
