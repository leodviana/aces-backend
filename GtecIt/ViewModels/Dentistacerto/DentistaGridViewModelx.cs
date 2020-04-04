using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class DentistaGridViewModelx 
    {

        public DentistaGridViewModelx()
        {
            this.Orcamentos = new List<OrcamentoEditViewModel>();
        }

        public int id_grldentista { get; set; }
        public int? Id_grlbasico { get; set; }

        public string Ativo { get; set; }
        public virtual PessoaViewModel Idgrlbasic { get; set; }
        public List<OrcamentoEditViewModel> Orcamentos { get; set; }
        
    }
}