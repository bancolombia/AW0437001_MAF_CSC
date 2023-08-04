using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Bancolombia.Riesgo.MAFWeb.Entidades.CalificacionCartera;
using Bancolombia.Riesgo.MAF.Entidades.CalificacionCartera;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Fachada = Bancolombia.Riesgo.MAF.Negocio.Fachadas.CalificacionCartera;

namespace Bancolombia.Riesgo.MAFWeb.CalificacionCartera.Reporte
{
    public partial class PDFMasivo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Rol"] != null)
            {
                if (Request.QueryString["cliente"] == null)
                {
                    ErroresEntity error = new ErroresEntity();
                    error.Error = "MEN.0391";
                    error.Url = "../CalificacionMasCartera.aspx";
                    error.Log = "No existe información de cliente";
                    Session["Error"] = error;
                    Response.Redirect("Errores/Error.aspx");
                    Response.End();
                }
                else
                {
                    try
                    {
                        string cliente = Request.QueryString["cliente"].ToString();
                        string rol = string.Empty;
                        //Proxy.CalificacionCartera proxycartera = new Proxy.CalificacionCartera();
                        ClienteMasivo cl = new ClienteMasivo();
                        if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
                        {
                            rol = "10";
                            cl = Fachada.CalificacionCartera.ConsultarClienteMasRiesgos(cliente, rol).clienteMasivo[0];
                        }
                        else if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC") || ((Usuario)Session["Rol"]).rol.Equals("Consulta") || ((Usuario)Session["Rol"]).rol.Equals("Superfinanciera"))
                        {
                            rol = "20";
                            cl = Fachada.CalificacionCartera.ConsultarClienteMasRiesgos(cliente, rol).clienteMasivo[0];
                        }

                        //COnsultar cliente en cartera
                        HTMLToPdf(HTMLRoles(cl).ToString());

                    }
                    catch (Exception ex)
                    {
                        ErroresEntity errEnt = new ErroresEntity();
                        errEnt.Url = Request.UrlReferrer.PathAndQuery;
                        errEnt.Log = ex.Message + "//ApplicationPage:CalificacionCliente";
                        errEnt.Error = "MEN.0453";
                        Session["Error"] = errEnt;
                        Response.Redirect("../Errores/Error.aspx");
                    }
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            }
        }



