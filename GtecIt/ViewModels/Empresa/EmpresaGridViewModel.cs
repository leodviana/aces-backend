using System;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class EmpresaGridViewModel 
    {
        public int id_Grlempresa { get; set; }
        public string descricao { get; set; }
        public string endereco { get; set; }
        public string logo { get; set; }
        public string Status { get; set; }
        public string altura_logo { get; set; }
        public string comprimentro_logo { get; set; }
    }
}