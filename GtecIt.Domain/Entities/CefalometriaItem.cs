using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    
        public partial class CefalometriaItem
        {
            public int id_Grlcefdent { get; set; }
            public int? id_grldentista { get; set; }
            public int? id_GrlCefalometrias { get; set; }
            public int? cd_usuario { get; set; }
            public virtual Cefalometria grlcefalometria { get; set; }
            public virtual Dentista grldentista { get; set; }
        }
    
}