        /// <summary>
        /// genera el html del perfil comercial
        /// </summary>
        /// <param name="objCliente"></param>
        /// <returns></returns>
        protected StringBuilder HTMLRoles(ClienteMasivo objCliente)
        {
            var nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ",";
            nfi.NumberGroupSeparator = ".";
            nfi.CurrencyDecimalSeparator = ",";
            nfi.CurrencyGroupSeparator = ".";
            StringBuilder html = new StringBuilder();
            StringBuilder errores = new StringBuilder();

            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<title></title>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC") || ((Usuario)Session["Rol"]).rol.Equals("Consulta") || ((Usuario)Session["Rol"]).rol.Equals("Superfinanciera"))
            {
                html.AppendLine("<table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;border-collapse: collapse;\" border=\"1\" width=\"100%\"> ");
                html.AppendLine("                <tr> ");
                html.AppendLine("                    <td style=\"padding:10px;\"> ");
                html.AppendLine("                           <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;border-collapse:collapse;\" border=\"0\" width=\"100%\">");
                html.AppendLine("                               <tr> ");
                html.AppendLine("                                   <td style=\"font-size:7px; color:white; font-weight:bold; text-align:center; font-family:Verdana;\" colspan=\"6\" bgcolor=\"black\"> ");
                html.AppendLine("                                           COMENTARIOS CALIFICACION PROCESO MASIVO ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                               </tr> ");
                html.AppendLine("                               <tr> ");
                html.AppendLine("                                   <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\">  ");
                html.AppendLine("                                           Identificación Cliente:<b> </b> ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                                   <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
                html.AppendLine("                                           " + objCliente.idCliente + " ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                                   <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\">  ");
                html.AppendLine("                                            Nombre:<b> </b> ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                                   <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
                html.AppendLine("                                           " + objCliente.nombreCliete + " ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                                   <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
                html.AppendLine("                                           Usuario Comercial:<b> </b> ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                                   <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
                html.AppendLine("                                           " + objCliente.usrComercial + " ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                               </tr> ");
                html.AppendLine("                               <tr> ");
                html.AppendLine("                                   <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\">  ");
                html.AppendLine("                                           Entidad:<b> </b> ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                                   <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
                html.AppendLine("                                           " + objCliente.entidad + " ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                                   <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\">  ");
                html.AppendLine("                                           Banca:<b> </b> ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                                   <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
                html.AppendLine("                                           " + objCliente.banca + " ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                                   <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
                html.AppendLine("                                           Usuario Riesgos:<b> </b> ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                                   <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
                html.AppendLine("                                           " + objCliente.usrRiesgos + " ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                               </tr> ");
                html.AppendLine("                           </table>");
                html.AppendLine("                    </td> ");
                html.AppendLine("                </tr> ");
                html.AppendLine("</table>");
            }
            else if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
            {
                html.AppendLine("<table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;border-collapse: collapse;\" border=\"1\" width=\"100%\"> ");
                html.AppendLine("                <tr> ");
                html.AppendLine("                    <td style=\"padding:10px;\"> ");
                html.AppendLine("                           <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;border-collapse:collapse;\" border=\"0\" width=\"100%\">");
                html.AppendLine("                               <tr> ");
                html.AppendLine("                                   <td style=\"font-size:7px; color:white; font-weight:bold; text-align:center; font-family:Verdana;\" colspan=\"4\" bgcolor=\"black\"> ");
                html.AppendLine("                                           COMENTARIOS CALIFICACION PROCESO MASIVO ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                               </tr> ");
                html.AppendLine("                               <tr> ");
                html.AppendLine("                                   <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\">  ");
                html.AppendLine("                                           Identificación Cliente:<b> </b> ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                                   <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
                html.AppendLine("                                           " + objCliente.idCliente + " ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                                   <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\">  ");
                html.AppendLine("                                            Nombre:<b> </b> ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                                   <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
                html.AppendLine("                                           " + objCliente.nombreCliete + " ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                               </tr> ");
                html.AppendLine("                               <tr> ");
                html.AppendLine("                                   <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\">  ");
                html.AppendLine("                                           Entidad:<b> </b> ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                                   <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
                html.AppendLine("                                           " + objCliente.entidad + " ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                                   <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\">  ");
                html.AppendLine("                                           Banca:<b> </b> ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                                   <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
                html.AppendLine("                                           " + objCliente.banca + " ");
                html.AppendLine("                                   </td> ");
                html.AppendLine("                               </tr> ");
                html.AppendLine("                           </table>");
                html.AppendLine("                    </td> ");
                html.AppendLine("                </tr> ");
                html.AppendLine("</table>");
            }

            if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC") || ((Usuario)Session["Rol"]).rol.Equals("Consulta") || ((Usuario)Session["Rol"]).rol.Equals("Superfinanciera"))
            {
                html.AppendLine("<table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;border-collapse: collapse;\" border=\"1\" width=\"100%\"> ");
                html.AppendLine("   <tr> ");
                html.AppendLine("       <td>");
                html.AppendLine("           <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;border-collapse:collapse;\" border=\"0\" width=\"100%\">");

                html.AppendLine("                <tr> ");
                html.AppendLine("                    <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\">  ");
                html.AppendLine("                        &nbsp; ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       Calificacion Actual");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       Calificacion Recomendada Riesgo");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       Calificacion Recomendada Comercial");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       Calificacion Ratificada");
                html.AppendLine("                   </td> ");
                html.AppendLine("               </tr> ");

                html.AppendLine("               <tr> ");
                html.AppendLine("                    <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\">  ");
                html.AppendLine("                        Calificacion Interna <b> </b> ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       " + objCliente.calIntAct + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       " + objCliente.calInRecR + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       " + objCliente.calInRecC + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       " + objCliente.calInRatR + " ");
                html.AppendLine("                   </td> ");
                html.AppendLine("               </tr> ");

                html.AppendLine("               <tr> ");
                html.AppendLine("                    <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\">  ");
                html.AppendLine("                        Calificacion Externa <b> </b> ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       " + objCliente.calExAct + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       " + objCliente.calExRecR + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       " + objCliente.calExRecC + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       " + objCliente.calExRatR + " ");
                html.AppendLine("                   </td> ");
                html.AppendLine("               </tr> ");

                html.AppendLine("               <tr> ");
                html.AppendLine("                    <td style=\"font-size:5px; color:black; text-align:left; font-family:Verdana\" colspan=\"5\">  ");
                html.AppendLine("                        Justificacion Calificacion Recomenda Riesgos");
                html.AppendLine("                    </td> ");
                html.AppendLine("               </tr> ");

                html.AppendLine("               <tr style=\"border-top:1px solid black;\"> ");
                html.AppendLine("                    <td style=\"font-size:5px; color:black; text-align:center; font-family:Verdana;\" colspan=\"5\" border=\"1\" >  ");
                html.AppendLine("                      " + objCliente.custRatRie + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("               </tr> ");

                html.AppendLine("               <tr> ");
                html.AppendLine("                    <td style=\"font-size:5px; color:black; text-align:left; font-family:Verdana\" colspan=\"5\">  ");
                html.AppendLine("                        Justificacion Calificacion Comercial");
                html.AppendLine("                    </td> ");
                html.AppendLine("               </tr> ");

                html.AppendLine("               <tr style=\"border-top:1px solid black;\"> ");
                html.AppendLine("                    <td style=\"font-size:5px; color:black; text-align:center; font-family:Verdana;\" colspan=\"5\" border=\"1\">  ");
                html.AppendLine("                      " + objCliente.justCalCom + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("               </tr> ");

            }
            else if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
            {
                html.AppendLine("<table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;border-collapse: collapse;\" border=\"1\" width=\"100%\"> ");
                html.AppendLine("   <tr> ");
                html.AppendLine("       <td>");
                html.AppendLine("           <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;border-collapse:collapse;\" border=\"0\" width=\"100%\">");

                html.AppendLine("                <tr> ");
                html.AppendLine("                    <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\">  ");
                html.AppendLine("                        &nbsp; ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       Calificacion Actual");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       Calificacion Recomendada Riesgo");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       Calificacion Recomendada Comercial");
                html.AppendLine("                    </td> ");
                html.AppendLine("               </tr> ");

                html.AppendLine("               <tr> ");
                html.AppendLine("                    <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\">  ");
                html.AppendLine("                        Calificacion Interna <b> </b> ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       " + objCliente.calIntAct + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       " + objCliente.calInRecR + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       " + objCliente.calInRecC + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("               </tr> ");

                html.AppendLine("               <tr> ");
                html.AppendLine("                    <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\">  ");
                html.AppendLine("                        Calificacion Externa <b> </b> ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       " + objCliente.calExAct + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       " + objCliente.calExRecR + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td class=\"lblSubTitInfo\" style=\"font-weight: bold; text-align: center;\"> ");
                html.AppendLine("                       " + objCliente.calExRecC + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("               </tr> ");

                html.AppendLine("               <tr> ");
                html.AppendLine("                    <td style=\"font-size:5px; color:black; text-align:left; font-family:Verdana\" colspan=\"5\">  ");
                html.AppendLine("                        Justificacion Calificacion Recomenda Riesgos");
                html.AppendLine("                    </td> ");
                html.AppendLine("               </tr> ");

                html.AppendLine("               <tr style=\"border-top:1px solid black;\"> ");
                html.AppendLine("                    <td style=\"font-size:5px; color:black; text-align:center; font-family:Verdana;\" colspan=\"5\" border=\"1\" >  ");
                html.AppendLine("                      " + objCliente.custRatRie + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("               </tr> ");

                html.AppendLine("               <tr> ");
                html.AppendLine("                    <td style=\"font-size:5px; color:black; text-align:left; font-family:Verdana\" colspan=\"5\">  ");
                html.AppendLine("                        Justificacion Calificacion Comercial");
                html.AppendLine("                    </td> ");
                html.AppendLine("               </tr> ");

                html.AppendLine("               <tr style=\"border-top:1px solid black;\"> ");
                html.AppendLine("                    <td style=\"font-size:5px; color:black; text-align:center; font-family:Verdana;\" colspan=\"5\" border=\"1\">  ");
                html.AppendLine("                      " + objCliente.justCalCom + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("               </tr> ");


            }

            if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC") || ((Usuario)Session["Rol"]).rol.Equals("Consulta") || ((Usuario)Session["Rol"]).rol.Equals("Superfinanciera"))
            {

                html.AppendLine("               <tr> ");
                html.AppendLine("                    <td style=\"font-size:5px; color:black; text-align:left; font-family:Verdana\" colspan=\"5\">  ");
                html.AppendLine("                        Justificacion Calificacion Ratificada");
                html.AppendLine("                    </td> ");
                html.AppendLine("               </tr> ");

                html.AppendLine("               <tr style=\"border-top:1px solid black;\"> ");
                html.AppendLine("                    <td style=\"font-size:5px; color:black; text-align:center; font-family:Verdana;\" colspan=\"5\" border=\"1\">  ");
                html.AppendLine("                      " + objCliente.justRatR + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("               </tr> ");
            }
            html.AppendLine("           </table>");
            html.AppendLine("       </td> ");
            html.AppendLine("   </tr> ");
            html.AppendLine("</table>");









            if (errores.ToString().Length > 0)
            {
                html.AppendLine("<table border=\"0\" width=\"100%\">");
                html.AppendLine("<tr>");
                html.AppendLine("<td style=\"font-family: Verdana;font-size: 9px;color:black;text-align: left;height: 27px;vertical-align: middle;font-weight:bold\">");
                html.AppendLine("<label>" + errores.ToString() + "</label>");
                html.AppendLine("</td>");
                html.AppendLine("</tr>");
                html.AppendLine("</table>");
            }
            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return html;
        }


        protected void HTMLToPdf(string HTML)
        {
            HttpResponse Response = HttpContext.Current.Response;
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=Documento.pdf");
            Document document = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 40);
            PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();
            iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
            iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);

            foreach (IElement E in iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTML), styles))
                document.Add(E);

            document.Close();
            //ShowPdf(file);
        }

        /// <summary>
        /// muestra el pdf en pantalla
        /// </summary>
        /// <param name="s"></param>
        protected void ShowPdf(string s)
        {
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "inline;filename=" + s);
            Response.ContentType = "application/pdf";
            Response.WriteFile(s);
            Response.Flush();
            Response.Clear();
        }





    }
}
