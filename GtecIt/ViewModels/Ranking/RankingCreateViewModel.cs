using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class RankingCreateViewModel
    {
        public RankingCreateViewModel()
        {
            DropdownProduto = new List<SelectListItem>();
            DropdownProduto = new List<SelectListItem>();
            horarios = new List<HorarioProfessorEditViewModel>();
        }


        public int id_ranking { get; set; }
        public int id_grlbasico { get; set; }
        public int id_grlbasico_dupla { get; set; }
        public string categoria { get; set; }
        public double pontos { get; set; }
        public int posicao { get; set; }
        public List<SelectListItem> DropdownProduto { get; set; }
        public List<SelectListItem> DropdownProduto2 { get; set; }

        public List<HorarioProfessorEditViewModel> horarios { get; set; }
    }
}