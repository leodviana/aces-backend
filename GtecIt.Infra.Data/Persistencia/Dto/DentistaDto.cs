using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Infra.Data.Persistencia.Dto
{
    public class DentistaDto
    {
        public int id_grldentista { get; set; }
        public int? Id_grlbasico { get; set; }
        //public string cd_usuario { get; set; }
        public string Ativo { get; set; }

        public string  nome_dentista{get;set;}

        //  public virtual ICollection<OrcamentoEditViewModel> Orcamentos { get; set; }

        public  ICollection<HorarioProfessorDto> HorarioProfessor { get; set; }

        //public  ICollection<CefalometriaItemEditViewModel> cefalometriaItem { get; set; }
        public string pagina_origem { get; set; }
        public string teste { get; set; }
        public long orcamentoid { get; set; }
    }
}
