using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class ClienteIndexViewModelx 
    {
        public ClienteIndexViewModelx()
        {
            Grid = new List<ClienteGridViewModelx>();
        }

        public bool ConsultaTodos { get; set; }
        public int id_Grlcliente { get; set; }
        public int? Id_grlbasico { get; set; }

        public string Nome { get; set; }
        //public string cd_usuario { get; set; }
        public string Ativo { get; set; }
        public List<ClienteGridViewModelx> Grid { get; set; }

       
    }
}