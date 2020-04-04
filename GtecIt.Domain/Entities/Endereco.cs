using System;

namespace GtecIt.Domain.Entities
{
    public class Endereco
    {
        public int Id_Grendbasico { get; set; }
        public Nullable<int> Id_grlbasic { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public Nullable<int> Id_grlidtel { get; set; }
        public Nullable<int> Id_grlcdusu { get; set; }
        public virtual Pessoa grlbasic { get; set; }
        public virtual TipoTelefone grlidtel { get; set; }

    }
}
