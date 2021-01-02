using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class EventCreateViewModel
    {
        public EventCreateViewModel()
        {
            DropdownProfessor = new List<SelectListItem>();
            DropdownAluno = new List<SelectListItem>();
            //horarios = new List<HorarioProfessorEditViewModel>();
        }

        public int EventID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string ThemeColor { get; set; }
        public string IsFullDay { get; set; }
        public int id_grldentista { get; set; }
        public int id_contrato { get; set; }
        public List<SelectListItem> DropdownProfessor { get; set; }
        public List<SelectListItem> DropdownAluno { get; set; }
        // public List<HorarioProfessorEditViewModel> horarios { get; set; }*/
    }
}