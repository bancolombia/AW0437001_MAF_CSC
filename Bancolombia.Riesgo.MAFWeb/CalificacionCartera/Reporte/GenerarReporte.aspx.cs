using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bancolombia.Riesgo.MAFWeb.Entidades.CalificacionCartera;
using Bancolombia.Riesgo.MAF.Entidades.CalificacionCartera;
using System.Data;
using System.IO;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Fachada = Bancolombia.Riesgo.MAF.Negocio.Fachadas.CalificacionCartera;

namespace Bancolombia.Riesgo.MAFWeb.CalificacionCartera.Reporte
{
    public partial class GenerarReporte : System.Web.UI.Page
    {
        /// <summary>
        /// Método de refresco de la página.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //si la sesión es nula redirige a la página del login, si es d perfil adminsitrador muestra error
            //porque no tiene permisos para ingresar, de lo contrario, realiza las cargas segun el perfil

            if (Session["Rol"] == null)
            {
                Response.Redirect("../Login.aspx");
                Response.End();
            }
            else if (((Usuario)Session["Rol"]).rol.Equals("Administrador"))
            {
                ErroresEntity error = new ErroresEntity();
                error.Error = "MEN.0391";
                error.Url = "../CargarArchivo.aspx";
                error.Log = "Ingreso a sesión con usuario no permitido";
                Session["Error"] = error;
                Response.Redirect("../Errores/Error.aspx");
                Response.End();
            }
            if (!IsPostBack)
            {
                //ToolkitScriptManager1.RegisterPostBackControl(btnGenerar);
                pintarCampos();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            }

        }

        /// <summary>
        /// Consulta la información de las entidades y las muestra en el formulario
        /// </summary>
        protected void ConsultarEntidades()
        {
            //llenar listas
            //Proxy.CalificacionCartera cartera = new Proxy.CalificacionCartera();

            ObjetosCalificacion objEntidades = Fachada.CalificacionCartera.ConsultarEntidades();

            if (objEntidades.codigo.Trim().Equals("0000"))
            {
                //llenar listas

                Parametros[] parametros = Fachada.CalificacionCartera.ObtenerParametros("1", " ", " ", " ", "1", " ", " ", " ", " ", " ", " ", " ", " ", "0").ToArray();

                //entidad
                ddlEntidad.DataSource = parametros.Where(p => p.paramName.Trim().Contains("LENTIDAD") && p.paramSeq != 0 && string.IsNullOrEmpty(p.param6.Trim())).OrderBy(p => p.paramSeq);
                ddlEntidad.DataTextField = "param7";
                ddlEntidad.DataValueField = "param7";
                ddlEntidad.DataBind();

                //banca
                ddlBanca.DataSource = parametros.Where(p => p.paramName.Trim().Contains("LBANCA") && p.paramSeq != 0 && string.IsNullOrEmpty(p.param6.Trim())).OrderBy(p => p.paramSeq);
                ddlBanca.DataTextField = "param7";
                ddlBanca.DataValueField = "param7";
                ddlBanca.DataBind();

                //region
                ddlRegion.DataSource = parametros.Where(p => p.paramName.Trim().Contains("LREGION") && p.paramSeq != 0 && string.IsNullOrEmpty(p.param6.Trim())).OrderBy(p => p.paramSeq);
                ddlRegion.DataTextField = "param7";
                ddlRegion.DataValueField = "param7";
                ddlRegion.DataBind();

                //zona
                ddlZona.DataSource = parametros.Where(p => p.paramName.Trim().Contains("LZONA") && p.paramSeq != 0 && string.IsNullOrEmpty(p.param6.Trim())).OrderBy(p => p.paramSeq);
                ddlZona.DataTextField = "param7";
                ddlZona.DataValueField = "param7";
                ddlZona.DataBind();

                //estado
                ddlEstado.DataSource = parametros.Where(p => p.paramName.Trim().Contains("LESTCALIF") && p.paramSeq != 0 && string.IsNullOrEmpty(p.param6.Trim())).OrderBy(p => p.paramSeq);
                ddlEstado.DataTextField = "param7";
                ddlEstado.DataValueField = "param7";
                ddlEstado.DataBind();

                ListItem liBanca = new ListItem("Seleccione...", "-1");
                ddlBanca.Items.Insert(0, liBanca);

                foreach (ListItem li in ddlEntidad.Items)
                {
                    li.Attributes.Add("title", li.Text);
                }

                ListItem liEntidad = new ListItem("Seleccione...", "-1");
                ddlEntidad.Items.Insert(0, liEntidad);

                foreach (ListItem li in ddlBanca.Items)
                {
                    li.Attributes.Add("title", li.Text);
                }

                ListItem liRegion = new ListItem("Seleccione...", "-1");
                ddlRegion.Items.Insert(0, liRegion);

                foreach (ListItem li in ddlRegion.Items)
                {
                    li.Attributes.Add("title", li.Text);
                }

                ListItem liZona = new ListItem("Seleccione...", "-1");
                ddlZona.Items.Insert(0, liZona);

                foreach (ListItem li in ddlZona.Items)
                {
                    li.Attributes.Add("title", li.Text);
                }

                ListItem liEstado = new ListItem("Seleccione...", "-1");
                ddlEstado.Items.Insert(0, liZona);

                foreach (ListItem li in ddlEstado.Items)
                {
                    li.Attributes.Add("title", li.Text);
                }
            }
            else
            {
                throw new Exception("ERRORSP@" + objEntidades.codigo + "/" + objEntidades.descripcion);
            }

        }

