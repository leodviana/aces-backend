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
    public class RelatorioController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public RelatorioController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RelTitulosareceber(RelGeralViewlModel model, string acao)
        {
            model.inicio = DateTime.Now;
            model.fim = DateTime.Now;

            model.ConvenioDropDown =
                _uoW.Convenios.ObterTodos().Where(x => x.Ativo.Equals("S"))
                    .OrderBy(x => x.grlbasic.nome)
                    .Select(x => new SelectListItem { Text = x.grlbasic.nome, Value = x.id_grlconvenio.ToString() })
                    .ToList();

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RelTitulosareceber(RelGeralViewlModel model)
        {

            byte[] _contentBytes;
            var query = _uoW.Titulos.ObterTodos().Where(x => x.status.Equals("0")).AsQueryable();

            if (model.ordemid == 0)
            {
                return View(model);
            }
            else if (model.ordemid == 1)
            {
                query = query.Where(x => x.dt_emissao >= model.inicio && x.dt_emissao <= model.fim);
            }
            else if (model.ordemid == 2)
            {
                query = query.Where(x => x.dt_vencimento >= model.inicio && x.dt_vencimento <= model.fim);
            }
            else if (model.ordemid == 3)
            {
                query = query.Where(x => x.dt_pagamento >= model.inicio && x.dt_pagamento <= model.fim);
            }

            var allCustomer = Mapper.Map<List<TituloEditViewModel>>(query.ToList());

            var ds = new DataSet1();
            foreach (var titulos in allCustomer)
            {
                DataRow row = ds.Tables[2].NewRow();
                row["Id_orcamento"] = titulos.id_stqcporcamento;
                row["Num_titulo"] = titulos.num_titulo;
                row["Emissao"] = titulos.dt_emissao;
                row["vencimento"] = titulos.dt_vencimento;
                row["Valor"] = titulos.Valor;
                if (titulos.id_fintipopagamento == null)
                {
                    row["tipo_pagamento"] = "";
                }
                else
                {

                    var tipo = _uoW.TipoPagamentos.ObterPorId(Convert.ToInt32(titulos.id_fintipopagamento));
                    if (tipo != null)
                        row["tipo_pagamento"] = tipo.Descricao;
                    else
                    {
                        row["tipo_pagamento"] = "";
                    }
                }


                if (titulos.Valor_pago.HasValue)
                    row["Valor_pago"] = titulos.Valor_pago;
                else
                    row["Valor_pago"] = 0;

                if (titulos.dt_pagamento.HasValue)
                    row["dt_pagamento"] = titulos.dt_pagamento;
                else
                {
                    row["dt_pagamento"] = DBNull.Value;
                }
                //row["cliente"] = titulos.orcamento.grlcliente.grlbasic.nome.ToUpper();
                var item = _uoW.Orcamentos.ObterPorId(Convert.ToInt32(titulos.id_stqcporcamento));
                if (item != null)
                {

                    if (string.IsNullOrEmpty(item.grlcliente.grlbasic.nome))
                    {
                        row["cliente"] = " Sem aluno informado";
                    }
                    else
                    {
                        row["cliente"] = item.grlcliente.grlbasic.nome.ToUpper();
                    }
                    

                }
                ds.Tables[2].Rows.Add(row);


            }
            //pega o nome do relatorio

            //var relatorio = _relatorioApp.GetById(2);
            ReportDocument rd = new ReportDocument();
            // rd.Load(Path.Combine(Server.MapPath("~/Reports"), relatorio.relatorio));

            string empresa = "Gtec - " + GerenciaSession.EmpresaLogado.descricao;
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Rel004 - Titulos a Receber por periodo.rpt"));
            rd.DataDefinition.FormulaFields["data_inicial"].Text = "'" + model.inicio.ToString().Substring(0, 10) + "'";
            rd.DataDefinition.FormulaFields["data_final"].Text = "'" + model.fim.ToString().Substring(0, 10) + "'";  //.Formulas(0) = "data_inicial  = '" & dtinicio & "'"
            rd.DataDefinition.FormulaFields["empresa"].Text = "'" + empresa + "'";
            if (model.ConvenioId == 1)
            {
                //viewer.SetParameters(new ReportParameter("cliente", "TODOS OS CLIENTES"));
                rd.DataDefinition.FormulaFields["Ordem"].Text = "'EMISSÃO'";
            }
            else if (model.ordemid == 2)
            {
                rd.DataDefinition.FormulaFields["Ordem"].Text = "' VENCIMENTO  '";
            }
            else if (model.ordemid == 3)
            {
                rd.DataDefinition.FormulaFields["Ordem"].Text = "' PAGAMENTO  '";
            }
            rd.SetDataSource(ds);
            //rd.SetDataSource(ds.Tables[1]);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            string filePath = Server.MapPath("~/temp/Rel004 - Titulos a Receber por periodo.pdf");

            rd.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            stream.Seek(0, SeekOrigin.Begin);
            stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            string nome_arquivo = tratanomedoarquivo(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nome_arquivo);


        }

        public ActionResult RelAtendimentoPeriodo(RelGeralViewlModel model, string acao)
        {
            model.inicio = DateTime.Now;
            model.fim = DateTime.Now;

            model.ConvenioDropDown =
               _uoW.Planos.ObterTodos()
                    .OrderBy(x => x.desc_plano)
                    .Select(x => new SelectListItem { Text = x.desc_plano, Value = x.idGrlplanos.ToString() })
                    .ToList();

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RelAtendimentoPeriodo(RelGeralViewlModel model)
        {

            byte[] _contentBytes;

            /*var query = _context.Vendas.Where(x => x.Cliente.Pessoa.EmpresaId == (GerenciaSession.UsarioLogado.Pessoa.EmpresaId)).AsQueryable();

            if (DataInicial.HasValue && DataFinal.HasValue)
                query = query.Where(a => a.DataVenda >= DataInicial && a.DataVenda <= DataFinal);

            if (!cidade.IsNullOrWhiteSpace())
                query = query.Where(a => a.Cliente.Pessoa.Cidade == cidade);
             */
            var query = _uoW.Orcamentos.ObterTodos().Where(x => x.Dt_orcamento >= model.inicio && x.Dt_orcamento <= model.fim && x.status.Equals("0")).AsQueryable();
            if (model.ConvenioId != 0)
                query = query.Where(x => x.id_grlconvenio == model.ConvenioId);
            var allCustomer = Mapper.Map<List<OrcamentoRelatorioViewModel>>(query.ToList()).OrderBy(x => x.Dt_orcamento).ThenBy(x => x.id_Stqcporcamento);

            /*var allCustomer =
                Mapper.Map<List<OrcamentoEditViewModel>>(
                    _orcamentoApp.GetAll()
                        .Where(x => x.Dt_orcamento >=model.inicio && x.Dt_orcamento<=model.fim && x.status.Equals("0"))
                        .ToList()
                        .OrderBy(x => x.id_Stqcporcamento)).ToList();*/
            var ds = new DataSet1();
            foreach (var item in allCustomer)
            {

                DataRow row2 = ds.Tables[0].NewRow();
                row2["Id_orcamento"] = item.id_Stqcporcamento;
                row2["dt_orcamento"] = item.Dt_orcamento;
                row2["Paciente"] = item.grlcliente.grlbasic.nome.ToUpper();
                row2["dentista"] = item.grldentista.Idgrlbasic.nome.ToUpper();


                //pesquisa plano 
                var plano = _uoW.Planos.ObterPorId(Convert.ToInt32(item.id_grlconvenio));

                row2["endereco_pac"] = plano.desc_plano;
                row2["endereco_sol"] = item.Obs;
                /* if (item.Convenios.Guia.Equals("S"))
                 {
                     row2["guia"] = "S";
                 }
                 else
                 {
                     row2["guia"] = "N";
                 }*/
                decimal soma_itens = 0;
                foreach (var itens in item.itemorcamentos)
                {
                    if (itens.status.Equals("0"))
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
                        soma_itens = soma_itens + Convert.ToDecimal(itens.Valor_total);
                        ds.Tables[1].Rows.Add(row);
                    }
                }
                var teste = soma_itens;
                /* foreach (var titulos in item.titulos)
                 {
                     if (titulos.status.Equals("0"))
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
                 }*/
                ds.Tables[0].Rows.Add(row2);

            }
            //pega o nome do relatorio

            //var relatorio = _relatorioApp.GetById(2);
            ReportDocument rd = new ReportDocument();
            try
            {

                // rd.Load(Path.Combine(Server.MapPath("~/Reports"), relatorio.relatorio));

                string empresa = "Gtec - " + GerenciaSession.EmpresaLogado.descricao;


                rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Rel002 - Titulos a Receber por periodo.rpt"));
                rd.DataDefinition.FormulaFields["data_inicial"].Text = "'" + model.inicio.ToString().Substring(0, 10) + "'";
                rd.DataDefinition.FormulaFields["data_final"].Text = "'" + model.fim.ToString().Substring(0, 10) + "'";  //.Formulas(0) = "data_inicial  = '" & dtinicio & "'"
                rd.DataDefinition.FormulaFields["empresa"].Text = "'" + empresa + "'";
                if (model.ConvenioId == 0)
                {
                    //viewer.SetParameters(new ReportParameter("cliente", "TODOS OS CLIENTES"));
                    rd.DataDefinition.FormulaFields["Ordem"].Text = "'TODOS OS PLANOS'";
                }
                else
                {
                    var convenio = _uoW.Planos.ObterPorId(model.ConvenioId);

                    rd.DataDefinition.FormulaFields["Ordem"].Text = "'" + convenio.desc_plano + "'";
                }
                rd.SetDataSource(ds);
                //rd.SetDataSource(ds.Tables[1]);


                string filePath = Server.MapPath("~/temp/Rel002 - Contratos por periodo.pdf");
                // string filePath = @"C:\Aplicacoes\Aces\temp\Rel002 - Contratos por periodo.pdf";*/
                try
                {

                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);

                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                    stream.Seek(0, SeekOrigin.Begin);
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    var fileBytes = System.IO.File.ReadAllBytes(filePath);
                    string nome_arquivo = tratanomedoarquivo(filePath);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nome_arquivo);

                }
                catch (Exception ex)
                {
                    return null;
                }



            }
            catch(Exception ex)
            {
                return null;
            }
         }
        //*****
    
        public ActionResult RelAtendimentoPeriodosemtitulos(RelGeralViewlModel model, string acao)
        {
            model.inicio = DateTime.Now;
            model.fim = DateTime.Now;

            model.ConvenioDropDown =
               _uoW.Planos.ObterTodos()
                    .OrderBy(x => x.desc_plano)
                    .Select(x => new SelectListItem { Text = x.desc_plano, Value = x.idGrlplanos.ToString() })
                    .ToList();

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RelAtendimentoPeriodosemtitulos(RelGeralViewlModel model)
        {

            byte[] _contentBytes;

            var query = _uoW.Orcamentos.ObterTodos().Where(x => x.Dt_orcamento >= model.inicio && x.Dt_orcamento <= model.fim && x.status.Equals("0")).AsQueryable();
            if (model.ConvenioId != 0)
                query = query.Where(x => x.id_grlconvenio == model.ConvenioId);
            var allCustomer = Mapper.Map<List<OrcamentoRelatorioViewModel>>(query.ToList()).OrderBy(x => x.Dt_orcamento).ThenBy(x => x.id_Stqcporcamento);

            var ds = new DataSet1();
            foreach (var item in allCustomer)
            {
                if (item.titulos.Count == 0)
                {
                    if (item.Convenios.Guia.Equals("N"))
                    {
                        DataRow row2 = ds.Tables[0].NewRow();
                        row2["Id_orcamento"] = item.id_Stqcporcamento;
                        row2["dt_orcamento"] = item.Dt_orcamento;
                        row2["Paciente"] = item.grlcliente.grlbasic.nome.ToUpper();
                        row2["dentista"] = item.grldentista.Idgrlbasic.nome.ToUpper();

                        var plano = _uoW.Planos.ObterPorId(Convert.ToInt32(item.id_grlconvenio));

                        row2["endereco_pac"] = plano.desc_plano;
                        row2["endereco_sol"] = item.Obs;

                        decimal soma_itens = 0;
                        foreach (var itens in item.itemorcamentos)
                        {
                            if (itens.status.Equals("0"))
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
                                soma_itens = soma_itens + Convert.ToDecimal(itens.Valor_total);
                                ds.Tables[1].Rows.Add(row);
                            }
                        }
                        ds.Tables[0].Rows.Add(row2);
                    }
                }



            }
            //pega o nome do relatorio

            //var relatorio = _relatorioApp.GetById(2);
            ReportDocument rd = new ReportDocument();
            // rd.Load(Path.Combine(Server.MapPath("~/Reports"), relatorio.relatorio));

            string empresa = "Gtec - " + GerenciaSession.EmpresaLogado.descricao;
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Rel006 - Atendimentos sem titulos a receber por periodo.rpt"));
            rd.DataDefinition.FormulaFields["data_inicial"].Text = "'" + model.inicio.ToString().Substring(0, 10) + "'";
            rd.DataDefinition.FormulaFields["data_final"].Text = "'" + model.fim.ToString().Substring(0, 10) + "'";  //.Formulas(0) = "data_inicial  = '" & dtinicio & "'"
            rd.DataDefinition.FormulaFields["empresa"].Text = "'" + empresa + "'";
            if (model.ConvenioId == 0)
            {
                //viewer.SetParameters(new ReportParameter("cliente", "TODOS OS CLIENTES"));
                rd.DataDefinition.FormulaFields["Ordem"].Text = "'TODOS OS PLANOS'";
            }
            else
            {
                var convenio = _uoW.Planos.ObterPorId(model.ConvenioId);

                rd.DataDefinition.FormulaFields["Ordem"].Text = "'" + convenio.desc_plano + "'";
            }
            rd.SetDataSource(ds);
            //rd.SetDataSource(ds.Tables[1]);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            string filePath = Server.MapPath("~/temp/Rel006 - Contratos sem titulos a receber por periodo.pdf");

            rd.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            stream.Seek(0, SeekOrigin.Begin);
            stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            string nome_arquivo = tratanomedoarquivo(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nome_arquivo);


        }

        //*****
        public ActionResult RelAtendimentoPeriodocancelado(RelGeralViewlModel model, string acao)
        {
            model.inicio = DateTime.Now;
            model.fim = DateTime.Now;

            model.ConvenioDropDown =
                _uoW.Planos.ObterTodos()
                    .OrderBy(x => x.desc_plano)
                    .Select(x => new SelectListItem { Text = x.desc_plano, Value = x.idGrlplanos.ToString() })
                    .ToList();

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RelAtendimentoPeriodocancelado(RelGeralViewlModel model)
        {

            byte[] _contentBytes;

            /*var query = _context.Vendas.Where(x => x.Cliente.Pessoa.EmpresaId == (GerenciaSession.UsarioLogado.Pessoa.EmpresaId)).AsQueryable();

            if (DataInicial.HasValue && DataFinal.HasValue)
                query = query.Where(a => a.DataVenda >= DataInicial && a.DataVenda <= DataFinal);

            if (!cidade.IsNullOrWhiteSpace())
                query = query.Where(a => a.Cliente.Pessoa.Cidade == cidade);
             */
            var query = _uoW.Orcamentos.ObterTodos().Where(x => x.Dt_orcamento >= model.inicio && x.Dt_orcamento <= model.fim && x.status.Equals("2")).AsQueryable();
            if (model.ConvenioId != 0)
                query = query.Where(x => x.id_grlconvenio == model.ConvenioId);
            var allCustomer = Mapper.Map<List<OrcamentoRelatorioViewModel>>(query.ToList()).OrderBy(x => x.Dt_orcamento).ThenBy(x => x.id_Stqcporcamento);

            /*var allCustomer =
                Mapper.Map<List<OrcamentoEditViewModel>>(
                    _orcamentoApp.GetAll()
                        .Where(x => x.Dt_orcamento >=model.inicio && x.Dt_orcamento<=model.fim && x.status.Equals("0"))
                        .ToList()
                        .OrderBy(x => x.id_Stqcporcamento)).ToList();*/
            var ds = new DataSet1();
            foreach (var item in allCustomer)
            {

                DataRow row2 = ds.Tables[0].NewRow();
                row2["Id_orcamento"] = item.id_Stqcporcamento;
                row2["dt_orcamento"] = item.Dt_orcamento;
                row2["Paciente"] = item.grlcliente.grlbasic.nome.ToUpper();
                row2["dentista"] = item.grldentista.Idgrlbasic.nome.ToUpper();

                var plano = _uoW.Planos.ObterPorId(Convert.ToInt32(item.id_grlconvenio));

                row2["endereco_pac"] = plano.desc_plano;
                row2["endereco_sol"] = item.Obs;

                decimal soma_itens = 0;
                foreach (var itens in item.itemorcamentos)
                {
                    if (itens.status.Equals("2"))
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
                        soma_itens = soma_itens + Convert.ToDecimal(itens.Valor_total);
                        ds.Tables[1].Rows.Add(row);
                    }
                }
                var teste = soma_itens;
                /* foreach (var titulos in item.titulos)
                 {
                     if (titulos.status.Equals("0"))
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
                 }*/
                ds.Tables[0].Rows.Add(row2);

            }
            //pega o nome do relatorio

            //var relatorio = _relatorioApp.GetById(2);
            ReportDocument rd = new ReportDocument();
            // rd.Load(Path.Combine(Server.MapPath("~/Reports"), relatorio.relatorio));

            string empresa = "Gtec - " + GerenciaSession.EmpresaLogado.descricao;
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Rel005 - Atendimentos Cancelados por periodo.rpt"));
            rd.DataDefinition.FormulaFields["data_inicial"].Text = "'" + model.inicio.ToString().Substring(0, 10) + "'";
            rd.DataDefinition.FormulaFields["data_final"].Text = "'" + model.fim.ToString().Substring(0, 10) + "'";  //.Formulas(0) = "data_inicial  = '" & dtinicio & "'"
            rd.DataDefinition.FormulaFields["empresa"].Text = "'" + empresa + "'";
            if (model.ConvenioId == 0)
            {
                //viewer.SetParameters(new ReportParameter("cliente", "TODOS OS CLIENTES"));
                rd.DataDefinition.FormulaFields["Ordem"].Text = "'TODOS OS PLANOS'";
            }
            else
            {
                var convenio = _uoW.Planos.ObterPorId(model.ConvenioId);

                rd.DataDefinition.FormulaFields["Ordem"].Text = "'" + convenio.desc_plano + "'";
            }
            rd.SetDataSource(ds);
            //rd.SetDataSource(ds.Tables[1]);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            string filePath = Server.MapPath("~/temp/Rel005 - Contratos Cancelados por periodo.pdf");

            rd.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            stream.Seek(0, SeekOrigin.Begin);
            stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            string nome_arquivo = tratanomedoarquivo(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nome_arquivo);


        }

        //*******
        public ActionResult Relatendimentopordentista(RelGeralViewlModel model, string acao)
        {

            model.inicio = DateTime.Now;
            model.fim = DateTime.Now;

            model.ConvenioDropDown =
                _uoW.Dentistas.ObterTodos().Where(x => x.Ativo.Equals("S"))
                    .OrderBy(x => x.Idgrlbasic.nome)
                    .Select(x => new SelectListItem { Text = x.Idgrlbasic.nome, Value = x.id_grldentista.ToString() })
                    .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Relatendimentopordentista(RelGeralViewlModel model)
        {

            byte[] _contentBytes;

            /*var query = _context.Vendas.Where(x => x.Cliente.Pessoa.EmpresaId == (GerenciaSession.UsarioLogado.Pessoa.EmpresaId)).AsQueryable();

            if (DataInicial.HasValue && DataFinal.HasValue)
                query = query.Where(a => a.DataVenda >= DataInicial && a.DataVenda <= DataFinal);

            if (!cidade.IsNullOrWhiteSpace())
                query = query.Where(a => a.Cliente.Pessoa.Cidade == cidade);
             */
            var query = _uoW.Orcamentos.ObterTodos().Where(x => x.Dt_orcamento >= model.inicio && x.Dt_orcamento <= model.fim && x.status.Equals("0")).AsQueryable();
            if (model.ConvenioId != 0)
                query = query.Where(x => x.Id_grldentista == model.ConvenioId);
            var allCustomer = Mapper.Map<List<OrcamentoRelatorioViewModel>>(query.ToList()).OrderBy(x => x.Dt_orcamento).ThenBy(x => x.id_Stqcporcamento);

            /*var allCustomer =
                Mapper.Map<List<OrcamentoEditViewModel>>(
                    _orcamentoApp.GetAll()
                        .Where(x => x.Dt_orcamento >=model.inicio && x.Dt_orcamento<=model.fim && x.status.Equals("0"))
                        .ToList()
                        .OrderBy(x => x.id_Stqcporcamento)).ToList();*/
            var ds = new DataSet1();
            foreach (var item in allCustomer)
            {

                DataRow row2 = ds.Tables[0].NewRow();
                row2["Id_orcamento"] = item.id_Stqcporcamento;
                row2["dt_orcamento"] = item.Dt_orcamento;
                row2["Paciente"] = item.grlcliente.grlbasic.nome.ToUpper();
                row2["dentista"] = item.grldentista.Idgrlbasic.nome.ToUpper();

                var plano = _uoW.Planos.ObterPorId(Convert.ToInt32(item.id_grlconvenio));

                row2["endereco_pac"] = plano.desc_plano;
                row2["endereco_sol"] = item.Obs;
                /*if (item.Convenios.Guia.Equals("S"))
                {
                    row2["guia"] = "S";
                }
                else
                {
                    row2["guia"] = "N";
                }*/
                decimal soma_itens = 0;
                foreach (var itens in item.itemorcamentos)
                {
                    if (itens.status.Equals("0"))
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
                        soma_itens = soma_itens + Convert.ToDecimal(itens.Valor_total);
                        ds.Tables[1].Rows.Add(row);
                    }
                }

                ds.Tables[0].Rows.Add(row2);

            }
            //pega o nome do relatorio

            //var relatorio = _relatorioApp.GetById(2);
            ReportDocument rd = new ReportDocument();
            // rd.Load(Path.Combine(Server.MapPath("~/Reports"), relatorio.relatorio));

            string empresa = "Gtec - " + GerenciaSession.EmpresaLogado.descricao;
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Rel007 - Atendimentos por Dentista.rpt"));
            rd.DataDefinition.FormulaFields["data_inicial"].Text = "'" + model.inicio.ToString().Substring(0, 10) + "'";
            rd.DataDefinition.FormulaFields["data_final"].Text = "'" + model.fim.ToString().Substring(0, 10) + "'";  //.Formulas(0) = "data_inicial  = '" & dtinicio & "'"
            rd.DataDefinition.FormulaFields["empresa"].Text = "'" + empresa + "'";
            if (model.ConvenioId == 0)
            {
                //viewer.SetParameters(new ReportParameter("cliente", "TODOS OS CLIENTES"));
                rd.DataDefinition.FormulaFields["Ordem"].Text = "'TODOS OS PROFESSORES'";
            }
            else
            {
                var convenio = _uoW.Dentistas.ObterPorId(model.ConvenioId);

                rd.DataDefinition.FormulaFields["Ordem"].Text = "'" + convenio.Idgrlbasic.nome + "'";
            }
            rd.SetDataSource(ds);
            //rd.SetDataSource(ds.Tables[1]);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            string filePath = Server.MapPath("~/temp/Rel007 - Contratos por Professor.pdf");

            rd.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            stream.Seek(0, SeekOrigin.Begin);
            stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            string nome_arquivo = tratanomedoarquivo(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nome_arquivo);


        }
        //***********************


        //*******
        public ActionResult RelAulasporprofessor(RelGeralViewlModel model, string acao)
        {

            model.inicio = DateTime.Now;
            model.fim = DateTime.Now;

            model.ConvenioDropDown =
                _uoW.Dentistas.ObterTodos().Where(x => x.Ativo.Equals("S"))
                    .OrderBy(x => x.Idgrlbasic.nome)
                    .Select(x => new SelectListItem { Text = x.Idgrlbasic.nome, Value = x.id_grldentista.ToString() })
                    .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RelAulasporprofessor(RelGeralViewlModel model)
        {

            string aux_data = model.fim.ToString().Substring(0, 10);
            aux_data = aux_data + " 23:59:59";
            model.fim = Convert.ToDateTime(aux_data);
            aux_data = model.inicio.ToString().Substring(0, 10);
            aux_data = aux_data + " 00:00:00";
            model.inicio = Convert.ToDateTime(aux_data);

            byte[] _contentBytes;

            /*var query = _context.Vendas.Where(x => x.Cliente.Pessoa.EmpresaId == (GerenciaSession.UsarioLogado.Pessoa.EmpresaId)).AsQueryable();

            if (DataInicial.HasValue && DataFinal.HasValue)
                query = query.Where(a => a.DataVenda >= DataInicial && a.DataVenda <= DataFinal);

            if (!cidade.IsNullOrWhiteSpace())
                query = query.Where(a => a.Cliente.Pessoa.Cidade == cidade);
             */
            var query = _uoW.Aulas.ObterTodos().Where(x => x.inicio >= model.inicio && x.inicio <= model.fim && x.status.Equals("1")).OrderBy(x=>x.inicio).AsQueryable();
            if (model.ConvenioId != 0)
                query = query.Where(x => x.id_grldentista == model.ConvenioId);


            var ds = new Models.DataSet1();
            foreach (var item in query.ToList())
            {
                if (item.id_Stqcporcamento==2173)
                {
                    var teste = "";
                }
                if (item.id_Stqcporcamento != null)
                {

                    var contrato = _uoW.Orcamentos.ObterPorId(Convert.ToInt32(item.id_Stqcporcamento));
                    string dentista_nome = "";

                    /*DataRow row2 = ds.Tables[0].NewRow();
                    row2["Id_orcamento"] = contrato.id_Stqcporcamento;
                    row2["dt_orcamento"] = contrato.Dt_orcamento;
                    row2["Paciente"] = contrato.grlcliente.grlbasic.nome.ToUpper();
                    row2["Status"] = contrato.status;*/
                    //row2["dentista"] = contrato.grldentista.Idgrlbasic.nome.ToUpper();
                    //var dentista = item.id_grldentista;
                    var dentista = _uoW.Dentistas.ObterPorId(Convert.ToInt16(item.id_grldentista));
                    if (dentista!=null)
                    {
                        var pessoa = _uoW.Pessoas.ObterPorId(Convert.ToInt16(dentista.Id_grlbasico));
                       // row2["dentista"] = pessoa.nome;*/
                        dentista_nome  = pessoa.nome;
                    }



                   // row2["endereco_pac"] = plano.desc_plano;
                    //row2["endereco_sol"] = contrato.Obs;
                    //ds.Tables[0].Rows.Add(row2);*/
                    var plano = _uoW.Planos.ObterPorId(Convert.ToInt32(contrato.id_grlconvenio));
                    string aula = item.inicio.ToString();
                    aula = aula.Substring(0, 10) + " " + aula.Substring(11, 5) + "-" + item.final.ToString().Substring(11, 5);

                    var itens = _uoW.OrcamentoItens.ObterTodos().Where(x => x.id_stqporcamento == item.id_Stqcporcamento && x.status != "2").ToList();
                    var descricao = "";
                    decimal? valor_total = 0;
                    var qtd_aulas = 0;
                    decimal? valor2 = 0;
                    decimal? vl_jogo = 0;
                    var dupla = false;
                    decimal? valoraula = 0;

                    
                    foreach (var itemorcamento in itens)
                    {
                        var precoplano = _uoW.PrecosPlano.ObterTodos().Where(x => x.id_stqcdprd == itemorcamento.id_stqcdprd && x.idGrlplanos == contrato.id_grlconvenio).OrderBy(x=>x.vigencia).ToList();
                        if (precoplano.Count>1)
                        {
                            foreach (var item_plano in precoplano)
                            {
                                if (item_plano.vigencia<=contrato.Dt_orcamento)
                                {
                                    qtd_aulas = item_plano.qtd_aulas;
                                    valor2 = item_plano.valor2;
                                }
                            }
                            
                        }
                        else
                        {
                            qtd_aulas = precoplano[0].qtd_aulas;
                            valor2 = precoplano[0].valor2;
                        }
                        
                        descricao = itemorcamento.produtos.desc_produto;
                        if (descricao.Contains("dupla"))
                        {
                            dupla = true;
                            vl_jogo = Convert.ToDecimal(valor2) / 2;
                        }
                        else if (descricao.Contains("DUPLA"))
                        {
                            dupla = true;
                            vl_jogo = Convert.ToDecimal(valor2) / 2;
                        }
                        else
                        {
                            vl_jogo = Convert.ToDecimal(valor2);
                        }

                        valor_total = (Convert.ToDecimal((itemorcamento.qtd) * Convert.ToDecimal(itemorcamento.Vl_unitario)) - Convert.ToDecimal(itemorcamento.
                            desconto));
                        //  valoraula = (valor_total - vl_jogo);


                        if (itemorcamento.descontoperc != 0)
                        {

                            // valor_total = (Convert.ToDecimal((itemorcamento.qtd) * Convert.ToDecimal(itemorcamento.Vl_unitario)));
                            valoraula = (valor_total - vl_jogo);
                            valoraula = valoraula - (valoraula * (itemorcamento.descontoperc / 100));
                            valor_total = valor_total - (valor_total * (itemorcamento.descontoperc / 100));

                        }
                        else
                        {
                            valoraula = (valor_total - vl_jogo);
                        }


                        //valor do plano *40%/numero de aulas 
                        valoraula = valoraula * Convert.ToDecimal(0.4) / qtd_aulas;
                        if (dupla)
                            valoraula = valoraula * 2;

                    }

                    string aluno_filtro = contrato.grlcliente.grlbasic.nome.ToUpper();
                    string filtro = "aluno ='" + aluno_filtro + "'"  + " and aula='" +aula + "'"; 
                    DataRow[] pesquisa = ds.Tables["DataTable5"].Select(filtro);

                    if (pesquisa.Count()==0)
                    {
                        DataRow row = ds.Tables["DataTable5"].NewRow();
                        row["Id_orcamento"] = item.id_Stqcporcamento;
                        row["id_professor"] = item.id_grldentista;
                        row["inicio"] = item.inicio;
                        row["Fim"] = item.final;
                        row["plano"] = plano.desc_plano;
                        row["aula"] = aula;
                        row["aluno"] = contrato.grlcliente.grlbasic.nome.ToUpper();
                        row["professor"] = dentista_nome; //contrato.grldentista.Idgrlbasic.nome.ToUpper();
                        row["data_aula"] = item.inicio.ToString().Substring(0, 10);
                        row["servico"] = descricao;
                        row["Valor"] = valor_total;
                        row["valor_base"] = valoraula;
                        row["Status"] = contrato.status;
                        row["Aula_grupo"] = Convert.ToDateTime(item.inicio.ToString().Substring(0, 10));
                        ds.Tables["DataTable5"].Rows.Add(row);
                    }
                                       


                }

            }
            //pega o nome do relatorio

            //var relatorio = _relatorioApp.GetById(2);
            ReportDocument rd = new ReportDocument();
            // rd.Load(Path.Combine(Server.MapPath("~/Reports"), relatorio.relatorio));

            string empresa = "Gtec - " + GerenciaSession.EmpresaLogado.descricao;
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Rel011 - Aulas por Professor.rpt"));
            rd.DataDefinition.FormulaFields["data_inicial"].Text = "'" + model.inicio.ToString().Substring(0, 10) + "'";
            rd.DataDefinition.FormulaFields["data_final"].Text = "'" + model.fim.ToString().Substring(0, 10) + "'";  //.Formulas(0) = "data_inicial  = '" & dtinicio & "'"
            rd.DataDefinition.FormulaFields["empresa"].Text = "'" + empresa + "'";

            if (model.ConvenioId == 0)
            {
                //viewer.SetParameters(new ReportParameter("cliente", "TODOS OS CLIENTES"));
                rd.DataDefinition.FormulaFields["Ordem"].Text = "'TODOS OS PROFESSORES'";
            }
            else
            {
                var convenio = _uoW.Dentistas.ObterPorId(model.ConvenioId);

                rd.DataDefinition.FormulaFields["Ordem"].Text = "'" + convenio.Idgrlbasic.nome + "'";
            }
           // rd.DataDefinition.SortFields[7].SortDirection = CrystalDecisions.Shared.SortDirection.AscendingOrder;
            //rd.DataDefinition.SortFields["data_aula"]
            rd.SetDataSource(ds);
            //rd.SetDataSource(ds.Tables[1]);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            string filePath = Server.MapPath("~/temp/Rel011 - Aulas por Professor.pdf");

            rd.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            stream.Seek(0, SeekOrigin.Begin);
            stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            string nome_arquivo = tratanomedoarquivo(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nome_arquivo);


        }
        //***********************
        public ActionResult Relatendimentopordentistasintetico(RelGeralViewlModel model, string acao)
        {

            model.inicio = DateTime.Now;
            model.fim = DateTime.Now;

            model.ConvenioDropDown =
                _uoW.Dentistas.ObterTodos().Where(x => x.Ativo.Equals("S"))
                    .OrderBy(x => x.Idgrlbasic.nome)
                    .Select(x => new SelectListItem { Text = x.Idgrlbasic.nome, Value = x.id_grldentista.ToString() })
                    .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Relatendimentopordentistasintetico(RelGeralViewlModel model)
        {

            byte[] _contentBytes;

            /*var query = _context.Vendas.Where(x => x.Cliente.Pessoa.EmpresaId == (GerenciaSession.UsarioLogado.Pessoa.EmpresaId)).AsQueryable();

            if (DataInicial.HasValue && DataFinal.HasValue)
                query = query.Where(a => a.DataVenda >= DataInicial && a.DataVenda <= DataFinal);

            if (!cidade.IsNullOrWhiteSpace())
                query = query.Where(a => a.Cliente.Pessoa.Cidade == cidade);
             */
            var query = _uoW.Orcamentos.ObterTodos().Where(x => x.Dt_orcamento >= model.inicio && x.Dt_orcamento <= model.fim && x.status.Equals("0")).AsQueryable();
            if (model.ConvenioId != 0)
                query = query.Where(x => x.Id_grldentista == model.ConvenioId);
            var allCustomer = Mapper.Map<List<OrcamentoRelatorioViewModel>>(query.ToList()).OrderBy(x => x.Dt_orcamento).ThenBy(x => x.id_Stqcporcamento);

            /*var allCustomer =
                Mapper.Map<List<OrcamentoEditViewModel>>(
                    _orcamentoApp.GetAll()
                        .Where(x => x.Dt_orcamento >=model.inicio && x.Dt_orcamento<=model.fim && x.status.Equals("0"))
                        .ToList()
                        .OrderBy(x => x.id_Stqcporcamento)).ToList();*/
            var ds = new DataSet1();
            foreach (var item in allCustomer)
            {

                DataRow row2 = ds.Tables[0].NewRow();
                row2["Id_orcamento"] = item.id_Stqcporcamento;
                row2["dt_orcamento"] = item.Dt_orcamento;
                row2["Paciente"] = item.grlcliente.grlbasic.nome.ToUpper();
                row2["dentista"] = item.grldentista.Idgrlbasic.nome.ToUpper();

                var plano = _uoW.Planos.ObterPorId(Convert.ToInt32(item.id_grlconvenio));

                row2["endereco_pac"] = plano.desc_plano;
                row2["endereco_sol"] = item.Obs;

                decimal soma_itens = 0;
                foreach (var itens in item.itemorcamentos)
                {
                    if (itens.status.Equals("0"))
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
                        soma_itens = soma_itens + Convert.ToDecimal(itens.Valor_total);
                        ds.Tables[1].Rows.Add(row);
                    }
                }

                ds.Tables[0].Rows.Add(row2);

            }
            //pega o nome do relatorio

            //var relatorio = _relatorioApp.GetById(2);
            ReportDocument rd = new ReportDocument();
            // rd.Load(Path.Combine(Server.MapPath("~/Reports"), relatorio.relatorio));

            string empresa = "Gtec - " + GerenciaSession.EmpresaLogado.descricao;
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Rel008 - Atendimentos por Dentista sintetico.rpt"));
            rd.DataDefinition.FormulaFields["data_inicial"].Text = "'" + model.inicio.ToString().Substring(0, 10) + "'";
            rd.DataDefinition.FormulaFields["data_final"].Text = "'" + model.fim.ToString().Substring(0, 10) + "'";  //.Formulas(0) = "data_inicial  = '" & dtinicio & "'"
            rd.DataDefinition.FormulaFields["empresa"].Text = "'" + empresa + "'";
            if (model.ConvenioId == 0)
            {
                //viewer.SetParameters(new ReportParameter("cliente", "TODOS OS CLIENTES"));
                rd.DataDefinition.FormulaFields["Ordem"].Text = "'TODOS OS PROFESSORES'";
            }
            else
            {
                var convenio = _uoW.Planos.ObterPorId((model.ConvenioId));

                rd.DataDefinition.FormulaFields["Ordem"].Text = "'" + convenio.desc_plano + "'";
            }
            rd.SetDataSource(ds);
            //rd.SetDataSource(ds.Tables[1]);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            string filePath = Server.MapPath("~/temp/Rel008 - Conotratos por Professor sintetico.pdf");

            rd.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            stream.Seek(0, SeekOrigin.Begin);
            stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            string nome_arquivo = tratanomedoarquivo(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nome_arquivo);


        }
        //****

        //****
        public ActionResult RelExamespordentista(RelGeralViewlModel model, string acao)
        {

            model.inicio = DateTime.Now;
            model.fim = DateTime.Now;

            model.ConvenioDropDown =
               _uoW.Dentistas.ObterTodos().Where(x => x.Ativo.Equals("S"))
                    .OrderBy(x => x.Idgrlbasic.nome)
                    .Select(x => new SelectListItem { Text = x.Idgrlbasic.nome, Value = x.id_grldentista.ToString() })
                    .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RelExamespordentista(RelGeralViewlModel model)
        {

            byte[] _contentBytes;

            /*var query = _context.Vendas.Where(x => x.Cliente.Pessoa.EmpresaId == (GerenciaSession.UsarioLogado.Pessoa.EmpresaId)).AsQueryable();

            if (DataInicial.HasValue && DataFinal.HasValue)
                query = query.Where(a => a.DataVenda >= DataInicial && a.DataVenda <= DataFinal);

            if (!cidade.IsNullOrWhiteSpace())
                query = query.Where(a => a.Cliente.Pessoa.Cidade == cidade);
             */
            var query = _uoW.Orcamentos.ObterTodos().Where(x => x.Dt_orcamento >= model.inicio && x.Dt_orcamento <= model.fim && x.status.Equals("0")).AsQueryable();
            if (model.ConvenioId != 0)
                query = query.Where(x => x.Id_grldentista == model.ConvenioId);
            var allCustomer = Mapper.Map<List<OrcamentoRelatorioViewModel>>(query.ToList()).OrderBy(x => x.Dt_orcamento).ThenBy(x => x.id_Stqcporcamento);

            /*var allCustomer =
                Mapper.Map<List<OrcamentoEditViewModel>>(
                    _orcamentoApp.GetAll()
                        .Where(x => x.Dt_orcamento >=model.inicio && x.Dt_orcamento<=model.fim && x.status.Equals("0"))
                        .ToList()
                        .OrderBy(x => x.id_Stqcporcamento)).ToList();*/
            var ds = new DataSet1();
            foreach (var item in allCustomer)
            {

                DataRow row2 = ds.Tables[0].NewRow();
                row2["Id_orcamento"] = item.id_Stqcporcamento;
                row2["dt_orcamento"] = item.Dt_orcamento;
                row2["Paciente"] = item.grlcliente.grlbasic.nome.ToUpper();
                row2["dentista"] = item.grldentista.Idgrlbasic.nome.ToUpper();

                var plano = _uoW.Planos.ObterPorId(Convert.ToInt32(item.id_grlconvenio));

                row2["endereco_pac"] = plano.desc_plano;
                row2["endereco_sol"] = item.Obs;

                decimal soma_itens = 0;
                foreach (var itens in item.itemorcamentos)
                {
                    if (itens.status.Equals("0"))
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
                        soma_itens = soma_itens + Convert.ToDecimal(itens.Valor_total);
                        ds.Tables[1].Rows.Add(row);
                    }
                }

                ds.Tables[0].Rows.Add(row2);

            }
            //pega o nome do relatorio

            //var relatorio = _relatorioApp.GetById(2);
            ReportDocument rd = new ReportDocument();
            // rd.Load(Path.Combine(Server.MapPath("~/Reports"), relatorio.relatorio));

            string empresa = "Gtec - " + GerenciaSession.EmpresaLogado.descricao;
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Rel009 - Exames por Dentista.rpt"));
            rd.DataDefinition.FormulaFields["data_inicial"].Text = "'" + model.inicio.ToString().Substring(0, 10) + "'";
            rd.DataDefinition.FormulaFields["data_final"].Text = "'" + model.fim.ToString().Substring(0, 10) + "'";  //.Formulas(0) = "data_inicial  = '" & dtinicio & "'"
            rd.DataDefinition.FormulaFields["empresa"].Text = "'" + empresa + "'";
            if (model.ConvenioId == 0)
            {
                //viewer.SetParameters(new ReportParameter("cliente", "TODOS OS CLIENTES"));
                rd.DataDefinition.FormulaFields["Ordem"].Text = "'TODOS OS PROFESSORES'";
            }
            else
            {
                var convenio = _uoW.Planos.ObterPorId(model.ConvenioId);

                rd.DataDefinition.FormulaFields["Ordem"].Text = "'" + convenio.desc_plano + "'";
            }
            rd.SetDataSource(ds);
            //rd.SetDataSource(ds.Tables[1]);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            string filePath = Server.MapPath("~/temp/Rel009 - Serviços por Professor.pdf");

            rd.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            stream.Seek(0, SeekOrigin.Begin);
            stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            string nome_arquivo = tratanomedoarquivo(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nome_arquivo);


        }
        //***


        //
        public ActionResult RelCursoPeriodo(RelGeralViewlModel model, string acao)
        {
            model.inicio = DateTime.Now;
            model.fim = DateTime.Now;

            model.ConvenioDropDown =
               _uoW.Centrodecusto.ObterTodos().Where(x => x.Ativo.Equals("S"))
                    .OrderBy(x => x.desc_ccusto)
                    .Select(x => new SelectListItem { Text = x.desc_ccusto, Value = x.Id_grlccust.ToString() })
                    .ToList();

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RelCursoPeriodo(RelGeralViewlModel model)
        {

            byte[] _contentBytes;


            var query = _uoW.Orcamentos.ObterTodos().Where(x => x.Dt_orcamento >= model.inicio && x.Dt_orcamento <= model.fim && x.status.Equals("0")).AsQueryable();
            if (model.ConvenioId != 0)
                query = query.Where(x => x.id_grlconvenio == model.ConvenioId);
            var allCustomer = Mapper.Map<List<OrcamentoRelatorioViewModel>>(query.ToList()).OrderBy(x => x.Dt_orcamento).ThenBy(x => x.id_Stqcporcamento);

            var ds = new DataSet1();
            foreach (var item in allCustomer)
            {

                DataRow row2 = ds.Tables[0].NewRow();
                row2["Id_orcamento"] = item.id_Stqcporcamento;
                row2["dt_orcamento"] = item.Dt_orcamento;
                row2["Paciente"] = item.grlcliente.grlbasic.nome.ToUpper();
                row2["dentista"] = item.grldentista.Idgrlbasic.nome.ToUpper();

                row2["endereco_pac"] = item.Convenios.grlbasic.nome.ToUpper();
                row2["endereco_sol"] = item.Obs;
                if (item.Convenios.Guia.Equals("S"))
                {
                    row2["guia"] = "S";
                }
                else
                {
                    row2["guia"] = "N";
                }
                decimal soma_itens = 0;
                foreach (var itens in item.itemorcamentos)
                {
                    if (itens.status.Equals("0"))
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
                        soma_itens = soma_itens + Convert.ToDecimal(itens.Valor_total);
                        ds.Tables[1].Rows.Add(row);
                    }
                }
                var teste = soma_itens;

                ds.Tables[0].Rows.Add(row2);

            }
            //pega o nome do relatorio

            //var relatorio = _relatorioApp.GetById(2);
            ReportDocument rd = new ReportDocument();
            // rd.Load(Path.Combine(Server.MapPath("~/Reports"), relatorio.relatorio));

            string empresa = "Gtec - " + GerenciaSession.EmpresaLogado.descricao;
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Rel010 - Cursos por periodo Sintético.rpt"));
            rd.DataDefinition.FormulaFields["data_inicial"].Text = "'" + model.inicio.ToString().Substring(0, 10) + "'";
            rd.DataDefinition.FormulaFields["data_final"].Text = "'" + model.fim.ToString().Substring(0, 10) + "'";  //.Formulas(0) = "data_inicial  = '" & dtinicio & "'"
            rd.DataDefinition.FormulaFields["empresa"].Text = "'" + empresa + "'";
            if (model.ConvenioId == 0)
            {
                //viewer.SetParameters(new ReportParameter("cliente", "TODOS OS CLIENTES"));
                rd.DataDefinition.FormulaFields["Ordem"].Text = "'TODOS OS CURSOS'";
            }
            else
            {
                var convenio = _uoW.Centrodecusto.ObterPorId(model.CursoId);

                rd.DataDefinition.FormulaFields["Ordem"].Text = "'" + convenio.desc_ccusto + "'";
            }
            rd.SetDataSource(ds);
            //rd.SetDataSource(ds.Tables[1]);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            string filePath = Server.MapPath("~/temp/Rel010 - Cursos por periodo Sintético.pdf");

            rd.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            stream.Seek(0, SeekOrigin.Begin);
            stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            string nome_arquivo = tratanomedoarquivo(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nome_arquivo);


        }
        //***
        public ActionResult RelAtendimentoPeriodoporconnvenio(RelGeralViewlModel model, string acao)
        {
            model.inicio = DateTime.Now;
            model.fim = DateTime.Now;

            model.ConvenioDropDown =
               _uoW.Planos.ObterTodos()
                    .OrderBy(x => x.desc_plano)
                    .Select(x => new SelectListItem { Text = x.desc_plano, Value = x.idGrlplanos.ToString() })
                    .ToList();

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RelAtendimentoPeriodoporconnvenio(RelGeralViewlModel model)
        {

            byte[] _contentBytes;

            /*var query = _context.Vendas.Where(x => x.Cliente.Pessoa.EmpresaId == (GerenciaSession.UsarioLogado.Pessoa.EmpresaId)).AsQueryable();

            if (DataInicial.HasValue && DataFinal.HasValue)
                query = query.Where(a => a.DataVenda >= DataInicial && a.DataVenda <= DataFinal);

            if (!cidade.IsNullOrWhiteSpace())
                query = query.Where(a => a.Cliente.Pessoa.Cidade == cidade);
             */
            var query = _uoW.Orcamentos.ObterTodos().Where(x => x.Dt_orcamento >= model.inicio && x.Dt_orcamento <= model.fim && x.status.Equals("0")).AsQueryable();
            if (model.ConvenioId != 0)
                query = query.Where(x => x.id_grlconvenio == model.ConvenioId);
            var allCustomer = Mapper.Map<List<OrcamentoRelatorioViewModel>>(query.ToList()).OrderBy(x => x.Dt_orcamento).ThenBy(x => x.id_Stqcporcamento);

            /*var allCustomer =
                Mapper.Map<List<OrcamentoEditViewModel>>(
                    _orcamentoApp.GetAll()
                        .Where(x => x.Dt_orcamento >=model.inicio && x.Dt_orcamento<=model.fim && x.status.Equals("0"))
                        .ToList()
                        .OrderBy(x => x.id_Stqcporcamento)).ToList();*/
            var ds = new DataSet1();
            foreach (var item in allCustomer)
            {

                DataRow row2 = ds.Tables[0].NewRow();
                row2["Id_orcamento"] = item.id_Stqcporcamento;
                row2["dt_orcamento"] = item.Dt_orcamento;
                row2["Paciente"] = item.grlcliente.grlbasic.nome.ToUpper();
                row2["dentista"] = item.grldentista.Idgrlbasic.nome.ToUpper();

                var plano = _uoW.Planos.ObterPorId(Convert.ToInt32(item.id_grlconvenio));

                row2["endereco_pac"] = plano.desc_plano;
                row2["endereco_sol"] = item.Obs;

                decimal soma_itens = 0;
                foreach (var itens in item.itemorcamentos)
                {
                    if (itens.status.Equals("0"))
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
                        soma_itens = soma_itens + Convert.ToDecimal(itens.Valor_total);
                        ds.Tables[1].Rows.Add(row);
                    }
                }
                var teste = soma_itens;
                /* foreach (var titulos in item.titulos)
                 {
                     if (titulos.status.Equals("0"))
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
                 }*/
                ds.Tables[0].Rows.Add(row2);

            }
            //pega o nome do relatorio

            //var relatorio = _relatorioApp.GetById(2);
            ReportDocument rd = new ReportDocument();
            // rd.Load(Path.Combine(Server.MapPath("~/Reports"), relatorio.relatorio));

            string empresa = "Gtec - " + GerenciaSession.EmpresaLogado.descricao;
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Rel003 - Atendimentos por periodo Sintético.rpt"));
            rd.DataDefinition.FormulaFields["data_inicial"].Text = "'" + model.inicio.ToString().Substring(0, 10) + "'";
            rd.DataDefinition.FormulaFields["data_final"].Text = "'" + model.fim.ToString().Substring(0, 10) + "'";  //.Formulas(0) = "data_inicial  = '" & dtinicio & "'"
            rd.DataDefinition.FormulaFields["empresa"].Text = "'" + empresa + "'";
            if (model.ConvenioId == 0)
            {
                //viewer.SetParameters(new ReportParameter("cliente", "TODOS OS CLIENTES"));
                rd.DataDefinition.FormulaFields["Ordem"].Text = "'TODOS OS PLANOS'";
            }
            else
            {
                var convenio = _uoW.Planos.ObterPorId(model.ConvenioId);

                rd.DataDefinition.FormulaFields["Ordem"].Text = "'" + convenio.desc_plano + "'";
            }
            rd.SetDataSource(ds);
            //rd.SetDataSource(ds.Tables[1]);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            string filePath = Server.MapPath("~/temp/Rel003 - Contratos por periodo Sintético.pdf");

            rd.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            stream.Seek(0, SeekOrigin.Begin);
            stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            string nome_arquivo = tratanomedoarquivo(filePath);
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
    }
}
