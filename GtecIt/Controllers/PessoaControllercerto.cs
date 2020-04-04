using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using System.Collections.Generic;
using System.Data;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GtecIt.Util;
using GtecIt.ViewModels;
using GtecIt.Domain.Entities;
using GtecIt.Infra.Data.Core;
using GtecIt.Models;
using Microsoft.Ajax.Utilities;




namespace GtecIt.Controllers
{
    public class PessoaControllercerto : Controller
    {
        private readonly IUnitOfWork _uoW;

        public PessoaControllercerto(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }


        public ActionResult Index(PessoaIndexViewModel model, string tipoacao)
        {

            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<PessoaGridViewModel>>(_uoW.Pessoas.ObterTodos().ToList().OrderBy(x => x.nome));
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<PessoaGridViewModel>>(_uoW.Pessoas.ObterTodos().Where(x => x.nome.Contains(model.nome)).ToList().OrderBy(x => x.nome));


            return View(model);

        }

        public ActionResult Create(PessoaCreateViewModel model)
        {

            model.DropdownEstado = _uoW.EstadoCivils.ObterTodos()
                   .OrderBy(x => x.descricao)
                   .Select(x => new SelectListItem { Text = x.descricao, Value = x.Id_grlcivil.ToString() })
                   .ToList();

            model.DropdownIdentifica = _uoW.TipoTelefones.ObterTodos()
                    .OrderBy(x => x.descricao)
                    .Select(x => new SelectListItem { Text = x.descricao, Value = x.id_grlidtel.ToString() })
                    .ToList();

            model.DropdownSexo = _uoW.Sexos.ObterTodos()
                    .OrderBy(x => x.descricao)
                    .Select(x => new SelectListItem { Text = x.descricao, Value = x.Id_gercdsexo.ToString() })
                    .ToList();
            //preencher lista
            // model.EstadoCivil = Mapper.Map<List<EstadoCivilEditViewModel>>(_estadoCivilApp.GetAll().ToList());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PessoaCreateViewModel model, FormCollection form)
        {
            model.ddd_telefone = FiltroDeCaracteres.RemoverCaracteresEspeciais(model.ddd_telefone);
            model.ddd_telefone2 = FiltroDeCaracteres.RemoverCaracteresEspeciais(model.ddd_telefone2);


            checa_relacionamento(model);

            ModelState.Clear();

            if (!TryValidateModel(model))
            {
                var validationErrors = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());

                return View(model);
            }
            model.status = "0";
            var pessoa = Mapper.Map<Pessoa>(model);
            _uoW.Pessoas.Salvar(pessoa);
            _uoW.Complete();
            var idPessoa = pessoa.Id_grlbasico;

            //return Json(true);
            if (model.OrcamentoEmAndamento)
            {
                // ferificar com o duan  porque nao aceita assim 

                Cliente  cliente2  = new Cliente();
                cliente2.Id_grlbasico = idPessoa;
                cliente2.Ativo = "S";
                //
                //ClienteEditViewModel model_cliente = new ClienteEditViewModel {Id_grlbasico = idPessoa,Ativo = "S"};
                //var cliente = Mapper.Map<Cliente>(model_cliente);
                _uoW.Clientes.Salvar(cliente2);
                _uoW.Complete();

                var idCliente = cliente2.id_Grlcliente;

                //  _clienteApp.AddReturnId(new Cliente { Id_grlbasico = idPessoa, Ativo = "S" });
                return RedirectToAction("Create", "Orcamento", new OrcamentoCreateViewModel { id_grlcliente = idCliente });
            }

            if (model.OrcamentoEmAndamentodentista)
            {
                Dentista dentista = new Dentista {Id_grlbasico = idPessoa, Ativo = "S"};
                //DentistaEditViewModel model_dentista = new DentistaEditViewModel { Id_grlbasico = idPessoa, Ativo = "S" };
                //var dentista = Mapper.Map<Dentista>(model_dentista);
                _uoW.Dentistas.Salvar(dentista);
                _uoW.Complete();
                var idDentista = dentista.id_grldentista;

                //var idDentista = _dentistaApp.AddReturnId(new Dentista() { Id_grlbasico = idPessoa, Ativo = "S" });
                return RedirectToAction("Create", "Orcamento", new OrcamentoCreateViewModel { Id_grldentista = idDentista });
            }

            if (model.Dentista)
            {
                var idDentista = _uoW.Pessoas.ObterPorId(idPessoa).Id_grlbasico; //_pessoaApp.GetAll().LastOrDefault().Id_grlbasico;
                return RedirectToAction("Create", "Dentista", new DentistaCreateViewModel { Id_grlbasico = idDentista });
            }

            if (model.fornecedor)
            {
                var idDentista = _uoW.Pessoas.ObterPorId(idPessoa).Id_grlbasico; //_pessoaApp.GetAll().LastOrDefault().Id_grlbasico;
                return RedirectToAction("Create", "Fornecedor", new FornecedorCreateViewModel { Id_grlbasic = idDentista });
            }

            if (model.Usuario)
            {
                var idDentista = _uoW.Pessoas.ObterPorId(idPessoa).Id_grlbasico; //_pessoaApp.GetAll().LastOrDefault().Id_grlbasico;
                //var idDentista = _pessoaApp.GetAll().LastOrDefault().Id_grlbasico;
                return RedirectToAction("Create", "Login", new DentistaCreateViewModel { Id_grlbasico = idDentista, Ativo = "S" });
            }

            if (model.OrcamentoEmAndamentoedicao)
            {
                Cliente cliente = new Cliente {Id_grlbasico = idPessoa, Ativo = "S"};

                //ClienteEditViewModel model_cliente = new ClienteEditViewModel { Id_grlbasico = idPessoa, Ativo = "S" };
                //var cliente = Mapper.Map<Cliente>(model_cliente);
                _uoW.Clientes.Salvar(cliente);
                _uoW.Complete();

                var idCliente = cliente.id_Grlcliente;
                //var idCliente = _clienteApp.AddReturnId(new Cliente { Id_grlbasico = idPessoa, Ativo = "S" });
                return RedirectToAction("Edit", "Orcamento", new { id_grlcliente = idCliente, codigo = Convert.ToInt32(model.orcamentoid) });
            }
            if (model.dentista_em_cadastro)
            {
                Dentista dentista = new Dentista { Id_grlbasico = idPessoa, Ativo = "S" };
                //DentistaEditViewModel model_dentista = new DentistaEditViewModel { Id_grlbasico = idPessoa, Ativo = "S" };
                //var dentista = Mapper.Map<Dentista>(model_dentista);
                _uoW.Dentistas.Salvar(dentista);
                _uoW.Complete();
                var idDentista = dentista.id_grldentista;
                //return RedirectToAction("Create", "Dentista", new DentistaCreateViewModel { Id_grlbasico = idDentista ,dentista_em_cadastro = true,orcamentoid =model.orcamentoid});
                if (model.orcamentoid==0)
                    return RedirectToAction("Create", "Orcamento", new { codigo = Convert.ToInt32(model.orcamentoid) });
                else

                  return RedirectToAction("Edit", "Orcamento", new { codigo = Convert.ToInt32(model.orcamentoid) });
            }
            if (model.cliente_em_cadastro)
            {
                Cliente cliente = new Cliente { Id_grlbasico = idPessoa, Ativo = "S" };

                //ClienteEditViewModel model_cliente = new ClienteEditViewModel { Id_grlbasico = idPessoa, Ativo = "S" };
                //var cliente = Mapper.Map<Cliente>(model_cliente);
                _uoW.Clientes.Salvar(cliente);
                _uoW.Complete();

                var idCliente = cliente.id_Grlcliente;
                if (model.orcamentoid==0)
                    return RedirectToAction("Create", "Orcamento");
                else
                  return RedirectToAction("Edit", "Orcamento", new { id_grlcliente = idCliente, codigo = Convert.ToInt32(model.orcamentoid) });
                //var idCliente= _uoW.Pessoas.ObterPorId(idPessoa).Id_grlbasico; //_pessoaApp.GetAll().LastOrDefault().Id_grlbasico;
                // return RedirectToAction("Create", "Cliente", new ClienteCreateViewModel { Id_grlbasico = idCliente, cliente_em_cadastro = true });
            }
            if (model.cliente_em_cadastro_curso)
            {
                Cliente cliente = new Cliente { Id_grlbasico = idPessoa, Ativo = "S" };

                //ClienteEditViewModel model_cliente = new ClienteEditViewModel { Id_grlbasico = idPessoa, Ativo = "S" };
                //var cliente = Mapper.Map<Cliente>(model_cliente);
                _uoW.Clientes.Salvar(cliente);
                _uoW.Complete();

                var idCliente = cliente.id_Grlcliente;
                if (model.orcamentoid == 0)
                    return RedirectToAction("Create", "Curso");
                else
                    return RedirectToAction("Edit", "Curso", new { id_grlcliente = idCliente, codigo = Convert.ToInt32(model.orcamentoid) });
                //var idCliente= _uoW.Pessoas.ObterPorId(idPessoa).Id_grlbasico; //_pessoaApp.GetAll().LastOrDefault().Id_grlbasico;
                // return RedirectToAction("Create", "Cliente", new ClienteCreateViewModel { Id_grlbasico = idCliente, cliente_em_cadastro = true });
            }
            if (model.fornecedor_em_cadastro)
            {
                Fornecedor dentista = new Fornecedor { Id_grlbasic = idPessoa, Ativo = "S" };
                //DentistaEditViewModel model_dentista = new DentistaEditViewModel { Id_grlbasico = idPessoa, Ativo = "S" };
                //var dentista = Mapper.Map<Dentista>(model_dentista);
                _uoW.Fornecedores.Salvar(dentista);
                _uoW.Complete();
                var idDentista = dentista.Id_grlfornecedor;
                //return RedirectToAction("Create", "Dentista", new DentistaCreateViewModel { Id_grlbasico = idDentista ,dentista_em_cadastro = true,orcamentoid =model.orcamentoid});
                if (model.orcamentoid == 0)
                    return RedirectToAction("Create", "NotaEntrada", new { codigo = Convert.ToInt32(model.orcamentoid) });
                else

                    return RedirectToAction("Edit", "NotaEntrada", new { codigo = Convert.ToInt32(model.orcamentoid) });
            }
            return RedirectToAction("Edit", new { codigo = idPessoa });
        }

