using GtecIt.ViewModels;

namespace GtecIt.Util
{
    public class GerenciaSession : GerenciaSessionBase
    {
        private const string NomeUsarioLogado = "NomeUsarioLogado";
        private const string NomeEmpresaLogado = "NomeEmpresaLogado";

        public static UsuarioLogadoViewModel UsarioLogado
        {
            get
            {
                return LeComDefault<UsuarioLogadoViewModel>(NomeUsarioLogado);
            }
            set
            {
                Atualiza(NomeUsarioLogado, value);
            }
        }

        public static EmpresaLogadoViewModel EmpresaLogado
        {
            get
            {
                return LeComDefault<EmpresaLogadoViewModel>(NomeEmpresaLogado);
            }
            set
            {
                Atualiza(NomeEmpresaLogado, value);
            }
        }
    }
}