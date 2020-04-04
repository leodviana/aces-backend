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
using WebGrease.Css.Extensions;

namespace GtecIt.Controllers
{
    public class TituloapagarController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public TituloapagarController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index(TituloapagarIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<TituloapagarGridViewModel>>(_uoW.Titulosapagar.ObterTodos().ToList().OrderBy(x => x.id_fintitpg));
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<TituloapagarGridViewModel>>(_uoW.Titulosapagar.ObterTodos().Where(x => x.id_fintitpg.Equals(model.id_fintitpg)).ToList().OrderBy(x => x.id_fintitpg));
            return View(model);

        }

        public ActionResult Create(string codigo, string data)
        {
            var model = new TituloapagarCreateViewModel
            {
                parcelas = 1,
                Id_stqnoten = Convert.ToInt32(codigo),
                num_titulo = codigo,
                dt_emissao = Convert.ToDateTime(data),
                dt_vencimento = Convert.ToDateTime(data)
            };

            /*model.DropdownProduto = _produtoApp.GetAll()
                 .OrderBy(x => x.desc_produto)
                 .Select(x => new SelectListItem { Text = x.desc_produto, Value = x.Id_stqcdprd.ToString() })
                 .ToList();
            */
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TituloapagarCreateViewModel model)
        {
            ModelState.Clear();

            if (!TryValidateModel(model))
            {
                var validationErrors = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());

                return Json(validationErrors);
            }

            if (!ValidarSomatorioTitulos1(model))
            {
                model.resultado = false;
                model.alerta = "alert alert-danger";
                model.Icone = "glyphicon glyphicon-remove";
                model.Mensagem = "A soma dos títulos não pode ser maior do que soma dos valores dos serviços.";
                return Json(model);
            }


            var data_base = Convert.ToDateTime(model.dt_vencimento.Value);

            if (model.parcelas > 0)
            {
                for (var i = 0; i < model.parcelas; i++)
                {
                    int cont = i + 1;

                    string num_titulo = model.Id_stqnoten + "/" + cont;
                    var aux_data = Convert.ToDateTime(data_base.AddMonths(i));
                    model.num_titulo = num_titulo;
                    model.dt_vencimento = aux_data;
                    model.Status = "0";
                    _uoW.Titulosapagar.Salvar(Mapper.Map<Tituloapagar>(model));
                    _uoW.Complete();
                    num_titulo = "";

                }
            }
            else
            {
                model.Status = "0";
               _uoW.Titulosapagar.Salvar(Mapper.Map<Tituloapagar>(model));
                _uoW.Complete();

            }
            model.alerta = "alert alert-success";

