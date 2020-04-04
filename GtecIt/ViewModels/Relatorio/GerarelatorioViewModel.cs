using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GtecIt.ViewModels

{
    public class GerarelatorioViewModel
    {

        public int id_Stqcporcamento { get; set; }
        public int id_relatorio { get; set; }

        public List<SelectListItem> DropDownTipoRelatorio { get; set; }
       /* public IEnumerable<SelectListItem> DropDownTipoRelatorio
        {
            get
            {
                var lst = new List<SelectListItem>
                {
                    new SelectListItem {Text = "Relatorio 01", Value = "1"},
                    new SelectListItem {Text = "Etiqueta Padrão", Value = "2"}
                };
                return lst;
            }
        }*/
    }
}