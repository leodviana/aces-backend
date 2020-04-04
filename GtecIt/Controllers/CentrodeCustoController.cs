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
    public class CentrodeCustoController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public CentrodeCustoController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index(CentrodeCustoIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<CentrodeCustoGridViewModel>>(_uoW.Centrodecusto.ObterTodos().ToList());
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<CentrodeCustoGridViewModel>>(_uoW.Centrodecusto.ObterTodos().Where(x => x.desc_ccusto.Contains(model.desc_ccusto)).ToList());
            return View(model);

        }

        public ActionResult Create()
        {
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(CentrodeCustoCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);
            model.Ativo = "S";
            try
            {
                _uoW.Centrodecusto.Salvar(Mapper.Map<CentrodeCusto>(model));
                _uoW.Complete();
            }
            catch (Exception ex)
            {
                        
                throw;
            }
            
            return Json(true);
        }

        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<CentrodeCustoEditViewModel>(_uoW.Centrodecusto.ObterPorId(codigo));

            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(CentrodeCustoEditViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);
           // model.Ativo = "S";
            _uoW.Centrodecusto.Atualizar(Mapper.Map<CentrodeCusto>(model));
            _uoW.Complete();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.Centrodecusto.ObterPorId(codigo);

            if (model == null)
            {
                return Json(false);
            }

          //  _uoW.Centrodecusto.RemoverPorId(codigo);
            model.Ativo = "N";
            _uoW.Centrodecusto.Atualizar(Mapper.Map<CentrodeCusto>(model));
            _uoW.Complete();
            return Json(true);
        }

        public bool VerificarFiltroVazio(CentrodeCustoIndexViewModel model)
        {
            model = model ?? new CentrodeCustoIndexViewModel() ;

            var ehVazio = model.desc_ccusto.IsNullOrWhiteSpace();

            return ehVazio;
        }

        public JsonResult ObterDescricaoCentrodeCusto(int codigo)
        {
            var fornecedor = _uoW.Centrodecusto.ObterTodos().FirstOrDefault(x => x.Id_grlccust == codigo);

            return fornecedor == null ? Json(false, JsonRequestBehavior.AllowGet) : Json(fornecedor.desc_ccusto, JsonRequestBehavior.AllowGet);
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
                        var model = _uoW.Centrodecusto.ObterTodos().Where(x => x.Id_grlccust == codigo);

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_grlccust);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.desc_ccusto);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalProfisao({0}, '{1}');\"  class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_grlccust, item.desc_ccusto);
                            html += "</tr>";
                        }
                    }
                    break;
                case "descricao":
                    {
                        var model =
                            _uoW.Centrodecusto.ObterTodos()
                                .Where(x => x.desc_ccusto.ToLower().Trim().Contains(filtro.ToLower().Trim()));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_grlccust);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.desc_ccusto);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalProfissao({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_grlccust, item.desc_ccusto);
                            html += "</tr>";
                        }
                    }
                    break;
                case "todos":
                    {
                        var model = _uoW.Centrodecusto.ObterTodos();

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_grlccust);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.desc_ccusto);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalProfissao({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_grlccust, item.desc_ccusto);
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

            _uoW.Centrodecusto.Salvar(Mapper.Map<CentrodeCusto>(model));

            var TipoTelefone = _uoW.Centrodecusto.ObterTodos().OrderByDescending(x => x.Id_grlccust).FirstOrDefault();

            return Json(TipoTelefone);
        }

        public JsonResult AutoCompleteCentrodeCustoPreFetch()
        {
            try
            {
                var resultado = _uoW.Centrodecusto.ObterTodos().Select(x =>
                new
                {
                    Id = x.Id_grlccust.ToString(),
                    Codigo = x.Id_grlccust.ToString(),
                    Descricao = x.desc_ccusto.ToString(),
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
