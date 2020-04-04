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
    public class TipoTelefoneController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public TipoTelefoneController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index(TipoTelefoneIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<TipoTelefoneGridViewModel>>(_uoW.TipoTelefones.ObterTodos().ToList());
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<TipoTelefoneGridViewModel>>(_uoW.TipoTelefones.ObterTodos().Where(x => x.descricao.Contains(model.Descricao)).ToList());
            return View(model);

        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(TipoTelefoneCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            _uoW.TipoTelefones.Salvar(Mapper.Map<TipoTelefone>(model));
            _uoW.Complete();
            return Json(true);
        }

        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<TipoTelefoneEditViewModel>(_uoW.TipoTelefones.ObterPorId(codigo));

            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(TipoTelefoneEditViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            _uoW.TipoTelefones.Atualizar(Mapper.Map<TipoTelefone>(model));
            _uoW.Complete();

            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.TipoTelefones.ObterPorId(codigo);

            if (model == null)
            {
                return Json(false);
            }

            _uoW.TipoTelefones.RemoverPorId(codigo);
            _uoW.Complete();

            return Json(true);
        }

        public bool VerificarFiltroVazio(TipoTelefoneIndexViewModel model)
        {
            model = model ?? new TipoTelefoneIndexViewModel();

            var ehVazio = model.Descricao.IsNullOrWhiteSpace();

            return ehVazio;
        }

        public JsonResult ObterDescricaoTipoTelefone(int codigo)
        {
            var fornecedor = _uoW.TipoTelefones.ObterTodos().FirstOrDefault(x => x.id_grlidtel == codigo);

            return fornecedor == null ? Json(false, JsonRequestBehavior.AllowGet) : Json(fornecedor.descricao, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ObterTipoTelefone(string tipoConsulta, string filtro)
        {
            string html = "";
            //// tipoConsulta = codigo || descricao

            switch (tipoConsulta)
            {
                case "codigo":
                    {
                        var codigo = Convert.ToInt32(filtro);
                        var model = _uoW.TipoTelefones.ObterTodos().Where(x => x.id_grlidtel == codigo);

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.id_grlidtel);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.descricao);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalTipoTelefone({0}, '{1}');\"  class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.id_grlidtel, item.descricao);
                            html += "</tr>";
                        }
                    }
                    break;
                case "descricao":
                    {
                        var model =
                            _uoW.TipoTelefones.ObterTodos()
                                .Where(x => x.descricao.ToLower().Trim().Contains(filtro.ToLower().Trim()));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.id_grlidtel);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.descricao);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalTipoTelefone({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.id_grlidtel, item.descricao);
                            html += "</tr>";
                        }
                    }
                    break;
                case "todos":
                    {
                        var model = _uoW.TipoTelefones.ObterTodos();

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.id_grlidtel);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.descricao);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalTipoTelefone({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.id_grlidtel, item.descricao);
                            html += "</tr>";
                        }
                    }
                    break;
            }

            return Json(html);
        }

        public ActionResult ObterTipoTelefone2(string tipoConsulta, string filtro)
        {
            string html = "";
            //// tipoConsulta = codigo || descricao

            switch (tipoConsulta)
            {
                case "codigo":
                    {
                        var codigo = Convert.ToInt32(filtro);
                        var model = _uoW.TipoTelefones.ObterTodos().Where(x => x.id_grlidtel == codigo);

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.id_grlidtel);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.descricao);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalTpfone2({0}, '{1}');\"  class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.id_grlidtel, item.descricao);
                            html += "</tr>";
                        }
                    }
                    break;
                case "descricao":
                    {
                        var model =
                            _uoW.TipoTelefones.ObterTodos()
                                .Where(x => x.descricao.ToLower().Trim().Contains(filtro.ToLower().Trim()));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.id_grlidtel);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.descricao);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalTpfone2({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.id_grlidtel, item.descricao);
                            html += "</tr>";
                        }
                    }
                    break;
                case "todos":
                    {
                        var model = _uoW.TipoTelefones.ObterTodos();

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.id_grlidtel);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.descricao);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalTpfone2({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.id_grlidtel, item.descricao);
                            html += "</tr>";
                        }
                    }
                    break;
            }

            return Json(html);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateModal(TipoTelefoneCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            _uoW.TipoTelefones.Salvar(Mapper.Map<TipoTelefone>(model));
            _uoW.Complete();
            var TipoTelefone = _uoW.TipoTelefones.ObterTodos().OrderByDescending(x => x.id_grlidtel).FirstOrDefault();

            return Json(TipoTelefone);
        }

    }
}
