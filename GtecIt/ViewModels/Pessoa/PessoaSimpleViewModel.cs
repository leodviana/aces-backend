using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GtecIt.ViewModels
{
    public class PessoaSimpleViewModel
    {
        public int PessoaId { get; set; }
        public int? EmpresaId { get; set; }
        public string TipoPessoa { get; set; }
        public string CpfCnpj { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}