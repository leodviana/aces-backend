using GtecIt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class HorarioProfessorCreateViewModel
    {
        public int idgercdhorarioProf { get; set; }
        public int id_grldentista { get; set; }
        public string horario { get; set; }

        public bool segunda { get; set; }
        public bool terca { get; set; }
        public bool quarta { get; set; }
        public bool quinta { get; set; }
        public bool sexta { get; set; }
        public bool sabado { get; set; }


        
        public string status { get; set; }
        public string Dia { get; set; }
        public int? id_Stqcporcamento { get; set; }
        public int? id_Stqcporcamento_dupla { get; set; }
        public string professor { get; set; }

      //  public virtual DentistaEditViewModel dentistas { get; set; }
        //public virtual ICollection<OrcamentoEditViewModel> orcamentos { get; set; }

        public List<SelectListItem> DropdownAlunos { get; set; }
        public List<SelectListItem> Dropdownstatus { get; set; }
        // public virtual Dentista dentistas { get; set; }


    }
}