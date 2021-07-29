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
    public class EventsController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public EventsController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }
        public ActionResult Index(EventCreateViewModel model)
        {
            model.DropdownProfessor =
               _uoW.Dentistas.ObterTodos().Where(x => x.Ativo.Equals("S"))
                   .OrderBy(x => x.Idgrlbasic.nome)
                   .Select(x => new SelectListItem { Text = x.Idgrlbasic.nome, Value = x.id_grldentista.ToString() })
                   .ToList();
            model.DropdownAluno =
              _uoW.Orcamentos.ObterTodos().Where(x => x.status.Equals("0"))
                  .OrderBy(x => x.grlcliente.grlbasic.nome)
                  .Select(x => new SelectListItem { Text = x.grlcliente.grlbasic.nome, Value = x.id_Stqcporcamento.ToString() })
                  .ToList();
            return View(model);
        }

        public JsonResult GetEvents(int? id_professor, int? id_contrato)
        {
            var events = new List<Events>();

            if ( id_professor == null)
            {
                id_professor = 0;
            }
            if( id_contrato == null)
            {
                id_contrato = 0;
            }
            var aulas = _uoW.Aulas.ObterTodos().AsQueryable();
            if(id_professor!=0 )
            {
                aulas = aulas.Where(x => x.id_grldentista==id_professor);
            }
            if (id_contrato != 0 )
            {
                aulas = aulas.Where(x => x.id_Stqcporcamento == id_contrato);
            }
            foreach (var item in aulas.ToList())

               {
                   var evento = new Events();
                   evento.EventID = item.idGercdaulas;
                   evento.Start = item.inicio.Value;
                   evento.End = item.final.Value;
                   evento.Description = String.IsNullOrEmpty(item.Description) ? "" : item.Description; //;
                   evento.Subject = item.Subject;//String.IsNullOrEmpty(item.Subject) ? item.Subject : item.Subject.Substring(0, 30); //; String.IsNullOrEmpty(item.Subject) ?  "" : item.Subject ;
                   evento.ThemeColor = item.Theme_color;
                   evento.contrato = item.id_Stqcporcamento;
                   evento.professor = item.id_grldentista;
                   events.Add(evento);
               }
               
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
           
            
           
        }
        public ActionResult getContratos()
        {

            return Json(_uoW.Orcamentos.ObterTodos().Where(x => x.status == "0").OrderBy(x=>x.grlcliente.grlbasic.nome).Select(x => new
            {
                DepartmentID = x.id_Stqcporcamento,
                DepartmentName = x.grlcliente.grlbasic.nome
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getProfessores()
        {

            return Json(_uoW.Dentistas.ObterTodos().Where(x => x.Ativo == "S").OrderBy(x=>x.Idgrlbasic.nome).Select(x => new
            {
                ProfessorID = x.id_grldentista,
                ProfessorName = x.Idgrlbasic.nome
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveEvent(Events e)
        {

            var status = false;

            if (e.Start == null)
            {
                var mensagem = new List<String>();

                mensagem.Add("Informe o horario inicial da aula!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }
            if (e.contrato <= 0)
            {
                var mensagem = new List<String>();

                mensagem.Add("Selecione um Aluno!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }

            if (e.Start == DateTime.MinValue)
            {
                var mensagem = new List<String>();

                mensagem.Add("Informe o inicio da aula!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }
            if (e.End == DateTime.MinValue)
            {
                var mensagem = new List<String>();

                mensagem.Add("Informe o Termino da aula!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }
            if (e.Start >= e.End)
            {
                var mensagem = new List<String>();

                mensagem.Add("Periodo errado!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }
            if (e.EventID > 0)
            {
                //Update the event
                var v = _uoW.Aulas.ObterTodos().Where(a => a.idGercdaulas == e.EventID).FirstOrDefault();
                if (v != null)
                {
                    v.Subject = e.Subject;
                    v.inicio = e.Start;
                    v.final = e.End;
                    v.Description = e.Description;
                    v.IsFullDay = "1";
                    v.Theme_color = e.ThemeColor;
                    v.id_Stqcporcamento = e.contrato;
                    v.dia_semana = convertePortugues(e.Start.DayOfWeek.ToString());
                    v.id_grldentista = e.professor;
                    v.status = "1";
                    _uoW.Aulas.Atualizar(v);
                }
            }
            else
            {
                var teste_horario  = _uoW.Aulas.ObterTodos().Where(a => a.id_grldentista==e.professor && a.id_Stqcporcamento==e.contrato && a.inicio==e.Start && a.final==e.End).FirstOrDefault();
                if (teste_horario!=null)
                {
                    var mensagem = new List<String>();

                    mensagem.Add("Horario já está ocupado!");
                    var resposta = new
                    {

                        Sucesso = false,
                        msg = mensagem
                    };

                    return Json(resposta);
                }
                var novo = new Aulas();
                novo.Subject = e.Subject;
                novo.inicio = e.Start;
                novo.final = e.End;
                novo.Description = e.Description;
                novo.IsFullDay = "1";
                novo.Theme_color = e.ThemeColor;
                novo.id_Stqcporcamento = e.contrato;
                novo.status = "1";
                novo.id_grldentista = e.professor;
                novo.dia_semana = convertePortugues(e.Start.DayOfWeek.ToString());
                _uoW.Aulas.Salvar(novo);
                //  dc.Events.Add(e);
            }

            _uoW.Complete();
            status = true;
            var response = new
            {

                Sucesso = true,
                msg = ""
            };
            return Json(response);
            // return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;

            var v = _uoW.Aulas.ObterTodos().Where(a => a.idGercdaulas == eventID).FirstOrDefault();
            if (v != null)
            {
                _uoW.Aulas.Remover(v);
                _uoW.Complete();
                status = true;
            }

            return new JsonResult { Data = new { status = status } };
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
    }


}


