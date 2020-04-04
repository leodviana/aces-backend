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
    public class BancoController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public BancoController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index(BancoIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<BancoGridViewModel>>(_uoW.Bancos.ObterTodos());
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid =
                Mapper.Map<List<BancoGridViewModel>>(
                    _uoW.Bancos.ObterTodos().Where(x => x.desc_banco.Contains(model.desc_banco)).ToList());
            return View(model);

        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(BancoCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            try
            {
                _uoW.Bancos.Salvar(Mapper.Map<Banco>(model));
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
                Mapper.Map<BancoEditViewModel>(_uoW.Bancos.ObterTodos().FirstOrDefault(x => x.id_Fincdbanco == codigo));
            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(BancoEditViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            _uoW.Bancos.Atualizar(Mapper.Map<Banco>(model));
            _uoW.Complete();
            // _bancoApp.Update(Mapper.Map<Banco>(model));

            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.Bancos.ObterPorId(codigo); //_bancoApp.GetById(codigo);

            if (model == null)
            {
                return Json(false);
            }

            _uoW.Bancos.RemoverPorId(codigo);
            _uoW.Complete();
            //_bancoApp.Remove(model);

            return Json(true);
        }

        public bool VerificarFiltroVazio(BancoIndexViewModel model)
        {
            model = model ?? new BancoIndexViewModel();

            var ehVazio = model.desc_banco.IsNullOrWhiteSpace();

            return ehVazio;
        }
    }
}
