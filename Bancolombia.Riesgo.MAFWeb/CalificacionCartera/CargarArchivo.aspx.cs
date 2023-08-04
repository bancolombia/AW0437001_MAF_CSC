using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bancolombia.Riesgo.MAFWeb.Entidades.CalificacionCartera;
using System.IO;
using Fachada = Bancolombia.Riesgo.MAF.Negocio.Fachadas.CalificacionCartera;
using Bancolombia.Riesgo.MAFWeb.Mensajes;
using Bancolombia.Riesgo.MAF.Entidades.CalificacionCartera;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Microsoft.Office.Interop.Excel;

namespace Bancolombia.Riesgo.MAFWeb.CalificacionCartera
{
    public partial class CargarArchivo : System.Web.UI.Page
    {

        /// <summary>
        /// Método de refresco de la página donde se valida si existe sesión activa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //si la sesión es nula redirige al login
            //si es diferente de administrador muestra un error, sino, muestra los campos para el administrador
            if (Session["Rol"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else if (!((Usuario)Session["Rol"]).rol.Equals("Administrador"))
            {
                ErroresEntity error = new ErroresEntity();
                error.Error = "MEN.0391";
                error.Url = "../CalificacionSemCartera.aspx";
                error.Log = "Ingreso a sesión con usuario no permitido";
                Session["Error"] = error;
                Response.Redirect("Errores/Error.aspx");
            }
            if (!IsPostBack)
            {
                Master.mpTitulo = "Carga de Archivos - Administrador";
                Master.mpRol = ((Usuario)Session["Rol"]).rol;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            }

        }

        /// <summary>
        /// Realiza la carga del archivo seleccionado en el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["parametros"] != null)
                {
                    Parametros[] param = (Parametros[])Session["parametros"];

                    //Valida la existencia de parámetros
                    if (param.Length == 0)
                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0420").Texto);

                    //verificar existencia de ruta y archivo definidos
                    IEnumerable<Parametros> rutas = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("TWEBOTAC") && p.paramSeq == 1).OrderBy(p => p.paramSeq);
                    IEnumerable<Parametros> rutas2 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("TWEBOTAC") && p.paramSeq == 3).OrderBy(p => p.paramSeq);
                    if (rutas.ToArray().Length == 0 || rutas.ToArray()[0].param3.Trim().Equals(string.Empty))
                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0421").Texto);
                    if (rutas2.ToArray().Length == 0 || rutas.ToArray()[0].param3.Trim().Equals(string.Empty))
                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0438").Texto);

                    lblMensaje.Text = "";
                    lblExito.Text = "";
                    //Verifica si tiene un archivo seleccionado


                    //Ejecuta el procedimiento almacenado para carga del archivo desde la ruta del servidor al IFS
                    //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
                    string cargar = Fachada.CalificacionCartera.CargaArchivo("asdasd");

                    //Carga el mensaje de éxito cuando no ocurren errores en el sistema
                    if (cargar.Trim().ToLower().Contains("registros"))
                    {
                        Fachada.CalificacionCartera.ActualizarLogCSC("", "22", ((Usuario)Session["Rol"]).usuario,
                                "ADMINISTRADOR", "", "", "", "", rutas.ToArray()[0].param3);

                        if (rutas2.ToArray()[0].param4.Trim().Equals("S"))
                        {
                            //string ruta = System.Configuration.ConfigurationSettings.AppSettings["RutaCargaArchivo"].ToString().Replace(@"\", "/");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Carga", "javascript:alert('" + cargar.Trim() + ". " + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0430").Texto + "');location.reload();", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Carga", "javascript:alert('" + cargar.Trim() + ". " + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0436").Texto + "');location.reload();", true);
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Carga", "javascript:alert('" + cargar.Trim() + "');location.reload();", true);
                    }


                }
                else
                {
                    throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0420").Texto);
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CargarArchivo";
                errEnt.Error = "MEN.0410";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }

