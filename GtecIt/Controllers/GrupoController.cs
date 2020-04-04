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
    public class GrupoController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public GrupoController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index(GrupoIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<GrupoGridViewModel>>(_uoW.Grupos.ObterTodos().ToList());
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<GrupoGridViewModel>>(_uoW.Grupos.ObterTodos().Where(x => x.desc_grupo.Contains(model.desc_grupo)).ToList());
            return View(model);

        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(GrupoCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

           _uoW.Grupos.Salvar(Mapper.Map<Grupo>(model));
            _uoW.Complete();
            return Json(true);
        }

        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<GrupoEditViewModel>(_uoW.Grupos.ObterPorId(codigo));

            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(GrupoEditViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            _uoW.Grupos.Atualizar(Mapper.Map<Grupo>(model));
            _uoW.Complete();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model =_uoW.Grupos.ObterPorId(codigo);

            if (model == null)
            {
                return Json(false);
            }

            _uoW.Grupos.RemoverPorId(codigo);

            return Json(true);
        }

        public bool VerificarFiltroVazio(GrupoIndexViewModel model)
        {
            model = model ?? new GrupoIndexViewModel();

            var ehVazio = model.desc_grupo.IsNullOrWhiteSpace();

            return ehVazio;
        }
        public JsonResult ObterDescricaoGrupo(int codigo)
        {
            var fornecedor = _uoW.Grupos.ObterTodos().FirstOrDefault(x => x.Id_stqcdgrp == codigo);


            return fornecedor == null ? Json(false, JsonRequestBehavior.AllowGet) : Json(fornecedor.desc_grupo, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObterGrupo(string tipoConsulta, string filtro)
        {
            string html = "";
            //// tipoConsulta = codigo || descricao

            switch (tipoConsulta)
            {
                case "codigo":
                    {
                        var codigo = Convert.ToInt32(filtro);
                        var model = _uoW.Grupos.ObterTodos().Where(x => x.Id_stqcdgrp == codigo);

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_stqcdgrp);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.desc_grupo);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalGrupo({0}, '{1}');\"  class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_stqcdgrp, item.desc_grupo);
                            html += "</tr>";
                        }
                    }
                    break;
                case "descricao":
                    {
                        var model =
                           _uoW.Grupos.ObterTodos()
                                .Where(x => x.desc_grupo.ToLower().Trim().Contains(filtro.ToLower().Trim()));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_stqcdgrp);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.desc_grupo);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalGrupo({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_stqcdgrp, item.desc_grupo);
                            html += "</tr>";
                        }
                    }
                    break;
                case "todos":
                    {
                        var model = _uoW.Grupos.ObterTodos();

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_stqcdgrp);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.desc_grupo);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalGrupo({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_stqcdgrp, item.desc_grupo);
                            html += "</tr>";
                        }
                    }
                    break;
            }

            return Json(html);
        }

        public JsonResult CreateModal(GrupoCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            _uoW.Grupos.Salvar(Mapper.Map<Grupo>(model));

            var TipoTelefone = _uoW.Grupos.ObterTodos().OrderByDescending(x => x.Id_stqcdgrp).FirstOrDefault();

            return Json(TipoTelefone);
        }

        public JsonResult AutoCompleteGrupoPreFetch()
        {
            try
            {
                var resultado = _uoW.Grupos.ObterTodos().Select(x =>
                new
                {
                    Id = x.Id_stqcdgrp.ToString(),
                    Codigo = x.Id_stqcdgrp.ToString(),
                    Descricao = x.desc_grupo.ToString(),
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
