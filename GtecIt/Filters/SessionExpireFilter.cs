using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using GtecIt.Controllers;
using GtecIt.Util;

namespace GtecIt.Filters
{
	public class SessionExpireFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var ctx = HttpContext.Current;
            //if ((filterContext.Controller is LoginController) || (filterContext.Controller is LoginController && (filterContext.ActionDescriptor.ActionName == "RecuperarSenha" || filterContext.ActionDescriptor.ActionName == "AtualizaSenhaUsuario")))
		    if ((filterContext.Controller is LoginController) ||(filterContext.ActionDescriptor.ActionName == "RecuperarSenha" || filterContext.ActionDescriptor.ActionName == "AtualizaSenhaUsuario"))
		    {
		        return;
		    }

		    if (GerenciaSession.UsarioLogado == null)
		    {
			        
		        var timeout = false;
		        if (ctx.Request.IsAuthenticated)
		        {
		            FormsAuthentication.SignOut();
		            ctx.Session.Abandon();
		            timeout = true;
		        }

		        var redirectTargetDictionary = new RouteValueDictionary
		        {
		            {"action", "Index"},
		            {"controller", "Login"},
		            {"timeout", timeout}
		        };

		        filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
		    }
		    base.OnActionExecuting(filterContext);
		}
	}
}