        /// <summary>
        /// Convierte el excel a formato csv para accederse desde iseries
        /// </summary>
        /// <param name="sourceFile">ubicación del archivo excel</param>
        /// <param name="targetFile">nueva ubicación y nombre del archivo resultante</param>
        protected void convertirACSV(string sourceFile, string targetFile)
        {

            Application app = new ApplicationClass();
            Workbook WB = app.Workbooks.Open(sourceFile, true, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            Sheets WS = WB.Worksheets;
            WB.SaveCopyAs(targetFile);

            WB.Close(false, "", true);
        }

        /// <summary>
        /// Realiza la exportación del archivo ubicado en una ruta especificada
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="archivo"></param>
        protected void exportar(string ruta, string archivo)
        {
            string _DownloadableProductFileName = archivo;

            System.IO.FileInfo FileName = new System.IO.FileInfo(ruta + _DownloadableProductFileName);
            FileStream myFile = new FileStream(ruta + _DownloadableProductFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            //Lee el archivo como binario
            BinaryReader _BinaryReader = new BinaryReader(myFile);


            //Verifica si el archivo existe en la ubicacion
            if (FileName.Exists)
            {
                long startBytes = 0;
                string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(ruta).ToString("r");
                string _EncodedData = HttpUtility.UrlEncode(_DownloadableProductFileName, Encoding.UTF8) + lastUpdateTiemStamp;

                Response.Clear();
                Response.Buffer = false;
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName.Name);
                Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                Response.AddHeader("Connection", "Keep-Alive");
                Response.ContentEncoding = Encoding.UTF8;

                //Envia los datos
                _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                //Divide los datos en paquetes de 1024 bytes 
                int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

                //Descarga en bloques de 1024 bytes
                int i;
                for (i = 0; i < maxCount && Response.IsClientConnected; i++)
                {
                    Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                    Response.Flush();
                }
            }
            else
            {
                throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0428").Texto);
            }
        }

        protected void btnCargarCric_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["parametros"] != null)
                {
                    Parametros[] param = (Parametros[])Session["parametros"];

                    //Valida la existencia de parámetros
                    if (param.Length == 0)
                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0420").Texto);

                    //verificar existencia de ruta y archivo definidos
                    IEnumerable<Parametros> rutas = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("TWEBOTAC") && p.paramSeq == 7).OrderBy(p => p.paramSeq);
                    if (rutas.ToArray().Length == 0 || rutas.ToArray()[0].param3.Trim().Equals(string.Empty))
                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0421").Texto);
                   
                    lblMensaje.Text = "";
                    lblExito.Text = "";
                    //Verifica si tiene un archivo seleccionado

                    //Ejecuta el procedimiento almacenado para carga del archivo desde la ruta del servidor al IFS
                    //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
                    string cargar = Fachada.CalificacionCartera.CargaArchivoMasivo("asdasd");


