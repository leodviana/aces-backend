using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class ClienteIndexViewModel 
    {
        public ClienteIndexViewModel()
        {
            Grid = new List<ClienteGridViewModel>();
        }

        public bool ConsultaTodos { get; set; }
        public int id_Grlcliente { get; set; }
        public int? Id_grlbasico { get; set; }

        public string Nome { get; set; }
        //public string cd_usuario { get; set; }
        public string Ativo { get; set; }
        public List<ClienteGridViewModel> Grid { get; set; }

       
    }
}