
using System.Collections.Generic;

namespace GtecIt.Domain.Entities
{
    
        public class Usuario 
    {
            public int UsuarioId { get; set; }
            public int? Id_grlbasico { get; set; }
            public string Login { get; set; }
            public string Senha { get; set; }
            public string Administrador { get; set; }
            public string Ativo { get; set; }
            public virtual Pessoa pessoas { get; set; }
          // public virtual ICollection<Fornecedor> fornecedores { get; set; }
       // public virtual ICollection<NotaEntrada> NotaEntradas { get; set; }
       // public virtual ICollection<NotaEntradaItem> NotaEntradaItems { get; set; }
       // public virtual ICollection<CentrodeCusto> CentrodeCustos { get; set; }
        public virtual ICollection<TipoEntrada> TipoEntradas { get; set; }
        public virtual ICollection<TipoNota> TipoNotas { get; set; }
    }
    
}