                    //Carga el mensaje de éxito cuando no ocurren errores en el sistema
                    if (cargar.Trim().Equals(string.Empty))
                    {
                        Fachada.CalificacionCartera.ActualizarLogCSC("", "22", ((Usuario)Session["Rol"]).usuario,
                                "ADMINISTRADOR", "", "", "", "", rutas.ToArray()[0].param3);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Carga", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0392").Texto + "');location.reload();", true);

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Carga", "javascript:alert('" + cargar.Trim() + "');location.reload();", true);
                    }


                }
                else
                {
                    throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0420").Texto);
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CargarArchivo";
                errEnt.Error = "MEN.0410";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }

        protected void btnCargarPEC_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["parametros"] != null)
                {
                    Parametros[] param = (Parametros[])Session["parametros"];

                    //Valida la existencia de parámetros
                    if (param.Length == 0)
                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0420").Texto);


                    //verificar existencia de ruta y archivo definidos
                    IEnumerable<Parametros> rutas = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("TWEBOTAC") && p.paramSeq == 4).OrderBy(p => p.paramSeq);

                    if (rutas.ToArray().Length == 0 || rutas.ToArray()[0].param3.Trim().Equals(string.Empty))
                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0445").Texto);

                    lblMensaje.Text = "";
                    lblExito.Text = "";
                    //Verifica si tiene un archivo seleccionado


                    //Ejecuta el procedimiento almacenado para carga del archivo desde la ruta del servidor al IFS
                    //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
                    string cargar = Fachada.CalificacionCartera.CargaArchivoPEC();


                    Fachada.CalificacionCartera.ActualizarLogCSC("", "22", ((Usuario)Session["Rol"]).usuario,
                                "ADMINISTRADOR", "", "", "", "", rutas.ToArray()[0].param3);


                    //Carga el mensaje de éxito cuando no ocurren errores en el sistema
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Carga", "javascript:alert('" + cargar.Substring(cargar.LastIndexOf('/') + 1).Trim() + "');location.reload();", true);
                }
                else
                {
                    throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0445").Texto);
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CargarArchivo";
                errEnt.Error = "MEN.0410";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }

        protected void btnCovenants_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["parametros"] != null)
                {
                    Parametros[] param = (Parametros[])Session["parametros"];

                    //Valida la existencia de parámetros
                    if (param.Length == 0)
                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0420").Texto);

                    //verificar existencia de ruta y archivo definidos
                    IEnumerable<Parametros> rutas = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("TWEBOTAC") && p.paramSeq == 5).OrderBy(p => p.paramSeq);

                    if (rutas.ToArray().Length == 0 || rutas.ToArray()[0].param3.Trim().Equals(string.Empty))
                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0446").Texto);

                    lblMensaje.Text = "";
                    lblExito.Text = "";
                    //Verifica si tiene un archivo seleccionado


                    //Ejecuta el procedimiento almacenado para carga del archivo desde la ruta del servidor al IFS
                    //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
                    string cargar = Fachada.CalificacionCartera.CargaArchivoCovenants();

                    Fachada.CalificacionCartera.ActualizarLogCSC("", "22", ((Usuario)Session["Rol"]).usuario,
                                "ADMINISTRADOR", "", "", "", "", rutas.ToArray()[0].param3);


                    //Carga el mensaje de éxito cuando no ocurren errores en el sistema
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Carga", "javascript:alert('" + cargar.Substring(cargar.LastIndexOf('/') + 1).Trim() + "');location.reload();", true);



                }
                else
                {
                    throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0446").Texto);
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CargarArchivo";
                errEnt.Error = "MEN.0410";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }

        protected void btnProrrogas_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["parametros"] != null)
                {
                    Parametros[] param = (Parametros[])Session["parametros"];

                    //Valida la existencia de parámetros
                    if (param.Length == 0)
                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0420").Texto);

                    //verificar existencia de ruta y archivo definidos
                    IEnumerable<Parametros> rutas = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("TWEBOTAC") && p.paramSeq == 6).OrderBy(p => p.paramSeq);

                    if (rutas.ToArray().Length == 0 || rutas.ToArray()[0].param3.Trim().Equals(string.Empty))
                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0447").Texto);

                    lblMensaje.Text = "";
                    lblExito.Text = "";
                    //Verifica si tiene un archivo seleccionado


                    //Ejecuta el procedimiento almacenado para carga del archivo desde la ruta del servidor al IFS
                    //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
                    string cargar = Fachada.CalificacionCartera.CargaArchivoProrrogas();

                    Fachada.CalificacionCartera.ActualizarLogCSC("", "22", ((Usuario)Session["Rol"]).usuario,
                                "ADMINISTRADOR", "", "", "", "", rutas.ToArray()[0].param3);

                    //Carga el mensaje de éxito cuando no ocurren errores en el sistema
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Carga", "javascript:alert('" + cargar.Substring(cargar.LastIndexOf('/') + 1).Trim() + "');location.reload();", true);



                }
                else
                {
                    throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0447").Texto);
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CargarArchivo";
                errEnt.Error = "MEN.0410";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }

        protected void btnIndicadores_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["parametros"] != null)
                {
                    Parametros[] param = (Parametros[])Session["parametros"];

                    //Valida la existencia de parámetros
                    if (param.Length == 0)
                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0420").Texto);

                    //verificar existencia de ruta y archivo definidos
                    IEnumerable<Parametros> rutas = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("TWEBOTAC") && p.paramSeq == 8).OrderBy(p => p.paramSeq);

                    if (rutas.ToArray().Length == 0 || rutas.ToArray()[0].param3.Trim().Equals(string.Empty))
                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0447").Texto);

                    lblMensaje.Text = "";
                    lblExito.Text = "";
                    //Verifica si tiene un archivo seleccionado


                    //Ejecuta el procedimiento almacenado para carga del archivo desde la ruta del servidor al IFS
                    //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
                    string cargar = Fachada.CalificacionCartera.CargaArchivoIndicadores();

                    Fachada.CalificacionCartera.ActualizarLogCSC("", "22", ((Usuario)Session["Rol"]).usuario,
                                "ADMINISTRADOR", "", "", "", "", rutas.ToArray()[0].param3);

                    //Carga el mensaje de éxito cuando no ocurren errores en el sistema
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Carga", "javascript:alert('" + cargar.Substring(cargar.LastIndexOf('/') + 1).Trim() + "');location.reload();", true);



                }
                else
                {
                    throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0447").Texto);
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CargarArchivo";
                errEnt.Error = "MEN.0410";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }


        protected void btnCargarPrMasivo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["parametros"] != null)
                {
                    Parametros[] param = (Parametros[])Session["parametros"];

                    //Valida la existencia de parámetros
                    if (param.Length == 0)
                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0420").Texto);

                    //verificar existencia de ruta y archivo definidos
                    IEnumerable<Parametros> rutas = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("TWEBOTAC") && p.paramSeq == 10).OrderBy(p => p.paramSeq);

                    if (rutas.ToArray().Length == 0 || rutas.ToArray()[0].param3.Trim().Equals(string.Empty))
                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0447").Texto);

                    lblMensaje.Text = "";
                    lblExito.Text = "";
                    //Verifica si tiene un archivo seleccionado


                    //Ejecuta el procedimiento almacenado para carga del archivo desde la ruta del servidor al IFS
                    //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
                    string cargar = Fachada.CalificacionCartera.CargaArchivoPrMasivo();

                    Fachada.CalificacionCartera.ActualizarLogCSC("", "22", ((Usuario)Session["Rol"]).usuario,
                                "ADMINISTRADOR", "", "", "", "", rutas.ToArray()[0].param3);

                    //Carga el mensaje de éxito cuando no ocurren errores en el sistema
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Carga", "javascript:alert('" + cargar.Substring(cargar.LastIndexOf('/') + 1).Trim() + "');location.reload();", true);



                }
                else
                {
                    throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0447").Texto);
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CargarArchivo";
                errEnt.Error = "MEN.0410";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }


    }
}
