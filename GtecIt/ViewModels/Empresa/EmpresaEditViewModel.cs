using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class EmpresaEditViewModel 
    {
        public EmpresaEditViewModel()
        {
          //  this.grlrelatorios = new List<RelatorioEditViewModel>();
        }
        public int id_Grlempresa { get; set; }
        public string descricao { get; set; }
        public string endereco { get; set; }
        public string logo { get; set; }
        //public virtual ICollection<RelatorioEditViewModel> grlrelatorios { get; set; }
        public string Status { get; set; }
        public string altura_logo { get; set; }
        public string comprimentro_logo { get; set; }
    }
}