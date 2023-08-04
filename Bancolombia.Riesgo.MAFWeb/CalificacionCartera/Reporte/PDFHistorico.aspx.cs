using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using Bancolombia.Riesgo.MAF.Entidades.CalificacionCartera;
using Bancolombia.Riesgo.MAFWeb.Entidades.CalificacionCartera;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Globalization;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Fachada = Bancolombia.Riesgo.MAF.Negocio.Fachadas.CalificacionCartera;

namespace Bancolombia.Riesgo.MAFWeb.CalificacionCartera.Reporte
{
    public partial class PDFHistorico : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Rol"] != null)
            {
                if (Request.QueryString["cliente"] == null)
                {
                    ErroresEntity error = new ErroresEntity();
                    error.Error = "MEN.0391";
                    error.Url = "../CalificacionCliente.aspx";
                    error.Log = "No existe información de cliente";
                    Session["Error"] = error;
                    Response.Redirect("Errores/Error.aspx");
                    Response.End();
                }
                else
                {
                    try
                    {
                        if (Request.QueryString["Prdo"] == null)
                        {
                            string cliente = Request.QueryString["cliente"].ToString();
                            string categoriaCliente = Request.QueryString["categoriaCliente"]?.ToString();
                            string rol = string.Empty;
                            //Proxy.CalificacionCartera proxycartera = new Proxy.CalificacionCartera();
                            Cliente cl;
                            if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
                            {
                                rol = "10";
                                cl = Fachada.CalificacionCartera.ConsultarCliente(rol, cliente).objCliente;
                            }
                            else if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC"))
                            {
                                rol = "20";
                                cl = Fachada.CalificacionCartera.ConsultarCliente(rol, cliente).objCliente;
                            }
                            else
                            {
                                rol = "30";
                                cl = Fachada.CalificacionCartera.ConsultarCliente(rol, cliente).objCliente;
                            }

                            //Consultar cliente en cartera.
                            HTMLToPdf(HTMLRoles(cl, categoriaCliente).ToString());
                        }
                        else
                        {

                            string cliente = Request.QueryString["cliente"].ToString();
                            string prdo = Request.QueryString["Prdo"];
                            string categoriaCliente = Request.QueryString["categoriaCliente"]?.ToString();

                            //Proxy.CalificacionCartera proxycartera = new Proxy.CalificacionCartera();
                            List<string> prdos = Fachada.CalificacionCartera.ConsultarPrdo();
                            if (!prdos.Contains(prdo))
                            {
                                ErroresEntity error = new ErroresEntity();
                                error.Error = "MEN.0454";
                                error.Url = Request.UrlReferrer.PathAndQuery;
                                error.Log = "Ingreso a sesión con usuario no permitido";
                                Session["Error"] = error;
                                Response.Redirect("Errores/Error.aspx");
                                Response.End();
                            }

                            Cliente cl;

                            cl = Fachada.CalificacionCartera.ConsultarNITPrdo(cliente, prdo).objCliente;

                            HTMLToPdf(HTMLRolesHis(cl, categoriaCliente).ToString());
                        }
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

            foreach (IElement E in iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTML), styles))
                document.Add(E);

            document.Close();
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

