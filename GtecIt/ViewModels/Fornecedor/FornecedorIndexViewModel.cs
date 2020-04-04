using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class FornecedorIndexViewModel 
    {
        public FornecedorIndexViewModel()
        {
            Grid = new List<FornecedorGridViewModel>();
        }
        
        

        public int Id_grlfornecedor { get; set; }
        public int Id_grlbasic { get; set; }
        public int Id_grlcdusu { get; set; }
        public string cd_usuario { get; set; }
        public string Ativo { get; set; }

        public string Nome { get; set; }
        public virtual PessoaViewModel grlbasico { get; set; }
        // public virtual Usuario grlcdusu { get; set; }
        
        public bool ConsultaTodos { get; set; }
        public List<FornecedorGridViewModel> Grid { get; set; }
    }
}