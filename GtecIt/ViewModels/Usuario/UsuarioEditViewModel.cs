using System.Collections.Generic;
using System.Web.Mvc;


namespace GtecIt.ViewModels
{
    public class UsuarioEditViewModel
    {
        public UsuarioEditViewModel()
        {
            
            //Filtro = Filtro ?? new UsuarioIndexViewModel();
        }

        public int UsuarioId { get; set; }
        public int? Id_grlbasico { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Administrador { get; set; }
        public string Ativo { get; set; }
        public IEnumerable<SelectListItem> Dropdownativo
        {
            get
            {
                var lst = new List<SelectListItem>
                {
                    new SelectListItem {Text = "SIM", Value = "S"},
                    new SelectListItem {Text = "Não", Value = "N"}
                };
                return lst;
            }
        }
        public IEnumerable<SelectListItem> Dropdownusuario
        {
            get
            {
                var lst = new List<SelectListItem>
                {
                    new SelectListItem {Text = "SIM", Value = "S"},
                    new SelectListItem {Text = "Não", Value = "N"}
                };
                return lst;
            }
        }
        public virtual PessoaEditViewModel pessoas { get; set; }
    //    public UsuarioIndexViewModel Filtro { get; set; }
    }
}
