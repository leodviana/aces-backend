using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GtecIt.ViewModels
{
    public class ProdutoCreateViewModel 
    {
        public ProdutoCreateViewModel()
        {
            //this.Orcamentos = new List<OrcamentoEditViewModel>();
           
        }

        public int Id_stqcdprd { get; set; }
        public int? cd_produto { get; set; }

        [Remote("nomeDuplicado", "Produto", HttpMethod = "POST", ErrorMessage = "Descrição de Serviço já Cadastrado!.")]
        public string desc_produto { get; set; }

        [Required(ErrorMessage = "O item digitado nao esta na lista!")]
        public int? Id_stqcdgrp { get; set; }

        public string nome_grupo { get; set; }

        public int? Id_stqsbgrp { get; set; }
        public string nome_subgrupo { get; set; }

      //  public int? Id_stqvlunmed_con { get; set; }
        

        private int? _Id_stqvlunmed_con = 1;

        
        public int? Id_stqvlunmed_con
        {
            get { return _Id_stqvlunmed_con; }
            set { _Id_stqvlunmed_con = value; }
        }

        private int? _Id_stqvlunmed_est = 1;

        public int? Id_stqvlunmed_est
        {
            get { return _Id_stqvlunmed_est; }
            set { _Id_stqvlunmed_est = value; }
        }
        /*public virtual PessoaEditViewModel grlbasic { get; set; }
        public virtual ICollection<OrcamentoEditViewModel> Orcamentos { get; set; }*/
    }
}