using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class UsuarioIndexViewModel
    {
        public UsuarioIndexViewModel()
        {
            Grid = new List<UsuarioGridViewModel>();
        }
        
        public string Nome { get; set; }

        public int UsuarioId { get; set; }
        public int? Id_grlbasico { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Administrador { get; set; }
        public string Ativo { get; set; }
        public string senha_sem { get; set; }
        public int Tipo_usuario { get; set; }
        // public virtual PessoaEditViewModel grlbasico { get; set; }
        public bool ConsultaTodos { get; set; }
        

        public List<UsuarioGridViewModel> Grid { get; set; }
    }
}
