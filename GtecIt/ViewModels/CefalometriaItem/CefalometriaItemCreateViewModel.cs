using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class CefalometriaItemCreateViewModel 
    {
        public CefalometriaItemCreateViewModel()
        {
            DropdownCefalometricas = new List<SelectListItem>();
        }
        public int id_Grlcefdent { get; set; }
        public int? id_grldentista { get; set; }
        public int? id_GrlCefalometrias { get; set; }
        public int? cd_usuario { get; set; }

        public List<SelectListItem> DropdownCefalometricas { get; set; }
    }
}