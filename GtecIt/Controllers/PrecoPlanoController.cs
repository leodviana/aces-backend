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
    public class PrecoPlanoController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public PrecoPlanoController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index(PrecoPlanoIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<PrecoPlanoGridViewModel>>(_uoW.PrecosPlano.ObterTodos().ToList().OrderBy(x => x.produtos.desc_produto));
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<PrecoPlanoGridViewModel>>(_uoW.Precos.ObterTodos().Where(x => x.produtos.desc_produto.Contains(model.produtos.desc_produto)).ToList().OrderBy(x => x.produtos.desc_produto));
            return View(model);

        }

        public ActionResult Create(string codigo)
        {
            var model = new PrecoPlanoCreateViewModel();

            model.DropdownConvenio = _uoW.Planos.ObterTodos()
                   .OrderBy(x => x.desc_plano)
                   .Select(x => new SelectListItem { Text = x.desc_plano, Value = x.idGrlplanos.ToString() })
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
        public ActionResult Create(PrecoPlanoCreateViewModel model)
        {
            if (!TryValidateModel(model))
            {
                var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                           .Where(v => v.ErrorMessage.Trim() != "")
                                           .Select(x => x.ErrorMessage)
                                           .Distinct()
                                           .ToList();

                /*var validationErrors = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());*/

                var resposta = new
                {

                    Sucesso = false,
                    msg = validationErrors
                };

                return Json(resposta);
                // return Json(new { sucesso = false, msg = validationErrors });
                //return Json(false, validationErrors);
            }
            //
            if (model.preco <= decimal.Zero)
            {
                var mensagem = new List<String>();

                mensagem.Add("O Preço tem que ser maior que zero!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }

            _uoW.PrecosPlano.Salvar(Mapper.Map<PrecoPlano>(model));
            _uoW.Complete();
            var response = new
            {

                Sucesso = true,
                msg = ""
            };

            return Json(response);
            // return RedirectToAction("Edit", new { codigo =_precoApp.GetAll().LastOrDefault().Id_stqcdprd});
        }



        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<PrecoPlanoEditViewModel>(_uoW.PrecosPlano.ObterPorId(codigo));


            model.DropdownConvenio = _uoW.Planos.ObterTodos()
                  .OrderBy(x => x.desc_plano)
                  .Select(x => new SelectListItem { Text = x.desc_plano, Value = x.idGrlplanos.ToString() })
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
        public ActionResult Edit(PrecoPlanoEditViewModel model)
        {
            /*model.ddd_telefone = FiltroDeCaracteres.RemoverCaracteresEspeciais(model.ddd_telefone);
            model.ddd_telefone2 = FiltroDeCaracteres.RemoverCaracteresEspeciais(model.ddd_telefone2);
            
            checa_relacionamento_edicao(model);*/

            if (!TryValidateModel(model))
            {
                var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                           .Where(v => v.ErrorMessage.Trim() != "")
                                           .Select(x => x.ErrorMessage)
                                           .Distinct()
                                           .ToList();

                /*var validationErrors = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());*/

                var resposta = new
                {

                    Sucesso = false,
                    msg = validationErrors
                };

                return Json(resposta);
                // return Json(new { sucesso = false, msg = validationErrors });
                //return Json(false, validationErrors);
            }
            //
            if (model.preco <= decimal.Zero)
            {
                var mensagem = new List<String>();

                mensagem.Add("O Preço tem que ser maior que zero!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }

            _uoW.PrecosPlano.Atualizar(Mapper.Map<PrecoPlano>(model));
            _uoW.Complete();

            var response = new
            {

                Sucesso = true,
                msg = ""
            };

            return Json(response);

        }


        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.PrecosPlano.ObterPorId(codigo);

            if (model == null)
            {
                return Json(false);
            }

            _uoW.PrecosPlano.RemoverPorId(codigo);
            _uoW.Complete();

            return Json(true);
        }

        public bool VerificarFiltroVazio(PrecoPlanoIndexViewModel model)
        {
            model = model ?? new PrecoPlanoIndexViewModel();

            var ehVazio = model.produtos.desc_produto.IsNullOrWhiteSpace();

            return ehVazio;
        }
        public JsonResult ObteValor(int codigo, int codigo2)
        {
            var fornecedor = _uoW.PrecosPlano.ObterTodos().OrderByDescending(x => x.vigencia).FirstOrDefault(x => x.produtos.Id_stqcdprd == codigo && x.idGrlplanos == codigo2);

            return fornecedor == null ? Json(false, JsonRequestBehavior.AllowGet) : Json(fornecedor.preco, JsonRequestBehavior.AllowGet);
        }


    }
}
