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
    public class ConvenioController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public ConvenioController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }
        
        public ActionResult Index(ConvenioIndexViewModel model, string tipoacao)
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
                    model.Grid = Mapper.Map<List<ConvenioGridViewModel>>(_uoW.Convenios.ObterTodos().ToList().OrderBy(x => x.grlbasic.nome));
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
            model.Grid = Mapper.Map<List<ConvenioGridViewModel>>(_uoW.Convenios.ObterTodos().Where(x => x.grlbasic.nome.Contains(model.Nome)).ToList().OrderBy(x => x.grlbasic.nome));
            return View(model);

        }

        public ActionResult Create()
        {
            var model = new ConvenioEditViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ConvenioEditViewModel model)
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
            var model2 = Mapper.Map<ConvenioEditViewModel>(_uoW.Convenios.ObterTodos().FirstOrDefault(x => x.id_grlbasico == Convert.ToInt32(model.id_grlbasico)));

            if (model2 == null)
            {
                try
                {
                    //_convenioApp.Add(Mapper.Map<Convenio>(model));
                    _uoW.Convenios.Salvar(Mapper.Map<Convenio>(model));
                    _uoW.Complete();
                }
                catch (Exception)
                {

                    throw;
                }

            }
            else
            {
               // _convenioApp.Update(Mapper.Map<Convenio>(model2));
               _uoW.Convenios.Atualizar(Mapper.Map<Convenio>(model));
                _uoW.Complete();
            }

            return RedirectToAction("Index");

        }




        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<ConvenioEditViewModel>(_uoW.Convenios.ObterPorId(codigo));

            if (model == null)
                return HttpNotFound();


            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ConvenioEditViewModel model)
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
            model.Guia = model.Guia.ToUpper();
          //  _convenioApp.Update(Mapper.Map<Convenio>(model));
            _uoW.Convenios.Atualizar(Mapper.Map<Convenio>(model));
            _uoW.Complete();
            return RedirectToAction("Index");

        }


        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            //var model = _convenioApp.GetById(codigo);
            var model = Mapper.Map<ConvenioEditViewModel>(_uoW.Convenios.ObterPorId(codigo));
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
                //_convenioApp.Update(Mapper.Map<Convenio>(model));
                _uoW.Convenios.Atualizar(Mapper.Map<Convenio>(model));
                _uoW.Complete();

            }
            catch (Exception)
            {

                throw;
            }
            // _convenioApp.Update(Mapper.Map<Convenio>(model));

            return Json(true);
        }

        public bool VerificarFiltroVazio(ConvenioIndexViewModel model)
        {
            model = model ?? new ConvenioIndexViewModel();

            var ehVazio = string.IsNullOrEmpty(model.Nome);

            return ehVazio;
        }
        public JsonResult ObterDescricaoCliente(int codigo)
        {
            var fornecedor = _uoW.Convenios.ObterTodos().FirstOrDefault(x => x.id_grlconvenio == codigo);


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
                        var model = _uoW.Convenios.ObterTodos().Where(x => x.id_grlconvenio == codigo && x.Ativo.Equals("S"));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.id_grlconvenio);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.grlbasic.nome);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalCliente({0}, '{1}');\"  class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.id_grlconvenio, item.grlbasic.nome);
                            html += "</tr>";
                        }
                    }
                    break;
                case "descricao":
                    {
                        var model =
                           _uoW.Convenios.ObterTodos()
                                .Where(x => x.grlbasic.nome.ToLower().Trim().Contains(filtro.ToLower().Trim()) && x.Ativo.Equals("S"));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.id_grlconvenio);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.grlbasic.nome);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalCliente({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.id_grlconvenio, item.grlbasic.nome);
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
        public JsonResult AutoCompleteConvenioPreFetch()
        {
            try
            {
                var resultado = _uoW.Convenios.ObterTodos().Select(x =>
                new
                {
                    Isn = x.id_grlconvenio.ToString(),
                    Codigo = x.id_grlbasico.ToString(),
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