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
    public class AulaLogController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public AulaLogController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }


        public ActionResult Index(AulaLogIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;
            else
            {

                model.inicio = DateTime.Now;
                model.Fim = DateTime.Now;
            }

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

               /* model.Grid = Mapper.Map<List<AulaLogGridViewModel>>(_uoW.AulasLog.ObterTodos().ToList());
                model.ConsultaTodos = true;
                return View(model);*/
            }

            model.ConsultaTodos = false;

            //model.Grid = Mapper.Map<List<AulaLogGridViewModel>>(_uoW.AulasLog.ObterTodos().Where(x => x.idGercdAulasLog.Equals(model.idGercdAulasLog)).ToList().OrderBy(x => x.idGercdAulasLog));
            var teste = _uoW.AulasLog.ObterAulas(model.idGercdAulasLog, model.inicio, model.Fim,Convert.ToInt32( model.id_Stqcporcamento_inicio)).ToList();

            model.Grid = Mapper.Map<List<AulaLogGridViewModel>>( _uoW.AulasLog.ObterAulas(model.idGercdAulasLog, model.inicio, model.Fim,Convert.ToInt32(model.id_Stqcporcamento_inicio)).ToList());
            model.ConsultaTodos = true;
            //migrar aulas 

            return View(model);

        }

      


      


        
        private string convertePortugues(string dia)
        {
            var dia_portugues = "";
            if (dia == "Monday")
            {
                dia_portugues = "Segunda";
            }
            if (dia == "Tuesday")
            {
                dia_portugues = "Terça";
            }
            if (dia == "Wednesday")
            {
                dia_portugues = "Quarta";
            }
            if (dia == "Thursday")
            {
                dia_portugues = "Quinta";
            }
            if (dia == "Friday")
            {
                dia_portugues = "Sexta";
            }
            if (dia == "Saturday")
            {
                dia_portugues = "Sábado";

            }
            return dia_portugues;
        }

        public ActionResult ObterQtd(string codigo, string codigo2)
        {
            var model = new AulaCreateViewModel();
            var orcamento = _uoW.Orcamentos.ObterPorId(Convert.ToInt32(codigo));

            model.Inicio_contrato = Convert.ToDateTime(orcamento.Dt_orcamento);
            model.id_Stqcporcamento = orcamento.id_Stqcporcamento;
            int? codigo_prd = 0;
            foreach (var item in orcamento.itemorcamentos)
            {
                codigo_prd = item.id_stqcdprd;
            }
            var plano = _uoW.PrecosPlano.ObterTodos().Where(x => x.id_stqcdprd == codigo_prd).FirstOrDefault();
            if (plano != null)
            {
                model.qtd_aulas = plano.qtd_aulas;

                model.Vencimento_contrato = model.Inicio_contrato.AddDays(plano.qtd_dias_plano);
            }
            var resposta = new
            {

                Sucesso = true,
                quantidade = model.qtd_aulas
            };
            return Json(resposta, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<AulaLogEditViewModel>(_uoW.AulasLog.ObterPorId(codigo));
            //model.id_stqporcamento = Convert.ToInt16(codigo);

            /* model.DropdownProduto = _uoW.Produtos.ObterTodos()
                  .OrderBy(x => x.desc_produto)
                  .Select(x => new SelectListItem { Text = x.desc_produto, Value = x.Id_stqcdprd.ToString() })
                  .ToList();
             if (model == null)
                 return HttpNotFound();
                 */
            return View(model);

        }
       
        public bool VerificarFiltroVazio(AulaLogIndexViewModel model)
        {
            model = model ?? new AulaLogIndexViewModel();

            var ehVazio = model.idGercdAulasLog.Equals(0);

            return ehVazio;
        }


        
    }

}