        /// <summary>
        /// dependiendo del rol que ingrese a la pantalla, pinta u oculta los páneles correspondientes a este
        /// </summary>
        protected void pintarCampos()
        {
            try
            {
                Master.mpTitulo = "Generación Reporte";

                lblUsuario.Text = ((Usuario)Session["Rol"]).usuario;

                if (DateTime.Today.Month <= 6)
                    lblFechaComite.Text = DateTime.Today.Year.ToString() + "06";
                else
                    lblFechaComite.Text = DateTime.Today.Year.ToString() + "12";

                //Show only the controls for Superfinanciera/Consulta
                Usuario user = new Usuario();
                user = (Usuario)Session["Rol"];
                if (user.rol == "Superfinanciera" || user.rol == "Consulta")
                {
                    tblReporteMasivo.Visible = false;
                    tblExportarLog.Visible = false;
                }

                ConsultarEntidades();
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CalificacionSemCartera";
                if (ex.Message.ToString().Contains("ERRORSP@"))
                    errEnt.Error = "ERRORSP@";
                else
                    errEnt.Error = "MEN.0457";
                Session["Error"] = errEnt;
                Response.Redirect("../Errores/Error.aspx");
            }
        }


        /// <summary>
        /// Permite generar el reporte en formato excel de las entidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            string parIndicador = "0";
            if (ddlEntidad.SelectedValue.ToString().Trim().ToUpper().Equals("TODOS"))
                parIndicador = "1";
            else
                parIndicador = "0";

            string entidad = string.Empty;
            string banca = string.Empty;
            string region = string.Empty;
            string zona = string.Empty;
            string estado = string.Empty;
            string usuarioRiesgos = string.Empty;
            //Se recupera el valor de las listas y se asigna valor a las variables
            entidad = (ddlEntidad.SelectedValue.Equals("-1") ? " " : ddlEntidad.SelectedValue);
            banca = (ddlBanca.SelectedValue.Equals("-1") ? " " : ddlBanca.SelectedValue);
            region = (ddlRegion.SelectedValue.Equals("-1") ? " " : ddlRegion.SelectedValue);
            zona = (ddlZona.SelectedValue.Equals("-1") ? " " : ddlZona.SelectedValue);
            estado = (ddlEstado.SelectedValue.Equals("-1") ? " " : ddlEstado.SelectedValue);
            usuarioRiesgos = (txtUsrRiesgos.Text.Equals(string.Empty) ? " " : txtUsrRiesgos.Text);



