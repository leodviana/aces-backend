namespace GtecIt.ViewModels
{
    public class UsuarioSimpleViewModel
    {
        public int UsuarioId { get; set; }
        public int? Id_grlbasico { get; set; }
        public virtual PessoaSimpleViewModel pessoas { get; set; }
    }
}