            model.Icone = "glyphicon glyphicon-ok";
            model.resultado = true;
            //model.Mensagem = "A soma dos títulos não pode ser maior do que soma dos valores dos serviços.";
            return Json(model);
        }



        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<TituloapagarEditViewModel>(_uoW.Titulosapagar.ObterPorId(codigo));

            //model.id_stqporcamento = Convert.ToInt16(codigo);

            /*model.DropdownProduto = _produtoApp.GetAll()
                 .OrderBy(x => x.desc_produto)
                 .Select(x => new SelectListItem { Text = x.desc_produto, Value = x.Id_stqcdprd.ToString() })
                 .ToList();*/
            if (model == null)
                return HttpNotFound();

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TituloapagarEditViewModel model)
        {

            ModelState.Clear();


            if (!TryValidateModel(model))
            {
                var validationErrors = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());

                return Json(validationErrors);
            }

            if (!ValidarSomatorioTitulos(model))
            {
                model.resultado = false;
                model.alerta = "alert alert-danger";
                model.Icone = "glyphicon glyphicon-remove";
                model.Mensagem = "A soma dos títulos não pode ser maior do que soma dos valores dos serviços.";
                return Json(model);
            }
            model.Status = "0";
            _uoW.Titulosapagar.Atualizar(Mapper.Map<Tituloapagar>(model));
            _uoW.Complete();
            model.resultado = true;
            return Json(model);
        }



        public ActionResult Quitacao(int codigo)
        {
            var model = Mapper.Map<TituloapagarEditViewModel>(_uoW.Titulosapagar.ObterPorId(codigo));

            model.dt_pagamento = DateTime.Now;
            model.Dropdowntpagamento =
                _uoW.TipoPagamentos.ObterTodos()
                    .OrderBy(x => x.Descricao)
                    .Select(x => new SelectListItem { Text = x.Descricao, Value = x.Codigo.ToString() })
                    .ToList();

            model.Dropdownbanco =
               _uoW.Bancos.ObterTodos()
                   .OrderBy(x => x.desc_banco)
                   .Select(x => new SelectListItem { Text = x.desc_banco, Value = x.id_Fincdbanco.ToString() })
                   .ToList();


            if (model == null)
                return HttpNotFound();

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Quitacao(TituloapagarEditViewModel model)
        {

            ModelState.Clear();


            if (!TryValidateModel(model))
            {
                var validationErrors = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());

                return Json(validationErrors);
            }
            model.Status = "0";
            _uoW.Titulosapagar.Atualizar(Mapper.Map<Tituloapagar>(model));
            _uoW.Complete();

            return Json(true);
        }

        [HttpPost]
        public ActionResult Abrir(int codigo)
        {

            var model = _uoW.Titulosapagar.ObterPorId(codigo);
            ModelState.Clear();


            if (!TryValidateModel(model))
            {
                var validationErrors = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());

                return Json(validationErrors);
            }
            try
            {
                model.Valor_pago = null;
                model.dt_pagamento = Convert.ToDateTime("01/01/1000 00:00:00");
                model.agencia = "";
                //model.banco = 0;
                model.doc_pagamento = "";
                model.id_fintipopagamento = null;
                model.num_conta = "";
                model.Status = "0";
                _uoW.Titulosapagar.Atualizar(model);
                _uoW.Complete();
            }
            catch (Exception ex)
            {

                throw;
            }


            return Json(true);
        }

        private bool ValidarSomatorioTitulos(TituloapagarEditViewModel model)
        {
            decimal valorTotalOrcamento = 0;
            decimal valor_final = 0;

            _uoW.NotaEntradaItems.ObterTodos().Where(x => x.id_stqnoten == model.Id_stqnoten).ForEach(x =>
            {
                valorTotalOrcamento = Convert.ToDecimal((x.valor_total));
                
                valor_final = valor_final + valorTotalOrcamento;
            });

            var titulos =_uoW.Titulosapagar.ObterTodos().Where(x => x.Id_stqnoten == model.Id_stqnoten && x.id_fintitpg != model.id_fintitpg).ToList();
            decimal valorTotalTitulos = 0;

            foreach (var titulo in titulos)
            {
                valorTotalTitulos += titulo.valor.Value;
            }


            valorTotalTitulos += model.valor.Value;



            if (valorTotalTitulos > valor_final)
            {
                return false;
            }
            return true;
        }

        private bool ValidarSomatorioTitulos1(TituloapagarCreateViewModel model)
        {
            decimal valorTotalOrcamento = 0;
            decimal valor_final = 0;

            _uoW.NotaEntradaItems.ObterTodos().Where(x => x.id_stqnoten == model.Id_stqnoten).ForEach(x =>
            {
                valorTotalOrcamento = Convert.ToDecimal((x.valor_total)) ;
                //valorTotalOrcamento = valorTotalOrcamento - Convert.ToDecimal(valorTotalOrcamento * (x.descontoperc / 100));
                valor_final = valor_final + valorTotalOrcamento;
            });

            var titulos =_uoW.Titulosapagar.ObterTodos().Where(x => x.Id_stqnoten == model.Id_stqnoten && x.id_fintitpg != model.id_fintitpg).ToList();
            decimal valorTotalTitulos = 0;

            foreach (var titulo in titulos)
            {
                valorTotalTitulos += titulo.valor.Value;
            }

            for (int i = 0; i < model.parcelas; i++)
            {
                valorTotalTitulos += model.valor.Value;
            }

            if (valorTotalTitulos > valor_final)
            {
                return false;
            }
            return true;
        }

        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.Titulosapagar.ObterPorId(codigo);

            if (model == null)
            {
                return Json(false);
            }

            _uoW.Titulosapagar.RemoverPorId(codigo);
            _uoW.Complete();
            return Json(true);
        }

        public bool VerificarFiltroVazio(TituloapagarIndexViewModel model)
        {
            model = model ?? new TituloapagarIndexViewModel();

            var ehVazio = model.id_fintitpg.Equals(0);

            return ehVazio;

        }


    }
}
