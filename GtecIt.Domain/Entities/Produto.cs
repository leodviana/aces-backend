using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Domain.Entities
{
    public class Produto
    {
        public Produto()
        {
            //this.stqitorcamentoes = new List<stqitorcamento>();
            this.ItensEntrada = new List<NotaEntradaItem>();
        }

        public int Id_stqcdprd { get; set; }
        public int? cd_produto { get; set; }
        public string desc_produto { get; set; }
        public int? Id_stqcdgrp { get; set; }
        public int? Id_stqsbgrp { get; set; }
        public int? Id_stqvlunmed_con { get; set; }
        public int Id_stqvlunmed_est { get; set; }
        public string fracionado { get; set; }
        public int? fator_fracao { get; set; }
        public int? num_dias_estmin { get; set; }
        public int? num_dias_estmax { get; set; }
        public int? num_dias_reposi { get; set; }
        public DateTime? dt_calc_consumo { get; set; }
        public int? num_dias_calc { get; set; }
        public decimal? consumo_medio { get; set; }
        public string indicador_abc { get; set; }
        public string indicador_xyz { get; set; }
        public string status_produto { get; set; }
        public Nullable<int> Id_grlcdusu { get; set; }
        
        public virtual Grupo grupos { get; set; }
        public virtual ICollection<OrcamentoItem> OrcamentosiItems { get; set; }
        public virtual SubGrupo subgrupos { get; set; }

        public virtual ICollection<Preco> precos { get; set; }
        public virtual ICollection<PrecoPlano> precosplano { get; set; }
        public virtual ICollection<NotaEntradaItem> ItensEntrada { get; set; }
    }
}
