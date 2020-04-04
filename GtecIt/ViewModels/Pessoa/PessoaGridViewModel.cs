using System;
using System.ComponentModel.DataAnnotations;

namespace GtecIt.ViewModels
{
    public class PessoaGridViewModel 
    {
        public string Id_grltopessoa { get; set; }
        public int Id_grlbasico { get; set; }
        [Required(ErrorMessage = "*")]
        public string nome { get; set; }
        public string razao_social { get; set; }

        public int sexo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
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
        public int? Id_grlcdusu { get; set; }
        
        public  SexoEditViewModel gercdsexo { get; set; }

        public string Sexodescricao { get; set; }
        public string status { get; set; }
    }
}