            ObjetosCalificacion objReporte = new ObjetosCalificacion();
            //Proxy.CalificacionCartera proxyCartera = new Proxy.CalificacionCartera();
            try
            {
                //PMO19939 - RF026-27
                if (ddlEntidad.SelectedIndex == 0)
                {
                    throw new Exception("ERRORSP@0001/Parametros No Validos");
                }

                //Session["Entidades"] = clientes();
                //GuardarComo("DescargarArchivo.aspx");
                //objReporte = proxyCartera.ConsultarReporteEntidades(ddlEntidad.SelectedValue.ToString(), parIndicador);

                //Invoking the new method. Waiting for changes on Webservice.
                objReporte = Fachada.CalificacionCartera.ConsultarReporteEntidad(entidad, parIndicador, region, banca, zona, estado, usuarioRiesgos);

                if (objReporte.codigo.Trim().Equals("0000"))
                {
                    if (objReporte.setDatos != null && objReporte.setDatos.Tables.Count > 0 && objReporte.setDatos.Tables[0].Rows.Count > 0)
                    {
                        Session["Entidades"] = objReporte.setDatos.Tables[0];

                        //PMO19939_REQ001_Exportar Log y Resultado del proceso Masivo.
                        GenerarReporteEntidades(objReporte.setDatos.Tables[0], "\\ReporteEntidades_{0}{1}");
                        //GuardarComo("DescargarArchivo.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Aviso", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0459").Texto + "');", true);
                    }
                }
                else
                {
                    throw new Exception("ERRORSP@0001/Error consultando los datos");
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CalificacionSemCartera";
                if (ex.Message.ToString().Contains("ERRORSP@"))
                    errEnt.Error = "ERRORSP@";
                else
                    errEnt.Error = "MEN.0458";
                Session["Error"] = errEnt;
                Response.Redirect("../Errores/Error.aspx");
            }
            finally
            {
                objReporte = null;
                //proxyCartera = null;
            }

            //GenerarReporteEntidades(clientes());


        }

