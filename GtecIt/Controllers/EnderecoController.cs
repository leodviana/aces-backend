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
    public class EnderecoController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public EnderecoController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index(EnderecoIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<EnderecoGridViewModel>>(_uoW.Enderecos.ObterTodos().ToList().OrderBy(x => x.Cep));
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<EnderecoGridViewModel>>(_uoW.Enderecos.ObterTodos().Where(x => x.Cep.Contains(model.Cep)).ToList().OrderBy(x => x.Cep));
            return View(model);

        }

        public ActionResult Create(string codigo)
        {
            var model = new EnderecoCreateViewModel();
            model.Id_grlbasic = Convert.ToInt16(codigo);
            //model.Id_grlcdusu = 1;
            //model.Id_grlidtel = 1;
            //model.Id_grlbasic = Convert.ToInt32(codigo);

            //model.DropdownIdentifica = _tipotelefoneApp.GetAll()
            //        .OrderBy(x => x.descricao)
            //        .Select(x => new SelectListItem { Text = x.descricao, Value = x.id_grlidtel.ToString()})
            //        .ToList();


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EnderecoCreateViewModel model)
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

           _uoW.Enderecos.Salvar(Mapper.Map<Endereco>(model));
            _uoW.Complete();

            return Json(true);
        }



        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<EnderecoEditViewModel>(_uoW.Enderecos.ObterPorId(codigo));

            if (model == null)
                return HttpNotFound();

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EnderecoEditViewModel model)
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

            _uoW.Enderecos.Atualizar(Mapper.Map<Endereco>(model));
            _uoW.Complete();

            return Json(true);
        }


        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.Enderecos.ObterPorId(codigo);

            if (model == null)
            {
                return Json(false);
            }

            _uoW.Enderecos.RemoverPorId(codigo);
            _uoW.Complete();

            return Json(true);
        }

        public bool VerificarFiltroVazio(EnderecoIndexViewModel model)
        {
            model = model ?? new EnderecoIndexViewModel();

            var ehVazio = model.Logradouro.IsNullOrWhiteSpace();

            return ehVazio;
        }
    }
}
