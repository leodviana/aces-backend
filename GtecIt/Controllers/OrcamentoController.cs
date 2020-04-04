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
    public class OrcamentoController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public OrcamentoController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }
        public ActionResult Renovacao(OrcamentoIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;
            else
            {
                model.Dt_inicio = DateTime.Now;
                model.Dt_fim = DateTime.Now;
            }
            if (VerificarFiltroVazio2(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);
                try
                {
                    model.Grid =
                        Mapper.Map<List<OrcamentoGridViewModel>>(
                            _uoW.Orcamentos.ObterTodos()
                                .Where(x => x.status.Equals("0") && x.id_Grltpatendimento == 3 && x.dt_renovacao >= model.Dt_inicio && x.dt_renovacao <= model.Dt_fim)
                                .ToList()
                                .OrderBy(x => x.id_Stqcporcamento));
                    model.ConsultaTodos = true;
                }
                catch (Exception e)
                {

                }


                return View(model);
            }

            /* model.ConsultaTodos = false;
             try
             {
                 model.Grid =
                     Mapper.Map<List<OrcamentoGridViewModel>>(
                        _uoW.Orcamentos.ObterTodos()
                             .Where(x => x.id_Stqcporcamento.Equals(model.id_Stqcporcamento) && (x.status.Equals("0") && (x.id_Grltpatendimento == 3)))
                             .ToList()
                             .OrderBy(x => x.id_Stqcporcamento));
             }
             catch (Exception e)
             {

             }*/

            return View(model);
        }

        public ActionResult Index(OrcamentoIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);
                try
                {
                    model.Grid =
                        Mapper.Map<List<OrcamentoGridViewModel>>(
                            _uoW.Orcamentos.ObterTodos()
                                .Where(x => x.status.Equals("0") && x.id_Grltpatendimento == 3)
                                .ToList()
                                .OrderBy(x => x.id_Stqcporcamento));
                    model.ConsultaTodos = true;
                }
                catch (Exception e)
                {

                }


                return View(model);
            }

            model.ConsultaTodos = false;
            try
            {
                model.Grid =
                    Mapper.Map<List<OrcamentoGridViewModel>>(
                       _uoW.Orcamentos.ObterTodos()
                            .Where(x => x.id_Stqcporcamento.Equals(model.id_Stqcporcamento) && (x.status.Equals("0") && (x.id_Grltpatendimento == 3)))
                            .ToList()
                            .OrderBy(x => x.id_Stqcporcamento));
            }
            catch (Exception e)
            {

            }

            return View(model);

        }


        public ActionResult CreateCliente()
        {
            return RedirectToAction("Create", "Cliente", new ClienteCreateViewModel { cliente_em_cadastro = true });
        }

        public ActionResult CreateClienteEdicao(long orcamento_id)
        {

            return RedirectToAction("Create", "Cliente", new ClienteCreateViewModel { cliente_em_cadastro = true, orcamentoid = orcamento_id });

        }
        public ActionResult CreatedentistaEdicao(long orcamento_id)
        {

            return RedirectToAction("Create", "Dentista", new DentistaCreateViewModel { dentista_em_cadastro = true, orcamentoid = orcamento_id });
        }
        public ActionResult CreateDentista()
        {
            // return RedirectToAction("Create", "Dentista", new PessoaCreateViewModel { OrcamentoEmAndamentodentista = true });
            return RedirectToAction("Create", "Dentista", new DentistaCreateViewModel { dentista_em_cadastro = true });
        }

        public ActionResult Create(OrcamentoCreateViewModel model)
        {
            model.Dt_orcamento = DateTime.Now;
            if (model.id_grlcliente != null && model.id_grlcliente > 0)
            {
                var cliente = _uoW.Clientes.ObterPorId(model.id_grlcliente.Value);
                var pessoa = _uoW.Pessoas.ObterTodos().FirstOrDefault(x => x.clientes.Any(y => y.id_Grlcliente == model.id_grlcliente && y.Ativo.Equals("S")));

                model.CodigoCliente = cliente.id_Grlcliente.ToString();
                model.NomeCliente = pessoa.nome;
            }
            if (model.Id_grldentista != null && model.Id_grldentista > 0)
            {
                var dentista = _uoW.Dentistas.ObterPorId(model.Id_grldentista.Value);
                var pessoa = _uoW.Pessoas.ObterTodos().FirstOrDefault(x => x.dentistas.Any(y => y.id_grldentista == model.Id_grldentista && y.Ativo.Equals("S")));

                model.Id_grldentista = dentista.id_grldentista;
                model.NomeDentista = pessoa.nome;
            }

            model.Dropdownconvenio =
                _uoW.Planos.ObterTodos()
                    .OrderBy(x => x.desc_plano)
                    .Select(x => new SelectListItem { Text = x.desc_plano, Value = x.idGrlplanos.ToString() })
                    .ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrcamentoCreateViewModel model, FormCollection form)
        {
            /* model.ddd_telefone = FiltroDeCaracteres.RemoverCaracteresEspeciais(model.ddd_telefone);
             model.ddd_telefone2 = FiltroDeCaracteres.RemoverCaracteresEspeciais(model.ddd_telefone2);
           
            
             checa_relacionamento(model);*/

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
            model.id_Grltpatendimento = 3;
            // model.id_grlconvenio = 1;
            var orcamento = Mapper.Map<Orcamento>(model);
            _uoW.Orcamentos.Salvar(orcamento);
            _uoW.Complete();
            var id_orcamento = orcamento.id_Stqcporcamento;
            //return Json(true);

            return RedirectToAction("Edit", new { codigo = _uoW.Orcamentos.ObterTodos().FirstOrDefault(x => x.id_Stqcporcamento == id_orcamento && x.status.Equals("0")).id_Stqcporcamento });

        }

        public JsonResult GetEvents()
        {
            var model = _uoW.Events.ObterTodos();//.ObterTodos().FirstOrDefault(x => x.id_Stqcporcamento == codigo && x.status.Equals("0")));


            var events = model.ToList();
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

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


        private void checa_relacionamento_edicao(PessoaEditViewModel model)
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

            var model = Mapper.Map<OrcamentoEditViewModel>(_uoW.Orcamentos.ObterPorId(codigo));//.ObterTodos().FirstOrDefault(x => x.id_Stqcporcamento == codigo && x.status.Equals("0")));

            if (model == null)
                return HttpNotFound();

            if (model.id_grlcliente.HasValue)
                model.NomeCliente = _uoW.Clientes.ObterPorId(model.id_grlcliente.Value).grlbasic.nome;

            if (model.Id_grldentista.HasValue)
                model.NomeDentista = _uoW.Dentistas.ObterPorId(model.Id_grldentista.Value).Idgrlbasic.nome;

            /*model.DropdownEstado = _estadoCivilApp.GetAll()
                   .OrderBy(x => x.descricao)
                   .Select(x => new SelectListItem { Text = x.descricao, Value = x.Id_grlcivil.ToString() })
                   .ToList();

            model.DropdownIdentifica = _tipotelefoneApp.GetAll()
                    .OrderBy(x => x.descricao)
                    .Select(x => new SelectListItem { Text = x.descricao, Value = x.id_grlidtel.ToString() })
                    .ToList();

            model.DropdownSexo = _sexoApp.GetAll()
                    .OrderBy(x => x.descricao)
                    .Select(x => new SelectListItem { Text = x.descricao, Value = x.Id_gercdsexo.ToString() })
                    .ToList();

            */
            model.Dropdownconvenio =
               _uoW.Planos.ObterTodos()
                   .OrderBy(x => x.desc_plano)
                   .Select(x => new SelectListItem { Text = x.desc_plano, Value = x.idGrlplanos.ToString() })
                   .ToList();
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrcamentoEditViewModel model, FormCollection form)
        {
            /*model.ddd_telefone = FiltroDeCaracteres.RemoverCaracteresEspeciais(model.ddd_telefone);
            model.ddd_telefone2 = FiltroDeCaracteres.RemoverCaracteresEspeciais(model.ddd_telefone2);
            
            checa_relacionamento_edicao(model);
            */
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

            _uoW.Orcamentos.Atualizar(Mapper.Map<Orcamento>(model));
            _uoW.Complete();

            return RedirectToAction("Index");

        }


        [HttpPost]
        public ActionResult Delete(int codigo)
        {
            var model = _uoW.Orcamentos.ObterPorId(codigo);
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

            model.status = "2";
            //itens
            _uoW.Orcamentos.Salvar(model);

            var model_itens =
                _uoW.OrcamentoItens.ObterTodos()
                    .Where(x => x.id_stqporcamento == codigo)
                    .ToList()
                    .OrderBy(x => x.id_stqporcamento).ToList();

            // var model_itens  = Mapper.Map<List<OrcamentoItemEditViewModel>>(_orcamentoitemApp.GetById(codigo)).ToList();
            foreach (var itens in model_itens)
            {
                itens.status = "2";
                _uoW.OrcamentoItens.Atualizar(itens);
            }
            //titulos
            var model_titulos = _uoW.Titulos.ObterTodos()
               .Where(x => x.id_stqcporcamento == codigo)
               .ToList()
               .OrderBy(x => x.id_stqcporcamento).ToList();

            foreach (var itens in model_titulos)
            {
                itens.status = "2";
                _uoW.Titulos.Atualizar(itens);
            }
            _uoW.Complete();

            return Json(true);
        }

        public bool VerificarFiltroVazio2(OrcamentoIndexViewModel model)
        {
            model = model ?? new OrcamentoIndexViewModel();

            var ehVazio = !(model.Dt_inicio.ToString().IsNullOrWhiteSpace());
            //var ehVazio = model.id_Stqcporcamento == 0;

            return ehVazio;
        }
        public bool VerificarFiltroVazio(OrcamentoIndexViewModel model)
        {
            model = model ?? new OrcamentoIndexViewModel();

            // var ehVazio = (model.Dt_inicio.ToString().IsNullOrWhiteSpace());
            var ehVazio = model.id_Stqcporcamento == 0;

            return ehVazio;
        }

        public ActionResult Renova(int codigo)
        {
            var model = Mapper.Map<OrcamentoEditViewModel>(_uoW.Orcamentos.ObterPorId(codigo));//.ObterTodos().FirstOrDefault(x => x.id_Stqcporcamento == codigo && x.status.Equals("0")));

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Renova(OrcamentoEditViewModel model2)
        {
            var model = _uoW.Orcamentos.ObterPorId(model2.id_Stqcporcamento);
            ModelState.Clear();

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

            // critica para quitar os titulos em aberto
            var em_aberto = false;
            foreach (var item_titulo in model.Titulos)
            {
                if (item_titulo.status.Equals("0"))
                {
                    if (item_titulo.Valor_pago==0)
                    {
                        em_aberto = true;
                    }
                    else if(!item_titulo.Valor_pago.HasValue)
                    {
                        em_aberto = true;

                    }
                }
            }
            if (em_aberto)
            {
                var mensagem = new List<String>();

                mensagem.Add("Não e possivel realizar a renovação.Contrato com parcelas em aberto!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }
            if (!model2.dt_renovacao.HasValue)
            {
                var mensagem = new List<String>();

                mensagem.Add("Informe a Data de Renovação!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }
            if (model2.dt_renovacao <= model.dt_renovacao)
            {
                var mensagem = new List<String>();

                mensagem.Add("Inicio do novo contrato  tem que ser maior que a data de renovação!");
                var resposta = new
                {

                    Sucesso = false,
                    msg = mensagem
                };

                return Json(resposta);
            }
            TimeSpan dias = model.dt_renovacao.Value - model.Dt_orcamento.Value;
            var novo = new Orcamento
            {
                // id_Stqcporcamento = model.id_Stqcporcamento,
                id_grlcliente = model.id_grlcliente,
                Id_grldentista = model.Id_grldentista,
                id_grlconvenio = model.id_grlconvenio,
                id_Grltpatendimento = model.id_Grltpatendimento,
                Obs = model.Obs,
                status = "0",
                Dt_orcamento = model2.dt_renovacao,
                dt_renovacao = model2.dt_renovacao.Value.AddDays(dias.TotalDays),

            };
            _uoW.Orcamentos.Salvar(novo);

            model.status = "1";
            var novo_item = new OrcamentoItem();
            foreach (var item in model.itemorcamentos)
            {
                if (item.status.Equals("0"))
                {

                    novo_item.id_stqporcamento = novo.id_Stqcporcamento;
                    novo_item.id_stqcdprd = item.id_stqcdprd;
                    novo_item.qtd = item.qtd;
                    novo_item.Vl_unitario = item.Vl_unitario;
                    novo_item.status = "0";


                    item.status = "1";


                }
            }

            _uoW.OrcamentoItens.Salvar(novo_item);

            _uoW.Complete();
            var response = new
            {

                Sucesso = true,
                msg = _uoW.Orcamentos.ObterTodos().FirstOrDefault(x => x.id_Stqcporcamento == novo.id_Stqcporcamento && x.status.Equals("0")).id_Stqcporcamento
            };
            return Json(response);
            // return RedirectToAction("Index");
            //  return RedirectToAction("Edit", new { codigo = _uoW.Orcamentos.ObterTodos().FirstOrDefault(x => x.id_Stqcporcamento == novo.id_Stqcporcamento && x.status.Equals("0")).id_Stqcporcamento });
        }

        public ActionResult geraRelatorio(int codigo)
        {
            var model = new GerarelatorioViewModel();
            model.id_Stqcporcamento = codigo;
            try
            {
                model.DropDownTipoRelatorio =
              _uoW.Relatorios.ObterTodos().Where(x => x.Empresa == GerenciaSession.EmpresaLogado.id_Grlempresa)
                  .OrderBy(x => x.relatorio)
                  .Select(x => new SelectListItem { Text = x.relatorio, Value = x.id_Grlrelatorios.ToString() })
                  .ToList();
            }
            catch (Exception)
            {

                throw;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult geraRelatorio(int codigo, int tipo)
        {
            string nome_arquivo = "";
            if (tipo == 1)
            {
                nome_arquivo = Etiqueta01(codigo);

            }
            else if (tipo == 2)
            {
                nome_arquivo = RelatorioVenda(codigo);
            }

            return Json(nome_arquivo);
        }

        public string RelatorioVenda(Int32 VendaId)
        {
            byte[] _contentBytes;

            var allCustomer =
                Mapper.Map<List<OrcamentoEditViewModel>>(
                    _uoW.Orcamentos.ObterTodos()
                        .Where(x => x.id_Stqcporcamento == VendaId)
                        .ToList()
                        .OrderBy(x => x.id_Stqcporcamento)).ToList();
            var ds = new DataSet1();
            foreach (var item in allCustomer)
            {

                DataRow row2 = ds.Tables[0].NewRow();
                row2["Id_orcamento"] = item.id_Stqcporcamento;
                row2["dt_orcamento"] = item.Dt_orcamento;
                row2["Paciente"] = item.grlcliente.grlbasic.nome;
                row2["dentista"] = item.grldentista.Idgrlbasic.nome;
                if (item.grldentista.Idgrlbasic.Enderecos.Count > 0)
                    row2["endereco_pac"] = item.grldentista.Idgrlbasic.Enderecos[0].Logradouro + "," + item.grldentista.Idgrlbasic.Enderecos[0].Complemento + " " + item.grldentista.Idgrlbasic.Enderecos[0].Bairro + " Cep " + item.grldentista.Idgrlbasic.Enderecos[0].Cep + " " + item.grldentista.Idgrlbasic.Enderecos[0].Cidade;
                else
                {
                    row2["endereco_pac"] = "";
                }

                foreach (var itens in item.itemorcamentos)
                {
                    DataRow row = ds.Tables[1].NewRow();
                    row["Id_orcamento"] = item.id_Stqcporcamento;
                    row["codigo"] = itens.id_stqcdprd;
                    row["procedimento"] = itens.produtos.desc_produto;
                    row["qtde"] = itens.qtd;
                    row["vl_unitario"] = itens.Vl_unitario;
                    row["desconto%"] = itens.descontoperc;
                    row["desconto"] = itens.desconto;
                    row["vl_total"] = itens.Valor_total;
                    ds.Tables[1].Rows.Add(row);
                }
                foreach (var titulos in item.titulos)
                {
                    DataRow row = ds.Tables[2].NewRow();
                    row["Id_orcamento"] = item.id_Stqcporcamento;
                    row["Num_titulo"] = titulos.num_titulo;
                    row["Emissao"] = titulos.dt_emissao;
                    row["vencimento"] = titulos.dt_vencimento;
                    row["Valor"] = titulos.Valor;
                    if (titulos.Valor_pago.HasValue)
                        row["Valor_pago"] = titulos.Valor_pago;
                    else
                        row["Valor_pago"] = DBNull.Value;

                    if (titulos.dt_pagamento.HasValue)
                        row["dt_pagamento"] = titulos.dt_pagamento;
                    else
                    {
                        row["dt_pagamento"] = DBNull.Value;
                    }
                    //row["Tipo_pagamento"] = titulos.tp_pagamento.Descricao;

                    ds.Tables[2].Rows.Add(row);
                }
                ds.Tables[0].Rows.Add(row2);

            }
            //pega o nome do relatorio

            var relatorio = _uoW.Relatorios.ObterPorId(2);
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), relatorio.relatorio));

            rd.SetDataSource(ds);
            //rd.SetDataSource(ds.Tables[1]);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            string filePath = Server.MapPath("~/temp/" + "Atendimento No." + VendaId.ToString() + ".pdf");

            rd.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            stream.Seek(0, SeekOrigin.Begin);
            stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return filePath;

            /* _contentBytes = StreamToBytes(rd.ExportToStream(ExportFormatType.PortableDocFormat));
            //var response = context.HttpContext.ApplicationInstance.Response;
            Response.Clear();
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.ContentType = "application/pdf";

            using (var stream = new MemoryStream(_contentBytes))
            {
                stream.WriteTo(Response.OutputStream);
                stream.Flush();
            }
            return View("RelatorioVenda");
            */
        }


        public string Etiqueta01(Int32 VendaId)
        {
            string filePath = "";
            BinaryReader binRead1 = null;
            FileStream fsOpen1 = null;
            Stream stream = null;
            byte[] _contentBytes = null;

            var allCustomer =
                Mapper.Map<List<OrcamentoEditViewModel>>(
                   _uoW.Orcamentos.ObterTodos()
                        .Where(x => x.id_Stqcporcamento == VendaId)
                        .ToList()
                        .OrderBy(x => x.id_Stqcporcamento)).ToList();
            DataSet1 ds = new DataSet1();
            ds.Clear();
            //preenche os dados da empresa 
            DataRow row3 = ds.Tables[3].NewRow();
            //var empresa = _empresaApp.GetAll().Where(x => x.id_Grlempresa == 1).FirstOrDefault();
            row3["empresa"] = GerenciaSession.EmpresaLogado.descricao;//"SERO";
            row3["Endereco"] = GerenciaSession.EmpresaLogado.endereco;//"Endereço: Praça Dr. Augusto Glória,205 São joão Nepomuceno-MG Cep 36680-000 Fone : (32) 3261-6565";
            //string caminhosalvo =  Server.MapPath(GerenciaSession.EmpresaLogado.logo); //"logo-perboyre-castelo-clinica.PNG");
            //string caminhosalvo = Path.Combine(Server.MapPath("~/images/"), GerenciaSession.EmpresaLogado.logo);
            string caminhosalvo = Server.MapPath("~/imgs/logo-perboyre-castelo-clinica.PNG");// + GerenciaSession.EmpresaLogado.logo);
            ViewBag.localizacao = caminhosalvo;
            if (System.IO.File.Exists(caminhosalvo))
            {
                ViewBag.localizacao = caminhosalvo;
                fsOpen1 = new FileStream(caminhosalvo, FileMode.Open);
                binRead1 = new BinaryReader(fsOpen1);
                row3["logo"] = binRead1.ReadBytes(Convert.ToInt32(binRead1.BaseStream.Length));

            }
            ds.Tables[3].Rows.Add(row3);

            foreach (var item in allCustomer)
            {

                DataRow row2 = ds.Tables[0].NewRow();
                row2["Id_orcamento"] = item.id_Stqcporcamento;
                row2["dt_orcamento"] = item.Dt_orcamento;
                row2["Paciente"] = item.grlcliente.grlbasic.nome;
                row2["dt_nascimento"] = item.grlcliente.grlbasic.dt_nascimento;
                row2["padrao"] = item.Convenios.grlbasic.nome;
                if (item.grlcliente.grlbasic.dt_nascimento.HasValue)
                {
                    DateTime dt_nasc = Convert.ToDateTime(item.grlcliente.grlbasic.dt_nascimento.ToString());
                    DateTime birthday = dt_nasc;// new DateTime(2007,10,18);

                    TimeSpan age = DateTime.Now - birthday;

                    int years = age.Days / 365;
                    int months = (age.Days - (years * 365)) / 30;
                    string strAge = String.Format("{0}a {1}m", years, months);
                    row2["idade"] = "Idade " + strAge;
                }
                row2["fone_paciente"] = item.grlcliente.grlbasic.ddd_telefone;
                row2["dentista"] = item.grldentista.Idgrlbasic.nome;
                if (item.grldentista.Idgrlbasic.Enderecos.Count > 0)
                    row2["endereco_pac"] = item.grldentista.Idgrlbasic.Enderecos[0].Logradouro + "," + item.grldentista.Idgrlbasic.Enderecos[0].Complemento + " " + item.grldentista.Idgrlbasic.Enderecos[0].Bairro + " Cep " + item.grldentista.Idgrlbasic.Enderecos[0].Cep + " " + item.grldentista.Idgrlbasic.Enderecos[0].Cidade;
                else
                {
                    row2["endereco_pac"] = "";
                }

                foreach (var itens in item.itemorcamentos)
                {
                    DataRow row = ds.Tables[1].NewRow();
                    row["Id_orcamento"] = item.id_Stqcporcamento;
                    row["codigo"] = itens.id_stqcdprd;
                    row["procedimento"] = itens.produtos.desc_produto;
                    row["qtde"] = itens.qtd;
                    row["vl_unitario"] = itens.Vl_unitario;
                    row["desconto%"] = itens.descontoperc;
                    row["desconto"] = itens.desconto;
                    row["vl_total"] = itens.Valor_total;
                    ds.Tables[1].Rows.Add(row);
                }
                ds.Tables[0].Rows.Add(row2);

            }
            var relatorio = _uoW.Relatorios.ObterPorId(1);
            ReportDocument rd = new ReportDocument();


            rd.Load(Path.Combine(Server.MapPath("~/Reports"), relatorio.relatorio));

            rd.SetDataSource(ds);


            //rd.SetDataSource(ds.Tables[1]);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            fsOpen1.Close();
            fsOpen1.Dispose();
            binRead1.Close();
            binRead1.Dispose();

            ds.Dispose();
            filePath = Server.MapPath("~/temp/" + "Etiqueta Atendimento " + VendaId.ToString() + ".pdf");

            rd.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);
            stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);



            return filePath;

        }

        public ActionResult BaixarArquivo(string pathArquivo)
        {

            var fileBytes = System.IO.File.ReadAllBytes(pathArquivo);
            string nome_arquivo = tratanomedoarquivo(pathArquivo);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nome_arquivo);

        }

        private string tratanomedoarquivo(string caminho)
        {
            string path_arquivo = caminho;
            int posicao = path_arquivo.LastIndexOf("\\") + 1;

            string nome_arquivo_final = path_arquivo.Substring(posicao);
            posicao = nome_arquivo_final.LastIndexOf(".");
            return nome_arquivo_final;
        }


        private static byte[] StreamToBytes(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }

}
