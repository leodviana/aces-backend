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
    public class PrecoController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public PrecoController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index(PrecoIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<PrecoGridViewModel>>(_uoW.Precos.ObterTodos().ToList().OrderBy(x => x.produtos.desc_produto));
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<PrecoGridViewModel>>(_uoW.Precos.ObterTodos().Where(x => x.produtos.desc_produto.Contains(model.produtos.desc_produto)).ToList().OrderBy(x => x.produtos.desc_produto));
            return View(model);

        }

        public ActionResult Create(string codigo)
        {
            var model = new PrecoCreateViewModel();

            model.DropdownConvenio = _uoW.Convenios.ObterTodos()
                   .OrderBy(x => x.grlbasic.nome)
                   .Select(x => new SelectListItem { Text = x.grlbasic.nome, Value = x.id_grlconvenio.ToString() })
                   .ToList();

            model.id_stqcdprd = Convert.ToInt32(codigo);
            /*model.DropdownIdentifica = _tipotelefoneApp.GetAll()
                    .OrderBy(x => x.descricao)
                    .Select(x => new SelectListItem { Text = x.descricao, Value = x.id_grlidtel.ToString()})
                    .ToList();

            model.DropdownSexo = _sexoApp.GetAll()
                    .OrderBy(x => x.descricao)
                    .Select(x => new SelectListItem { Text = x.descricao, Value = x.Id_gercdsexo.ToString() })
                    .ToList();
            //preencher lista
           // model.EstadoCivil = Mapper.Map<List<EstadoCivilEditViewModel>>(_estadoCivilApp.GetAll().ToList());*/
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PrecoCreateViewModel model)
        {
            /*model.ddd_telefone = FiltroDeCaracteres.RemoverCaracteresEspeciais(model.ddd_telefone);
            model.ddd_telefone2 = FiltroDeCaracteres.RemoverCaracteresEspeciais(model.ddd_telefone2);
           
            
            checa_relacionamento(model);
            */
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

            _uoW.Precos.Salvar(Mapper.Map<Preco>(model));
            _uoW.Complete();
            return Json(true);
            // return RedirectToAction("Edit", new { codigo =_precoApp.GetAll().LastOrDefault().Id_stqcdprd});
        }



        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<PrecoEditViewModel>(_uoW.Precos.ObterPorId(codigo));


            model.DropdownConvenio = _uoW.Convenios.ObterTodos()
                  .OrderBy(x => x.grlbasic.nome)
                  .Select(x => new SelectListItem { Text = x.grlbasic.nome, Value = x.id_grlconvenio.ToString() })
                  .ToList();


            /*if (model.Id_stqcdgrp.HasValue)
                model.nome_grupo = _grupoApp.GetById(model.Id_stqcdgrp.Value).desc_grupo;

            if (model.Id_stqsbgrp.HasValue)
                model.nome_subgrupo = _subgrupoApp.GetById(model.Id_stqsbgrp.Value).desc_subgrupo;
            /*if (model == null)
                return HttpNotFound();

            if (model.Id_grlprofi.HasValue)
                model.NomeProfissao = _profissaoApp.GetById(model.Id_grlprofi.Value).descricao;

            model.DropdownEstado = _estadoCivilApp.GetAll()
                   .OrderBy(x => x.descricao)
                   .Select(x => new SelectListItem { Text = x.descricao, Value = x.Id_grlcivil.ToString() })
                   .ToList();

            model.DropdownIdentifica = _tipotelefoneApp.GetAll()
                    .OrderBy(x => x.descricao)
                    .Select(x => new SelectListItem { Text = x.descricao, Value = x.id_grlidtel.ToString() })
                    .ToList();

            model.DropdownSexo = _sexoApp.GetAll()
                    .OrderBy(x => x.descricao)
                    .Select(x => new SelectListItem { Text = x.descricao, Value = x.Id_gercdsexo.ToString() })
                    .ToList();

            */

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PrecoEditViewModel model)
        {
            /*model.ddd_telefone = FiltroDeCaracteres.RemoverCaracteresEspeciais(model.ddd_telefone);
            model.ddd_telefone2 = FiltroDeCaracteres.RemoverCaracteresEspeciais(model.ddd_telefone2);
            
            checa_relacionamento_edicao(model);*/

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

            _uoW.Precos.Atualizar(Mapper.Map<Preco>(model));
            _uoW.Complete();

            return Json(true);

        }


        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.Precos.ObterPorId(codigo);

            if (model == null)
            {
                return Json(false);
            }

            _uoW.Precos.RemoverPorId(codigo);
            _uoW.Complete();

            return Json(true);
        }

        public bool VerificarFiltroVazio(PrecoIndexViewModel model)
        {
            model = model ?? new PrecoIndexViewModel();

            var ehVazio = model.produtos.desc_produto.IsNullOrWhiteSpace();

            return ehVazio;
        }
        public JsonResult ObteValor(int codigo, int codigo2)
        {
            var fornecedor = _uoW.Precos.ObterTodos().OrderByDescending(x => x.vigencia).FirstOrDefault(x => x.produtos.Id_stqcdprd == codigo && x.id_grlconvenio == codigo2);

            return fornecedor == null ? Json(false, JsonRequestBehavior.AllowGet) : Json(fornecedor.preco, JsonRequestBehavior.AllowGet);
        }


    }
}
