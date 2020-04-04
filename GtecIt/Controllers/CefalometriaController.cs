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
    public class CefalometriaController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public CefalometriaController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index(CefalometriaIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<CefalometriaGridViewModel>>(_uoW.Cefalometrias.ObterTodos());
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid =
                Mapper.Map<List<CefalometriaGridViewModel>>(
                    _uoW.Cefalometrias.ObterTodos().Where(x => x.desc_cefalometria.Contains(model.desc_cefalometria)).ToList());
            return View(model);

        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(CefalometriaCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            try
            {
                _uoW.Cefalometrias.Salvar(Mapper.Map<Cefalometria>(model));
                _uoW.Complete();
            }
            catch (Exception ex)
            {

            }
            // _bancoApp.Add(Mapper.Map<Banco>(model));

            return Json(true);
        }

        public ActionResult Edit(int codigo)
        {
            //var model = Mapper.Map<BancoEditViewModel>(_bancoApp.GetById(codigo));
            var model =
                Mapper.Map<CefalometriaEditViewModel>(_uoW.Cefalometrias.ObterTodos().FirstOrDefault(x => x.id_GrlCefalometrias == codigo));
            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(CefalometriaEditViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            _uoW.Cefalometrias.Atualizar(Mapper.Map<Cefalometria>(model));
            _uoW.Complete();
            // _bancoApp.Update(Mapper.Map<Banco>(model));

            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.Cefalometrias.ObterPorId(codigo); //_bancoApp.GetById(codigo);

            if (model == null)
            {
                return Json(false);
            }

            _uoW.Cefalometrias.RemoverPorId(codigo);
            _uoW.Complete();
            //_bancoApp.Remove(model);

            return Json(true);
        }

        public bool VerificarFiltroVazio(CefalometriaIndexViewModel model)
        {
            model = model ?? new CefalometriaIndexViewModel();

            var ehVazio = model.desc_cefalometria.IsNullOrWhiteSpace();

            return ehVazio;
        }
    }
}
