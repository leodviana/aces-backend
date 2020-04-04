using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public class TipoNota
    {
        public TipoNota()
        {
            this.NotasEntradas = new List<NotaEntrada>();
        }

        public int Id_stqtpnot { get; set; }
        public string desc_tp_nota { get; set; }
        public Nullable<int> Id_grlcdusu { get; set; }
        public virtual Usuario usuario { get; set; }
        public virtual ICollection<NotaEntrada> NotasEntradas { get; set; }
    }
}
