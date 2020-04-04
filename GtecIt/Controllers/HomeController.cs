using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using System.Collections.Generic;
using System.IO;
using GtecIt.Util;
using GtecIt.ViewModels;
using GtecIt.Domain.Entities;
using GtecIt.Infra.Data.Core;


using Microsoft.Ajax.Utilities;

namespace GtecIt.Controllers
{
    public class HomeController : Controller
    {
        
         readonly IUnitOfWork _uoW;

        
        
        public HomeController(IUnitOfWork uoW)
        {
            _uoW = uoW;
            
        }
        public ActionResult Index()
        {
            //Exemplo de uso comparação
            /*  var objeto = new Cliente
              {
                  Nome = "João da Silva", 
                  Endereco = "Rua A - CEP 60000-000",
                  LocalTrabalho = "Google",
                  Renda = (decimal) 100.00
              };

              var objetoAlterado = new Cliente
              {
                  Nome = "João Silva", 
                  Endereco = "Rua B - CEP 60000-000",
                  LocalTrabalho = string.Empty,
                  Renda = 0
              };


              var dicionarioAlteracoes = objeto.Compare(objetoAlterado, "Usuário Adm");

              foreach (var item in dicionarioAlteracoes)
              {
                  Response.Write(item.Value);
                  Response.Write("<br/>");
              }*/


            return View();
        }
        /*
        //Classe para exemplo de uso comparação
        public class Cliente : IAuditoria
        {

            [DisplayName("Nome")]
            public string Nome { get; set; }

            [DisplayName("Endereço")]
            public string Endereco { get; set; }

            [DisplayName("Renda")]
            public decimal Renda { get; set; }

            [DisplayName("Local de Trabalho")]
            public string LocalTrabalho { get; set; }

        }*/
    }
}