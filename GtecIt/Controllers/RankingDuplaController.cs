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




namespace GtecIt.Controllers
{
    
    public class RankingDuplaController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public RankingDuplaController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }


        public ActionResult Index(RankingIndexViewModel model, string tipoacao)
        {
            List<RankingGridViewModel> lista_nome = new List<RankingGridViewModel>();
            var ranking_pessoa = _uoW.ranking.ObterTodos().Where(x=>x.categoria.Equals("3")).OrderBy(x => x.pontos).ToList();
            foreach (var item in ranking_pessoa)
            {
                var nome_recuperado = _uoW.Pessoas.ObterPorId(item.id_grlbasico).nome;
                var nome_recuperado_dupla = _uoW.Pessoas.ObterPorId(item.id_grlbasico_dupla).nome;
                lista_nome.Add(new RankingGridViewModel { id_grlbasico = item.id_grlbasico, id_grlbasico_dupla = item.id_grlbasico_dupla, nome = nome_recuperado, nome_dupla = nome_recuperado_dupla, pontos = item.pontos, categoria = item.categoria, id_ranking = item.id_ranking, posicao = item.posicao });
            }
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);
               
                
                //  model.Grid = Mapper.Map<List<RankingGridViewModel>>(_uoW.ranking.ObterTodos().ToList().OrderBy(x => x.pontos));
                model.Grid = Mapper.Map<List<RankingGridViewModel>>(lista_nome.OrderByDescending(x => x.pontos)); //apper.Map<List<RankingGridViewModel>>(_uoW.ranking.ObterTodos().ToList().OrderBy(x => x.pontos));
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;

            //  model.Grid = Mapper.Map<List<RankingGridViewModel>>(_uoW.ranking.ObterTodos().Where(x => x..Contains(model.nome)).ToList().OrderBy(x => x.nome));
            model.Grid = Mapper.Map<List<RankingGridViewModel>>(lista_nome.Where(x => x.nome.Contains(model.nome)).OrderBy(x => x.pontos));


            return View(model);

        }

        public ActionResult Create(RankingCreateViewModel model)
        {

            var contrato = _uoW.Orcamentos.ObterTodos().Where(x => x.status.Equals("0") && x.id_Grltpatendimento == 3)
                                .ToList()
                                .OrderBy(x => x.id_Stqcporcamento) ;
            var lista_kanking = new List<PessoaEditViewModel>();
            foreach (var item in contrato)
            {
                lista_kanking.Add(new PessoaEditViewModel { Id_grlbasico = Convert.ToInt32(item.grlcliente.Id_grlbasico), nome = item.grlcliente.grlbasic.nome });

            }
            var professor = _uoW.Dentistas.ObterTodos().Where(x=>x.Ativo.Equals("S") )
                                .ToList();
            foreach (var item in professor)
            {
                lista_kanking.Add(new PessoaEditViewModel { Id_grlbasico = Convert.ToInt32(item.Idgrlbasic.Id_grlbasico), nome = item.Idgrlbasic.nome });

            }
            model.DropdownProduto = lista_kanking.OrderBy(x => x.nome)
                                    .Select(x => new SelectListItem { Text = x.nome, Value = x.Id_grlbasico.ToString() })
                                    .ToList();

            model.DropdownProduto = lista_kanking.OrderBy(x => x.nome)
                                  .Select(x => new SelectListItem { Text = x.nome, Value = x.Id_grlbasico.ToString() })
                                  .ToList();
            /* model.DropdownIdentifica = _uoW.TipoTelefones.ObterTodos()
                     .OrderBy(x => x.descricao)
                     .Select(x => new SelectListItem { Text = x.descricao, Value = x.id_grlidtel.ToString() })
                     .ToList();

             model.DropdownSexo = _uoW.Sexos.ObterTodos()
                     .OrderBy(x => x.descricao)
                     .Select(x => new SelectListItem { Text = x.descricao, Value = x.Id_gercdsexo.ToString() })
                     .ToList();
             //preencher lista
             // model.EstadoCivil = Mapper.Map<List<EstadoCivilEditViewModel>>(_estadoCivilApp.GetAll().ToList());*/
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RankingCreateViewModel model, FormCollection form)
        {
           

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
            if (model.id_grlbasico<=0)
            {
                var mensagem = new List<String>();

                mensagem.Add("Selecione o jogador!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }
            if (model.id_grlbasico_dupla <= 0)
            {
                var mensagem = new List<String>();

                mensagem.Add("Selecione o segundo jogador!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }
            if (model.pontos<=0)
            {
                var mensagem = new List<String>();

                mensagem.Add("Informe o numero de pontos!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }
            model.categoria = "3";
            var ja_existe = _uoW.ranking.ObterTodos().Where(x => x.id_grlbasico == model.id_grlbasico && x.id_grlbasico_dupla==model.id_grlbasico_dupla &&  x.categoria.Equals("3")).FirstOrDefault(); 
            if (ja_existe==null)
            {
                _uoW.ranking.Salvar(Mapper.Map<Ranking>(model));
            }
           
           else
            {
                ja_existe.pontos = model.pontos;
            }
            _uoW.Complete();
            //indexa a posicao do ranking 
            atualizaRanking();

            var response = new
            {

                Sucesso = true,
                msg = ""
            };
            return Json(response);
        }

        private void atualizaRanking()
        {
            var ranking = _uoW.ranking.ObterTodos().Where(x => x.categoria == "3").OrderByDescending(x => x.pontos).ToList();
            var contador = 1;
            foreach (var item in ranking)
            {
                item.posicao = contador;
                contador = contador + 1;
            }
            _uoW.Complete();
        }
        private void checa_relacionamento(PessoaCreateViewModel model)
        {
            if (model.Id_grlcivil == null)
            {
                model.Id_grlcivil = 3;
            }
            if (model.sexo == 0)
            {
                model.sexo = 5;
            }
            if (model.Id_grlidtel == null)
            {
                model.Id_grlidtel = 15;
            }
            if (model.Id_grlidtel02 == null)
            {
                model.Id_grlidtel02 = 15;
            }
            if (model.Id_grlprofi == null)
            {
                model.Id_grlprofi = 1;
            }
            if (model.Id_grlcidad == null)
            {
                model.Id_grlcidad = 1;
            }
        }


      

        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<RankingCreateViewModel>(_uoW.ranking.ObterPorId(codigo));
            var contrato = _uoW.Orcamentos.ObterTodos().Where(x => x.status.Equals("0") && x.id_Grltpatendimento == 3)
                               .ToList()
                               .OrderBy(x => x.id_Stqcporcamento);
            var lista_kanking = new List<PessoaEditViewModel>();
            foreach (var item in contrato)
            {
                lista_kanking.Add(new PessoaEditViewModel { Id_grlbasico = Convert.ToInt32(item.grlcliente.Id_grlbasico), nome = item.grlcliente.grlbasic.nome });

            }
            var professor = _uoW.Dentistas.ObterTodos().Where(x => x.Ativo.Equals("S"))
                                .ToList();
            foreach (var item in professor)
            {
                lista_kanking.Add(new PessoaEditViewModel { Id_grlbasico = Convert.ToInt32(item.Idgrlbasic.Id_grlbasico), nome = item.Idgrlbasic.nome });

            }
            model.DropdownProduto = lista_kanking.OrderBy(x => x.nome)
                                    .Select(x => new SelectListItem { Text = x.nome, Value = x.Id_grlbasico.ToString() })
                                    .ToList();



            model.DropdownProduto = lista_kanking.OrderBy(x => x.nome)
                                   .Select(x => new SelectListItem { Text = x.nome, Value = x.Id_grlbasico.ToString() })
                                   .ToList();


            if (model == null)
                return HttpNotFound();



            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RankingEditViewModel model)
        {
           

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

            if (model.id_grlbasico <= 0)
            {
                var mensagem = new List<String>();

                mensagem.Add("Selecione o jogador!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }
            if (model.id_grlbasico_dupla <= 0)
            {
                var mensagem = new List<String>();

                mensagem.Add("Selecione o jogador!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }
            if (model.pontos <= 0)
            {
                var mensagem = new List<String>();

                mensagem.Add("Informe o numero de pontos!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }
            model.categoria = "3";
            var ja_existe = _uoW.ranking.ObterTodos().Where(x => x.id_grlbasico == model.id_grlbasico && x.categoria.Equals("3")).FirstOrDefault();
            if (ja_existe == null)
            {
                _uoW.ranking.Salvar(Mapper.Map<Ranking>(model));
            }

            else
            {
                ja_existe.pontos = model.pontos;
            }
            _uoW.Complete();

            var response = new
            {

                Sucesso = true,
                msg = ""
            };
            atualizaRanking();
            return Json(response);

        }


        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            var model = _uoW.ranking.ObterPorId(codigo);

            if (model == null)
            {
                return Json(false);
            }

            _uoW.ranking.RemoverPorId(codigo);
            _uoW.Complete();
            atualizaRanking();
            return Json(true);
        }

        public bool VerificarFiltroVazio(RankingIndexViewModel model)
        {
            model = model ?? new RankingIndexViewModel();

            var ehVazio = model.nome.IsNullOrWhiteSpace();

            return ehVazio;
        }


        public JsonResult ObterDescricaonome(int codigo)
        {
            //var resultado;
            var fornecedor = _uoW.Pessoas.ObterTodos().FirstOrDefault(x => x.Id_grlbasico == codigo);
            if (fornecedor != null)
            {
                var resultado = new
                {
                    id = fornecedor.Id_grlbasico,
                    nome = fornecedor.nome,
                    email = fornecedor.email,
                    fone01 = fornecedor.ddd_telefone,
                    fone02 = fornecedor.ddd_telefone2,
                    email2 = fornecedor.Email2,
                    contato = fornecedor.contato,
                    cpf = fornecedor.cpf,
                    rg = fornecedor.rg,
                    encontrado = "T"
                };
                return Json(resultado, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var resultado = new
                {
                    nome = "",
                    email = "",
                    fone01 = "",
                    fone02 = "",
                    email2 = "",
                    contato = "",
                    encontrado = "F",
                    cpf = "",
                    rg = "",
                };
                return Json(resultado, JsonRequestBehavior.AllowGet);
                // return Json(resultado, JsonRequestBehavior.AllowGet);
            }

            //return fornecedor == null ? Json(false, JsonRequestBehavior.AllowGet) : Json(resultado, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ObterPessoa(string tipoConsulta, string filtro)
        {
            string html = "";
            //// tipoConsulta = codigo || descricao

            switch (tipoConsulta)
            {
                case "codigo":
                    {
                        var codigo = Convert.ToInt32(filtro);
                        var model = _uoW.Pessoas.ObterTodos().Where(x => x.Id_grlbasico == codigo);

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_grlbasico);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.nome);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalPessoa({0}, '{1}');\"  class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_grlbasico, item.nome);
                            html += "</tr>";
                        }
                    }
                    break;
                case "descricao":
                    {
                        var model =
                            _uoW.Pessoas.ObterTodos()
                                .Where(x => x.nome.ToLower().Trim().Contains(filtro.ToLower().Trim()));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_grlbasico);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.nome);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalPessoa({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.Id_grlbasico, item.nome);
                            html += "</tr>";
                        }
                    }
                    break;
                case "todos":
                    {
                        /* var model = _pessoaApp.GetAll();

                         foreach (var item in model)
                         {
                             html += "<tr>";
                             html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.Id_grlbasico);
                             html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.nome);
                             html +=
                                 string.Format(
                                     "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalPessoa({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                     item.Id_grlbasico, item.nome);
                             html += "</tr>";
                         }*/
                    }
                    break;
            }

            return Json(html);
        }
        public JsonResult AutoCompletePessoaPreFetch()
        {
            try
            {
                var resultado = _uoW.Pessoas.ObterTodos().Select(x =>
                new
                {
                    Id = x.Id_grlbasico.ToString(),
                    Codigo = x.Id_grlbasico.ToString(),
                    Descricao = x.nome.ToString(),
                }).ToList();

                return Json(new { success = true, results = resultado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }

}
