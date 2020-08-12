using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class AulaCreateViewModel 
    {
        public AulaCreateViewModel()
        {
            DropdownProduto = new List<SelectListItem>();
            horarios = new List<HorarioProfessorEditViewModel>();
        }
        public int idGercdaulas { get; set; }
        public int? id_Stqcporcamento { get; set; }
        
        public DateTime? final { get; set; }
        public string status { get; set; }


        public string inicio { get; set; }
       
        public string fim { get; set; }

        
        public int qtd_aulas { get; set; }
        public DateTime Inicio_contrato { get; set; }
        public DateTime Vencimento_contrato { get; set; }

        public bool segunda { get; set; }
        public bool terca { get; set; }
        public bool quarta { get; set; }
        public bool quinta { get; set; }
        public bool sexta { get; set; }
        public bool sabado { get; set; }
        public string dia_semana { get; set; }

        public int id_grldentista { get; set; }
        public string  nome_dentista { get; set; }

        public string nome_cliente { get; set; }

        public string Subject { get; set; }
        public string Description { get; set; }

        public string Theme_color { get; set; }
        public string IsFullDay { get; set; }
        public List<SelectListItem> DropdownProduto { get; set; }

        public List<HorarioProfessorEditViewModel> horarios { get; set; }
    }
}