        [HttpPost]
        public JsonResult nomeDuplicado(string nome)
        {
            var existe = _uoW.Pessoas.ObterTodos().Where(x => x.nome.ToUpper().Equals(nome.ToUpper())).FirstOrDefault(); ;
            if (existe != null)
                return Json(false);

            return Json(true);

        }

        private void checa_relacionamento(PessoaCreateViewModel model)
        {
            if (model.Id_grlcivil == null)
            {
                model.Id_grlcivil = 3;
            }
            if (model.sexo == 0)
            {
                model.sexo = 5;
            }
            if (model.Id_grlidtel == null)
            {
                model.Id_grlidtel = 15;
            }
            if (model.Id_grlidtel02 == null)
            {
                model.Id_grlidtel02 = 15;
            }
            if (model.Id_grlprofi == null)
            {
                model.Id_grlprofi = 1;
            }
            if (model.Id_grlcidad == null)
            {
                model.Id_grlcidad = 1;
            }
        }


        private void checa_relacionamento_edicao(PessoaEditViewModel model)
        {
            if (model.Id_grlcivil == null)
            {
                model.Id_grlcivil = 3;
            }
            if (model.sexo == 0)
            {
                model.sexo = 5;
            }
            if (model.Id_grlidtel == null)
            {
                model.Id_grlidtel = 15;
            }
            if (model.Id_grlidtel02 == null)
            {
                model.Id_grlidtel02 = 15;
            }
            if (model.Id_grlprofi == null)
            {
                model.Id_grlprofi = 1;
            }
            if (model.Id_grlcidad == null)
            {
                model.Id_grlcidad = 1;
            }
        }

