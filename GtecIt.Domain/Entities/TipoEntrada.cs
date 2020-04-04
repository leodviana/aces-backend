using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public class TipoEntrada
    {
        public TipoEntrada()
        {
            this.NotasEntradas = new List<NotaEntrada>();
        }

        public int Id_stqtpent { get; set; }
        public string desc_tp_entrada { get; set; }
        public Nullable<int> Id_grlcdusu { get; set; }
        public virtual Usuario grlcdusu { get; set; }
        public virtual ICollection<NotaEntrada> NotasEntradas { get; set; }
    }
}
