using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class PrecoCreateViewModel 
    {
        public PrecoCreateViewModel()
        {
            DropdownConvenio = new List<SelectListItem>();
        }
        public int id_grlproceprecos { get; set; }
        public int? id_grlconvenio { get; set; }
        public int? id_stqcdprd { get; set; }
        public DateTime? vigencia { get; set; }
        public decimal? preco { get; set; }
        public int? cd_usuario { get; set; }

        public List<SelectListItem> DropdownConvenio { get; set; }
    }
}