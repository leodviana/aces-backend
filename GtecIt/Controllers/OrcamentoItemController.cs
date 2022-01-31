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
    public class OrcamentoItemController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public OrcamentoItemController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }


        public ActionResult Index(OrcamentoItemIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<OrcamentoItemGridViewModel>>(_uoW.OrcamentoItens.ObterTodos().ToList().OrderBy(x => x.id_stqitorcamento));
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<OrcamentoItemGridViewModel>>(_uoW.OrcamentoItens.ObterTodos().Where(x => x.id_stqitorcamento.Equals(model.id_stqitorcamento)).ToList().OrderBy(x => x.id_stqitorcamento));
            return View(model);

        }

        public ActionResult Create(string codigo, string codigo2)
        {
            var model = new OrcamentoItemCreateViewModel();

            model.id_stqporcamento = Convert.ToInt16(codigo);

            model.DropdownProduto = _uoW.Produtos.ObterTodos()
                 .OrderBy(x => x.desc_produto)
                 .Select(x => new SelectListItem { Text = x.desc_produto, Value = x.Id_stqcdprd.ToString() })
                 .ToList();
            model.id_grlconvenio = Convert.ToInt32(codigo2);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrcamentoItemCreateViewModel model)
        {
            if (model.desconto == null)
            {
                model.desconto = 0;
            }
            if (model.descontoperc == null)
            {
                model.descontoperc = 0;
            }
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

            var qdt_produtos = _uoW.OrcamentoItens.ObterTodos().Where(x=>x.id_stqporcamento== model.id_stqporcamento).ToList();

            /*if (qdt_produtos.Count>0)
            {
                var mensagem = new List<String>();

                mensagem.Add("Nâo é possível incluir mais serviços!.Quantidade Excedida!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }*/
            model.status = "0";
            _uoW.OrcamentoItens.Salvar(Mapper.Map<OrcamentoItem>(model));
            //atualizar a renovacao 
            var orcamento = _uoW.Orcamentos.ObterPorId(Convert.ToInt32(model.id_stqporcamento));
            var qtd_dias = _uoW.PrecosPlano.ObterTodos().Where(x => x.id_stqcdprd == model.id_stqcdprd && x.idGrlplanos == orcamento.id_grlconvenio).FirstOrDefault();
            if (qtd_dias != null)
            {
                orcamento.dt_renovacao = orcamento.Dt_orcamento.Value.AddDays(qtd_dias.qtd_dias_plano);

            }
            _uoW.Complete();
            return Json(true);
        }



        public ActionResult Edit(int codigo, int codigo2)
        {
            var model = Mapper.Map<OrcamentoItemEditViewModel>(_uoW.OrcamentoItens.ObterPorId(codigo));
            //model.id_stqporcamento = Convert.ToInt16(codigo);

            model.DropdownProduto = _uoW.Produtos.ObterTodos()
                 .OrderBy(x => x.desc_produto)
                 .Select(x => new SelectListItem { Text = x.desc_produto, Value = x.Id_stqcdprd.ToString() })
                 .ToList();
            if (model == null)
                return HttpNotFound();
            model.id_grlconvenio = codigo2;
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrcamentoItemEditViewModel model)
        {

            if (model.Vl_unitario != null)
            {
                Decimal aux_vl = Convert.ToDecimal(model.Vl_unitario);


            }
            if (model.desconto == null)
            {
                model.desconto = 0;
            }
            if (model.descontoperc == null)
            {
                model.descontoperc = 0;
            }
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
            model.status = "0";
            _uoW.OrcamentoItens.Atualizar(Mapper.Map<OrcamentoItem>(model));

            //atualizar a renovacao 
            var orcamento = _uoW.Orcamentos.ObterPorId(Convert.ToInt32(model.id_stqporcamento));
            var qtd_dias = _uoW.PrecosPlano.ObterTodos().Where(x => x.id_stqcdprd == model.id_stqcdprd && x.idGrlplanos == orcamento.id_grlconvenio).FirstOrDefault();
            if (qtd_dias != null)
            {
                orcamento.dt_renovacao = orcamento.Dt_orcamento.Value.AddDays(qtd_dias.qtd_dias_plano);

            }
            _uoW.Complete();

            return Json(true);
        }


        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.OrcamentoItens.ObterPorId(codigo);

            if (model == null)
            {

                return Json(false);
            }
            try
            {

                //_orcamentoItemApp.Remove(model);
                _uoW.OrcamentoItens.RemoverPorId(codigo);
                _uoW.Complete();
            }
            catch (Exception)
            {

                throw;
            }


            return Json(true);
        }

        public bool VerificarFiltroVazio(OrcamentoItemIndexViewModel model)
        {
            model = model ?? new OrcamentoItemIndexViewModel();

            var ehVazio = model.id_stqitorcamento.Equals(0);

            return ehVazio;
        }
    }

}
