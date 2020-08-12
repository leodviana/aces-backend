using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class EventIndexViewModel
    {
        public EventIndexViewModel()
        {
            Grid = new List<EventGridViewModel>();
        }

        public int EventID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string ThemeColor { get; set; }
        public string IsFullDay { get; set; }
        public bool ConsultaTodos { get; set; }
        public List<EventGridViewModel> Grid { get; set; }
    }
}