        /// <summary>
        /// genera el html del perfil comercial
        /// </summary>
        /// <param name="objCliente"></param>
        /// <returns></returns>
        protected StringBuilder HTMLRoles(Cliente objCliente, string categoriaCliente)
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
            html.AppendLine("<table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" border=\"0\" width=\"100%\"> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\"> ");
            html.AppendLine("                        ROL: ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
            html.AppendLine("                        " + ((Usuario)Session["Rol"]).rol + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: right;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:justify;vertical-align:middle;\"> ");
            html.AppendLine("                        &nbsp; ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\" colspan=\"5\"> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\"> ");
            html.AppendLine("                        USUARIO: ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
            html.AppendLine("                        " + ((Usuario)Session["Rol"]).usuario + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: right;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Comité de:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:justify;vertical-align:middle;\"> ");
            html.AppendLine("                        " + objCliente.fecProc.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            // PMO19939 - RF004: Incluir el campo “Usuario Riesgos” en los PDF del Comité Presencial y Masivo
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\"> ");
            html.AppendLine("                        USUARIO RESPONSABLE: ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
            html.AppendLine("                        " + objCliente.usuarioRiesgo + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                       </td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            //
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\" colspan=\"5\"> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\" colspan=\"5\"> ");
            html.AppendLine("                        DATOS DEL CLIENTE</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td colspan=\"5\"> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Nit:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                        " + objCliente.nit.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                    &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Nombre:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                        " + objCliente.nombre.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Tipo de Comité:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                        " + objCliente.tipCom.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td colspan=\"5\"> ");
            html.AppendLine("                       <table border=\"0.5\" width=\"100%\"><tr><td>&nbsp;</td></tr></table>");
            html.AppendLine("                        </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("           </table>");
            html.AppendLine("           <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" border=\"0\" width=\"100%\"> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td  ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\" colspan=\"4\"> ");
            html.AppendLine("                        INFORMACIÓN DEL CLIENTE</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td colspan=\"4\"> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Sector económico:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                        " + objCliente.sectEcon.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Seguimiento recomendado en el comité anterior:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" > ");
            html.AppendLine("                        " + objCliente.segRecComAnt.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Segmento:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                        " + objCliente.segmento.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Grupo de Riesgo:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                        " + objCliente.grupoRiesgo.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Regional:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                        " + objCliente.regional.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Nivel de Riesgo AEC:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" > ");
            html.AppendLine("                        " + objCliente.aec.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            if (((Usuario)Session["Rol"]).rol.Equals("Superfinanciera") || ((Usuario)Session["Rol"]).rol.Equals("Consulta"))
            {
                html.AppendLine("                <tr> ");
                html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                html.AppendLine("                        &nbsp; ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                html.AppendLine("                        &nbsp; ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td> ");
                html.AppendLine("                        &nbsp;</td> ");
                html.AppendLine("                    <td > ");
                html.AppendLine("                        &nbsp;</td> ");
                html.AppendLine("                </tr> ");
            }
            else
            {
                html.AppendLine("                <tr> ");
                html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                html.AppendLine("                        Calificado por: ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                html.AppendLine("                        " + objCliente.calPor.Trim() + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td> ");
                html.AppendLine("                        &nbsp;</td> ");
                html.AppendLine("                    <td > ");
                html.AppendLine("                        &nbsp;</td> ");
                html.AppendLine("                </tr> ");
            }
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td  colspan=\"5\"> ");
            html.AppendLine("                       <table border=\"0.5\" width=\"100%\"><tr><td>&nbsp;</td></tr></table>");
            html.AppendLine("                        </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td  > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\" colspan=\"5\"> ");
            html.AppendLine("                        INFORMACIÓN DE ENDEUDAMIENTO</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td  > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("            </table> ");

            //Verifica qué información mostrar según el rol
            informacionEndeudamiento(((Usuario)Session["Rol"]).rol, ref html, objCliente);


            html.AppendLine("    <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" width=\"40%\"> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Anticipos de leasing:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                        " + objCliente.leasingAnticipos.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp; ");
            html.AppendLine("                        </td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp; ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("            </table> ");
            html.AppendLine("<table border=\"0.5\" style=\"border-color:black; border-collapse:collapse\" width=\"100%\"> ");
            html.AppendLine("        <tr> ");
            html.AppendLine("            <td style=\"border-color:black\"> ");
            html.AppendLine("                &nbsp;</td> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle\"> ");
            html.AppendLine("                Saldo Capital</td> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle\"> ");
            html.AppendLine("                Días de Mora</td> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle\"> ");
            html.AppendLine("                Reestructurado</td> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle\"> ");
            html.AppendLine("                Calif. Externa Modelo SFC</td> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle\"> ");
            html.AppendLine("                Calif. Externa Actual</td> ");
            html.AppendLine("        </tr> ");
            html.AppendLine("        <tr> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                Bancolombia</td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                <label>" + objCliente.salgoKBanco.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label> ");
            html.AppendLine("                " + objCliente.diasMoraBanco.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label>" + objCliente.reestrucBanco.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label>" + objCliente.calExternaModeloBanco.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label> ");
            html.AppendLine("                " + objCliente.calEBanco.Trim() + "</label></td> ");
            html.AppendLine("        </tr> ");
            html.AppendLine("        <tr> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                Factoring</td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                <label >" + objCliente.saldoKFactoring.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.diasMoraFactoring.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.reestrucFactoring.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.calExternaModeloFactoring.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.calEFactoring.Trim() + "</label></td> ");
            html.AppendLine("        </tr> ");
            html.AppendLine("        <tr> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                Sufi</td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.salgoKSufi.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.diasMoraSufi.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.reestrucSufi.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.calExternaModeloSufi.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.calESufi.Trim() + "</label></td> ");
            html.AppendLine("        </tr> ");
            html.AppendLine("        <tr> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"  ");
            html.AppendLine("                class=\"style1\"> ");
            html.AppendLine("                Leasing</td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.saldoKLeasing.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label style=\"font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                " + objCliente.diasMoraLeasing.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label style=\"font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                " + objCliente.reestructLeasing.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label style=\"font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                " + objCliente.calExternaModeloLeasing.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label style=\"font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                " + objCliente.calELeasing.Trim() + "</label></td> ");
            html.AppendLine("        </tr> ");
            html.AppendLine("    </table> ");
            html.AppendLine("    <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" width=\"50%\"> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                    EXTERIOR ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("            </table> ");
            html.AppendLine("<table border=\"0.5\" style=\"border-color:black; border-collapse:collapse\" width=\"100%\"> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                    &nbsp;</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle;border:solid 1px black\"> ");
            html.AppendLine("                    Saldo K</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle;border:solid 1px black\"> ");
            html.AppendLine("                    Saldo I</td> ");
            // PMO19939 - RF061: Modificar visualización “Consulta Proceso Anteriores” del comité presencial
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle;border:solid 1px black\"> ");
            html.AppendLine("                    Días de Mora</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle;border:solid 1px black\"> ");
            html.AppendLine("                    Reestructurado</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle;border:solid 1px black\"> ");
            html.AppendLine("                    Calif. Externa Actual</td> ");
            //
            html.AppendLine("                     ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                    Panamá</td> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label  >" + objCliente.saldoKPanama.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        " + objCliente.saldoIPanama.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            // PMO19939 - RF061: Modificar visualización “Consulta Proceso Anteriores” del comité presencial
            html.AppendLine("                    <td style=\"border-color:black;text-align:center;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label  >" + objCliente.diasMoraPanama.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:center;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label  >" + objCliente.reestructuradoPanama.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:center;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label  >" + objCliente.calExternaActualPanama.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            //
            html.AppendLine("                     ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                    Puerto Rico</td> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label >" + objCliente.saldoKPuertoRico.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label >" + objCliente.saldoIPuertoRico.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            // PMO19939 - RF061: Modificar visualización “Consulta Proceso Anteriores” del comité presencial
            html.AppendLine("                    <td style=\"border-color:black;text-align:center;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label >" + objCliente.diasMoraPuertoRico.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:center;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label >" + objCliente.reestructuradoPuertoRico.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:center;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label >" + objCliente.calExternaActualPuertoRico.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            //
            html.AppendLine("                     ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                 ");
            html.AppendLine("            </table> ");
            html.AppendLine("    &nbsp;<br /> ");
            html.AppendLine("   <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;font-weight:bold\">");
            html.AppendLine("      <tr><td><label>Nota: Cifras en Pesos Colombianos</label></td></tr>");
            html.AppendLine("           </table>");

            ObtenerInformacionFinanciera(((Usuario)Session["Rol"]).rol, objCliente, ref html, ref errores);

            CalificacionesComentarios(((Usuario)Session["Rol"]).rol, ref html, objCliente, categoriaCliente);

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

        /// <summary>
        /// obtiene las calificaciones y comentario segun el rol
        /// </summary>
        /// <param name="rol"></param>
        /// <param name="html"></param>
        /// <param name="errores"></param>
        /// <param name="objCliente"></param>
        private void CalificacionesComentarios(string rol, ref StringBuilder html, Cliente objCliente, string categoriaCliente)
        {

            string actividadCliente = "";
            string analisisPyG = "";
            string analisisBalance = "";
            string comentariosAdicionales = "";
            string comentariosRiesgos = "";
            //PMO 27494 adicionar los valores para las preguntas y decisión por periodo
            string prdoNvo = Request.QueryString["Prdo"];
            string SustentacionCalRecCom = "";
            string RespListaPregunta1 = "";
            string RespPregunta1 = "";

            string RespListaPregunta2 = "";
            string RespPregunta2 = "";

            string RespListaPregunta3 = "";
            string RespPregunta3 = "";

            string RespListaPregunta4 = "";
            string RespPregunta4 = "";

            string RespListaPregunta5 = "";
            string RespPregunta5 = "";

            string RespListaPregunta6 = "";
            string RespPregunta6 = "";

            string RespListaPregunta7 = "";
            string RespPregunta7 = "";

            string RespListaPregunta8 = "";
            string RespPregunta8 = "";

            string RespListaPregunta9 = "";
            string RespPregunta9 = "";

            string RespListaPregunta10 = "";
            string RespPregunta10 = "";

            string RespListaPregunta11 = "";
            string RespPregunta11 = "";

            string RespListaPregunta12 = "";
            string RespPregunta12 = "";

            string RespListaPregunta13 = "";
            string RespPregunta13 = "";

            string RespListaPregunta14 = "";
            string RespPregunta14 = "";

            string RespPregunta15 = "";

            string RespPregunta16 = "";

            string RespPregunta17 = "";

            string RespPregunta18 = "";

            string RespPregunta19 = "";

            string RespPregunta20 = "";

            string RespPregunta21 = "";

            string RespPregunta22 = "";

            #region "Comercial"


            if (rol.Equals("Comercial") && Request.QueryString["Prdo"] == null)
            {
                if (Convert.ToInt32(prdoNvo) >= 201812)
                {
                    html.AppendLine("    <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" border=\"0\" width=\"100%\"> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td colspan=\"2\" style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                CALIFICACIÓN</td> ");
                    html.AppendLine("            <td colspan=\"2\" style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                CALIFICACIONES A INGRESAR</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación Interna Actual:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.CalIntAct.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación Interna Ratificada: ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + (objCliente.calIntRec.Trim().Equals("0") ? "" : objCliente.calIntRec.Trim()) + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Fecha Califi. Interna Actual:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.fecCalInt.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Seguimiento de Covenants:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.segCovenants.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                El cliente se encuentra en lista de control:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.ListasDeControl.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("    </table> ");
                }
                else
                {
                    html.AppendLine("    <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" border=\"0\" width=\"100%\"> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td colspan=\"2\" style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                CALIFICACIÓN</td> ");
                    html.AppendLine("            <td colspan=\"2\" style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                CALIFICACIONES A INGRESAR</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación Interna Actual:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.CalIntAct.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación Interna Recomendada: ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + (objCliente.calIntRec.Trim().Equals("0") ? "" : objCliente.calIntRec.Trim()) + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación Interna Actual Nuevo Rating:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + objCliente.CalIntNRating.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación Nuevo Rating Recomendada:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: normal\"> ");
                    html.AppendLine("                <label>" + (objCliente.calNuevoRatRecom.Trim().Equals("0") ? "" : objCliente.calNuevoRatRecom.Trim()) + "</label></td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Fecha Califi. Interna Actual:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.fecCalInt.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Seguimiento de Covenants:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.segCovenants.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación MAF:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.calMAF.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                    Calificación MAF Nuevo Rating:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.CalMAFNRating.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("    </table> ");

                }

                actividadCliente = objCliente.actClienteCom.Trim();
                analisisPyG = objCliente.analisisPyGCom.Trim();
                analisisBalance = objCliente.analisisBalCom.Trim();
                comentariosAdicionales = objCliente.comAdCom.Trim();

            }
            #endregion
            #region "PIC, Superfinanciera, Consulta comentado antes de PMO27494"
            #endregion

            #region "PIC, Superfinanciera, Consulta"
            // PMO19939 - RF048: En el perfil “Consulta” y “Superfinanciera” para el comité presencial replicar la vista del Perfil “Riesgos” sin posibilidad de modificarla
            //else if (rol.Equals("Riesgos - PIC"))
            else
            {
                // PMO 27494 se genera nuevo panel dependiendo del periodo se visualizara el panel historico que corresponda.
                if (Convert.ToInt32(prdoNvo) >= 201812)
                {
                    html.AppendLine("    <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" border=\"0\" width=\"100%\"> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td colspan=\"2\" style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                CALIFICACIÓN</td> ");
                    html.AppendLine("            <td colspan=\"2\" style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                CALIFICACIONES A INGRESAR</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación Interna Actual:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.CalIntNRating.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación Interna Ratificada:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + (objCliente.calIntRatNRating.Trim().Equals("0") ? "" : objCliente.calIntRatNRating.Trim()) + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Fecha Calificación Interna Actual:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.fecCalInt.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación Externa Ratificada: ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + (objCliente.calExtRat.Trim().Equals("0") ? "" : objCliente.calExtRat.Trim()) + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Seguimiento de Covenants:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.segCovenants.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Seguimiento en próximo Comité:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + (objCliente.segProxComite.Trim().Equals("0") ? "" : objCliente.segProxComite.Trim()) + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("        <tr style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación Modelo Rating:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.CalMAFNRating.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Recomendación AEC:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + (objCliente.recAEC.Trim().Equals("0") ? "" : objCliente.recAEC.Trim()) + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                 Calificación Recomendada Alineación Grupo Bancolombia:</td> "); // PMO19939 - RF014: Modificar PDF Riesgos con el cambio de etiqueta “Calificación Recomendada PEC
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.CalifRecomenGrupo.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: top;font-weight: bold\"> ");
                    html.AppendLine("                Razón Calificación:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    processoRazCaliInt(objCliente.razCaliInt, ref html); //PMO19939 - RF021: Modificar el campo “Razón Calificación” con su nueva estructura en el PDF
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación Recomendada PEC (Sector Financiero):</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + objCliente.calSugeridaPEC.Trim() + "</label> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;font-weight: bold\"> ");
                    html.AppendLine("                El cliente se encuentra en lista de control:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + objCliente.ListasDeControl.Trim() + "</label> </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Tipo Cliente:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + (objCliente.tipoCliente.Trim().Equals("0") ? "" : objCliente.tipoCliente.Trim()) + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación Recomendada:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + (objCliente.calNuevoRatRecom.Trim().Equals("0") ? "" : objCliente.calNuevoRatRecom.Trim()) + "</label>");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Estado Calificación:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + (objCliente.estadoCalificacion.Trim().Equals("0") ? "" : objCliente.estadoCalificacion.Trim()) + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                &nbsp;");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Utilizó EEFF Cargados en la Herramienta:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + (objCliente.utilizoEEFF.Trim().Equals("0") ? "" : objCliente.utilizoEEFF.Trim()) + "</label>");
                    html.AppendLine("            </td> ");
                    //
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("    </table> ");
                    // PMO 27494 Asignacion de los valores de las preguntas
                    SustentacionCalRecCom = objCliente.SustentacionCalRecPIC.Trim();
                    RespListaPregunta1 = objCliente.RespListaPregunta1PIC.Trim();
                    RespPregunta1 = objCliente.RespPregunta1PIC.Trim();
                    RespListaPregunta2 = objCliente.RespListaPregunta2PIC.Trim();
                    RespPregunta2 = objCliente.RespPregunta2PIC.Trim();
                    RespListaPregunta3 = objCliente.RespListaPregunta3PIC.Trim();
                    RespPregunta3 = objCliente.RespPregunta3PIC.Trim();
                    RespListaPregunta4 = objCliente.RespListaPregunta4PIC.Trim();
                    RespPregunta4 = objCliente.RespPregunta4PIC.Trim();
                    RespListaPregunta5 = objCliente.RespListaPregunta5PIC.Trim();
                    RespPregunta5 = objCliente.RespPregunta5PIC.Trim();
                    RespListaPregunta6 = objCliente.RespListaPregunta6PIC.Trim();
                    RespPregunta6 = objCliente.RespPregunta6PIC.Trim();
                    RespListaPregunta7 = objCliente.RespListaPregunta7PIC.Trim();
                    RespPregunta7 = objCliente.RespPregunta7PIC.Trim();
                    RespListaPregunta8 = objCliente.RespListaPregunta8PIC.Trim();
                    RespPregunta8 = objCliente.RespPregunta8PIC.Trim();
                    RespListaPregunta9 = objCliente.RespListaPregunta9PIC.Trim();
                    RespPregunta9 = objCliente.RespPregunta9PIC.Trim();
                    RespListaPregunta10 = objCliente.RespListaPregunta10PIC.Trim();
                    RespPregunta10 = objCliente.RespPregunta10PIC.Trim();
                    RespListaPregunta11 = objCliente.RespListaPregunta11PIC.Trim();
                    RespPregunta11 = objCliente.RespPregunta11PIC.Trim();
                    RespListaPregunta12 = objCliente.RespListaPregunta12PIC.Trim();
                    RespPregunta12 = objCliente.RespPregunta12PIC.Trim();
                    RespListaPregunta13 = objCliente.RespListaPregunta13PIC.Trim();
                    RespPregunta13 = objCliente.RespPregunta13PIC.Trim();
                    RespListaPregunta14 = objCliente.RespListaPregunta14PIC.Trim();
                    RespPregunta14 = objCliente.RespPregunta14PIC.Trim();


                    //Nuevas modificaciones de periodos anteriores
                    RespPregunta15 = objCliente.RespPregunta15PIC?.Trim();
                    RespPregunta16 = objCliente.RespPregunta16PIC?.Trim();
                    RespPregunta17 = objCliente.RespPregunta17PIC?.Trim();
                    RespPregunta18 = objCliente.RespPregunta18PIC?.Trim();
                    RespPregunta19 = objCliente.RespPregunta19PIC?.Trim();
                    RespPregunta20 = objCliente.RespPregunta20PIC?.Trim();
                    RespPregunta21 = objCliente.RespPregunta21PIC?.Trim();
                    RespPregunta22 = objCliente.RespPregunta22PIC?.Trim();

                }
                else
                {
                    html.AppendLine("    <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" border=\"0\" width=\"100%\"> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td colspan=\"2\" style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                CALIFICACIÓN</td> ");
                    html.AppendLine("            <td colspan=\"2\" style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                CALIFICACIONES A INGRESAR</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación Interna Actual Nuevo Rating:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.CalIntNRating.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación Interna Ratificada Nuevo Rating:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + (objCliente.calIntRatNRating.Trim().Equals("0") ? "" : objCliente.calIntRatNRating.Trim()) + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Fecha Calificación Interna Actual:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.fecCalInt.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación Externa Ratificada: ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + (objCliente.calExtRat.Trim().Equals("0") ? "" : objCliente.calExtRat.Trim()) + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Seguimiento de Covenants:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.segCovenants.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Seguimiento en próximo Comité:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + (objCliente.segProxComite.Trim().Equals("0") ? "" : objCliente.segProxComite.Trim()) + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación MAF Nuevo Rating:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.CalMAFNRating.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Recomendación AEC:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + (objCliente.recAEC.Trim().Equals("0") ? "" : objCliente.recAEC.Trim()) + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                 Calificación Recomendada (Alineacion – PEC):</td> "); // PMO19939 - RF014: Modificar PDF Riesgos con el cambio de etiqueta “Calificación Recomendada PEC
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.calSugeridaPEC.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: top;font-weight: bold\"> ");
                    html.AppendLine("                Razón Calificación:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    processoRazCaliInt(objCliente.razCaliInt, ref html); //PMO19939 - RF021: Modificar el campo “Razón Calificación” con su nueva estructura en el PDF
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;font-weight: bold\"> ");
                    html.AppendLine("                Calificación Nuevo Rating Recomendada:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + (objCliente.calNuevoRatRecom.Trim().Equals("0") ? "" : objCliente.calNuevoRatRecom.Trim()) + "</label> </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Tipo Cliente:</td> "); // PMO19939 - RF024: Visualizar campo “Tipo Cliente” en el PDF de Riesgos del comité presencial
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + (objCliente.tipoCliente.Trim().Equals("0") ? "" : objCliente.tipoCliente.Trim()) + "</label> "); // PMO19939 - RF024: Visualizar campo “Tipo Cliente” en el PDF de Riesgos del comité presencial
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Fecha MAF:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + (objCliente.fechaMAF.Trim().Equals("0") ? "" : objCliente.fechaMAF.Trim()) + "</label>");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Estado Calificación:</td> "); // PMO19939 - RF024: Visualizar campo “Tipo Cliente” en el PDF de Riesgos del comité presencial
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + (objCliente.estadoCalificacion.Trim().Equals("0") ? "" : objCliente.estadoCalificacion.Trim()) + "</label> "); // PMO19939 - RF024: Visualizar campo “Tipo Cliente” en el PDF de Riesgos del comité presencial
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Puntaje MAF: ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + objCliente.puntajeMAF.Trim() + "</label> ");
                    html.AppendLine("            </td> ");
                    // PMO19939 - RF061: Modificar visualización “Consulta Proceso Anteriores” del comité presencial
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Utilizó EEFF Cargados en la Herramienta:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + (objCliente.utilizoEEFF.Trim().Equals("0") ? "" : objCliente.utilizoEEFF.Trim()) + "</label>");
                    html.AppendLine("            </td> ");
                    //
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                Cliente Pertenece a IFRS:</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                <label>" + objCliente.perteneceIFRS.Trim() + "</label> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                &nbsp;");
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("    </table> ");
                }

                if (objCliente.calIntRatNRating.Trim().Length > 0)
                {
                    actividadCliente = objCliente.actClientePIC.Trim();
                    analisisBalance = objCliente.analisisBalPIC.Trim();
                    analisisPyG = objCliente.analisisPyGPIC.Trim();
                    comentariosAdicionales = (string.IsNullOrEmpty(objCliente.comAuto.Trim()) ? objCliente.comAdPIC.Trim() : objCliente.comAuto.Trim());
                }
                else if (objCliente.calNuevoRatRecom.Trim().Length > 0)
                {
                    actividadCliente = objCliente.actClienteCom.Trim();
                    analisisBalance = objCliente.analisisBalCom.Trim();
                    analisisPyG = objCliente.analisisPyGCom.Trim();
                    comentariosAdicionales = objCliente.comAdCom.Trim();
                }

                comentariosRiesgos = objCliente.comentarioRiesgos.Trim();
            }

            #endregion


            #region "SuperConsulta"
            #endregion

            #region "Comentarios"
            SustentacionCalRecCom = HttpUtility.HtmlEncode(SustentacionCalRecCom);
            RespPregunta1 = HttpUtility.HtmlEncode(RespPregunta1);
            RespPregunta2 = HttpUtility.HtmlEncode(RespPregunta2);
            RespPregunta3 = HttpUtility.HtmlEncode(RespPregunta3);
            RespPregunta4 = HttpUtility.HtmlEncode(RespPregunta4);
            RespPregunta5 = HttpUtility.HtmlEncode(RespPregunta5);
            RespPregunta6 = HttpUtility.HtmlEncode(RespPregunta6);
            RespPregunta7 = HttpUtility.HtmlEncode(RespPregunta7);
            RespPregunta8 = HttpUtility.HtmlEncode(RespPregunta8);
            RespPregunta9 = HttpUtility.HtmlEncode(RespPregunta9);
            RespPregunta10 = HttpUtility.HtmlEncode(RespPregunta10);
            RespPregunta11 = HttpUtility.HtmlEncode(RespPregunta11);
            RespPregunta12 = HttpUtility.HtmlEncode(RespPregunta12);
            RespPregunta13 = HttpUtility.HtmlEncode(RespPregunta13);
            RespPregunta14 = HttpUtility.HtmlEncode(RespPregunta14);
            RespPregunta15 = HttpUtility.HtmlEncode(RespPregunta15);
            RespPregunta16 = HttpUtility.HtmlEncode(RespPregunta16);
            RespPregunta17 = HttpUtility.HtmlEncode(RespPregunta17);
            RespPregunta18 = HttpUtility.HtmlEncode(RespPregunta18);
            RespPregunta19 = HttpUtility.HtmlEncode(RespPregunta19);
            RespPregunta20 = HttpUtility.HtmlEncode(RespPregunta20);
            RespPregunta21 = HttpUtility.HtmlEncode(RespPregunta21);
            RespPregunta22 = HttpUtility.HtmlEncode(RespPregunta22);
            comentariosRiesgos = HttpUtility.HtmlEncode(comentariosRiesgos);

            if (Convert.ToInt32(prdoNvo) >= 201812)

            {
                #region "Comentarios NvoPdo"

                html.AppendLine("    <table style=\"font-family: Verdana;font-size: 7px;text-align:left;vertical-align:middle\" border=\"0\" width=\"100%\"> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\"> ");
                html.AppendLine("                RECOMENDACIÓN</td> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td  colspan=\"2\"> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\" colspan=\"2\"> ");
                html.AppendLine("                SUSTENTACIÓN DE CALIFICACIÓN RECOMENDADA ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: normal\" colspan=\"2\"> ");
                html.AppendLine("                (Describir actividad del cliente, hechos relevantes que generan variaciones en las cifras y argumentos que sustentan la calificación recomendada) ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\"> ");
                html.AppendLine("               <table border=\"0.5\" width=\"100%\"><tr><td>" + SustentacionCalRecCom.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");

                // PMO27494 Lista de preguntas y comentarios semidinamica.
                string pregunta = "";
                string Despregunta = "";
                Parametros[] Lista = null;
                Lista = (Parametros[])Session["parametros"];
                int i = 1;
                foreach (Parametros row in Lista.Where(p => p.paramName.Trim().Equals("PRFORMPRE")))
                {
                    if (row.paramSeq != 0)
                    {
                        Parametros Coleccion1 = Lista.Where(p => p.paramName.Trim().Equals("PRFORMPRE") && p.paramSeq == i).ElementAt(0);

                        if (row.paramSeq > 9)
                        {
                            if ((row.paramSeq == 10) && (categoriaCliente != "GB"))
                            {
                                html.AppendLine("        <tr> ");
                                html.AppendLine("            <td align=\"center\" style=\"font-family: Verdana;font-size: 9px;color:black;text-align: center;height: 27px;vertical-align: middle;font-weight:bold\" colspan=\"2\"> ");
                                html.AppendLine("                <label style=\"font-family: Verdana;font-size: 9px;color:black;text-align: center;height: 27px;vertical-align: middle;font-weight:bold\">Calificación para proyectos o sociedades Propósito</label> ");
                                html.AppendLine("            </td> ");
                                html.AppendLine("        </tr> ");
                                html.AppendLine("            <td align=\"center\" style=\"font-family: Verdana;font-size: 7px;color: black;text-align: center;height: middle;font-weight: normal\" colspan=\"2\"> ");
                                html.AppendLine("                (Diligencie sólo esta información si la exposición del cliente a calificar corresponde únicamente al desarrollo de un proyecto específico) ");
                                html.AppendLine("            </td> ");
                                html.AppendLine("        </tr> ");
                                html.AppendLine("        <tr> ");
                                html.AppendLine("            <td > ");
                                html.AppendLine("                &nbsp;</td> ");
                                html.AppendLine("            <td > ");
                                html.AppendLine("                &nbsp;</td> ");
                                html.AppendLine("        </tr> ");

                            }

                            if ((row.paramSeq == 15) && (categoriaCliente == "FI"))
                            {
                                html.AppendLine("        <tr> ");
                                html.AppendLine("            <td align=\"center\" style=\"font-family: Verdana;font-size: 9px;color:black;text-align: center;height: 27px;vertical-align: middle;font-weight:bold\" colspan=\"2\"> ");
                                html.AppendLine("                <label style=\"font-family: Verdana;font-size: 9px;color:black;text-align: center;height: 27px;vertical-align: middle;font-weight:bold\">Calificación para Fondos Inmobiliarios</label> ");
                                html.AppendLine("            </td> ");
                                html.AppendLine("        </tr> ");
                                html.AppendLine("        </tr> ");
                                html.AppendLine("        <tr> ");
                                html.AppendLine("            <td > ");
                                html.AppendLine("                &nbsp;</td> ");
                                html.AppendLine("            <td > ");
                                html.AppendLine("                &nbsp;</td> ");
                                html.AppendLine("        </tr> ");

                            }

                            if ((row.paramSeq == 19) && (categoriaCliente == "GB"))
                            {
                                html.AppendLine("        <tr> ");
                                html.AppendLine("            <td align=\"center\" style=\"font-family: Verdana;font-size: 9px;color:black;text-align: center;height: 27px;vertical-align: middle;font-weight:bold\" colspan=\"2\"> ");
                                html.AppendLine("                <label style=\"font-family: Verdana;font-size: 9px;color:black;text-align: center;height: 27px;vertical-align: middle;font-weight:bold\">Calificación para Gobierno (municipios y departamentos)</label> ");
                                html.AppendLine("            </td> ");
                                html.AppendLine("        </tr> ");
                                html.AppendLine("        </tr> ");
                                html.AppendLine("        <tr> ");
                                html.AppendLine("            <td > ");
                                html.AppendLine("                &nbsp;</td> ");
                                html.AppendLine("            <td > ");
                                html.AppendLine("                &nbsp;</td> ");
                                html.AppendLine("        </tr> ");

                            }
                        }
                        if (categoriaCliente != "GB")
                        {
                            if (Coleccion1.paramSeq <= 9)
                            {
                                pregunta = i + ") " + Coleccion1.param7;
                                Despregunta = Coleccion1.param8;

                                html.AppendLine("        <tr> ");
                                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\" colspan=\"2\"> ");
                                html.AppendLine("               " + pregunta.Replace("\n", "<br />").Trim());
                                html.AppendLine("            </td> ");
                                html.AppendLine("        <tr> ");
                                html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                html.AppendLine("               " + Despregunta.Replace("\n", "<br />").Trim());
                                html.AppendLine("            </td> ");
                                html.AppendLine("        </tr> ");
                            }

                            if ((Coleccion1.paramSeq > 9) && (Coleccion1.paramSeq <= 14))
                            {

                                pregunta = i - 9 + ") " + Coleccion1.param7;
                                Despregunta = Coleccion1.param8;

                                html.AppendLine("        <tr> ");
                                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\" colspan=\"2\"> ");
                                html.AppendLine("               " + pregunta.Replace("\n", "<br />").Trim());
                                html.AppendLine("            </td> ");
                                html.AppendLine("        <tr> ");
                                html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                html.AppendLine("               " + Despregunta.Replace("\n", "<br />").Trim());
                                html.AppendLine("            </td> ");
                                html.AppendLine("        </tr> ");
                            }

                        }

                        if ((categoriaCliente == "FI") && (Coleccion1.paramSeq > 14) && (Coleccion1.paramSeq <= 18))
                        {
                            pregunta = i - 14 + ") " + Coleccion1.param7;
                            Despregunta = Coleccion1.param8;

                            html.AppendLine("        <tr> ");
                            html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\" colspan=\"2\"> ");
                            html.AppendLine("               " + pregunta.Replace("\n", "<br />").Trim());
                            html.AppendLine("            </td> ");
                            html.AppendLine("        <tr> ");
                            html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                            html.AppendLine("               " + Despregunta.Replace("\n", "<br />").Trim());
                            html.AppendLine("            </td> ");
                            html.AppendLine("        </tr> ");
                        }

                        if ((categoriaCliente == "GB") && (Coleccion1.paramSeq > 18))
                        {
                            pregunta = i - 18 + ") " + Coleccion1.param7;
                            Despregunta = Coleccion1.param8;

                            html.AppendLine("        <tr> ");
                            html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\" colspan=\"2\"> ");
                            html.AppendLine("               " + pregunta.Replace("\n", "<br />").Trim());
                            html.AppendLine("            </td> ");
                            html.AppendLine("        <tr> ");
                            html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                            html.AppendLine("               " + Despregunta.Replace("\n", "<br />").Trim());
                            html.AppendLine("            </td> ");
                            html.AppendLine("        </tr> ");
                        }


                        if (categoriaCliente != "GB")
                        {
                            switch (i)
                            {
                                case 1:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta1.Replace("\n", "<br />").Trim() + "</td><td width=\"5%\">" + RespListaPregunta1.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 2:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta2.Replace("\n", "<br />").Trim() + "</td><td width=\"5%\">" + RespListaPregunta2.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 3:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta3.Replace("\n", "<br />").Trim() + "</td><td width=\"5%\">" + RespListaPregunta3.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 4:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta4.Replace("\n", "<br />").Trim() + "</td><td width=\"5%\">" + RespListaPregunta4.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 5:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta5.Replace("\n", "<br />").Trim() + "</td><td width=\"5%\">" + RespListaPregunta5.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 6:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta6.Replace("\n", "<br />").Trim() + "</td><td width=\"5%\">" + RespListaPregunta6.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 7:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta7.Replace("\n", "<br />").Trim() + "</td><td width=\"5%\">" + RespListaPregunta7.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 8:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta8.Replace("\n", "<br />").Trim() + "</td><td width=\"5%\">" + RespListaPregunta8.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 9:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta9.Replace("\n", "<br />").Trim() + "</td><td width=\"5%\">" + RespListaPregunta9.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 10:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta10.Replace("\n", "<br />").Trim() + "</td><td width=\"5%\">" + RespListaPregunta10.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 11:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta11.Replace("\n", "<br />").Trim() + "</td><td width=\"5%\">" + RespListaPregunta11.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 12:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta12.Replace("\n", "<br />").Trim() + "</td><td width=\"5%\">" + RespListaPregunta12.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 13:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta13.Replace("\n", "<br />").Trim() + "</td><td width=\"5%\">" + RespListaPregunta13.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 14:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta14.Replace("\n", "<br />").Trim() + "</td><td width=\"5%\">" + RespListaPregunta14.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                            }
                        }

                        if (categoriaCliente == "FI")
                        {
                            switch (i)
                            {
                                case 15:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta15.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 16:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta16.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 17:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta17.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 18:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta18.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                            }

                        }

                        if (categoriaCliente == "GB")
                        {
                            switch (i)
                            {
                                case 19:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta19.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 20:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta20.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 21:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta21.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                                case 22:
                                    html.AppendLine("        <tr> ");
                                    html.AppendLine("            <td  style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\" > ");//
                                    html.AppendLine("               <table border-spacing=\"5px\" border=\"0.5\"width=\"100%\"><tr><td width=\"90%\">" + RespPregunta22.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                                    html.AppendLine("            </td> ");
                                    html.AppendLine("        </tr> ");
                                    break;
                            }

                        }
                        i++;
                    }
                }
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td colspan=\"2\"> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td colspan=\"2\"> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td colspan=\"2\" > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");

                if (rol.Equals("Riesgos - PIC") || rol.Equals("Consulta") || rol.Equals("Superfinanciera"))
                {
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align:left;vertical-align: middle;font-weight: bold\" colspan=\"2\"> ");
                    html.AppendLine("                Comentarios Riesgos - Sustentación Calificación</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle;border: 1px solid gray\" colspan=\"2\"> ");
                    html.AppendLine("               <table border=\"0.5\" width=\"100%\"><tr><td>" + comentariosRiesgos.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                }

                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp; ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("    </table> ");
                #endregion


            }
            else
            {
                html.AppendLine("    <table style=\"font-family: Verdana;font-size: 7px;text-align:left;vertical-align:middle\" border=\"0\" width=\"100%\"> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\"> ");
                html.AppendLine("                RECOMENDACIÓN</td> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td  colspan=\"2\"> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\" colspan=\"2\"> ");
                html.AppendLine("                Actividad del cliente ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: normal\" colspan=\"2\"> ");
                html.AppendLine("                (Describir brevemente la actividad que realiza el cliente) ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle\" colspan=\"2\"> ");
                html.AppendLine("               <table border=\"0.5\" width=\"100%\"><tr><td>" + actividadCliente.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td colspan=\"2\"> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\" colspan=\"2\"> ");
                html.AppendLine("                Análisis de cifras financieras</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" colspan=\"2\"> ");
                html.AppendLine("                Descripción de los hechos relevantes que ocasionan variaciones en las cifras del PyG (No es transcripción de cifras financieras) ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle;border: 1px solid gray\" colspan=\"2\"> ");
                html.AppendLine("               <table border=\"0.5\" width=\"100%\"><tr><td>" + analisisPyG.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td colspan=\"2\"> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" colspan=\"2\"> ");
                html.AppendLine("                Descripción de los hechos relevantes que ocasionan variaciones en las cifras del Balance (No es transcripción de cifras financieras)");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle;border: 1px solid gray\" colspan=\"2\"> ");
                html.AppendLine("               <table border=\"0.5\" width=\"100%\"><tr><td>" + analisisBalance.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td colspan=\"2\" > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align:left;vertical-align: middle;font-weight: bold\" colspan=\"2\"> ");
                html.AppendLine("                Hechos significativos que aporten a la calificación recomendada</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;text-align:justify;vertical-align:middle;border: 1px solid gray\" colspan=\"2\"> ");
                html.AppendLine("               <table border=\"0.5\" width=\"100%\"><tr><td>" + comentariosAdicionales.Replace("\n", "<br />").Trim() + "</td></tr></table>");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp; ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("    </table> ");
            }

            #endregion
        }

        /// <summary>
        /// obtiene la informacion de endeudamiento segun el rol
        /// </summary>
        /// <param name="rol"></param>
        /// <param name="html"></param>
        /// <param name="errores"></param>
        /// <param name="objCliente"></param>
        private void informacionEndeudamiento(string rol, ref StringBuilder html, Cliente objCliente)
        {


            if (rol.Equals("Comercial") || rol.Equals("Riesgos - PIC"))
            {
                if (Request.QueryString["Prdo"] != null && rol.Equals("Comercial"))
                { }
                else
                {
                    html.AppendLine("    <table border=\"0\" width=\"40%\"> ");
                    //RF017
                    if (Session["parametros"] != null)
                    {
                        Parametros[] param = (Parametros[])Session["parametros"];
                        Parametros campoParametrico = param.Where(p => p.paramName.Trim().Equals("OPARMADI")
                                && p.paramSeq == 1).ElementAt(0);
                        html.AppendLine("                <tr> ");
                        html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\" colspan=\"2\"> ");
                        html.AppendLine("                        " + campoParametrico.param7 + "  </td> ");
                        html.AppendLine("                </tr> ");
                    }
                    //
                    html.AppendLine("                <tr> ");
                    html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                        #VECES MORA 30: </td> ");
                    html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                    " + objCliente.nroVecesMora30.Trim() + "</td> ");
                    html.AppendLine("                </tr> ");
                    html.AppendLine("                <tr> ");
                    html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                        #VECES MORA 60: </td> ");
                    html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                    " + objCliente.nroVecesMora60.Trim() + "</td> ");
                    html.AppendLine("                </tr> ");
                    html.AppendLine("                <tr> ");
                    html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                    html.AppendLine("                        MORA MÁXIMA: </td> ");
                    html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                    html.AppendLine("                        " + objCliente.moraMaxima.Trim() + "</td> ");
                    html.AppendLine("                </tr> ");
                    html.AppendLine("                <tr> ");
                    html.AppendLine("                    <td colspan=\"2\"> ");
                    html.AppendLine("                    &nbsp;</td> ");
                    html.AppendLine("                </tr> ");
                    html.AppendLine("            </table> ");
                    html.AppendLine("    <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" width=\"100%\"> ");
                    html.AppendLine("                <tr> ");
                    html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\" > ");
                    html.AppendLine("                        LOCAL ");
                    html.AppendLine("                    </td> ");
                    html.AppendLine("                </tr> ");
                    html.AppendLine("                <tr> ");
                    html.AppendLine("                    <td> ");
                    html.AppendLine("                    &nbsp;</td> ");
                    html.AppendLine("                </tr> ");
                    html.AppendLine("            </table> ");
                }
            }
        }

        /// <summary>
        /// obtiene la info financiera segun el perfil
        /// </summary>
        /// <param name="rol"></param>
        /// <param name="objCliente"></param>
        /// <param name="html"></param>
        /// <param name="errores"></param>
        private void ObtenerInformacionFinanciera(string rol, Cliente objCliente, ref StringBuilder html, ref StringBuilder errores)
        {
            var nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ",";
            nfi.NumberGroupSeparator = ".";
            nfi.CurrencyDecimalSeparator = ",";
            nfi.CurrencyGroupSeparator = ".";
            int fecha;


            if (int.TryParse(objCliente.fechaEstFros.Trim(), out fecha) && fecha.ToString().Length == 6)
            {
                html.AppendLine("    <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" border=\"0\" width=\"100%\"> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td  colspan=\"6\"> ");
                html.AppendLine("                       <table border=\"0.5\" width=\"100%\"><tr><td>&nbsp;</td></tr></table>");
                html.AppendLine("                </td> ");
                html.AppendLine("        </tr> ");

                if ((rol.Equals("Superfinanciera") || rol.Equals("Consulta")) || (rol.Equals("Comercial") && Request.QueryString["Prdo"] != null))
                {
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td  colspan=\"6\"> ");
                    html.AppendLine("                       &nbsp;");
                    html.AppendLine("                </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td  colspan=\"6\"> ");
                    html.AppendLine("                       &nbsp;");
                    html.AppendLine("                </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td  colspan=\"6\"> ");
                    html.AppendLine("                       &nbsp;");
                    html.AppendLine("                </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td  colspan=\"6\"> ");
                    html.AppendLine("                       &nbsp;");
                    html.AppendLine("                </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td  colspan=\"6\"> ");
                    html.AppendLine("                       &nbsp;");
                    html.AppendLine("                </td> ");
                    html.AppendLine("        </tr> ");
                }

                html.AppendLine("        <tr> ");
                html.AppendLine("            <td  colspan=\"6\"> ");
                html.AppendLine("                       &nbsp;");
                html.AppendLine("                </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td  colspan=\"6\"> ");
                html.AppendLine("                       &nbsp;");
                html.AppendLine("                </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td  colspan=\"6\"> ");
                html.AppendLine("                       &nbsp;");
                html.AppendLine("                </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td  colspan=\"6\"> ");
                html.AppendLine("                       &nbsp;");
                html.AppendLine("                </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td  colspan=\"6\"> ");
                html.AppendLine("                       &nbsp;");
                html.AppendLine("                </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td  colspan=\"2\"> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td colspan=\"4\"> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td colspan=\"3\" style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle> ");
                html.AppendLine("                <label style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\">INFORMACIÓN FINANCIERA: </label> ");
                html.AppendLine("                <label style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;font-weight:bold\">" + fecha + "</label> ");
                html.AppendLine("            </td> ");
                //RF052 INFORMACIÓN FINANCIERA SOLICITADA
                html.AppendLine("            <td colspan=\"3\" style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle> ");
                html.AppendLine("                <label style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\">INFORMACIÓN FINANCIERA SOLICITADA:  </label> ");
                html.AppendLine("                <label style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;font-weight:bold\">" + objCliente.corteEstFros + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td width=\"100\" > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td colspan=\"3\"> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 6px;color: black;text-align: center;vertical-align: middle;font-weight:bold;font-style:italic\" colspan=\"2\"> ");
                html.AppendLine("                PyG</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 6px;color: black;text-align: center;vertical-align: middle;font-weight:bold;font-style:italic\" colspan=\"2\"> ");
                html.AppendLine("                Balance</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 6px;color: black;text-align: center;vertical-align: middle;font-weight:bold;font-style:italic\" colspan=\"2\"> ");
                html.AppendLine("                Indicadores</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"width: 150px;\"> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"width: 150px;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                html.AppendLine("                Ventas:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label>" + objCliente.ventasPerAct.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                Pasivo Financiero CP:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label>" + (objCliente.pasivoFroCP.Trim().Equals(string.Empty) ? "0" : objCliente.pasivoFroCP.Trim()) + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                %Var. Ventas:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + objCliente.porcentVarVentas.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"width: 150px;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                html.AppendLine("                Ebitda en pesos:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + objCliente.ebitdaPerAct.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                Pasivo Financiero LP:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + (objCliente.pasivoFroLP.Trim().Equals(string.Empty) ? "0" : objCliente.pasivoFroLP.Trim()) + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                Endeudamiento:</td> ");
                double pasivo;
                double activo;
                string valorend = string.Empty;
                if (double.TryParse(objCliente.pasivoPerAct, NumberStyles.Float | NumberStyles.AllowParentheses | NumberStyles.AllowThousands, nfi, out pasivo) && double.TryParse(objCliente.activosPerAct, NumberStyles.Float | NumberStyles.AllowParentheses | NumberStyles.AllowThousands, nfi, out activo))
                {
                    string end = (pasivo / activo * 100).ToString(nfi);
                    if (end.Contains(','))
                    {
                        string num = end.Substring(0, end.IndexOf(','));
                        string dec = end.Substring(end.IndexOf(',') + 1, 2);
                        valorend = num + nfi.NumberDecimalSeparator + dec + "%";
                    }
                    else
                    {
                        if (activo == 0)
                        {
                            valorend = "Infinity";
                        }
                        else
                        {
                            valorend = end + nfi.NumberDecimalSeparator + "00%";
                        }
                    }
                }
                else
                {
                    valorend = "No se puede calcular";
                    errores.AppendLine(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0435").Texto + "</br>");
                }
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + valorend + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr style=\"width: 150px;font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                html.AppendLine("                Utilidad Operacional:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + objCliente.utilPerdidaOpPerAct.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                Capital Social:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + objCliente.capSocialPerAct.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                Margen EBITDA:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + objCliente.margenEbitda.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"width: 150px;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                html.AppendLine("                Intereses Pagados:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + objCliente.intPagadosPerAct.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                Total Activo:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label  >" + objCliente.activosPerAct.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                Cobertura Ebitda:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + objCliente.coberturaEbitda.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"width: 150px;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                html.AppendLine("                Utilidad Neta:</td> ");
                double utilidadNeta;
                if (double.TryParse(objCliente.utilPerNetaPerAct, NumberStyles.Float | NumberStyles.AllowParentheses | NumberStyles.AllowParentheses | NumberStyles.AllowThousands, nfi, out utilidadNeta))
                {

                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.utilPerNetaPerAct.Trim() + "</label> ");
                    html.AppendLine("            </td> ");

                }
                else
                {
                    errores.AppendLine(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0434").Texto + "</br>");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                    html.AppendLine("                <label >" + objCliente.utilPerNetaPerAct + "</label> ");
                    html.AppendLine("            </td> ");
                }
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                Total Pasivo:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + objCliente.pasivoPerAct.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                Margen Neto:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + objCliente.margenNeto.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"width: 150px;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                html.AppendLine("                Utilidad Bruta:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + objCliente.utilBrutaPerAct.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                Superávit de Capital:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + objCliente.superCapPerAct.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                Rotación de Cartera:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + objCliente.rotacionCartera.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"width: 150px;\"> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                Reservas:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + objCliente.reservasPerAct.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                Rotación de Inventarios:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + objCliente.rotacionInventarios.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"width: 150px;\"> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                Patrimonio:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + objCliente.patPerAct.Trim() + "</label></td> ");
                html.AppendLine("                <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                    Ciclo Financiero:</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + objCliente.cicloFinPerAct.Trim() + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"width: 150px;\"> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("                <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold;padding-left:10px\"> ");
                html.AppendLine("                    Deuda/Ebitda:</td> ");
                double PasivoFCP;
                double PasivoFLP;
                double deuda;
                double ebitdapesos;
                double ebitda;
                string ValorDeuda = string.Empty;
                if (objCliente.pasivoFroCP.Trim().Equals(string.Empty))
                    objCliente.pasivoFroCP = "0";
                if (objCliente.pasivoFroLP.Trim().Equals(string.Empty))
                    objCliente.pasivoFroLP = "0";
                if (double.TryParse(objCliente.pasivoFroCP.Trim(), NumberStyles.Float | NumberStyles.AllowParentheses | NumberStyles.AllowParentheses | NumberStyles.AllowThousands, nfi, out PasivoFCP) && double.TryParse(objCliente.pasivoFroLP, NumberStyles.Float | NumberStyles.AllowParentheses | NumberStyles.AllowParentheses | NumberStyles.AllowThousands, nfi, out PasivoFLP))
                {
                    deuda = PasivoFLP + PasivoFCP;
                    if (double.TryParse(objCliente.ebitdaPerAct.Trim(), NumberStyles.Float | NumberStyles.AllowParentheses | NumberStyles.AllowParentheses | NumberStyles.AllowThousands, nfi, out ebitdapesos))
                    {
                        //validar ebidta
                        int mes = int.Parse(fecha.ToString().Substring(4, 2));
                        if (mes <= 0 || mes > 12)
                            ValorDeuda = "0,00";
                        else
                        {
                            ebitda = (ebitdapesos * 12) / mes;
                            if (ebitda != 0)
                            {
                                ValorDeuda = Math.Round((deuda / ebitda), 2).ToString(nfi);
                            }
                            else
                            {
                                ValorDeuda = "0,00";
                            }
                        }
                    }
                    else
                    {
                        ValorDeuda = "0,00";
                    }
                    if (!ValorDeuda.Contains(","))
                    {
                        ValorDeuda = ValorDeuda + nfi.NumberDecimalSeparator + "00";
                    }
                    else
                    {
                        string[] nums = ValorDeuda.Split(',');
                        if (nums[1].Length == 1)
                        {
                            ValorDeuda = nums[0] + nfi.NumberDecimalSeparator + nums[1].PadRight(2, '0');
                        }
                    }
                }
                else
                {
                    errores.AppendLine(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0442").Texto + "</br>");
                }
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:right;vertical-align:middle\"> ");
                html.AppendLine("                <label >" + ValorDeuda + "</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"width: 150px;\"> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("                <td> ");
                html.AppendLine("                &nbsp; ");
                html.AppendLine("            </td> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp; ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");

                if (!rol.Equals("Superfinanciera"))
                {

                    string cumple = "";
                    if (!objCliente.causalDisolucion.Trim().Equals(string.Empty))
                    {
                        cumple = objCliente.causalDisolucion.Trim();
                    }
                    else
                    {
                        //Cumple o no causal de disolucion
                        if (objCliente.capSocialPerAct.Trim().Equals(string.Empty) || objCliente.patPerAct.Trim().Equals(string.Empty))
                        {
                            cumple = "NO";
                        }
                        else
                        {
                            int tipoCodigo;
                            IEnumerable<Parametros> causal;
                            Parametros[] param = (Parametros[])Session["parametros"];
                            if (int.TryParse(objCliente.tipDoc.Trim(), out tipoCodigo))
                            {
                                if (tipoCodigo != 1 && tipoCodigo != 2 && tipoCodigo != 4 && tipoCodigo != 9)
                                {
                                    causal = param.Where(p => p.paramName.Trim().Equals("VPROAUTO") && p.paramSeq == 1);
                                    double capital;
                                    double patrimonio;
                                    double prima;
                                    if (double.TryParse(objCliente.capSocialPerAct, NumberStyles.Float | NumberStyles.AllowThousands, nfi, out capital) && double.TryParse(objCliente.patPerAct, NumberStyles.Float | NumberStyles.AllowThousands, nfi, out patrimonio)
                                        && double.TryParse(objCliente.primaColAcPerAct, NumberStyles.Float | NumberStyles.AllowThousands, nfi, out prima))
                                    {
                                        if ((patrimonio / capital) * 100 < double.Parse(causal.ToArray()[0].param1.Trim(), nfi))
                                        {
                                            cumple = "SI";
                                        }
                                        else
                                        {
                                            cumple = "NO";
                                        }
                                    }
                                    else
                                    {
                                        errores.AppendLine(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0432").Texto + "</br>");
                                        cumple = "NO";
                                    }
                                }
                                else
                                {
                                    cumple = "No por ID";
                                }
                            }
                            else
                            {
                                cumple = "No por ID";
                            }
                        }
                    }
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;font-weight:bold\" colspan=\"6\"> ");
                    html.AppendLine("                Cumple Causal de disolución: " + cumple + "</td> ");
                    html.AppendLine("        </tr> ");

                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;font-weight:bold\" colspan=\"6\"> ");
                    html.AppendLine("                <label style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;font-weight:bold\"> ");
                    html.AppendLine("                Nota: Cifras en Pesos Colombianos</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" colspan=\"6\"> ");
                    html.AppendLine("                       <table border=\"0.5\" width=\"100%\"><tr><td>&nbsp;</td></tr></table>");
                    html.AppendLine("                </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"width: 150px;\"> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td  colspan=\"3\"> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"width: 150px;\"> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td  colspan=\"3\"> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("    </table> ");
                }
                else
                {
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;font-weight:bold\" colspan=\"6\"> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");

                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;font-weight:bold\" colspan=\"6\"> ");
                    html.AppendLine("                <label style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;font-weight:bold\"> ");
                    html.AppendLine("                Nota: Cifras en Pesos Colombianos</label> ");
                    html.AppendLine("            </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" colspan=\"6\"> ");
                    html.AppendLine("                       <table border=\"0.5\" width=\"100%\"><tr><td>&nbsp;</td></tr></table>");
                    html.AppendLine("                </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"width: 150px;\"> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td  colspan=\"3\"> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td style=\"width: 150px;\"> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td > ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("            <td  colspan=\"3\"> ");
                    html.AppendLine("                &nbsp;</td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("    </table> ");
                }
            }
            else
            {
                html.AppendLine("    <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" border=\"0\" width=\"100%\"> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                       <table border=\"0.5\" width=\"100%\"><tr><td>&nbsp;</td></tr></table>");
                html.AppendLine("                </td> ");
                html.AppendLine("        </tr> ");
                if ((rol.Equals("Superfinanciera") || rol.Equals("Consulta")) || (rol.Equals("Comercial") && Request.QueryString["Prdo"] != null))
                {
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td  colspan=\"6\"> ");
                    html.AppendLine("                       &nbsp;");
                    html.AppendLine("                </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td  colspan=\"6\"> ");
                    html.AppendLine("                       &nbsp;");
                    html.AppendLine("                </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td  colspan=\"6\"> ");
                    html.AppendLine("                       &nbsp;");
                    html.AppendLine("                </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td  colspan=\"6\"> ");
                    html.AppendLine("                       &nbsp;");
                    html.AppendLine("                </td> ");
                    html.AppendLine("        </tr> ");
                    html.AppendLine("        <tr> ");
                    html.AppendLine("            <td  colspan=\"6\"> ");
                    html.AppendLine("                       &nbsp;");
                    html.AppendLine("                </td> ");
                    html.AppendLine("        </tr> ");
                }
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\"> ");
                html.AppendLine("                INFORMACIÓN FINANCIERA: ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td > ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td align=\"center\" style=\"font-family: Verdana;font-size: 9px;color:black;text-align: center;height: 27px;vertical-align: middle;font-weight:bold\"> ");
                html.AppendLine("                <label style=\"font-family: Verdana;font-size: 9px;color:black;text-align: center;height: 27px;vertical-align: middle;font-weight:bold\">Información Financiera aún no disponible en el sistema</label> ");
                html.AppendLine("            </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                html.AppendLine("                &nbsp;</td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("        <tr> ");
                html.AppendLine("            <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" > ");
                html.AppendLine("                       <table border=\"0.5\" width=\"100%\"><tr><td>&nbsp;</td></tr></table>");
                html.AppendLine("                </td> ");
                html.AppendLine("        </tr> ");
                html.AppendLine("    </table> ");

            }
        }

        /// <summary>
        /// genera el html de los perfiles, historico
        /// </summary>
        /// <param name="objCliente"></param>
        /// <returns></returns>
        protected StringBuilder HTMLRolesHis(Cliente objCliente, string categoriaCliente)
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
            html.AppendLine("<table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" border=\"0\" width=\"100%\"> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\"> ");
            html.AppendLine("                        ROL: ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
            html.AppendLine("                        " + ((Usuario)Session["Rol"]).rol + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: right;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:justify;vertical-align:middle;\"> ");
            html.AppendLine("                        &nbsp; ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\" colspan=\"5\"> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-size:7px; color:black; font-weight:bold; text-align:justify; font-family:Verdana\"> ");
            html.AppendLine("                        USUARIO: ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;font-weight:bold\"> ");
            html.AppendLine("                        " + ((Usuario)Session["Rol"]).usuario + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: right;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Comité de:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:justify;vertical-align:middle;\"> ");
            html.AppendLine("                        " + objCliente.fecProc.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\" colspan=\"5\"> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\" colspan=\"5\"> ");
            html.AppendLine("                        DATOS DEL CLIENTE</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td colspan=\"5\"> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Nit:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                        " + objCliente.nit.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                    &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Nombre:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                        " + objCliente.nombre.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Tipo de Comité:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                        " + objCliente.tipCom.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                    <td colspan=\"5\"> ");
            html.AppendLine("                       <table border=\"0.5\" width=\"100%\"><tr><td>&nbsp;</td></tr></table>");
            html.AppendLine("                        </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("           </table>");
            html.AppendLine("           <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" border=\"0\" width=\"100%\"> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td  ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\" colspan=\"4\"> ");
            html.AppendLine("                        INFORMACIÓN DEL CLIENTE</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td colspan=\"4\"> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Sector económico:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                        " + objCliente.sectEcon.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Seguimiento recomendado en el comité anterior:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" > ");
            html.AppendLine("                        " + objCliente.segRecComAnt.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Segmento:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                        " + objCliente.segmento.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Grupo de Riesgo:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                        " + objCliente.grupoRiesgo.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Regional:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                        " + objCliente.regional.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Nivel de Riesgo AEC:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" > ");
            html.AppendLine("                        " + objCliente.aec.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            if (!((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC"))
            {
                html.AppendLine("                <tr> ");
                html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                html.AppendLine("                        &nbsp; ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                html.AppendLine("                        &nbsp; ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td> ");
                html.AppendLine("                        &nbsp;</td> ");
                html.AppendLine("                    <td > ");
                html.AppendLine("                        &nbsp;</td> ");
                html.AppendLine("                </tr> ");
            }
            else
            {
                html.AppendLine("                <tr> ");
                html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
                html.AppendLine("                        Calificado por: ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
                html.AppendLine("                        " + objCliente.calPor.Trim() + " ");
                html.AppendLine("                    </td> ");
                html.AppendLine("                    <td> ");
                html.AppendLine("                        &nbsp;</td> ");
                html.AppendLine("                    <td > ");
                html.AppendLine("                        &nbsp;</td> ");
                html.AppendLine("                </tr> ");
            }
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td  colspan=\"5\"> ");
            html.AppendLine("                       <table border=\"0.5\" width=\"100%\"><tr><td>&nbsp;</td></tr></table>");
            html.AppendLine("                        </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td  > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\" colspan=\"5\"> ");
            html.AppendLine("                        INFORMACIÓN DE ENDEUDAMIENTO</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                    <td  > ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("            </table> ");


            informacionEndeudamiento(((Usuario)Session["Rol"]).rol, ref html, objCliente);

            html.AppendLine("    <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" width=\"40%\"> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                        Anticipos de leasing:</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                        " + objCliente.leasingAnticipos.Trim() + " ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp; ");
            html.AppendLine("                        </td> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp; ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("            </table> ");
            html.AppendLine("<table border=\"0.5\" style=\"border-color:black; border-collapse:collapse\" width=\"100%\"> ");
            html.AppendLine("        <tr> ");
            html.AppendLine("            <td style=\"border-color:black\"> ");
            html.AppendLine("                &nbsp;</td> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle\"> ");
            html.AppendLine("                Saldo Capital</td> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle\"> ");
            html.AppendLine("                Días de Mora</td> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle\"> ");
            html.AppendLine("                Reestructurado</td> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle\"> ");
            html.AppendLine("                Calif. Externa Modelo SFC</td> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle\"> ");
            html.AppendLine("                Calif. Externa Actual</td> ");
            html.AppendLine("        </tr> ");
            html.AppendLine("        <tr> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                Bancolombia</td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                <label>" + objCliente.salgoKBanco.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label> ");
            html.AppendLine("                " + objCliente.diasMoraBanco.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label>" + objCliente.reestrucBanco.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label>" + objCliente.calExternaModeloBanco.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label> ");
            html.AppendLine("                " + objCliente.calEBanco.Trim() + "</label></td> ");
            html.AppendLine("        </tr> ");
            html.AppendLine("        <tr> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                Factoring</td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                <label >" + objCliente.saldoKFactoring.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.diasMoraFactoring.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.reestrucFactoring.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.calExternaModeloFactoring.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.calEFactoring.Trim() + "</label></td> ");
            html.AppendLine("        </tr> ");
            html.AppendLine("        <tr> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                Sufi</td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.salgoKSufi.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.diasMoraSufi.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.reestrucSufi.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.calExternaModeloSufi.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.calESufi.Trim() + "</label></td> ");
            html.AppendLine("        </tr> ");
            html.AppendLine("        <tr> ");
            html.AppendLine("            <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"  ");
            html.AppendLine("                class=\"style1\"> ");
            html.AppendLine("                Leasing</td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                <label > ");
            html.AppendLine("                " + objCliente.saldoKLeasing.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label style=\"font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                " + objCliente.diasMoraLeasing.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label style=\"font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                " + objCliente.reestructLeasing.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label style=\"font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                " + objCliente.calExternaModeloLeasing.Trim() + "</label></td> ");
            html.AppendLine("            <td style=\"border-color:black;text-align:center;font-family: Verdana;font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                <label style=\"font-family: Verdana;font-size: 7px;color:black;vertical-align:middle\"> ");
            html.AppendLine("                " + objCliente.calELeasing.Trim() + "</label></td> ");
            html.AppendLine("        </tr> ");
            html.AppendLine("    </table> ");
            html.AppendLine("    <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle\" width=\"50%\"> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:left;vertical-align:middle\"> ");
            html.AppendLine("                    EXTERIOR ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td> ");
            html.AppendLine("                        &nbsp;</td> ");
            html.AppendLine("                </tr> ");
            html.AppendLine("            </table> ");
            html.AppendLine("<table border=\"0.5\" style=\"border-color:black; border-collapse:collapse\" width=\"100%\"> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                    &nbsp;</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle;border:solid 1px black\"> ");
            html.AppendLine("                    Saldo K</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle;border:solid 1px black\"> ");
            html.AppendLine("                    Saldo I</td> ");
            // PMO19939 - RF061: Modificar visualización “Consulta Proceso Anteriores” del comité presencial
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle;border:solid 1px black\"> ");
            html.AppendLine("                    Días de Mora</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle;border:solid 1px black\"> ");
            html.AppendLine("                    Reestructurado</td> ");
            html.AppendLine("                    <td style=\"font-family: Verdana;font-size: 7px;color:black;font-weight: bold;text-align:center;vertical-align:middle;border:solid 1px black\"> ");
            html.AppendLine("                    Calif. Externa Actual</td> ");
            //
            html.AppendLine("                     ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                    Panamá</td> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label  >" + objCliente.saldoKPanama.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        " + objCliente.saldoIPanama.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            // PMO19939 - RF061: Modificar visualización “Consulta Proceso Anteriores” del comité presencial
            html.AppendLine("                    <td style=\"border-color:black;text-align:center;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label  >" + objCliente.diasMoraPanama.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:center;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label  >" + objCliente.reestructuradoPanama.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:center;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label  >" + objCliente.calExternaActualPanama.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            //
            html.AppendLine("                     ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                <tr> ");
            html.AppendLine("                    <td style=\"border-color:black;font-family: Verdana;font-size: 7px;color: black;text-align: left;vertical-align: middle;font-weight: bold\"> ");
            html.AppendLine("                    Puerto Rico</td> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label >" + objCliente.saldoKPuertoRico.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:right;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label >" + objCliente.saldoIPuertoRico.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            // PMO19939 - RF061: Modificar visualización “Consulta Proceso Anteriores” del comité presencial
            html.AppendLine("                    <td style=\"border-color:black;text-align:center;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label >" + objCliente.diasMoraPuertoRico.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:center;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label >" + objCliente.reestructuradoPuertoRico.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            html.AppendLine("                    <td style=\"border-color:black;text-align:center;font-family: Verdana; color:black;vertical-align:middle;font-size: 7px\"> ");
            html.AppendLine("                        <label >" + objCliente.calExternaActualPuertoRico.Trim() + "</label> ");
            html.AppendLine("                    </td> ");
            //
            html.AppendLine("                     ");
            html.AppendLine("                </tr> ");
            html.AppendLine("                 ");
            html.AppendLine("            </table> ");
            html.AppendLine("    &nbsp;<br /> ");
            html.AppendLine("   <table style=\"font-family: Verdana;font-size: 7px;color:black;text-align:left;vertical-align:middle;font-weight:bold\">");
            html.AppendLine("      <tr><td><label>Nota: Cifras en Pesos Colombianos</label></td></tr>");
            html.AppendLine("           </table>");

            ObtenerInformacionFinanciera(((Usuario)Session["Rol"]).rol, objCliente, ref html, ref errores);

            CalificacionesComentarios(((Usuario)Session["Rol"]).rol, ref html, objCliente, categoriaCliente);

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

        //PMO019939 - RF021: Modificar el campo “Razón Calificación” con su nueva estructura en el PDF
        private void processoRazCaliInt(string razCaliInt, ref StringBuilder html)
        {
            if (razCaliInt.Trim().Equals("0"))
            {
                html.AppendLine("                <label></label> ");
            }
            else
            {
                List<string> RazCaliInt = new List<string>();
                if (razCaliInt.Trim().Contains("|"))
                {
                    RazCaliInt = razCaliInt.Trim().Split('|').ToList();
                }
                else
                {
                    RazCaliInt.Add(razCaliInt.Trim());
                }

                foreach (string _razCaliInt in RazCaliInt)
                {
                    html.AppendLine("                <label>" + _razCaliInt + "</label><br/>");
                }
            }
        }
    }
}
