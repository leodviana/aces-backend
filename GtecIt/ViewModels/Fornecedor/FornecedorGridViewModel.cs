using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class FornecedorGridViewModel 
    {
        public FornecedorGridViewModel()
        {
            this.NotasEntradas = new List<NotaEntradaEditViewModel>();
        }

        public int Id_grlfornecedor { get; set; }
        public int Id_grlbasic { get; set; }
        public int Id_grlcdusu { get; set; }
        public string cd_usuario { get; set; }
        public string Ativo { get; set; }
        public string Nome { get; set; }
        public virtual PessoaViewModel grlbasico { get; set; }
        // public virtual Usuario grlcdusu { get; set; }
        public virtual List<NotaEntradaEditViewModel> NotasEntradas { get; set; }
    }
}