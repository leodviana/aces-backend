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
    public class CefalometriaItemController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public CefalometriaItemController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }
        public ActionResult Index(CefalometriaItemIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<CefalometriaItemGridViewModel>>(_uoW.CefalometriaItems.ObterTodos().OrderBy(x => x.grlcefalometria.desc_cefalometria));

                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;
            //model.Grid =
              //  Mapper.Map<List<BancoGridViewModel>>(
                //    _uoW.Bancos.ObterTodos().Where(x => x.desc_banco.Contains(model.desc_banco)).ToList());

            model.Grid = Mapper.Map<List<CefalometriaItemGridViewModel>>(_uoW.CefalometriaItems.ObterTodos().Where(x => x.grlcefalometria.desc_cefalometria.Contains(model.grlcefalometria.desc_cefalometria)).ToList());
            return View(model);

        }

        public ActionResult Create(string codigo)
        {
            var model = new CefalometriaItemCreateViewModel();

            model.id_grldentista = Convert.ToInt16(codigo);
            try
            {
                model.DropdownCefalometricas = _uoW.Cefalometrias.ObterTodos().OrderBy(x => x.desc_cefalometria)
                 
                 .Select(x => new SelectListItem { Text = x.desc_cefalometria, Value = x.id_GrlCefalometrias.ToString() })
                 .ToList();
            }
            catch (Exception)
            {

                throw;
            }


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(CefalometriaItemCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            //Mapper.Map<BancoEditViewModel>(_uoW.Bancos.ObterTodos().FirstOrDefault(x => x.id_Fincdbanco == codigo));
            try
            {
                var model2 =
                    Mapper.Map<CefalometriaItemEditViewModel>(
                        _uoW.CefalometriaItems.ObterTodos()
                            .FirstOrDefault(
                                x =>
                                    x.id_grldentista ==model.id_grldentista &&
                                    x.id_GrlCefalometrias == model.id_GrlCefalometrias));

                if (model2 == null)
                {
                    //_uoW.Bancos.Salvar(Mapper.Map<Banco>(model));
                    _uoW.CefalometriaItems.Salvar(Mapper.Map<CefalometriaItem>(model));
                    _uoW.Complete();
                }
                else
                {
                    _uoW.CefalometriaItems.Atualizar(Mapper.Map<CefalometriaItem>(model2));
                    _uoW.Complete();
                    //_cefalometriaItemApp.Update(Mapper.Map<CefalometriaItem>(model2));
                }
            }
            catch (Exception ex)
            {

            }

            return Json(true);
        }

        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<CefalometriaItemEditViewModel>(_uoW.CefalometriaItems.ObterPorId((codigo)));

            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(CefalometriaItemEditViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(false);

            _uoW.CefalometriaItems.Atualizar(Mapper.Map<CefalometriaItem>(model));
            _uoW.Complete();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.CefalometriaItems.ObterPorId((codigo));

            if (model == null)
            {
                return Json(false);
            }

            _uoW.CefalometriaItems.RemoverPorId(codigo);
            _uoW.Complete();

            return Json(true);
        }

        public bool VerificarFiltroVazio(CefalometriaItemIndexViewModel model)
        {
            model = model ?? new CefalometriaItemIndexViewModel();

            var ehVazio = model.grlcefalometria.desc_cefalometria.IsNullOrWhiteSpace();

            return ehVazio;
        }
    }
}