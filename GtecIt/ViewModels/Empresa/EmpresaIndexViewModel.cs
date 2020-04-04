using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.WebPages.Html;

namespace GtecIt.ViewModels
{
    public class EmpresaIndexViewModel 
    {
        public EmpresaIndexViewModel()
        {
            Grid = new List<EmpresaGridViewModel>();
        }

        public int id_Grlempresa { get; set; }
        public string descricao { get; set; }
        public string endereco { get; set; }
        public string logo { get; set; }
        public string Status { get; set; }
        public bool ConsultaTodos { get; set; }
        
        public List<EmpresaGridViewModel> Grid { get; set; }
        public string altura_logo { get; set; }
        public string comprimentro_logo { get; set; }

        
    }
}