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
using GtecIt.Util;
using GtecIt.ViewModels;
using GtecIt.Domain.Entities;
using GtecIt.Infra.Data.Core;
using Microsoft.Ajax.Utilities;


namespace GtecIt.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public ProdutoController(IUnitOfWork uoW)
        {
            _uoW = uoW;
            

        }

        public ActionResult Index(ProdutoIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<ProdutoGridViewModel>>(_uoW.Produtos.ObterTodos().ToList().OrderBy(x => x.desc_produto));
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<ProdutoGridViewModel>>(_uoW.Produtos.ObterTodos().Where(x => x.desc_produto.Contains(model.desc_produto)).ToList().OrderBy(x => x.desc_produto));
            return View(model);

        }

        public ActionResult Create()
        {
            var model = new ProdutoCreateViewModel();

            /* model.DropdownGrupo = _grupoApp.GetAll()
                    .OrderBy(x => x.desc_grupo)
                    .Select(x => new SelectListItem { Text = x.desc_grupo, Value = x.Id_stqcdgrp.ToString()})
                    .ToList();

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
        public ActionResult Create(ProdutoCreateViewModel model)
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
                SetarMensagens(new MensagemAuxiliarViewModel() { Tipo = "danger", Mensagens = new List<string>() { validationErrors } });
                return View(model);
            }
            var produto = Mapper.Map<Produto>(model);
            _uoW.Produtos.Salvar(produto);
            _uoW.Complete();
            var idPessoa =produto.Id_stqcdprd;

            //_uoW.Produtos.Salvar(Mapper.Map<Produto>(model));
            //_uoW.Complete();
            
            //_produtoApp.Add(Mapper.Map<Produto>(model));

            //return Json(true);*/
            return RedirectToAction("Edit", new { codigo = idPessoa });
        }


        private void SetarMensagens(MensagemAuxiliarViewModel model)
        {
            TempData["Mensagens"] = model;
        }
        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<ProdutoEditViewModel>(_uoW.Produtos.ObterPorId(codigo));

            if (model.Id_stqcdgrp.HasValue)
                model.nome_grupo = _uoW.Grupos.ObterPorId(model.Id_stqcdgrp.Value).desc_grupo;

            if (model.Id_stqsbgrp.HasValue)
                model.nome_subgrupo = _uoW.SubGrupos.ObterPorId(model.Id_stqsbgrp.Value).desc_subgrupo;
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
        public ActionResult Edit(ProdutoEditViewModel model)
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

            _uoW.Produtos.Atualizar(Mapper.Map<Produto>(model));
            _uoW.Complete();
            
            return RedirectToAction("Index");

        }


        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.Produtos.ObterPorId(codigo);

            if (model == null)
            {
                return Json(false);
            }
            _uoW.Produtos.RemoverPorId(codigo);
            _uoW.Complete();
            //_produtoApp.Remove(model);

            return Json(true);
        }

        public bool VerificarFiltroVazio(ProdutoIndexViewModel model)
        {
            model = model ?? new ProdutoIndexViewModel();

            var ehVazio = model.desc_produto.IsNullOrWhiteSpace();

            return ehVazio;
        }
        public JsonResult ObterDescricaoProduto(int codigo)
        {
            var fornecedor = _uoW.Produtos.ObterTodos().FirstOrDefault(x => x.Id_stqcdprd == codigo);

            return fornecedor == null ? Json(false, JsonRequestBehavior.AllowGet) : Json(fornecedor.desc_produto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult nomeDuplicado(string desc_produto)
        {
            var existe = _uoW.Produtos.ObterTodos().Where(x => x.desc_produto.ToUpper().Equals(desc_produto.ToUpper())).FirstOrDefault(); ;
            if (existe != null)
                return Json(false);

            return Json(true);

        }
    }
}
