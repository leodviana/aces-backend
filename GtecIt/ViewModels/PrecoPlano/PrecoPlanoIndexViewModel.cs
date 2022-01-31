using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class PrecoPlanoIndexViewModel 
    {
        public PrecoPlanoIndexViewModel()
        {
           // cefalometriaItems = new List<CefalometriaItemEditViewModel>();
            Grid = new List<PrecoPlanoGridViewModel>();
        }

        public int idgrlplanosprecos { get; set; }
        public int? idGrlplanos { get; set; }
        public int? id_stqcdprd { get; set; }
        public DateTime? vigencia { get; set; }
        public decimal? preco { get; set; }
        public int? cd_usuario { get; set; }
        public int qtd_aulas { get; set; }
        public int qtd_dias_plano { get; set; }
        public decimal? valor2 { get; set; }
        public virtual PlanoEditViewModel planos { get; set; }
        public virtual ProdutoEditViewModel produtos { get; set; }
        public bool ConsultaTodos { get; set; }

       // public virtual ICollection<CefalometriaItemEditViewModel> cefalometriaItems { get; set; }
        public List<PrecoPlanoGridViewModel> Grid { get; set; }
    }
}