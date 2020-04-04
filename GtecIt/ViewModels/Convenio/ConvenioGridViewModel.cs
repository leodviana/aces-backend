using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class ConvenioGridViewModel 
    {
        public ConvenioGridViewModel()
        {
         //   this.Orcamentos = new List<OrcamentoEditViewModel>();
        }
        public int id_grlconvenio { get; set; }
        public int? id_grlbasico { get; set; }
        public string Guia { get; set; }
        public string Ativo { get; set; }
        public int? cd_usuario { get; set; }

      //  public List<OrcamentoEditViewModel> Orcamentos { get; set; }
        public virtual PessoaEditViewModel grlbasic { get; set; }
        
        
    }
}