        public ActionResult Edit(int codigo)
        {

            var model = Mapper.Map<PessoaEditViewModel>(_uoW.Pessoas.ObterPorId(codigo));

            if (model == null)
                return HttpNotFound();

            if (model.Id_grlprofi.HasValue)
                model.NomeProfissao = _uoW.Profissoes.ObterPorId(Convert.ToInt32(model.Id_grlprofi)).descricao;

            model.DropdownEstado = _uoW.EstadoCivils.ObterTodos()
                   .OrderBy(x => x.descricao)
                   .Select(x => new SelectListItem { Text = x.descricao, Value = x.Id_grlcivil.ToString() })
                   .ToList();

            model.DropdownIdentifica =_uoW.TipoTelefones.ObterTodos()
                    .OrderBy(x => x.descricao)
                    .Select(x => new SelectListItem { Text = x.descricao, Value = x.id_grlidtel.ToString() })
                    .ToList();

            model.DropdownSexo = _uoW.Sexos.ObterTodos()
                    .OrderBy(x => x.descricao)
                    .Select(x => new SelectListItem { Text = x.descricao, Value = x.Id_gercdsexo.ToString() })
                    .ToList();



            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PessoaEditViewModel model)
        {
            model.ddd_telefone = FiltroDeCaracteres.RemoverCaracteresEspeciais(model.ddd_telefone);
            model.ddd_telefone2 = FiltroDeCaracteres.RemoverCaracteresEspeciais(model.ddd_telefone2);

            checa_relacionamento_edicao(model);

            ModelState.Clear();

            if (!TryValidateModel(model))
            {
                var validationErrors = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());

                return View(model);
            }

