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
    public class TituloController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public TituloController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index(TituloIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<TituloGridViewModel>>(_uoW.Titulos.ObterTodos().ToList().OrderBy(x => x.id_fintitrc));
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<TituloGridViewModel>>(_uoW.Titulos.ObterTodos().Where(x => x.id_fintitrc.Equals(model.id_fintitrc)).ToList().OrderBy(x => x.id_fintitrc));
            return View(model);

        }

        public ActionResult Create(string codigo, string data)
        {
            var model = new TituloCreateViewModel
            {
                parcelas = 1,
                id_stqcporcamento = Convert.ToInt32(codigo),
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
        public ActionResult Create(TituloCreateViewModel model)
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

                    string num_titulo = model.id_stqcporcamento + "/" + cont;
                    var aux_data = Convert.ToDateTime(data_base.AddMonths(i));
                    model.num_titulo = num_titulo;
                    model.dt_vencimento = aux_data;
                    model.status = "0";
                    _uoW.Titulos.Salvar(Mapper.Map<Titulo>(model));
                    _uoW.Complete();
                    num_titulo = "";

                }
            }
            else
            {
                model.status = "0";
               _uoW.Titulos.Salvar(Mapper.Map<Titulo>(model));
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
            var model = Mapper.Map<TituloEditViewModel>(_uoW.Titulos.ObterPorId(codigo));

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
        public ActionResult Edit(TituloEditViewModel model)
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
            model.status = "0";
            _uoW.Titulos.Atualizar(Mapper.Map<Titulo>(model));
            _uoW.Complete();
            model.alerta = "alert alert-success";

            model.Icone = "glyphicon glyphicon-ok";
            model.resultado = true;
            //model.Mensagem = "A soma dos títulos não pode ser maior do que soma dos valores dos serviços.";
            return Json(model);
        }



        public ActionResult Quitacao(int codigo)
        {
            var model = Mapper.Map<TituloEditViewModel>(_uoW.Titulos.ObterPorId(codigo));

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
        public ActionResult Quitacao(TituloEditViewModel model)
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
            model.status = "0";
            _uoW.Titulos.Atualizar(Mapper.Map<Titulo>(model));
            _uoW.Complete();
            return Json(true);
        }

        [HttpPost]
        public ActionResult Abrir(int codigo)
        {

            var model =_uoW.Titulos.ObterPorId(codigo);
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
                model.status = "0";
                _uoW.Titulos.Atualizar(model);
                _uoW.Complete();
            }
            catch (Exception ex)
            {

                throw;
            }


            return Json(true);
        }

        private bool ValidarSomatorioTitulos(TituloEditViewModel model)
        {
            decimal valorTotalOrcamento = 0;
            decimal valor_final = 0;

            _uoW.OrcamentoItens.ObterTodos().Where(x => x.id_stqporcamento == model.id_stqcporcamento).ForEach(x =>
            {
                valorTotalOrcamento = Convert.ToDecimal((x.Vl_unitario * x.qtd) - x.desconto);
                valorTotalOrcamento = valorTotalOrcamento - Convert.ToDecimal(valorTotalOrcamento * (x.descontoperc / 100));
                valor_final = valor_final + valorTotalOrcamento;
            });

            var titulos =_uoW.Titulos.ObterTodos().Where(x => x.id_stqcporcamento == model.id_stqcporcamento && x.id_fintitrc != model.id_fintitrc).ToList();
            decimal valorTotalTitulos = 0;

            foreach (var titulo in titulos)
            {
                valorTotalTitulos += titulo.Valor.Value;
            }


            valorTotalTitulos += model.Valor.Value;



            if (valorTotalTitulos > valor_final)
            {
                return false;
            }
            return true;
        }

        private bool ValidarSomatorioTitulos1(TituloCreateViewModel model)
        {
            decimal valorTotalOrcamento = 0;
            decimal valor_final = 0;

            var itens = _uoW.OrcamentoItens.ObterTodos().Where(x => x.id_stqporcamento == model.id_stqcporcamento && x.status.Equals("0"));
            foreach (var item in itens)
            {
               
                valorTotalOrcamento = Convert.ToDecimal((item.Vl_unitario * item.qtd) - item.desconto);
                valorTotalOrcamento = valorTotalOrcamento - Convert.ToDecimal(valorTotalOrcamento * (item.descontoperc / 100));
                valor_final = valor_final + valorTotalOrcamento;
            }
           

            var titulos =_uoW.Titulos.ObterTodos().Where(x => x.id_stqcporcamento == model.id_stqcporcamento && x.id_fintitrc != model.id_fintitrc).ToList();
            decimal valorTotalTitulos = 0;

            foreach (var titulo in titulos)
            {
                valorTotalTitulos += titulo.Valor.Value;
            }

            for (int i = 0; i < model.parcelas; i++)
            {
                valorTotalTitulos += model.Valor.Value;
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
            var model = _uoW.Titulos.ObterPorId(codigo);

            if (model == null)
            {
                return Json(false);
            }

            _uoW.Titulos.RemoverPorId(codigo);
            _uoW.Complete();
            return Json(true);
        }

        public bool VerificarFiltroVazio(TituloIndexViewModel model)
        {
            model = model ?? new TituloIndexViewModel();

            var ehVazio = model.id_fintitrc.Equals(0);

            return ehVazio;

        }


    }
}
