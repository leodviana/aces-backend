using System;

namespace GtecIt.ViewModels
{
    public class AulaGridViewModel 
    {
        public int idGercdaulas { get; set; }
        public int? id_Stqcporcamento { get; set; }
        public DateTime? inicio { get; set; }
        public DateTime? fim { get; set; }
        public string status { get; set; }
        public string dia_semana { get; set; }
        public string nome_dentista_inicial { get; set; }
        public string nome_dentista_final { get; set; }
    }
}