            _uoW.Pessoas.Atualizar(Mapper.Map<Pessoa>(model));
            _uoW.Complete();


            return RedirectToAction("Index");

        }


        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.Pessoas.ObterPorId(codigo);

            if (model == null)
            {
                return Json(false);
            }

            _uoW.Pessoas.RemoverPorId(codigo);
            _uoW.Complete();
            return Json(true);
        }

        public bool VerificarFiltroVazio(PessoaIndexViewModel model)
        {
            model = model ?? new PessoaIndexViewModel();

            var ehVazio = model.nome.IsNullOrWhiteSpace();

            return ehVazio;
        }


        public JsonResult ObterDescricaonome(int codigo)
        {
            //var resultado;
            var fornecedor = _uoW.Pessoas.ObterTodos().FirstOrDefault(x => x.Id_grlbasico == codigo);
            if (fornecedor != null)
            {
                var resultado = new
                {
                    nome = fornecedor.nome,
                    email = fornecedor.email,
                    fone01 = fornecedor.ddd_telefone,
                    fone02 = fornecedor.ddd_telefone2,
                    email2 = fornecedor.Email2,
                    contato = fornecedor.contato,
                    encontrado = "T"
                };
                return Json(resultado, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var resultado = new
                {
                    nome = "",
                    email = "",
                    fone01 = "",
                    fone02 = "",
                    email2 = "",
                    contato = "",
                    encontrado = "F"
                };
                return Json(resultado, JsonRequestBehavior.AllowGet);
               // return Json(resultado, JsonRequestBehavior.AllowGet);
            }

            //return fornecedor == null ? Json(false, JsonRequestBehavior.AllowGet) : Json(resultado, JsonRequestBehavior.AllowGet);
        }



      

        public ActionResult ObterPessoa(string tipoConsulta, string filtro)
        {
            string html = "";
            //// tipoConsulta = codigo || descricao

            switch (tipoConsulta)
            {
                case "codigo":
                    {
                        var codigo = Convert.ToInt32(filtro);
                        var model = _uoW.Pessoas.ObterTodos().Where(x => x.Id_grlbasico == codigo);

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_grlbasico);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.nome);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalPessoa({0}, '{1}');\"  class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_grlbasico, item.nome);
                            html += "</tr>";
                        }
                    }
                    break;
                case "descricao":
                    {
                        var model =
                            _uoW.Pessoas.ObterTodos()
                                .Where(x => x.nome.ToLower().Trim().Contains(filtro.ToLower().Trim()));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_grlbasico);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.nome);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalPessoa({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_grlbasico, item.nome);
                            html += "</tr>";
                        }
                    }
                    break;
                case "todos":
                    {
                        /* var model = _pessoaApp.GetAll();

                         foreach (var item in model)
                         {
                             html += "<tr>";
                             html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_grlbasico);
                             html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.nome);
                             html +=
                                 string.Format(
                                     "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalPessoa({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                     item.Id_grlbasico, item.nome);
                             html += "</tr>";
                         }*/
                    }
                    break;
            }

            return Json(html);
        }
        public JsonResult AutoCompletePessoaPreFetch()
        {
            try
            {
                var resultado = _uoW.Pessoas.ObterTodos().Select(x =>
                new
                {
                    Id = x.Id_grlbasico.ToString(),
                    Codigo = x.Id_grlbasico.ToString(),
                    Descricao = x.nome.ToString(),
                }).ToList();

                return Json(new { success = true, results = resultado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }

}
