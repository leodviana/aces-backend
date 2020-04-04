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
    public class ClienteControllercerto : Controller
    {
        private readonly IUnitOfWork _uoW;

        public ClienteControllercerto(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index(ClienteIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);
                //por que nao posso usar o mapeamento do nome desse modelo 
                try
                {
                    model.Grid = Mapper.Map<List<ClienteGridViewModel>>(_uoW.Clientes.ObterTodos().ToList().OrderBy(x => x.grlbasic.nome));
                    model.ConsultaTodos = true;
                }
                catch (Exception)
                {

                    throw;
                }

                return View(model);
            }

            model.ConsultaTodos = false;
            //por que nao posso usar o mapeamento do nome desse modelo 
            model.Grid = Mapper.Map<List<ClienteGridViewModel>>(_uoW.Clientes.ObterTodos().Where(x => x.grlbasic.nome.Contains(model.Nome)).ToList().OrderBy(x => x.grlbasic.nome));
            return View(model);

        }

        public ActionResult Create(ClienteCreateViewModel model , string teste)
        {
            
            if (model.Id_grlbasico != null && model.Id_grlbasico > 0)
            {

                var pessoa = _uoW.Pessoas.ObterTodos().FirstOrDefault(x => x.Id_grlbasico == model.Id_grlbasico);
                //var pessoa = _pessoaApp.GetAll().FirstOrDefault(x => x.clientes.Any(y => y.id_Grlcliente == model.id_grlcliente));
                //model.CodigoCliente = cliente.id_Grlcliente.ToString();
                model.NomeCliente= pessoa.nome;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClienteCreateViewModel model)
        {

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

            //var model2 = Mapper.Map<ClienteEditViewModel>(_uoW.Clientes.ObterTodos().FirstOrDefault(x => x.Id_grlbasico == model.Id_grlbasico));
            var model2 = _uoW.Clientes.ObterTodos().FirstOrDefault(x => x.Id_grlbasico == model.Id_grlbasico);
            if (model2 == null)
            {
                var cliente = Mapper.Map<Cliente>(model);
                //_dentistaApp.Add(Mapper.Map<Dentista>(model));
                _uoW.Clientes.Salvar(cliente);
                _uoW.Complete();
                var id_clientes = cliente.id_Grlcliente;
                if (model.cliente_em_cadastro)
                {
                    if (model.orcamentoid == 0)
                        return RedirectToAction("Create", "Orcamento");
                    else
                        return RedirectToAction("Edit", "Orcamento", new { codigo = Convert.ToInt32(model.orcamentoid) });

                }
                if (model.cliente_em_cadastro_curso)
                {
                    if (model.orcamentoid == 0)
                        return RedirectToAction("Create", "Curso");
                    else
                        return RedirectToAction("Edit", "Curso", new { codigo = Convert.ToInt32(model.orcamentoid) });

                }
                //if (model.cliente_em_cadastro)

                //  return RedirectToAction("Create", "Orcamento");

                return RedirectToAction("Edit", new { codigo = _uoW.Clientes.ObterPorId(id_clientes).id_Grlcliente });

               
            }
            else
            {

                model2.Ativo = model.Ativo;
                //_dentistaApp.Update(Mapper.Map<Dentista>(model2));
                _uoW.Clientes.Atualizar(model2);
                _uoW.Complete();

                if (model.cliente_em_cadastro)
                {
                    if (model.orcamentoid == 0)
                        return RedirectToAction("Create", "Orcamento");
                    else
                        return RedirectToAction("Edit", "Orcamento", new { codigo = Convert.ToInt32(model.orcamentoid) });

                }
                if (model.cliente_em_cadastro_curso)
                {
                    if (model.orcamentoid == 0)
                        return RedirectToAction("Create", "curso");
                    else
                        return RedirectToAction("Edit", "Curso", new { codigo = Convert.ToInt32(model.orcamentoid) });

                }
                // if (model.cliente_em_cadastro)

                //   return RedirectToAction("Create", "Orcamento");

                return RedirectToAction("Edit", new { codigo = model2.id_Grlcliente });
                
            }

           // return RedirectToAction("Index");
            //return RedirectToAction("Edit", new { codigo =_pessoaApp.GetAll().LastOrDefault().Id_grlbasico});
        }


        public ActionResult CreateCliente(string origem,long orcamentoid)
        {
            if (origem.Equals("cliente"))
            {
                return RedirectToAction("Create", "Pessoa", new PessoaCreateViewModel { cliente_em_cadastro = true, orcamentoid = orcamentoid });
            }
            else if (origem.Equals("cliente_curso"))
            {
                return RedirectToAction("Create", "Pessoa", new PessoaCreateViewModel { cliente_em_cadastro_curso = true, orcamentoid = orcamentoid });
            }
            else
            {
                return RedirectToAction("Create", "Pessoa",
                    new PessoaCreateViewModel {cliente_em_cadastro = true, orcamentoid = orcamentoid});
            }




        }

        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<ClienteEditViewModel>(_uoW.Clientes.ObterPorId(codigo));

            if (model == null)
                return HttpNotFound();


            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClienteEditViewModel model)
        {

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

            // _clienteApp.Update(Mapper.Map<Cliente>(model));
            _uoW.Clientes.Atualizar(Mapper.Map<Cliente>(model));
            _uoW.Complete();


            return RedirectToAction("Index");

        }


        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            //var model = _convenioApp.GetById(codigo);
            var model = Mapper.Map<ClienteEditViewModel>(_uoW.Clientes.ObterPorId(codigo));
            ModelState.Clear();

            if (!TryValidateModel(model))
            {
                var validationErrors = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());
                return Json(false);

            }
            if (model == null)
            {
                return Json(false);
            }

            model.Ativo = "N";
            try
            {
                //_clienteApp.Update(Mapper.Map<Cliente>(model));
                _uoW.Clientes.Atualizar(Mapper.Map<Cliente>(model));
                _uoW.Complete();

            }
            catch (Exception)
            {

                throw;
            }
            // _convenioApp.Update(Mapper.Map<Convenio>(model));

            return Json(true);
        }

        public bool VerificarFiltroVazio(ClienteIndexViewModel model)
        {
            model = model ?? new ClienteIndexViewModel();

            var ehVazio = string.IsNullOrEmpty(model.Nome);

            return ehVazio;
        }
        public JsonResult ObterDescricaoCliente(int codigo)
        {
            var fornecedor = _uoW.Clientes.ObterTodos().FirstOrDefault(x => x.id_Grlcliente == codigo && x.Ativo.Equals("S"));

            
            return fornecedor == null ? Json(false, JsonRequestBehavior.AllowGet) : Json(fornecedor.grlbasic.nome, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObterCliente(string tipoConsulta, string filtro)
        {
            string html = "";
            //// tipoConsulta = codigo || descricao

            switch (tipoConsulta)
            {
                case "codigo":
                    {
                        var codigo = Convert.ToInt32(filtro);
                        var model =_uoW.Clientes.ObterTodos().Where(x => x.id_Grlcliente == codigo && x.Ativo.Equals("S"));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.id_Grlcliente);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.grlbasic.nome);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalCliente({0}, '{1}');\"  class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.id_Grlcliente, item.grlbasic.nome);
                            html += "</tr>";
                        }
                    }
                    break;
                case "descricao":
                    {
                        var model =
                            _uoW.Clientes.ObterTodos().Where(x => x.grlbasic.nome.ToLower().Trim().Contains(filtro.ToLower().Trim()) && x.Ativo.Equals("S"));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.id_Grlcliente);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.grlbasic.nome);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalCliente({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.id_Grlcliente, item.grlbasic.nome);
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
        public JsonResult AutoCompleteClientePreFetch()
        {
            try
            {
                var resultado = _uoW.Clientes.ObterTodos().Select(x =>
                new
                {
                           
                    Id = x.id_Grlcliente.ToString(),
                    Codigo = x.id_Grlcliente.ToString(),
                    Descricao = x.grlbasic.nome.ToString(),
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
