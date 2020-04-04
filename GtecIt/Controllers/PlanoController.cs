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
    public class PlanoController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public PlanoController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index(PlanoIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<PlanoGridViewModel>>(_uoW.Planos.ObterTodos().ToList());
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<PlanoGridViewModel>>(_uoW.Planos.ObterTodos().Where(x => x.desc_plano.Contains(model.desc_plano)).ToList());
            return View(model);

        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(PlanoCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

           _uoW.Planos.Salvar(Mapper.Map<Plano>(model));
            _uoW.Complete();
            return Json(true);
        }

        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<PlanoEditViewModel>(_uoW.Planos.ObterPorId(codigo));

            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(PlanoEditViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            _uoW.Planos.Atualizar(Mapper.Map<Plano>(model));
            _uoW.Complete();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model =_uoW.Planos.ObterPorId(codigo);

            if (model == null)
            {
                return Json(false);
            }

            _uoW.Planos.RemoverPorId(codigo);

            return Json(true);
        }

        public bool VerificarFiltroVazio(PlanoIndexViewModel model)
        {
            model = model ?? new PlanoIndexViewModel();

            var ehVazio = model.desc_plano.IsNullOrWhiteSpace();

            return ehVazio;
        }
        public JsonResult ObterDescricaoGrupo(int codigo)
        {
            var fornecedor = _uoW.Planos.ObterTodos().FirstOrDefault(x => x.idGrlplanos == codigo);


            return fornecedor == null ? Json(false, JsonRequestBehavior.AllowGet) : Json(fornecedor.desc_plano, JsonRequestBehavior.AllowGet);
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
                        var model = _uoW.Planos.ObterTodos().Where(x => x.idGrlplanos == codigo);

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.idGrlplanos);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.desc_plano);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalGrupo({0}, '{1}');\"  class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.idGrlplanos, item.desc_plano);
                            html += "</tr>";
                        }
                    }
                    break;
                case "descricao":
                    {
                        var model =
                           _uoW.Planos.ObterTodos()
                                .Where(x => x.desc_plano.ToLower().Trim().Contains(filtro.ToLower().Trim()));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.idGrlplanos);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.desc_plano);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalGrupo({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.idGrlplanos, item.desc_plano);
                            html += "</tr>";
                        }
                    }
                    break;
                case "todos":
                    {
                        var model = _uoW.Planos.ObterTodos();

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.idGrlplanos);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.desc_plano);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalGrupo({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.idGrlplanos, item.desc_plano);
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

            _uoW.Planos.Salvar(Mapper.Map<Plano>(model));

            var TipoTelefone = _uoW.Planos.ObterTodos().OrderByDescending(x => x.idGrlplanos).FirstOrDefault();

            return Json(TipoTelefone);
        }

        public JsonResult AutoCompleteGrupoPreFetch()
        {
            try
            {
                var resultado = _uoW.Planos.ObterTodos().Select(x =>
                new
                {
                    Id = x.idGrlplanos.ToString(),
                    Codigo = x.idGrlplanos.ToString(),
                    Descricao = x.desc_plano.ToString(),
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
