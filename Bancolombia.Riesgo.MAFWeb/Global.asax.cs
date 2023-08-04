using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Bancolombia.Riesgo.MAFWeb.Clases.CalificacionCartera;
using System.IO;

namespace Bancolombia.Riesgo.MAFWeb
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //HttpContext con = HttpContext.Current;
            //if (con.Request.Url.ToString().Contains("CalificacionCartera"))
            //{
            //    Exception err = Server.GetLastError();
            //    LogRegister Err = new LogRegister();
            //    Err.ErrorLog(Server.MapPath("Logs/ErrorLog"), err.Message);
            //    Server.Transfer("~/errores/Errores.aspx?Master=Cartera");
            //}
        }

        protected void Session_End(object sender, EventArgs e)
        {
            if (Session["Reporte"] != null)
            {
                if (File.Exists(Session["Reporte"].ToString()))
                {
                    File.Delete(Session["Reporte"].ToString());
                }
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}