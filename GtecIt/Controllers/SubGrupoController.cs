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
    public class SubGrupoController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public SubGrupoController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index(SubGrupoIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<SubGrupoGridViewModel>>(_uoW.SubGrupos.ObterTodos().ToList());
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<SubGrupoGridViewModel>>(_uoW.SubGrupos.ObterTodos().Where(x => x.desc_subgrupo.Contains(model.desc_subgrupo)).ToList());
            return View(model);

        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(SubGrupoCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            try
            { 
            _uoW.SubGrupos.Salvar(Mapper.Map<SubGrupo>(model));
            }
            catch(Exception ex)
            {

            }
            return Json(true);
        }

        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<SubGrupoEditViewModel>(_uoW.SubGrupos.ObterPorId(codigo));

            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(SubGrupoEditViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            _uoW.SubGrupos.Atualizar(Mapper.Map<SubGrupo>(model));

            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.SubGrupos.ObterPorId(codigo);

            if (model == null)
            {
                return Json(false);
            }

            _uoW.SubGrupos.RemoverPorId(codigo);

            return Json(true);
        }

        public bool VerificarFiltroVazio(SubGrupoIndexViewModel model)
        {
            model = model ?? new SubGrupoIndexViewModel();

            var ehVazio = model.desc_subgrupo.IsNullOrWhiteSpace();

            return ehVazio;
        }
        public JsonResult ObterDescricaoSubGrupo(int codigo)
        {
            var fornecedor = _uoW.SubGrupos.ObterTodos().FirstOrDefault(x => x.Id_stqsbgrp == codigo);


            return fornecedor == null ? Json(false, JsonRequestBehavior.AllowGet) : Json(fornecedor.desc_subgrupo, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObterSubGrupo(string tipoConsulta, string filtro)
        {
            string html = "";
            //// tipoConsulta = codigo || descricao

            switch (tipoConsulta)
            {
                case "codigo":
                    {
                        var codigo = Convert.ToInt32(filtro);
                        var model = _uoW.SubGrupos.ObterTodos().Where(x => x.Id_stqsbgrp == codigo);

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_stqsbgrp);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.desc_subgrupo);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalSubGrupo({0}, '{1}');\"  class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_stqsbgrp, item.desc_subgrupo);
                            html += "</tr>";
                        }
                    }
                    break;
                case "descricao":
                    {
                        var model =
                            _uoW.SubGrupos.ObterTodos()
                                .Where(x => x.desc_subgrupo.ToLower().Trim().Contains(filtro.ToLower().Trim()));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_stqsbgrp);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.desc_subgrupo);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalSubGrupo({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_stqsbgrp, item.desc_subgrupo);
                            html += "</tr>";
                        }
                    }
                    break;
                case "todos":
                    {
                        var model = _uoW.SubGrupos.ObterTodos();

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_stqsbgrp);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.desc_subgrupo);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalSubGrupo({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_stqsbgrp, item.desc_subgrupo);
                            html += "</tr>";
                        }
                    }
                    break;
            }

            return Json(html);
        }
        public JsonResult CreateModal(SubGrupoCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            _uoW.SubGrupos.Salvar(Mapper.Map<SubGrupo>(model));

            var TipoTelefone = _uoW.SubGrupos.ObterTodos().OrderByDescending(x => x.Id_stqsbgrp).FirstOrDefault();

            return Json(TipoTelefone);
        }
        public JsonResult AutoCompleteSubGrupoPreFetch()
        {
            try
            {
                var resultado = _uoW.SubGrupos.ObterTodos().Select(x =>
                new
                {
                    Id = x.Id_stqsbgrp.ToString(),
                    Codigo = x.Id_stqsbgrp.ToString(),
                    Descricao = x.desc_subgrupo.ToString(),
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
