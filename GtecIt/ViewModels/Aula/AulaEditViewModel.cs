using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class AulaEditViewModel 
    {
        public int idGercdaulas { get; set; }
        public int? id_Stqcporcamento { get; set; }

        [Required(ErrorMessage = "Informe o horario inicial")]
        public string inicio { get; set; }
        [Required(ErrorMessage = "Informe o horario Final")]
        public string fim { get; set; }
        

        public DateTime? final { get; set; }
        public string status { get; set; }
       
        public int qtd_aulas { get; set; }
        public DateTime Inicio_contrato { get; set; }
        public DateTime Vencimento_contrato { get; set; }
        public string dia_semana { get; set; }
        public bool segunda { get; set; }
        public bool terca { get; set; }
        public bool quarta { get; set; }
        public bool quinta { get; set; }
        public bool sexta { get; set; }
        public bool sabado { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }

        public string Theme_color { get; set; }
        public string IsFullDay { get; set; }
        public int id_grldentista { get; set; }
        public string nome_dentista { get; set; }
        public virtual OrcamentoEditViewModel orcamentos { get; set; }
       // public List<HorarioProfessorEditViewModel> horarios { get; set; }
    }
}