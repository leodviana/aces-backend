using System.Collections.Generic;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class RelGeralViewlModel
    {

        public int ClienteId { get; set; }


        public int ConvenioId { get; set; }

        public int CursoId { get; set; }

        public string Nome { get; set; }
        public string Codigo { get; set; }
        public System.DateTime inicio { get; set; }
        public System.DateTime fim { get; set; }
        public int UsuarioId { get; set; }

        public List<SelectListItem> ClienteDropDown { get; set; }
        public List<SelectListItem> ContatoDropDown { get; set; }
        public List<SelectListItem> CidadeDropDown { get; set; }


        public List<SelectListItem> ConvenioDropDown { get; set; }
        public string UF { get; set; }

        public string cidade { get; set; }

        public string contato { get; set; }

        public string descricaoUF { get; set; }
        public virtual PessoaEditViewModel Pessoa { get; set; }

        public int ordemid { get; set; }
        public IEnumerable<SelectListItem> DropDownOrdem
        {
            get
            {
                var lst = new List<SelectListItem>
                {
                    new SelectListItem {Text = "Emissão", Value = "1"},
                    new SelectListItem {Text = "Vencimento", Value = "2"},
                    new SelectListItem {Text = "Pagamento", Value = "3"}
                };
                return lst;
            }
        }
       
    }
}