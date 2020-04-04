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
    public class FornecedorController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public FornecedorController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }
        public ActionResult Index(FornecedorIndexViewModel model, string tipoacao)
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
                    model.Grid = Mapper.Map<List<FornecedorGridViewModel>>(_uoW.Fornecedores.ObterTodos().ToList().OrderBy(x => x.grlbasico.nome));
                    model.ConsultaTodos = true;
                }
                catch (Exception ex)
                {
                    
                    throw;
                }
                
                return View(model);
            }

            model.ConsultaTodos = false;
            //por que nao posso usar o mapeamento do nome desse modelo 
            model.Grid = Mapper.Map<List<FornecedorGridViewModel>>(_uoW.Fornecedores.ObterTodos().Where(x => x.grlbasico.nome.Contains(model.Nome)).ToList().OrderBy(x => x.grlbasico.nome));
            return View(model);

        }

        public ActionResult Create(FornecedorCreateViewModel model)
        {
            //var model = new DentistaCreateViewModel();

            if (model.Id_grlbasic != null && model.Id_grlbasic > 0)
            {

                var pessoa = _uoW.Pessoas.ObterTodos().FirstOrDefault(x => x.Id_grlbasico == model.Id_grlbasic);
                //var pessoa = _pessoaApp.GetAll().FirstOrDefault(x => x.clientes.Any(y => y.id_Grlcliente == model.id_grlcliente));
                //model.CodigoCliente = cliente.id_Grlcliente.ToString();
                model.NomeFornecedor = pessoa.nome;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FornecedorCreateViewModel model, FormCollection form)
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

            //var model2 = Mapper.Map<DentistaEditViewModel>(_uoW.Dentistas.ObterPorId(Convert.ToInt32(model.Id_grlbasico)));
            var model2 = _uoW.Fornecedores.ObterTodos().FirstOrDefault(x=>x.Id_grlbasic==model.Id_grlbasic);
            if (model2 == null)
            {
                var fornecedor = Mapper.Map<Fornecedor>(model);
                //_dentistaApp.Add(Mapper.Map<Dentista>(model));
                _uoW.Fornecedores.Salvar(fornecedor);
                _uoW.Complete();
                var id_dentistas = fornecedor.Id_grlfornecedor;
                if (model.fornecedor_em_cadastro )
                {
                    if (model.orcamentoid==0)
                      return RedirectToAction("Create", "NotaEntrada");
                    else
                      return RedirectToAction("Edit", "NotaEntrada", new { codigo = Convert.ToInt32(model.orcamentoid) });

                }
                return RedirectToAction("Edit", new { codigo = _uoW.Fornecedores.ObterPorId(id_dentistas).Id_grlfornecedor });
            }
            else
            {
                model2.Ativo = model.Ativo;
                //_dentistaApp.Update(Mapper.Map<Dentista>(model2));
                _uoW.Fornecedores.Atualizar(model2);
                _uoW.Complete();
                if (model.fornecedor_em_cadastro)
                {
                    if (model.orcamentoid == 0)
                        return RedirectToAction("Create", "NotaEntrada");
                    else
                        return RedirectToAction("Edit", "NotaEntrada", new { codigo = Convert.ToInt32(model.orcamentoid) });

                }


                return RedirectToAction("Edit", new { codigo = model2.Id_grlfornecedor });
            }


            // return RedirectToAction("Index");

        }

        public ActionResult CreateFornecedor(string origem ,long orcamentoid)
        {
            if (origem.Equals("fornecedor"))
            {
                return RedirectToAction("Create", "Pessoa", new PessoaCreateViewModel { fornecedor_em_cadastro = true ,orcamentoid = orcamentoid});
            }
            else
            {
                return RedirectToAction("Create", "Pessoa", new PessoaCreateViewModel { fornecedor = true, orcamentoid = orcamentoid });
            }
              
         }


        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<FornecedorEditViewModel>(_uoW.Fornecedores.ObterPorId(codigo));

            if (model == null)
                return HttpNotFound();

            /*if (model.Id_grlprofi.HasValue)
                model.NomeProfissao = _profissaoApp.GetById(model.Id_grlprofi.Value).descricao;

            model.DropdownEstado = _estadoCivilApp.GetAll()
                   .OrderBy(x => x.descricao)
                   .Select(x => new SelectListItem { Text = x.descricao, Value = x.Id_grlcivil.ToString() })
                   .ToList();

            model.DropdownIdentifica = _tipotelefoneApp.GetAll()
                    .OrderBy(x => x.descricao)
                    .Select(x => new SelectListItem { Text = x.descricao, Value = x.id_grlidtel.ToString() })
                    .ToList();

            model.DropdownSexo = _sexoApp.GetAll()
                    .OrderBy(x => x.descricao)
                    .Select(x => new SelectListItem { Text = x.descricao, Value = x.Id_gercdsexo.ToString() })
                    .ToList();


            */
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FornecedorEditViewModel model)
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

            //_dentistaApp.Update(Mapper.Map<Dentista>(model));
            _uoW.Fornecedores.Atualizar(Mapper.Map<Fornecedor>(model));
            _uoW.Complete();

            return RedirectToAction("Index");

        }


        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            //var model = _convenioApp.GetById(codigo);
            var model = _uoW.Fornecedores.ObterPorId(codigo);
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
                //_dentistaApp.Update(Mapper.Map<Dentista>(model));
                _uoW.Fornecedores.Atualizar(model);
                _uoW.Complete();
            }
            catch (Exception)
            {

                throw;
            }
            // _convenioApp.Update(Mapper.Map<Convenio>(model));

            return Json(true);
        }

        public bool VerificarFiltroVazio(FornecedorIndexViewModel model)
        {
            model = model ?? new FornecedorIndexViewModel();

            var ehVazio = string.IsNullOrEmpty(model.Nome);

            return ehVazio;
        }
        public JsonResult ObterDescricaoFornedor(int codigo)
        {
            var fornecedor = _uoW.Fornecedores.ObterTodos().FirstOrDefault(x => x.Id_grlfornecedor == codigo && x.Ativo.Equals("S"));


            return fornecedor == null ? Json(false, JsonRequestBehavior.AllowGet) : Json(fornecedor.grlbasico.nome, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObterFornecedor(string tipoConsulta, string filtro)
        {
            string html = "";
            //// tipoConsulta = codigo || descricao

            switch (tipoConsulta)
            {
                case "codigo":
                    {
                        var codigo = Convert.ToInt32(filtro);
                        var model =_uoW.Fornecedores.ObterTodos().Where(x => x.Id_grlfornecedor == codigo && x.Ativo.Equals("S"));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_grlfornecedor);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.grlbasico.nome);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalDentista({0}, '{1}');\"  class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_grlfornecedor, item.grlbasico.nome);
                            html += "</tr>";
                        }
                    }
                    break;
                case "descricao":
                    {
                        var model =
                            _uoW.Fornecedores.ObterTodos()
                                .Where(x => x.grlbasico.nome.ToLower().Trim().Contains(filtro.ToLower().Trim()) && x.Ativo.Equals("S"));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_grlfornecedor);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.grlbasico.nome);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalDentista({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_grlfornecedor, item.grlbasico.nome);
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

        public JsonResult AutoCompleteFornecedorPreFetch()
        {
            try
            {
                var resultado = _uoW.Fornecedores.ObterTodos().Select(x =>
                new
                {
                    Id = x.Id_grlfornecedor.ToString(),
                    Codigo = x.Id_grlfornecedor.ToString(),
                    Descricao = x.grlbasico.nome.ToString(),
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