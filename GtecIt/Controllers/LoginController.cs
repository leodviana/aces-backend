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
    public class LoginController : Controller
    {
        
         readonly IUnitOfWork _uoW;

        
        
        public LoginController(IUnitOfWork uoW)
        {
            _uoW = uoW;
            
        }
        public ActionResult Index(bool? timeout)
        {

            //Para incluir             
           // _uoW.Usuarios.Salvar(new Usuario() { Administrador = "S", Ativo = "S", Id_grlbasico = 6, Login = "Usuário", Senha = "teste" });
           // _uoW.Complete();

            //Para alterar
           // var usuario = _uoW.Usuarios.ObterPorId(5);
           // if (usuario != null)
           //     usuario.Login = "Usuário Editado";
          //  _uoW.Complete();

            //GerenciaSession.UsarioLogado = Mapper.Map<UsuarioLogadoViewModel>(_context.Usuarios.FirstOrDefault(x => x.Login == "admin"));
            //return RedirectToAction("Index", "Home");

            GerenciaSession.EmpresaLogado =
                       Mapper.Map<EmpresaLogadoViewModel>(_uoW.Empresas.ObterTodos().FirstOrDefault(x => x.Status.Equals("0")));
            
            
            ViewBag.Success = string.Empty;
            ViewBag.altura = GerenciaSession.EmpresaLogado.altura_logo;//"220px;";
            ViewBag.logo = "~/Images/" + GerenciaSession.EmpresaLogado.logo;

            ViewBag.comprimento = GerenciaSession.EmpresaLogado.comprimentro_logo;//"180px;";
            if (TempData["Mensagem"] != null)
            {
                ViewBag.Success = TempData["Mensagem"];
            }

            if (timeout != null && timeout.Value == true)
            {
                ViewBag.Error = "Sua sessão expirou.";
            }
            else if (timeout != null && timeout.Value == false)
            {
                ViewBag.Error = "Efetue o login para acessar o sistema.";
            }

            //GerenciaSession.UsarioLogado = Mapper.Map<UsuarioLogadoViewModel>(usuarioLogin);
            //GerenciaSession.UsarioLogado = Mapper.Map<UsuarioLogadoViewModel>(_context.Usuarios.FirstOrDefault(x => x.Login == "admin"));
            
            
            return View();
        }

        public ActionResult Create(UsuarioCreateViewModel model)
        {
            //var model = new DentistaCreateViewModel();

            if (model.Id_grlbasico != null && model.Id_grlbasico > 0)
            {

                var pessoa = _uoW.Pessoas.ObterTodos().FirstOrDefault(x => x.Id_grlbasico == model.Id_grlbasico);
                //var pessoa = _pessoaApp.GetAll().FirstOrDefault(x => x.clientes.Any(y => y.id_Grlcliente == model.id_grlcliente));
                //model.CodigoCliente = cliente.id_Grlcliente.ToString();
                model.nome = pessoa.nome;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioCreateViewModel model, FormCollection form)
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

            var model2 = Mapper.Map<UsuarioEditViewModel>(_uoW.Usuarios.ObterTodos().FirstOrDefault(x => x.Id_grlbasico == Convert.ToInt32(model.Id_grlbasico)));

            if (model2 == null)
            {
                var hash = GetMd5Hash(MD5.Create(), model.Senha);
                model.Senha = hash;
                _uoW.Usuarios.Salvar(Mapper.Map<Usuario>(model));
                _uoW.Complete();
                return RedirectToAction("Edit", new { codigo = _uoW.Usuarios.ObterTodos().LastOrDefault().UsuarioId });
            }
            else
            {
                model2.Ativo = model.Ativo;
                _uoW.Usuarios.Atualizar(Mapper.Map<Usuario>(model2));
                _uoW.Complete();
                return RedirectToAction("Edit", new { codigo = model2.UsuarioId });
            }


            // return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioEditViewModel model)
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
            var hash = GetMd5Hash(MD5.Create(), model.Senha);
            model.Senha = hash;
            _uoW.Usuarios.Atualizar(Mapper.Map<Usuario>(model));
            _uoW.Complete();
            
            return RedirectToAction("Index2");

        }
        public ActionResult Edit(int codigo)
        {
            var model = Mapper.Map<UsuarioEditViewModel>(_uoW.Usuarios.ObterPorId(codigo));

            if (model == null)
                return HttpNotFound();

           
            return View(model);
        }
        public bool VerificarFiltroVazio(UsuarioIndexViewModel model)
        {
            model = model ?? new UsuarioIndexViewModel();

            var ehVazio = model.UsuarioId.Equals(0);

            return ehVazio;

        }
        public ActionResult Index2(UsuarioIndexViewModel model, string tipoacao)
        {
            if (tipoacao != null)
                model.ConsultaTodos = true;

            if (VerificarFiltroVazio(model))
            {
                if (!model.ConsultaTodos)
                    return View(model);
                try
                {
                   // var teste = _usuarioApp.GetAll().ToList().OrderBy(x => x.pessoas.nome);
                    model.Grid =Mapper.Map<List<UsuarioGridViewModel>>(_uoW.Usuarios.ObterTodos().ToList().OrderBy(x => x.pessoas.nome));
                    model.ConsultaTodos = true;
                    
                }
                catch (Exception ex)
                {

                }
                return View(model);
            }

            model.ConsultaTodos = false;
            //model.Grid = Mapper.Map<List<UsuarioGridViewModel>>(_usuarioApp.GetAll().Where(x => x.UsuarioId==model.UsuarioId).OrderBy(x => x.UsuarioId)).ToList();
            model.Grid = Mapper.Map<List<UsuarioGridViewModel>>(_uoW.Usuarios.ObterTodos().Where(x => x.pessoas.nome.Contains(model.Nome)).ToList().OrderBy(x => x.pessoas.nome));
            return View(model);

        }
        [HttpPost]
        public ActionResult Index(string cnpj, string senha)
        {
            try
            {
                ViewBag.altura = GerenciaSession.EmpresaLogado.altura_logo;//"220px;";
                ViewBag.logo = "~/Images/" + GerenciaSession.EmpresaLogado.logo;

                ViewBag.comprimento = GerenciaSession.EmpresaLogado.comprimentro_logo;//"180px;";
             //   var usuarioLogin = _usuarioApp.geFirstOrDefault(x => x.Login == cnpj);
                var usuarioLogin = _uoW.Usuarios.ObterTodos().FirstOrDefault(x => x.Login == cnpj);
                if (usuarioLogin == null)
                {
                    ViewBag.Error = "Login não cadastrado.";
                    return View();
                }

                if (usuarioLogin.Ativo.ToUpper() != "S")
                {
                    ViewBag.Error = "Login inativo.";
                    return View();
                }

                var hash = GetMd5Hash(MD5.Create(), senha);

                //if (!_context.Usuarios.Any(x => x.Login == cnpj && x.Senha == hash))
                if (!_uoW.Usuarios.ObterTodos().Any(x => x.Login == cnpj && x.Senha == hash))
                {
                    ViewBag.Error = "Senha inválida.";
                    return View();
                }
                try
                {
                    GerenciaSession.EmpresaLogado =
                        Mapper.Map<EmpresaLogadoViewModel>(_uoW.Empresas.ObterTodos().FirstOrDefault(x => x.Status.Equals("0")));

                    GerenciaSession.UsarioLogado = Mapper.Map<UsuarioLogadoViewModel>(usuarioLogin);
                }
                catch (Exception ex)
                {

                }

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index", "Home");
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



        public ActionResult Createusuario()
        {
            return RedirectToAction("Create", "Pessoa", new PessoaCreateViewModel { Usuario = true });
        }
        public ActionResult Logout()
        {
            //FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AtualizaSenhaUsuario()
        {
            var model = new UsuarioTrocaSenhaViewModel();

            if (TempData["AtualizaSenhaUsuario"] != null)
            {
                model.Login = TempData["AtualizaSenhaUsuario"].ToString();
                model.Message = "Sua senha expirou. Por favor, defina uma nova senha.";
                model.MessageClass = "danger";
                return View(model);
            }


            return View(model);
        }

        [HttpPost]
        public ActionResult AtualizaSenhaUsuario(UsuarioTrocaSenhaViewModel model)
        {
           /* try
            {
                var usuarioLogin = _context.Usuarios.FirstOrDefault(x => x.Login == model.Login);

                if (usuarioLogin == null)
                {
                    ViewBag.Error = "Login não cadastrado.";
                    return View();
                }

                if (usuarioLogin.Ativo.ToUpper() != "S")
                {
                    ViewBag.Error = "Login inativo.";
                    return View();
                }

                var senhaAtualDigitadaCript = GetMd5Hash(MD5.Create(), model.Senha);

                if (!_context.Usuarios.Any(x => x.Login == model.Login && x.Senha == senhaAtualDigitadaCript))
                {
                    ViewBag.Error = "Senha atual não confere.";
                    return View();
                }

                if (model.NovaSenha != model.NovaSenhaConfirmar)
                {
                    model.Message = "Erro: Nova senha e confirmação de nova senha não conferem.";
                    model.MessageClass = "danger";

                    return View("AtualizaSenhaUsuario", model);
                }

                var senhaNovaDigitadaCript = GetMd5Hash(MD5.Create(), model.NovaSenha);

                usuarioLogin.Senha = senhaNovaDigitadaCript;

                _context.Entry(usuarioLogin).State = EntityState.Modified;
                _context.SaveChanges();

                TempData["Mensagem"] = "Senha atualizada com sucesso.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                model.Message = "Erro: " + ex.Message;
                model.MessageClass = "danger";

                return View("TrocaSenhaUsuario", model);
            }*/
            return View("TrocaSenhaUsuario", model);
        }

        [HttpGet]
        public ActionResult TrocaSenhaUsuario()
        {
            return View(new UsuarioTrocaSenhaViewModel());
        }

        [HttpPost]
        public ActionResult TrocaSenhaUsuario(UsuarioTrocaSenhaViewModel model)
        {
            /*try
            {
                var usuarioLogin = _context.Usuarios.FirstOrDefault(x => x.Login == model.Login);

                if (usuarioLogin == null)
                {
                    model.Message = "Login não cadastrado.";
                    model.MessageClass = "danger";
                    return View(model);
                }

                if (usuarioLogin.Ativo.ToUpper() != "S")
                {
                    model.Message = "Login inativo.";
                    model.MessageClass = "danger";
                    return View(model);
                }

                var senhaAtualDigitadaCript = GetMd5Hash(MD5.Create(), model.Senha);

                if (!_context.Usuarios.Any(x => x.Login == model.Login && x.Senha == senhaAtualDigitadaCript))
                {
                    model.Message = "Senha atual não confere.";
                    model.MessageClass = "danger";
                    return View(model);
                }

                if (model.NovaSenha != model.NovaSenhaConfirmar)
                {
                    model.Message = "Nova senha e confirmação de nova senha não conferem.";
                    model.MessageClass = "danger";
                    return View(model);
                }

                var senhaNovaDigitadaCript = GetMd5Hash(MD5.Create(), model.NovaSenha);

                usuarioLogin.Senha = senhaNovaDigitadaCript;

                _context.Entry(usuarioLogin).State = EntityState.Modified;
                _context.SaveChanges();

                model.Message = "Nova senha definida com sucesso. Use-a a partir do próximo login";
                model.MessageClass = "success";
                return View(model);

                //TempData["Mensagem"] = "Senha atualizada com sucesso.";
                //return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                model.Message = "Erro: " + ex.Message;
                model.MessageClass = "danger";

               
            }*/
            return View("TrocaSenhaUsuario", model);
        }

        [HttpPost]
        public ActionResult EsqueciSenha(string login)
        {
            //var retorno = new ServicoClient().ResetarSenha(login, "Web Seguro");

            //var alterado = Convert.ToBoolean(XElement.Parse(retorno).Elements("retorno").Select(x => x.Element("alterado").Value).FirstOrDefault());
            //var msg = XElement.Parse(retorno).Elements("retorno").Select(x => x.Element("msgretorno").Value).FirstOrDefault();

            //var response = new { Alterado = alterado, Mensagem = msg };

            //return Json(response);
            throw new Exception();
        }
    }
}