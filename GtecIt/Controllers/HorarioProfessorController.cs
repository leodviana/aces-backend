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
    public class HorarioProfessorController : Controller
    {
        private readonly IUnitOfWork _uoW;
        List<string> semana;
        public HorarioProfessorController(IUnitOfWork uoW)
        {
            _uoW = uoW;
            
        }


        public ActionResult Index(HorarioProfessorIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);

                model.Grid = Mapper.Map<List<HorarioProfessorGridViewModel>>(_uoW.horarioprofessor.ObterTodos().ToList().OrderBy(x => x.idgercdhorarioProf));
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            model.Grid = Mapper.Map<List<HorarioProfessorGridViewModel>>(_uoW.horarioprofessor.ObterTodos().Where(x => x.idgercdhorarioProf.Equals(model.idgercdhorarioProf)).ToList().OrderBy(x => x.idgercdhorarioProf));
            return View(model);

        }
        public ActionResult inicializa()
        {
            List<string> semana = new List<string>();

            semana.Add("Segunda");
            semana.Add("Terça");
            semana.Add("Quarta");
            semana.Add("Quinta");
            semana.Add("Sexta");
            semana.Add("Sábado");

            try
            {
                var dentistas = _uoW.Dentistas.ObterTodos().ToList();
                List<string> horas = new List<string>();
                DateTime inicio = new DateTime(2019, 10, 10, 05, 30, 00);
                foreach (var dent in dentistas)
                {
                    DateTime aux_hora = inicio;
                    for (int i = 0; i < 34; i++)
                    {

                        foreach (var dia in semana)
                        {
                            var novo_horario = new HorarioProfessor();
                            if (dia.Equals("Segunda"))
                            {
                                
                                novo_horario.Dia = "Segunda";

                            }
                            else if (dia.Equals("Terça"))
                            {

                              
                                novo_horario.Dia = "Terça";
                            }
                            else if (dia.Equals("Quarta"))
                            {
                               
                                novo_horario.Dia = "Quarta";

                            }
                            else if (dia.Equals("Quinta"))
                            {
                               novo_horario.Dia = "Quinta";
                            }
                            else if (dia.Equals("Sexta"))
                            {
                               
                                novo_horario.Dia = "Sexta";

                            }
                            else if (dia.Equals("Sábado"))
                            {
                               
                                novo_horario.Dia = "Sábado";
                            }
                            novo_horario.id_grldentista = dent.id_grldentista;
                            novo_horario.horario = aux_hora.ToShortTimeString();
                            novo_horario.status = "1";
                            _uoW.horarioprofessor.Salvar(novo_horario);
                            _uoW.Complete();


                        }
                        aux_hora = aux_hora.AddMinutes(30);

                    }


                }
            }
            catch (Exception ex)
            {

            }




            return View();
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
            // model.qtd_aulas = 
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HorarioProfessorCreateViewModel model)
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

            
            if (model.id_Stqcporcamento != null)
            {
                model.status = "4";
            }
            // var orcamento = Mapper.Map<Orcamento>(model);
            _uoW.horarioprofessor.Atualizar(Mapper.Map<HorarioProfessor>(model));
            if (model.segunda)
            {
                var novo_horario = _uoW.horarioprofessor.ObterTodos().Where(x => x.horario == model.horario && x.Dia == "Segunda" && x.id_grldentista==model.id_grldentista).FirstOrDefault();
                if (novo_horario!=null)
                {
                    novo_horario.id_Stqcporcamento = model.id_Stqcporcamento;
                    novo_horario.status = "4";
                    _uoW.horarioprofessor.Atualizar(novo_horario);
                }
               
               
            }
            if (model.terca)
            {
                var novo_horario = _uoW.horarioprofessor.ObterTodos().Where(x => x.horario == model.horario && x.Dia == "Terça" && x.id_grldentista == model.id_grldentista).FirstOrDefault();
                if (novo_horario != null)
                {
                    novo_horario.id_Stqcporcamento = model.id_Stqcporcamento;
                    novo_horario.status = "4";
                    _uoW.horarioprofessor.Atualizar(novo_horario);
                }


            }
            if (model.quarta)
            {
                var novo_horario = _uoW.horarioprofessor.ObterTodos().Where(x => x.horario == model.horario && x.Dia == "Quarta" && x.id_grldentista == model.id_grldentista).FirstOrDefault();
                if (novo_horario != null)
                {
                    novo_horario.id_Stqcporcamento = model.id_Stqcporcamento;
                    novo_horario.status = "4";
                    _uoW.horarioprofessor.Atualizar(novo_horario);
                }


            }
            if (model.quinta)
            {
                var novo_horario = _uoW.horarioprofessor.ObterTodos().Where(x => x.horario == model.horario && x.Dia == "Quinta" && x.id_grldentista == model.id_grldentista).FirstOrDefault();
                if (novo_horario != null)
                {
                    novo_horario.id_Stqcporcamento = model.id_Stqcporcamento;
                    novo_horario.status = "4";
                    _uoW.horarioprofessor.Atualizar(novo_horario);
                }


            }
            if (model.sexta)
            {
                var novo_horario = _uoW.horarioprofessor.ObterTodos().Where(x => x.horario == model.horario && x.Dia == "Sexta" && x.id_grldentista == model.id_grldentista).FirstOrDefault();
                if (novo_horario != null)
                {
                    novo_horario.id_Stqcporcamento = model.id_Stqcporcamento;
                    novo_horario.status = "4";
                    _uoW.horarioprofessor.Atualizar(novo_horario);
                }


            }
            if (model.sabado)
            {
                var novo_horario = _uoW.horarioprofessor.ObterTodos().Where(x => x.horario == model.horario && x.Dia == "Sábado" && x.id_grldentista == model.id_grldentista).FirstOrDefault();
                if (novo_horario != null)
                {
                    novo_horario.id_Stqcporcamento = model.id_Stqcporcamento;
                    novo_horario.status = "4";
                    _uoW.horarioprofessor.Atualizar(novo_horario);
                }


            }
            _uoW.Complete();
            

            var response = new
            {

                Sucesso = true,
                msg = ""
            };

            return Json(response);
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


            var model = Mapper.Map<HorarioProfessorCreateViewModel>(_uoW.horarioprofessor.ObterPorId(codigo));


            var dentista = _uoW.Dentistas.ObterPorId(model.id_grldentista);
            model.professor = dentista.Idgrlbasic.nome;

            model.DropdownAlunos = _uoW.Orcamentos.ObterTodos()
                .Where(x => x.status == "0")
                .OrderBy(x => x.grlcliente.grlbasic.nome)
                .Select(x => new SelectListItem { Text = x.grlcliente.grlbasic.nome, Value = x.id_Stqcporcamento.ToString() })
                .ToList();


            model.Dropdownstatus = _uoW.entrega.ObterTodos()
              
               .OrderBy(x => x.Desc_entrega)
               .Select(x => new SelectListItem { Text = x.Desc_entrega, Value = x.idEntrega.ToString() })
               .ToList();
           
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
            
            _uoW.Aulas.Atualizar(Mapper.Map<Aulas>(model));
            _uoW.Complete();

            return Json(true);
        }


        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.horarioprofessor.ObterPorId(codigo);

            if (model == null)
            {

                return Json(false);
            }
            try
            {

                //_orcamentoItemApp.Remove(model);
                model.status = "2";
                _uoW.horarioprofessor.Atualizar(model);
                _uoW.Complete();
            }
            catch (Exception)
            {

                throw;
            }


            return Json(true);
        }

        public bool VerificarFiltroVazio(HorarioProfessorIndexViewModel model)
        {
            model = model ?? new HorarioProfessorIndexViewModel();

            var ehVazio = model.idgercdhorarioProf.Equals(0);

            return ehVazio;
        }
    }

}
