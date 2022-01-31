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

namespace GtecIt.Controllers
{
    public class DentistaController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public DentistaController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }
        public ActionResult Index(DentistaIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);
                //por que nao posso usar o mapeamento do nome desse modelo 
                model.Grid = Mapper.Map<List<DentistaGridViewModel>>(_uoW.Dentistas.ObterTodos().ToList().OrderBy(x => x.Idgrlbasic.nome));
                model.ConsultaTodos = true;
                return View(model);
            }

            model.ConsultaTodos = false;
            //por que nao posso usar o mapeamento do nome desse modelo 
            model.Grid = Mapper.Map<List<DentistaGridViewModel>>(_uoW.Dentistas.ObterTodos().Where(x => x.Idgrlbasic.nome.Contains(model.Nome)).ToList().OrderBy(x => x.Idgrlbasic.nome));
            return View(model);

        }

        public ActionResult Create(DentistaCreateViewModel model, string origem, long orcamento)
        {
            //var model = new DentistaCreateViewModel();
            model.Dropdownnome =
               _uoW.Pessoas.ObterTodos()
                   .OrderBy(x => x.nome)
                   .Select(x => new SelectListItem { Text = x.nome, Value = x.Id_grlbasico.ToString() })
                   .ToList();
            if (model.Id_grlbasico != null && model.Id_grlbasico > 0)
            {

                var pessoa = _uoW.Pessoas.ObterTodos().FirstOrDefault(x => x.Id_grlbasico == model.Id_grlbasico);
                //var pessoa = _pessoaApp.GetAll().FirstOrDefault(x => x.clientes.Any(y => y.id_Grlcliente == model.id_grlcliente));
                //model.CodigoCliente = cliente.id_Grlcliente.ToString();
                model.NomeDentista = pessoa.nome;
            }
            model.origem = origem;
            model.orcamentoid = orcamento;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DentistaCreateViewModel model, FormCollection form)
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

            //var model2 = Mapper.Map<DentistaEditViewModel>(_uoW.Dentistas.ObterPorId(Convert.ToInt32(model.Id_grlbasico)));
            var model2 = _uoW.Dentistas.ObterTodos().FirstOrDefault(x => x.Id_grlbasico == model.Id_grlbasico);
            if (model2 == null)
            {
                var dentista = Mapper.Map<Dentista>(model);
                //_dentistaApp.Add(Mapper.Map<Dentista>(model));
                _uoW.Dentistas.Salvar(dentista);

                
                _uoW.Complete();
                inicializahorario(dentista.id_grldentista);
                var id_dentistas = dentista.id_grldentista;
                if (model.origem.Equals("vendedor_orcamento"))
                {
                    if (model.orcamentoid == 0)
                        return RedirectToAction("Create", "Orcamento");
                    else
                        return RedirectToAction("Edit", "Orcamento", new { codigo = Convert.ToInt32(model.orcamentoid) });

                }
                return RedirectToAction("Edit", new { codigo = _uoW.Dentistas.ObterPorId(id_dentistas).id_grldentista, pagina_origem = "vendedor" });
            }
            else
            {
                model2.Ativo = model.Ativo;
                //_dentistaApp.Update(Mapper.Map<Dentista>(model2));
                _uoW.Dentistas.Atualizar(model2);
                _uoW.Complete();
                var novo_horario = _uoW.horarioprofessor.ObterTodos().Where(x=>x.id_grldentista==model2.id_grldentista).ToList();
                if (novo_horario.Count==0)
                {
                    inicializahorario(model2.id_grldentista);
                }
                if (model.origem.Equals("vendedor_orcamento"))
                {
                    
                    if (model.orcamentoid == 0)
                        return RedirectToAction("Create", "Orcamento");
                    else
                        return RedirectToAction("Edit", "Orcamento", new { codigo = Convert.ToInt32(model.orcamentoid) });

                }

                return RedirectToAction("Edit", new { codigo = _uoW.Dentistas.ObterPorId(model2.id_grldentista).id_grldentista, pagina_origem = "vendedor" });
                //return RedirectToAction("Edit", new { codigo = model2.id_grldentista, pagina_origem = "vendedor" });
            }


            // return RedirectToAction("Index");

        }

        public void inicializahorario(int id)
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
                var dentistas = _uoW.Dentistas.ObterTodos().Where(x => x.id_grldentista == id).ToList();
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




         
        }

        public ActionResult CreateDentista(string origem, long orcamentoid)
        {
            if (origem.Equals("dentista"))
            {
                return RedirectToAction("Create", "Pessoa", new PessoaCreateViewModel { dentista_em_cadastro = true, orcamentoid = orcamentoid });
            }
            else if (origem.Equals("vendedor"))
            {
                return RedirectToAction("Create", "Pessoa", new PessoaCreateViewModel { origem = "vendedor", orcamentoid = orcamentoid });
            }
            else if (origem.Equals("vendedor_orcamento"))
            {
                return RedirectToAction("Create", "Pessoa", new PessoaCreateViewModel { dentista_em_cadastro = true, origem = "vendedor_orcamento", orcamentoid = orcamentoid });
            }
            else
            {
                return RedirectToAction("Create", "Pessoa", new PessoaCreateViewModel { Dentista = true, orcamentoid = orcamentoid });
            }

        }


        public ActionResult Edit(int codigo, string pagina_origem, long orcamentoid = 0)
        {
            /*var consulta = _uoW.horarioprofessor.ObterTodos()
                                                .Where(x => x.Dia == "segunda" && 
                                                            x.id_grldentista==codigo)
                                                .Select(x => x.dentistas).FirstOrDefault();

            //var lista = consulta.ToList();
            //var contador = consulta.Count();

          /*  var query = _uoW.Dentistas.ObterTodos().Where(x=>x.id_grldentista == codigo &&
            x.HorarioProfessor.Any(d => d.Dia.ToLower() == "segunda")).FirstOrDefault();
            */


            //var model2 = _uoW.Dentistas.ObterPorId(codigo);
            var model = Mapper.Map<DentistaEditViewModel2>(_uoW.Dentistas.ObterPorId(codigo));
            //model2.Orcamentos.Count();

            //model.HorarioProfessor = model.HorarioProfessor.Where(x => x.Dia.ToLower() == "segunda")
            //                                               .ToList();


          //  model.teste = "domingo";
            if (model == null)
                return HttpNotFound();

            if (model.Idgrlbasic.Id_grlprofi.HasValue)
                model.Idgrlbasic.NomeProfissao = _uoW.Profissoes.ObterPorId(Convert.ToInt32(model.Idgrlbasic.Id_grlprofi)).descricao;

            model.Idgrlbasic.DropdownEstado = _uoW.EstadoCivils.ObterTodos()
                 .OrderBy(x => x.descricao)
                 .Select(x => new SelectListItem { Text = x.descricao, Value = x.Id_grlcivil.ToString() })
                 .ToList();

            model.Idgrlbasic.DropdownIdentifica = _uoW.TipoTelefones.ObterTodos()
                    .OrderBy(x => x.descricao)
                    .Select(x => new SelectListItem { Text = x.descricao, Value = x.id_grlidtel.ToString() })
                    .ToList();

            model.Idgrlbasic.DropdownSexo = _uoW.Sexos.ObterTodos()
                    .OrderBy(x => x.descricao)
                    .Select(x => new SelectListItem { Text = x.descricao, Value = x.Id_gercdsexo.ToString() })
                    .ToList();
             model.orcamentoid = orcamentoid;
           
            if (model == null)
                return HttpNotFound();
            if (string.IsNullOrEmpty(pagina_origem))
                model.pagina_origem = "";
            else
                model.pagina_origem = pagina_origem;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DentistaEditViewModel model)
        {

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

            if (model.Idgrlbasic.sexo == 0)
            {
                model.Idgrlbasic.sexo = 5;
            }
            //_dentistaApp.Update(Mapper.Map<Dentista>(model));
            _uoW.Dentistas.Atualizar(Mapper.Map<Dentista>(model));
            _uoW.Complete();
            var pessoa = _uoW.Pessoas.ObterPorId(model.Idgrlbasic.Id_grlbasico);
            if (pessoa != null)
            {
                pessoa.razao_social = model.Idgrlbasic.razao_social;
                pessoa.Nome_fantasia = model.Idgrlbasic.razao_social;
                pessoa.insc_municipal = model.Idgrlbasic.insc_municipal;
                pessoa.insc_estadual = model.Idgrlbasic.insc_estadual;
                pessoa.cgc = model.Idgrlbasic.cgc;
                pessoa.sexo = model.Idgrlbasic.sexo;
                pessoa.dt_nascimento = model.Idgrlbasic.dt_nascimento;
                pessoa.Id_grlprofi = model.Idgrlbasic.Id_grlprofi;
                pessoa.Id_grlcidad = model.Idgrlbasic.Id_grlcidad;
                pessoa.Id_grlcivil = model.Idgrlbasic.Id_grlcivil;
                pessoa.cpf = model.Idgrlbasic.cpf;
                pessoa.rg = model.Idgrlbasic.rg;

                pessoa.contato = model.Idgrlbasic.contato;
                pessoa.ddd_telefone = model.Idgrlbasic.ddd_telefone;
                pessoa.Id_grlidtel = model.Idgrlbasic.Id_grlidtel;
                pessoa.ddd_telefone2 = model.Idgrlbasic.ddd_telefone2;
                pessoa.Id_grlidtel02 = model.Idgrlbasic.Id_grlidtel02;
                pessoa.email = model.Idgrlbasic.email;
                pessoa.Email2 = model.Idgrlbasic.Email2;
                _uoW.Pessoas.Atualizar(pessoa);

            }

            //  _uoW.Pessoas.Atualizar(Mapper.Map<Pessoa>(model.grlbasic));
            _uoW.Complete();

            if (model.pagina_origem == null)
            {
                return RedirectToAction("Index");
            }
            else if (model.pagina_origem.Equals("Pessoa"))
            {
                return RedirectToAction("Index", "Cliente");
                // return RedirectToAction("Create", "Orcamento");
            }
            else if (model.pagina_origem.Equals("Orcamento"))
            {
                return RedirectToAction("Create", "Orcamento");
                //return RedirectToAction("Edit", "Orcamento", new { codigo = Convert.ToInt32(model.orcamentoid) });
            }
            
           // return RedirectToAction("Index");
            return RedirectToAction("Index");

        }
        private void SetarMensagens(MensagemAuxiliarViewModel model)
        {
            TempData["Mensagens"] = model;
        }

        [HttpPost]
        public JsonResult Delete(int codigo)
        {
            //var model = _convenioApp.GetById(codigo);
            var model = _uoW.Dentistas.ObterPorId(codigo);
            ModelState.Clear();

            if (!TryValidateModel(model))
            {
                var validationErrors = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());
                return Json(false);

            }
            if (model == null)
            {
                return Json(false);
            }

            model.Ativo = "N";
            try
            {
                //_dentistaApp.Update(Mapper.Map<Dentista>(model));
                _uoW.Dentistas.Atualizar(Mapper.Map<Dentista>(model));
                _uoW.Complete();
            }
            catch (Exception)
            {

                throw;
            }
            // _convenioApp.Update(Mapper.Map<Convenio>(model));

            return Json(true);
        }

        public bool VerificarFiltroVazio(DentistaIndexViewModel model)
        {
            model = model ?? new DentistaIndexViewModel();

            var ehVazio = string.IsNullOrEmpty(model.Nome);

            return ehVazio;
        }
        public JsonResult ObterDescricaoDentista(int codigo)
        {
            var fornecedor = _uoW.Dentistas.ObterTodos().FirstOrDefault(x => x.id_grldentista == codigo && x.Ativo.Equals("S"));


            return fornecedor == null ? Json(false, JsonRequestBehavior.AllowGet) : Json(fornecedor.Idgrlbasic.nome, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObterDentista(string tipoConsulta, string filtro)
        {
            string html = "";
            //// tipoConsulta = codigo || descricao

            switch (tipoConsulta)
            {
                case "codigo":
                    {
                        var codigo = Convert.ToInt32(filtro);
                        var model = _uoW.Dentistas.ObterTodos().Where(x => x.id_grldentista == codigo && x.Ativo.Equals("S"));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.id_grldentista);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.Idgrlbasic.nome);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalDentista({0}, '{1}');\"  class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.id_grldentista, item.Idgrlbasic.nome);
                            html += "</tr>";
                        }
                    }
                    break;
                case "descricao":
                    {
                        var model =
                            _uoW.Dentistas.ObterTodos()
                                .Where(x => x.Idgrlbasic.nome.ToLower().Trim().Contains(filtro.ToLower().Trim()) && x.Ativo.Equals("S"));

                        foreach (var item in model)
                        {
                            html += "<tr>";
                            html += string.Format("<td class=\"col-sm-2 text-center\">{0}</td>", item.id_grldentista);
                            html += string.Format("<td class=\"col-sm-9\">{0}</td>", item.Idgrlbasic.nome);
                            html +=
                                string.Format(
                                    "<td class=\"col-sm-1\"><a href=\"javascript:btnConfirmarModalDentista({0}, '{1}');\" class=\"btn-confirmar\"><span class=\"glyphicon glyphicon-ok\"></span></a></td>",
                                    item.id_grldentista, item.Idgrlbasic.nome);
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

        public JsonResult AutoCompleteDentistaPreFetch()
        {
            try
            {
                var resultado = _uoW.Dentistas.ObterTodos().Select(x =>
                new
                {
                    Id = x.id_grldentista.ToString(),
                    Codigo = x.id_grldentista.ToString(),
                    Descricao = x.Idgrlbasic.nome.ToString(),
                }).ToList();

                return Json(new { success = true, results = resultado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public ActionResult incluirApp(int codigo)
        {

            var model = _uoW.Pessoas.ObterPorId(codigo);
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
                if (string.IsNullOrEmpty(model.email))
                {
                    var mensagem = new List<String>();

                    mensagem.Add("Professor sem email cadastrado");
                    var resposta = new
                    {

                        Sucesso = false,
                        msg = mensagem
                    };

                    return Json(resposta);
                }

                var user = _uoW.Usuarios.ObterTodos().Where(x => x.Id_grlbasico == model.Id_grlbasico).FirstOrDefault();
                if (user == null)
                {
                    var novo = new Usuario(); ;
                    novo.Id_grlbasico = model.Id_grlbasico;
                    novo.Login = model.email;
                    var hash = GetMd5Hash(MD5.Create(), "123");
                    novo.Senha = hash;
                    novo.senha_sem = "123";
                    novo.Administrador = "N";
                    novo.Ativo = "S";
                    novo.Tipo_usuario = 3;
                    _uoW.Usuarios.Salvar(novo);
                    _uoW.Complete();
                    var response = new
                    {

                        Sucesso = true,
                        msg = ""
                    };
                    return Json(response);
                }
                else
                {
                    user.Login = model.email;
                    _uoW.Usuarios.Atualizar(user);
                    _uoW.Complete();
                    var response = new
                    {

                        Sucesso = true,
                        msg = ""
                    };
                    return Json(response);
                }

            }
            catch (Exception ex)
            {

                var response = new
                {

                    Sucesso = false,
                    msg = ex.Message.ToString()
                };
                return Json(response);
            }



        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}