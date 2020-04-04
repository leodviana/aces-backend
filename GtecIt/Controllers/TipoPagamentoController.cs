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
    public class TipoPagamentoController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public TipoPagamentoController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index(TipoPagamentoIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<TipoPagamentoGridViewModel>>(_uoW.TipoPagamentos.ObterTodos().ToList());
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<TipoPagamentoGridViewModel>>(_uoW.TipoPagamentos.ObterTodos().Where(x => x.Descricao.Contains(model.Descricao)).ToList());
            return View(model);

        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(TipoPagamentoCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            _uoW.TipoPagamentos.Salvar(Mapper.Map<TipoPagamento>(model));
            _uoW.Complete();
            return Json(true);
        }

        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<TipoPagamentoEditViewModel>(_uoW.TipoPagamentos.ObterPorId(codigo));

            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(TipoPagamentoEditViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            _uoW.TipoPagamentos.Atualizar(Mapper.Map<TipoPagamento>(model));
            _uoW.Complete();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.TipoPagamentos.ObterPorId(codigo);

            if (model == null)
            {
                return Json(false);
            }

            _uoW.TipoPagamentos.RemoverPorId(codigo);
            _uoW.Complete();

            return Json(true);
        }

        public bool VerificarFiltroVazio(TipoPagamentoIndexViewModel model)
        {
            model = model ?? new TipoPagamentoIndexViewModel();

            var ehVazio = model.Descricao.IsNullOrWhiteSpace();

            return ehVazio;
        }
    }
}
