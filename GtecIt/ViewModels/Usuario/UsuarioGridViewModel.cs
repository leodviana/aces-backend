namespace GtecIt.ViewModels
{
    public class UsuarioGridViewModel
    {
        public int UsuarioId { get; set; }
        public int? Id_grlbasico { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Administrador { get; set; }
        public string Ativo { get; set; }
        public string senha_sem { get; set; }
        public int Tipo_usuario { get; set; }
        public virtual PessoaEditViewModel pessoas { get; set; }
        
    }
}
