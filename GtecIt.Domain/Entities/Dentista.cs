using System.Collections.Generic;

namespace GtecIt.Domain.Entities
{
    public  class Dentista
    {

        public Dentista()
        {
            this.Orcamentos = new List<Orcamento>();
            this.cefalometriaItem = new List<CefalometriaItem>();
        }

        public int id_grldentista { get; set; }
        public int? Id_grlbasico { get; set; }
        //public string cd_usuario { get; set; }
        public virtual Pessoa Idgrlbasic { get; set; }
        public string Ativo { get; set; }

        public virtual ICollection<Orcamento> Orcamentos { get; set; }
        public virtual ICollection<HorarioProfessor> HorarioProfessor { get; set; }
        public virtual ICollection<CefalometriaItem> cefalometriaItem { get; set; }
         
        
    }
}
