using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using System.Collections.Generic;
using System.Data;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GtecIt.Util;
using GtecIt.ViewModels;
using GtecIt.Domain.Entities;
using GtecIt.Infra.Data.Core;
using GtecIt.Models;
using Microsoft.Ajax.Utilities;

using GtecIt.Util;


namespace GtecIt.Controllers
{
    public class NotaEntradaItemController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public NotaEntradaItemController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }


        public ActionResult Index(NotaEntradaItemIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<NotaEntradaItemGridViewModel>>(_uoW.NotaEntradaItems.ObterTodos().ToList().OrderBy(x => x.Id_stqentra));
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<NotaEntradaItemGridViewModel>>(_uoW.NotaEntradaItems.ObterTodos().Where(x => x.Id_stqentra.Equals(model.Id_stqentra)).ToList().OrderBy(x => x.Id_stqentra));
            return View(model);

        }

        public ActionResult Create(string codigo, string codigo2)
        {
            var model = new NotaEntradaItemCreateViewModel();

            model.id_stqnoten = Convert.ToInt16(codigo);

            model.DropdownProduto = _uoW.Produtos.ObterTodos()
                 .OrderBy(x => x.desc_produto)
                 .Select(x => new SelectListItem { Text = x.desc_produto, Value = x.Id_stqcdprd.ToString() })
                 .ToList();
           
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NotaEntradaItemCreateViewModel model)
        {
            
            ModelState.Clear();

            if (!TryValidateModel(model))
            {
                var validationErrors = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());

                return View(model);
            }
            model.status_entrada = "0";
            _uoW.NotaEntradaItems.Salvar(Mapper.Map<NotaEntradaItem>(model));
            _uoW.Complete();
            return Json(true);
        }



        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<NotaEntradaItemEditViewModel>(_uoW.NotaEntradaItems.ObterPorId(codigo));
            //model.id_stqporcamento = Convert.ToInt16(codigo);

            model.DropdownProduto = _uoW.Produtos.ObterTodos()
                 .OrderBy(x => x.desc_produto)
                 .Select(x => new SelectListItem { Text = x.desc_produto, Value = x.Id_stqcdprd.ToString() })
                 .ToList();
            if (model == null)
                return HttpNotFound();

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NotaEntradaItemEditViewModel model)
        {

            
            ModelState.Clear();

            if (!TryValidateModel(model))
            {
                var validationErrors = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());

                return View(model);
            }
            model.status_entrada = "0";
            _uoW.NotaEntradaItems.Atualizar(Mapper.Map<NotaEntradaItem>(model));
            _uoW.Complete();

            return Json(true);
        }


        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.NotaEntradaItems.ObterPorId(codigo);

            if (model == null)
            {

                return Json(false);
            }
            try
            {

                //_orcamentoItemApp.Remove(model);
                _uoW.NotaEntradaItems.RemoverPorId(codigo);
                _uoW.Complete();
            }
            catch (Exception)
            {

                throw;
            }


            return Json(true);
        }

        public bool VerificarFiltroVazio(NotaEntradaItemIndexViewModel model)
        {
            model = model ?? new NotaEntradaItemIndexViewModel();

            var ehVazio = model.id_stqnoten.Equals(0);

            return ehVazio;
        }
    }

}
