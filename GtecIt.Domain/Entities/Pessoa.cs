using System;
using System.Collections.Generic;

namespace GtecIt.Domain.Entities
{
    public class Pessoa
    {

        public Pessoa()
        {
            this.enderecos = new List<Endereco>();
            this.clientes = new List<Cliente>();
            this.dentistas = new List<Dentista>();
            this.convenios = new List<Convenio>();
            this.usuarios = new List<Usuario>();
        }

        public int Id_grlbasico { get; set; }
       public string Id_grltopessoa { get; set; }
       public string nome { get; set; }
        public string razao_social { get; set; }
        public string Nome_fantasia { get; set; }
        public string insc_municipal { get; set; }
        public string insc_estadual { get; set; }
        public string cgc { get; set; }

        public int sexo { get; set; }

        public DateTime? dt_nascimento { get; set; }
        public int? Id_grlprofi { get; set; }
        public int? Id_grlcidad { get; set; }
        public int? Id_grlcivil { get; set; }
        public string cpf { get; set; }
        public string rg { get; set; }
        
        public string contato { get; set; }
        public string ddd_telefone { get; set; }
        public int? Id_grlidtel { get; set; }
        public string ddd_telefone2 { get; set; }
        public int? Id_grlidtel02 { get; set; }
        public string email { get; set; }
        public string Email2 { get; set; }

        public string status { get; set; }
        public int? Id_grlcdusu { get; set; }
        public virtual Sexo gercdsexo { get; set; }
        public virtual TipoTelefone grlidtel { get; set; }
        public virtual TipoTelefone grlidtel1 { get; set; }

        public virtual ICollection<Endereco> enderecos { get; set; }
        public virtual ICollection<Cliente> clientes { get; set; }
        public virtual ICollection<Dentista> dentistas { get; set; }
        public virtual ICollection<Convenio> convenios { get; set; }
        public virtual ICollection<Usuario> usuarios { get; set; }

        public virtual ICollection<Fornecedor> Fornecedores { get; set; }
    }
    
}
