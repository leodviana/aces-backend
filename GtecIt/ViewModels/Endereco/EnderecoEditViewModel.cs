﻿namespace GtecIt.ViewModels
{
    public class EnderecoEditViewModel 
    {
        public int Id_Grendbasico { get; set; }
        public int? Id_grlbasic { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public int? Id_grlidtel { get; set; }
        public int? Id_grlcdusu { get; set; }

    }
}