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
    public class AulaController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public AulaController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }


        public ActionResult Index(AulaIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<AulaGridViewModel>>(_uoW.Aulas.ObterTodos().ToList().OrderBy(x => x.idGercdaulas));
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<AulaGridViewModel>>(_uoW.Aulas.ObterTodos().Where(x => x.idGercdaulas.Equals(model.idGercdaulas)).ToList().OrderBy(x => x.idGercdaulas));
            return View(model);

        }

        public ActionResult Create(string codigo, string codigo2)
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

            model.inicio = DateTime.Now.ToShortTimeString();
            model.fim = DateTime.Now.ToShortTimeString();

            model.DropdownProduto = _uoW.Dentistas.ObterTodos()
               .Where(x => x.id_grldentista == orcamento.Id_grldentista)
               .OrderBy(x => x.Idgrlbasic.nome)
               .Select(x => new SelectListItem { Text = x.Idgrlbasic.nome, Value = x.id_grldentista.ToString() })
               .ToList();


            model.nome_dentista = _uoW.Dentistas.ObterTodos()
               .Where(x => x.id_grldentista == orcamento.Id_grldentista)
               .OrderBy(x => x.Idgrlbasic.nome).FirstOrDefault().Idgrlbasic.nome;

            model.id_grldentista = Convert.ToInt16(orcamento.Id_grldentista);
            var lista_horarios = _uoW.horarioprofessor.ObterTodos().Where(x => x.id_Stqcporcamento == model.id_Stqcporcamento).ToList();
            foreach (var item in lista_horarios)
            {
                var novo = new HorarioProfessorEditViewModel();
                novo.id_Stqcporcamento = item.id_Stqcporcamento;
                novo.id_Stqcporcamento_dupla = item.id_Stqcporcamento_dupla;
                novo.idgercdhorarioProf = item.idgercdhorarioProf;
                novo.id_grldentista = Convert.ToInt16(item.id_grldentista);
                novo.Dia = item.Dia;
                novo.horario = item.horario;
                model.horarios.Add(novo);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AulaCreateViewModel model)
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
            /* var hora_final = Convert.ToDateTime(model.fim) - Convert.ToDateTime(model.inicio);
             var tempo_total = (hora_final.Hours * 60) + hora_final.Minutes;
             //
             if ((Convert.ToDateTime(model.inicio)>= Convert.ToDateTime(model.fim)))
             {
                 var mensagem = new List<String>();

                 mensagem.Add("O Horario final tem que ser maior que o inicial");
                 var resposta = new
                 {

                     Sucesso = false,
                     msg = mensagem
                 };

                 return Json(resposta);
             }
             if (tempo_total<30)
             {
                 var mensagem = new List<String>();

                 mensagem.Add("A duração da aula é menor que 30 minutos");
                 var resposta = new
                 {

                     Sucesso = false,
                     msg = mensagem
                 };

                 return Json(resposta);
             }
             */

            var lista_horarios = _uoW.horarioprofessor.ObterTodos().Where(x => x.id_Stqcporcamento == model.id_Stqcporcamento).ToList();
            List<string> dias_selecionados = new List<string>();
            List<string> nome_dias = new List<string>();


            DateTime inicio_contrato = model.Inicio_contrato;
            DateTime fim_contrato = model.Vencimento_contrato;
            int i = 1;
            DateTime data_corrente;

            data_corrente = inicio_contrato.AddDays(i);
            while (data_corrente <= fim_contrato)
            {
                //converter o dia da semana para portugeus 
                var dia = convertePortugues(data_corrente.DayOfWeek.ToString());
                foreach (var item in lista_horarios)
                {
                    if (dia == item.Dia)
                    {
                        grava_horario(model,data_corrente.ToString() ,item.horario,item.Dia );
                    }
                    
                }
                i++;
                data_corrente = inicio_contrato.AddDays(i);

            }


            var response = new
            {

                Sucesso = true,
                msg = ""
            };

            return Json(response);
        }


        private bool grava_horario(AulaCreateViewModel model , string data_corrente,string horario,string dia)
        {
            try
            {
                //string dia_aux = dia.Remove(10, 8);
                string aula_corrente_inicio =data_corrente.Substring(0,10) + " " + horario;
               // string aula_corrente_fim = dia.Remove(10, 9) + " " + model.fim.Remove(5, 3);
                var aula_nova = new Aulas();
                aula_nova.inicio = Convert.ToDateTime(aula_corrente_inicio);
                aula_nova.final = aula_nova.inicio.Value.AddMinutes(30);
                aula_nova.id_Stqcporcamento = model.id_Stqcporcamento;
                aula_nova.dia_semana = dia;
                aula_nova.status = "1";
                _uoW.Aulas.Salvar(Mapper.Map<Aulas>(aula_nova));
                _uoW.Complete();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
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
            var model = Mapper.Map<AulaEditViewModel>(_uoW.OrcamentoItens.ObterPorId(codigo));
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AulaEditViewModel model)
        {

            /* if (model.Vl_unitario != null)
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
             }*/
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
            _uoW.Aulas.Atualizar(Mapper.Map<Aulas>(model));
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

        public bool VerificarFiltroVazio(AulaIndexViewModel model)
        {
            model = model ?? new AulaIndexViewModel();

            var ehVazio = model.idGercdaulas.Equals(0);

            return ehVazio;
        }
    }

}
