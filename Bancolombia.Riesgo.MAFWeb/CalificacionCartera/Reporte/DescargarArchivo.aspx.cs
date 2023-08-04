using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Bancolombia.Riesgo.MAFWeb.CalificacionCartera.Reporte
{
    public partial class DescargarArchivo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dtClientes = (DataTable)Session["Entidades"];
            GenerarReporteEntidades(dtClientes);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
        }

        /// <summary>
        /// Genera el excel con la información de la tabla
        /// </summary>
        /// <param name="entidades"></param>
        protected void GenerarReporteEntidades(DataTable entidades)
        {
            string RutaExcel = System.AppDomain.CurrentDomain.BaseDirectory + "CalificacionCartera\\Archivos";//Environment.CurrentDirectory;
            string NombreArchivo = string.Format("\\ReporteEntidades_{0}{1}", HttpContext.Current.User.Identity.Name.Remove(0, HttpContext.Current.User.Identity.Name.IndexOf("\\") + 1), ".xls");
            const string FIELDSEPARATOR = "\t";
            const string ROWSEPARATOR = "\n";
            StringBuilder output = new StringBuilder();
            // Escribir encabezados    
            foreach (DataColumn dc in entidades.Columns)
            {
                output.Append(dc.ColumnName);
                output.Append(FIELDSEPARATOR);
            }
            output.Append(ROWSEPARATOR);
            foreach (DataRow item in entidades.Rows)
            {
                foreach (object value in item.ItemArray)
                {
                    //output.Append(value.ToString().Replace('\n', ' ').Replace('\r', ' ').Replace('.', ','));
                    output.Append(value.ToString().Replace('\n', ' ').Replace('\r', ' '));
                    output.Append(FIELDSEPARATOR);
                }
                // Escribir una línea de registro        
                output.Append(ROWSEPARATOR);
            }
            // Valor de retorno    
            // output.ToString();
            StreamWriter sw = new StreamWriter(RutaExcel + NombreArchivo, false, System.Text.Encoding.GetEncoding("iso-8859-1"));
            //StreamWriter sw = new StreamWriter(RutaExcel + NombreArchivo);
            sw.Write(output.ToString());
            sw.Close();

            GuardarComo(RutaExcel + NombreArchivo);

        }

        /// <summary>
        /// Habilita la opción de almacenar la información en disco local o abrirlo
        /// </summary>
        protected void GuardarComo(string ruta)
        {
            Session["Reporte"] = ruta;
            Response.ContentType = "application/ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + string.Format("ReporteEntidades_{0}{1}", HttpContext.Current.User.Identity.Name.Remove(0, HttpContext.Current.User.Identity.Name.IndexOf("\\") + 1), ".xls"));
            Response.TransmitFile(ruta);
            Response.End();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseMe", "window.close();", true);
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}
