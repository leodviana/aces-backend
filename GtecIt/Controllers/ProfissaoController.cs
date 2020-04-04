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
    public class ProfissaoController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public ProfissaoController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index(ProfissaoIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<ProfissaoGridViewModel>>(_uoW.Profissoes.ObterTodos().ToList());
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<ProfissaoGridViewModel>>(_uoW.Profissoes.ObterTodos().Where(x => x.descricao.Contains(model.descricao)).ToList());
            return View(model);

        }

        public ActionResult Create()
        {
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(ProfissaoCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            _uoW.Profissoes.Salvar(Mapper.Map<Profissao>(model));
            _uoW.Complete();
            return Json(true);
        }

        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<ProfissaoEditViewModel>(_uoW.Profissoes.ObterPorId(codigo));

            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(ProfissaoEditViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            _uoW.Profissoes.Salvar(Mapper.Map<Profissao>(model));
            _uoW.Complete();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.Profissoes.ObterPorId(codigo);

            if (model == null)
            {
                return Json(false);
            }

            _uoW.Profissoes.RemoverPorId(codigo);
            _uoW.Complete();
            return Json(true);
        }

        public bool VerificarFiltroVazio(ProfissaoIndexViewModel model)
        {
            model = model ?? new ProfissaoIndexViewModel();

            var ehVazio = model.descricao.IsNullOrWhiteSpace();

            return ehVazio;
        }

        public JsonResult ObterDescricaoProfissao(int codigo)
        {
            var fornecedor = _uoW.Profissoes.ObterTodos().FirstOrDefault(x => x.Id_grlprofi == codigo);

            return fornecedor == null ? Json(false, JsonRequestBehavior.AllowGet) : Json(fornecedor.descricao, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObterProfissao(string tipoConsulta, string filtro)
        {
            string html = "";
            //// tipoConsulta = codigo || descricao

            switch (tipoConsulta)
            {
                case "codigo":
                    {
                        var codigo = Convert.ToInt32(filtro);
                        var model = _uoW.Profissoes.ObterTodos().Where(x => x.Id_grlprofi == codigo);

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_grlprofi);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.descricao);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalProfisao({0}, '{1}');\"  class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_grlprofi, item.descricao);
                            html += "</tr>";
                        }
                    }
                    break;
                case "descricao":
                    {
                        var model =
                            _uoW.Profissoes.ObterTodos()
                                .Where(x => x.descricao.ToLower().Trim().Contains(filtro.ToLower().Trim()));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_grlprofi);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.descricao);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalProfissao({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_grlprofi, item.descricao);
                            html += "</tr>";
                        }
                    }
                    break;
                case "todos":
                    {
                        var model = _uoW.Profissoes.ObterTodos();

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_grlprofi);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.descricao);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalProfissao({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_grlprofi, item.descricao);
                            html += "</tr>";
                        }
                    }
                    break;
            }

            return Json(html);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateModal(ProfissaoCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);
            try
            {
                _uoW.Profissoes.Salvar(Mapper.Map<Profissao>(model));
                _uoW.Complete();

                var TipoTelefone = _uoW.Profissoes.ObterTodos().OrderByDescending(x => x.Id_grlprofi).FirstOrDefault();
                return Json(TipoTelefone);
            }
            catch(Exception ex)
            {

            }
            return Json(false);
        }

    }
}
