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

            //migrar aulas 

            return View(model);

        }

        public ActionResult Create(string codigo, string codigo2)
        {
            var model = new AulaCreateViewModel();
            var orcamento = _uoW.Orcamentos.ObterPorId(Convert.ToInt32(codigo));

            model.Inicio_contrato = Convert.ToDateTime(orcamento.Dt_orcamento);
            model.id_Stqcporcamento = orcamento.id_Stqcporcamento;
            model.nome_cliente = orcamento.grlcliente.grlbasic.nome;
            int? codigo_prd = 0;
            int _plano = Convert.ToInt32(codigo2);
            foreach (var item in orcamento.itemorcamentos)
            {
                codigo_prd = item.id_stqcdprd;
            }
            var plano = _uoW.PrecosPlano.ObterTodos().Where(x => x.id_stqcdprd == codigo_prd && x.idGrlplanos == _plano).FirstOrDefault();
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
                novo.nome_dentista = item.dentistas.Idgrlbasic.nome;
                novo.Dia = item.Dia;
                novo.horario = item.horario;
                model.horarios.Add(novo);
            }
            /* var aulas = _uoW.Aulas.ObterTodos().Where(x=>x.id_Stqcporcamento== model.id_Stqcporcamento).ToList();
             foreach (var item in aulas)
             {
                 var aula_nova = new Events();
                 aula_nova.EventID = item.idGercdaulas;
                 aula_nova.Subject = "Contrato" + item.id_Stqcporcamento;

                 aula_nova.Start = item.inicio.Value;
                 aula_nova.End = item.final.Value;
                 _uoW.Events.Salvar(aula_nova);
             }
             _uoW.Complete();*/
            return View(model);
        }


        public ActionResult visualizar(string codigo, string codigo2)
        {
            var model = new AulaCreateViewModel();
            var orcamento = _uoW.Orcamentos.ObterPorId(Convert.ToInt32(codigo));

            model.Inicio_contrato = Convert.ToDateTime(orcamento.Dt_orcamento);
            model.id_Stqcporcamento = orcamento.id_Stqcporcamento;
            model.nome_cliente = orcamento.grlcliente.grlbasic.nome;
            int? codigo_prd = 0;
            int _plano = Convert.ToInt32(codigo2);
            foreach (var item in orcamento.itemorcamentos)
            {
                codigo_prd = item.id_stqcdprd;
            }
            var plano = _uoW.PrecosPlano.ObterTodos().Where(x => x.id_stqcdprd == codigo_prd && x.idGrlplanos == _plano).FirstOrDefault();
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
                novo.nome_dentista = item.dentistas.Idgrlbasic.nome;
                novo.Dia = item.Dia;
                novo.horario = item.horario;
                model.horarios.Add(novo);
            }
            /* var aulas = _uoW.Aulas.ObterTodos().Where(x=>x.id_Stqcporcamento== model.id_Stqcporcamento).ToList();
             foreach (var item in aulas)
             {
                 var aula_nova = new Events();
                 aula_nova.EventID = item.idGercdaulas;
                 aula_nova.Subject = "Contrato" + item.id_Stqcporcamento;

                 aula_nova.Start = item.inicio.Value;
                 aula_nova.End = item.final.Value;
                 _uoW.Events.Salvar(aula_nova);
             }
             _uoW.Complete();*/
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



                var resposta = new
                {

                    Sucesso = false,
                    msg = validationErrors
                };

                return Json(resposta);

            }
            //fazer a rotina para apgar as aulas que serao dadas 
            var aulas_naorealizadas = _uoW.Aulas.ObterTodos().Where(x => x.id_Stqcporcamento == model.id_Stqcporcamento).ToList();

            foreach (var item in aulas_naorealizadas)
            {
                if (item.inicio >= DateTime.Now)
                    _uoW.Aulas.RemoverPorId(item.idGercdaulas);
            }


            var qtd_aulascontrato = _uoW.Aulas.ObterTodos().Where(x => x.id_Stqcporcamento == model.id_Stqcporcamento && x.status != "2").ToList();
            if (qtd_aulascontrato.Count() >= model.qtd_aulas)
            {
                var mensagem = new List<String>();

                mensagem.Add("Quantidade excedida de aulas!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }




            var lista_horarios = _uoW.horarioprofessor.ObterTodos().Where(x => x.id_Stqcporcamento == model.id_Stqcporcamento).ToList();
            List<string> dias_selecionados = new List<string>();
            List<string> nome_dias = new List<string>();


            DateTime inicio_contrato; 
                
                
            DateTime fim_contrato = model.Vencimento_contrato;
            int i = 0;
            DateTime data_corrente;
            if (qtd_aulascontrato.Count() > 0)
            {
                inicio_contrato = DateTime.Now;
                data_corrente = DateTime.Now.AddDays(i);
            }
               
            else
            {
                inicio_contrato= model.Inicio_contrato;
                data_corrente = inicio_contrato.AddDays(i);
            }
             
            int cont_aulas = 1;
            while (data_corrente <= fim_contrato)
            {
                if (cont_aulas <= (model.qtd_aulas-qtd_aulascontrato.Count))
                {
                    //converter o dia da semana para portugeus 
                    var dia = convertePortugues(data_corrente.DayOfWeek.ToString());
                    foreach (var item in lista_horarios)
                    {
                        if (dia == item.Dia)
                        {
                            grava_horario(model, data_corrente.ToString(), item.horario, item.Dia);
                            cont_aulas = cont_aulas + 1;
                        }

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


        private bool grava_horario(AulaCreateViewModel model, string data_corrente, string horario, string dia)
        {
            try
            {
                //string dia_aux = dia.Remove(10, 8);
                string aula_corrente_inicio = data_corrente.Substring(0, 10) + " " + horario;
                // string aula_corrente_fim = dia.Remove(10, 9) + " " + model.fim.Remove(5, 3);
                var aula_nova = new Aulas();

                aula_nova.Subject = model.nome_cliente + " Contrato " + model.id_Stqcporcamento;
                aula_nova.inicio = Convert.ToDateTime(aula_corrente_inicio);
                aula_nova.final = aula_nova.inicio.Value.AddMinutes(30);
                aula_nova.id_Stqcporcamento = model.id_Stqcporcamento;
                aula_nova.id_grldentista = model.id_grldentista;
                aula_nova.dia_semana = dia;
                aula_nova.status = "1";
                _uoW.Aulas.Salvar(Mapper.Map<Aulas>(aula_nova));
                _uoW.Complete();
            }
            catch (Exception ex)
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
            var model = _uoW.Aulas.ObterTodos().Where(x => x.id_Stqcporcamento == codigo).ToList();

            if (model == null)
            {

                return Json(false);
            }
            try
            {

                foreach (var item in model)
                {
                    if (item.inicio >= DateTime.Now)
                        _uoW.Aulas.RemoverPorId(item.idGercdaulas);
                }

                // _uoW.Complete();
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