        /// <summary>
        /// Realiza la transformación del reporte a formato excel
        /// </summary>
        /// 
        // PMO19939_REQ001_Exportar Iniciar y Resultado del Proceso Masivo.
        protected void GenerarReporteEntidades(DataTable entidades, string nomArchivo)
        {
            string RutaExcel = System.AppDomain.CurrentDomain.BaseDirectory + "CalificacionCartera\\Archivos";//Environment.CurrentDirectory;
            //PMO19939_REQ001_Exportar Log y Resultado del proceso Masivo.
            string NombreArchivo = string.Format(nomArchivo, HttpContext.Current.User.Identity.Name.Remove(0, HttpContext.Current.User.Identity.Name.IndexOf("\\") + 1), ".xls");
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
                    output.Append(value.ToString().Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' ').Replace('"', '\''));
                    output.Append(FIELDSEPARATOR);
                }
                // Escribir una línea de registro        
                output.Append(ROWSEPARATOR);
            }
            // Valor de retorno    
            // output.ToString();
            StreamWriter sw = new StreamWriter(RutaExcel + NombreArchivo, false, System.Text.Encoding.GetEncoding("iso-8859-1"));
            sw.Write(output.ToString());
            sw.Close();

            string fileName = NombreArchivo.Replace("\\", string.Empty).Replace(".xls", string.Empty);
            //Proxy.CalificacionCartera calificacion = new Proxy.CalificacionCartera();
            Fachada.CalificacionCartera.ActualizarLogCSC("", "23", ((Usuario)Session["Rol"]).usuario,
                                ((Usuario)Session["Rol"]).rol.ToUpper(), "", "", "", "", fileName);

            //PMO19939_REQ001_Exportar Log y Resultado del proceso Masivo.
            GuardarComo(RutaExcel + NombreArchivo, nomArchivo);

        }

        /// <summary>
        /// Habilita la opción de almacenar la información en disco local o abrirlo
        /// </summary>
        /// 
        //PMO19939_REQ001_Exportar Log y Resultado del proceso Masivo.
        protected void GuardarComo(string ruta, string header)
        {
            FileInfo file = new FileInfo(ruta);
            Session["Reporte"] = ruta;
            Response.ContentType = "application/ms-excel";
            //PMO19939_REQ001_Exportar Log y Resultado del proceso Masivo
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + string.Format(header, HttpContext.Current.User.Identity.Name.Remove(0, HttpContext.Current.User.Identity.Name.IndexOf("\\") + 1), ".xls"));
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.TransmitFile(ruta);
            Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();

            //OpenNewBrowserWindow(ruta, this);
        }

        protected void OpenNewBrowserWindow(string Url, Control control)
        {

            ScriptManager.RegisterStartupScript(control, control.GetType(), "Open", "window.open('" + Url + "');", true);

        }

        protected DataTable clientes()
        {
            DataTable dtClientes = new DataTable();

            DataColumn dt1 = new DataColumn("CARNITCLI");
            DataColumn dt2 = new DataColumn("CARNOMCLI");
            DataColumn dt3 = new DataColumn("CARENTIDAD");
            DataColumn dt4 = new DataColumn("CARFESFROS");
            DataColumn dt5 = new DataColumn("CARCALIPOR");
            DataColumn dt6 = new DataColumn("CRIESTCALI");
            DataColumn dt7 = new DataColumn("CARBANCA");
            DataColumn dt8 = new DataColumn("CARREGIONA");
            DataColumn dt9 = new DataColumn("CARZONA");
            DataColumn dt10 = new DataColumn("CRICALINRE");
            DataColumn dt11 = new DataColumn("CRICALINRA");
            DataColumn dt12 = new DataColumn("CRICALNRRE");
            DataColumn dt13 = new DataColumn("CRICINRANR");
            DataColumn dt14 = new DataColumn("CRICALEXRA");
            DataColumn dt15 = new DataColumn("CRIACTCLIP");
            DataColumn dt16 = new DataColumn("CRIANALPYP");
            DataColumn dt17 = new DataColumn("CRIANALBAP");
            DataColumn dt18 = new DataColumn("CRICOMENTP");

            dtClientes.Columns.Add(dt1);
            dtClientes.Columns.Add(dt2);
            dtClientes.Columns.Add(dt3);
            dtClientes.Columns.Add(dt4);
            dtClientes.Columns.Add(dt5);
            dtClientes.Columns.Add(dt6);
            dtClientes.Columns.Add(dt7);
            dtClientes.Columns.Add(dt8);
            dtClientes.Columns.Add(dt9);
            dtClientes.Columns.Add(dt10);
            dtClientes.Columns.Add(dt11);
            dtClientes.Columns.Add(dt12);
            dtClientes.Columns.Add(dt13);
            dtClientes.Columns.Add(dt14);
            dtClientes.Columns.Add(dt15);
            dtClientes.Columns.Add(dt16);
            dtClientes.Columns.Add(dt17);
            dtClientes.Columns.Add(dt18);

            for (int i = 0; i < 500; i++)
            {
                DataRow dr1 = dtClientes.NewRow();
                dtClientes.Rows.Add(dr1);
                for (int j = 0; j < 18; j++)
                {
                    dtClientes.Rows[i][j] = "Prueba" + i.ToString() + j.ToString();
                }
            }

            return dtClientes;
        }

        /// <summary>
        /// PMO19939-RF010 Descargar el archivo “Log de cambios” a Excel desde el perfil de Riesgos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportarLog_Click(object sender, EventArgs e)
        {
            try
            {
                // PMO19939_REQ001_Exportar Iniciar y Resultado del Proceso Masivo
                if (ValidarExportarLog())
                {

                    //Proxy.CalificacionCartera calificacion = new Proxy.CalificacionCartera();
                    ObjetosCalificacion objReporte = new ObjetosCalificacion();

                    if (rdoLogPresencial.Checked)
                    {
                        objReporte = Fachada.CalificacionCartera.ExportarLog("7", "");
                    }
                    else if (rdoLogMasivo.Checked)
                    {
                        objReporte = Fachada.CalificacionCartera.ExportarLog("8", "");
                    }
                    else if (rdoLogSobreUnCliente.Checked)
                    {
                        objReporte = Fachada.CalificacionCartera.ExportarLog("9", txtNitCliente.Text);
                    }

                    if (objReporte.codigo.Trim().Equals("0000"))
                    {
                        if (objReporte.setDatos != null && objReporte.setDatos.Tables.Count > 0 && objReporte.setDatos.Tables[0].Rows.Count > 0)
                        {


                            GenerarReporteEntidades(objReporte.setDatos.Tables[0], "\\LogCSC_{0}{1}");
                            //GuardarComo("DescargarArchivo.aspx");
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Aviso", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0459").Texto + "');", true);
                        }
                    }
                    else
                    {
                        throw new Exception("ERRORSP@" + objReporte.codigo + "/" + objReporte.descripcion);
                    }
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:ExportarCalificacion";
                if (ex.Message.ToString().Contains("ERRORSP@"))
                    errEnt.Error = "ERRORSP@";
                else
                    errEnt.Error = "MEN.0458";
                Session["Error"] = errEnt;
                Response.Redirect("../Errores/Error.aspx");
            }
        }

        /// <summary>
        /// Validar Exportar Log
        /// </summary>
        /// <returns>True/False</returns>
        private bool ValidarExportarLog()
        {
            bool resp = true;
            Int32 nitCliente;

            // Check if enteredtxtNitCliMasivo is numeric
            bool isNumeric = Int32.TryParse(txtNitCliente.Text.Trim(), out nitCliente);

            if (!rdoLogPresencial.Checked && !rdoLogMasivo.Checked && !rdoLogSobreUnCliente.Checked)
            {
                Mensajes.Mensajes.MostrarMensaje("MAF", "MEN.0472", this);
                resp = false;
            }
            else if (rdoLogSobreUnCliente.Checked && (txtNitCliente.Text == string.Empty || !isNumeric))
            {
                Mensajes.Mensajes.MostrarMensaje("MAF", "MEN.0473", this);
                resp = false;
            }
            return resp;
        }

        protected void btnGenerarRepMasivo_Click(object sender, EventArgs e)
        {
            try
            {
                //PMO19939_REQ001_Exportar Log y Resultado del proceso Masivo.
                //Proxy.CalificacionCartera calificacion = new Proxy.CalificacionCartera();
                ObjetosCalificacion objReporte = new ObjetosCalificacion();
                objReporte = Fachada.CalificacionCartera.DescargaArchivoMasivo(string.Empty);

                if (objReporte.codigo.Trim().Equals("0000"))
                {
                    if (objReporte.setDatos != null && objReporte.setDatos.Tables.Count > 0 && objReporte.setDatos.Tables[0].Rows.Count > 0)
                    {
                        GenerarReporteEntidades(objReporte.setDatos.Tables[0], "\\Descarga_PMasivo_{0}{1}");
                        //GuardarComo("DescargarArchivo.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Aviso", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0459").Texto + "');", true);
                    }
                }
                else
                {
                    throw new Exception("ERRORSP@" + objReporte.codigo + "/" + objReporte.descripcion);
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:GeneracionReporteMasivo";
                if (ex.Message.ToString().Contains("ERRORSP@"))
                    errEnt.Error = "ERRORSP@";
                else
                    errEnt.Error = "MEN.0458";
                Session["Error"] = errEnt;
                Response.Redirect("../Errores/Error.aspx");
            }

        }
    }
}


