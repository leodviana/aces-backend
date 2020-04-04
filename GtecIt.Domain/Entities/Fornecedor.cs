using System.Collections.Generic;

namespace GtecIt.Domain.Entities
{
    public class Fornecedor
    {
        public Fornecedor()
        {
            this.NotasEntradas = new List<NotaEntrada>();
        }

        public int Id_grlfornecedor { get; set; }
        public int Id_grlbasic { get; set; }
        public int Id_grlcdusu { get; set; }
        public string cd_usuario { get; set; }
        public string Ativo { get; set; }
        public virtual Pessoa grlbasico { get; set; }
       // public virtual Usuario grlcdusu { get; set; }
        public virtual ICollection<NotaEntrada> NotasEntradas { get; set; }
    }
}
