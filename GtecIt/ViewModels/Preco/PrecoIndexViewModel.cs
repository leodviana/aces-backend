using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class PrecoIndexViewModel 
    {
        public PrecoIndexViewModel()
        {
           // cefalometriaItems = new List<CefalometriaItemEditViewModel>();
            Grid = new List<PrecoGridViewModel>();
        }

        public int id_grlproceprecos { get; set; }
        public int? id_grlconvenio { get; set; }
        public int? id_stqcdprd { get; set; }
        public DateTime? vigencia { get; set; }
        public decimal? preco { get; set; }
        public int? cd_usuario { get; set; }
        public virtual ConvenioEditViewModel convenios { get; set; }
        public virtual ProdutoEditViewModel produtos { get; set; }
        public bool ConsultaTodos { get; set; }

       // public virtual ICollection<CefalometriaItemEditViewModel> cefalometriaItems { get; set; }
        public List<PrecoGridViewModel> Grid { get; set; }
    }
}