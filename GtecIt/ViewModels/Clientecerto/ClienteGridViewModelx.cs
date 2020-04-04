using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class ClienteGridViewModelx 
    {
        public ClienteGridViewModelx()
        {
            this.Orcamentos = new List<OrcamentoEditViewModel>();
        }
        public int id_Grlcliente { get; set; }
        public int? Id_grlbasico { get; set; }
        /*public string cd_usuario { get; set; }*/
        public string Ativo { get; set; }
        public List<OrcamentoEditViewModel> Orcamentos { get; set; }
        public virtual PessoaEditViewModel grlbasic { get; set; }
        
        
    }
}