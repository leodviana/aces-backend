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
           // DropdownProduto = new List<SelectListItem>();
            //horarios = new List<HorarioProfessorEditViewModel>();
        }

        public int EventID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string ThemeColor { get; set; }
        public string IsFullDay { get; set; }

        /* public List<SelectListItem> DropdownProduto { get; set; }

         public List<HorarioProfessorEditViewModel> horarios { get; set; }*/
    }
}