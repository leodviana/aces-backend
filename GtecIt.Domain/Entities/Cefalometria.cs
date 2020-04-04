using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    
        public partial class Cefalometria
        {
            public Cefalometria()
            {
               cefalometriaItem = new List<CefalometriaItem>();
            }
            public int id_GrlCefalometrias { get; set; }
            public string desc_cefalometria { get; set; }

            public virtual ICollection<CefalometriaItem> cefalometriaItem { get; set; }
        }
    
}
