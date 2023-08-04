using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bancolombia.Riesgo.MAFWeb.Entidades.CalificacionCartera;
using System.Data;

using Fachada = Bancolombia.Riesgo.MAF.Negocio.Fachadas.CalificacionCartera;
using System.Globalization;
using Bancolombia.Riesgo.MAF.Entidades.CalificacionCartera;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Bancolombia.Riesgo.MAFWeb.CalificacionCartera
{
    public partial class CalificacionClienteFinm : System.Web.UI.Page
    {

        #region "Eventos1"

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Rol"] == null)
            {
                Response.Redirect("Login.aspx");
                Response.End();
            }
            else if (((Usuario)Session["Rol"]).rol.Equals("Administrador"))
            {
                ErroresEntity error = new ErroresEntity();
                error.Error = "MEN.0391";
                error.Url = "../CargarArchivo.aspx";
                error.Log = "Ingreso a sesión con usuario no permitido";
                Session["Error"] = error;
                Response.Redirect("Errores/Error.aspx");
                Response.End();
            }
            if (!IsPostBack)
            {
                ViewState["Cliente"] = null;
                ViewState["Cambios"] = null;
                Master.mpTitulo = "Calificación de Cliente";
                lblUsuario.Text = ((Usuario)Session["Rol"]).usuario;

                llenarCliente(Request.QueryString["Codigo"].ToString());
                StringBuilder mensaje = new StringBuilder();
                if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
                {
                    mensaje.AppendLine("Los campos con * son obligatorios.");
                }
                else if (((Usuario)Session["Rol"]).rol.Equals("Riesgos "))
                {
                    mensaje.AppendLine("Los campos con * son obligatorios y/o tienen formato incorrecto:");
                    mensaje.AppendLine("Numérico: Número entero o número decimal positivo con (,) como separador y máximo dos decimales.");
                }

                vsErrores.HeaderText = mensaje.ToString();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            }
        }

        protected void cmdOk_Click(object sender, EventArgs e)
        {
            Cliente objCliente = (Cliente)ViewState["Cliente"];
            string respuesta = "";
            

            if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
            {
                if (validarPreguntas())
                {
                    establecerEncodingCamposTexto();
                    objCliente.calIntRec = " ";
                    objCliente.calNuevoRatRecom = (ddlCalNRRecom.SelectedValue.Trim().Equals("0") ? " " : ddlCalNRRecom.SelectedValue.Trim());

                    objCliente.SustentacionCalRecCom = (txtSustentacionCalRec.Text.Trim().Equals(string.Empty) ? " " : txtSustentacionCalRec.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta1Com = (txtRespuestaPregunta1.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta1.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta2Com = (txtRespuestaPregunta2.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta2.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta3Com = (txtRespuestaPregunta3.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta3.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta4Com = (txtRespuestaPregunta4.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta4.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta5Com = (txtRespuestaPregunta5.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta5.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta6Com = (txtRespuestaPregunta6.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta6.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta7Com = (txtRespuestaPregunta7.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta7.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta8Com = (txtRespuestaPregunta8.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta8.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta9Com = (txtRespuestaPregunta9.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta9.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta10Com = (txtRespuestaPregunta10.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta10.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta11Com = (txtRespuestaPregunta11.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta11.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta12Com = (txtRespuestaPregunta12.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta12.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta13Com = (txtRespuestaPregunta13.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta13.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta14Com = (txtRespuestaPregunta14.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta14.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta15Com = (txtRespuestaPregunta15.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta15.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta16Com = (txtRespuestaPregunta16.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta16.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta17Com = (txtRespuestaPregunta17.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta17.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta18Com = (txtRespuestaPregunta18.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta18.Text.Trim().Replace("|", " "));

                    objCliente.RespPregunta19Com = string.Empty;
                    objCliente.RespPregunta20Com = string.Empty;
                    objCliente.RespPregunta21Com = string.Empty;
                    objCliente.RespPregunta22Com = string.Empty;

                    //Valores seleccionados en las listas de respuestas.

                    objCliente.RespListaPregunta1Com = (ddlPregunta1.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta1.SelectedValue.Trim());
                    objCliente.RespListaPregunta2Com = (ddlPregunta2.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta2.SelectedValue.Trim());
                    objCliente.RespListaPregunta3Com = (ddlPregunta3.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta3.SelectedValue.Trim());
                    objCliente.RespListaPregunta4Com = (ddlPregunta4.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta4.SelectedValue.Trim());
                    objCliente.RespListaPregunta5Com = (ddlPregunta5.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta5.SelectedValue.Trim());
                    objCliente.RespListaPregunta6Com = (ddlPregunta6.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta6.SelectedValue.Trim());
                    objCliente.RespListaPregunta7Com = (ddlPregunta7.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta7.SelectedValue.Trim());
                    objCliente.RespListaPregunta8Com = (ddlPregunta8.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta8.SelectedValue.Trim());
                    objCliente.RespListaPregunta9Com = (ddlPregunta9.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta9.SelectedValue.Trim());
                    objCliente.RespListaPregunta10Com = (ddlPregunta10.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta10.SelectedValue.Trim());
                    objCliente.RespListaPregunta11Com = (ddlPregunta11.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta11.SelectedValue.Trim());
                    objCliente.RespListaPregunta12Com = (ddlPregunta12.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta12.SelectedValue.Trim());
                    objCliente.RespListaPregunta13Com = (ddlPregunta13.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta13.SelectedValue.Trim());
                    objCliente.RespListaPregunta14Com = (ddlPregunta14.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta14.SelectedValue.Trim());

                    //Fin PMO27494
                    objCliente.causalDisolucion = (lblCausalDisulocionResp.Text.Trim().Equals(string.Empty) ? " " : lblCausalDisulocionResp.Text);

                    // PMO18879
                    objCliente.comentarioRiesgos = (txtComentariosRiesgos.Text.Trim().Equals(string.Empty) ? " " : txtComentariosRiesgos.Text.Trim());
                    //PMO27494
                    respuesta = Fachada.CalificacionCartera.ActualizarCliente(objCliente, ((Usuario)Session["Rol"]).usuario, "11");
                    if (!respuesta.Contains("0000"))
                        throw new Exception("ERRORSP@" + respuesta);

                    List<PropiedadesCalificacion> propiedades = new List<PropiedadesCalificacion>();

                  
                    propiedades = guardarPropiedadesComunes(propiedades);



                    PropiedadesCalificacion propiedad6 = new PropiedadesCalificacion();
                    propiedad6.nombrePripiedad = ddlCalNRRecom.ID;
                    propiedad6.valorPropiedad = ddlCalNRRecom.SelectedValue.Trim();

                    propiedades.Add(propiedad6);

                    ViewState["Cambios"] = propiedades;

                    //hide your modal popup manually
                    mpeValidacionCal.Hide();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0469").Texto + "')", true);
                }
            }
            else if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC"))
            {
                if (validarPreguntas())
                {
                    establecerEncodingCamposTexto();

                    objCliente.SustentacionCalRecPIC = (txtSustentacionCalRec.Text.Trim().Equals(string.Empty) ? " " : txtSustentacionCalRec.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta1PIC = (txtRespuestaPregunta1.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta1.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta2PIC = (txtRespuestaPregunta2.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta2.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta3PIC = (txtRespuestaPregunta3.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta3.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta4PIC = (txtRespuestaPregunta4.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta4.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta5PIC = (txtRespuestaPregunta5.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta5.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta6PIC = (txtRespuestaPregunta6.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta6.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta7PIC = (txtRespuestaPregunta7.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta7.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta8PIC = (txtRespuestaPregunta8.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta8.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta9PIC = (txtRespuestaPregunta9.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta9.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta10PIC = (txtRespuestaPregunta10.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta10.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta11PIC = (txtRespuestaPregunta11.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta11.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta12PIC = (txtRespuestaPregunta12.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta12.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta13PIC = (txtRespuestaPregunta13.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta13.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta14PIC = (txtRespuestaPregunta14.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta14.Text.Trim().Replace("|", " "));
                    objCliente.comentarioRiesgos = (txtComentariosRiesgos.Text.Trim()).Equals(string.Empty) ? " " : txtComentariosRiesgos.Text.Trim().Replace("|", " ");

                    objCliente.RespPregunta15PIC = (txtRespuestaPregunta15.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta15.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta16PIC = (txtRespuestaPregunta16.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta16.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta17PIC = (txtRespuestaPregunta17.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta17.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta18PIC = (txtRespuestaPregunta18.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta18.Text.Trim().Replace("|", " "));

                    objCliente.RespPregunta19PIC = string.Empty;
                    objCliente.RespPregunta20PIC = string.Empty;
                    objCliente.RespPregunta21PIC = string.Empty;
                    objCliente.RespPregunta22PIC = string.Empty;

                    //Valores seleccionados en las listas de respuestas.

                    objCliente.RespListaPregunta1PIC = (ddlPregunta1.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta1.SelectedValue.Trim());
                    objCliente.RespListaPregunta2PIC = (ddlPregunta2.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta2.SelectedValue.Trim());
                    objCliente.RespListaPregunta3PIC = (ddlPregunta3.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta3.SelectedValue.Trim());
                    objCliente.RespListaPregunta4PIC = (ddlPregunta4.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta4.SelectedValue.Trim());
                    objCliente.RespListaPregunta5PIC = (ddlPregunta5.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta5.SelectedValue.Trim());
                    objCliente.RespListaPregunta6PIC = (ddlPregunta6.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta6.SelectedValue.Trim());
                    objCliente.RespListaPregunta7PIC = (ddlPregunta7.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta7.SelectedValue.Trim());
                    objCliente.RespListaPregunta8PIC = (ddlPregunta8.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta8.SelectedValue.Trim());
                    objCliente.RespListaPregunta9PIC = (ddlPregunta9.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta9.SelectedValue.Trim());
                    objCliente.RespListaPregunta10PIC = (ddlPregunta10.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta10.SelectedValue.Trim());
                    objCliente.RespListaPregunta11PIC = (ddlPregunta11.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta11.SelectedValue.Trim());
                    objCliente.RespListaPregunta12PIC = (ddlPregunta12.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta12.SelectedValue.Trim());
                    objCliente.RespListaPregunta13PIC = (ddlPregunta13.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta13.SelectedValue.Trim());
                    objCliente.RespListaPregunta14PIC = (ddlPregunta14.SelectedValue.Trim().Equals("0") ? " " : ddlPregunta14.SelectedValue.Trim());

                    //Fin PMO27494

                    objCliente.calIntRat = " ";
                    objCliente.calIntRatNRating = (ddlCalifInternaRNR.SelectedValue.Trim().Equals("0") ? " " : ddlCalifInternaRNR.SelectedValue.Trim());
                    objCliente.calExtRat = (ddlCalifExternaP.SelectedValue.Trim().Equals("0") ? " " : ddlCalifExternaP.SelectedValue.Trim());
                    objCliente.segProxComite = (ddlSeguimiento.SelectedValue.Trim().Equals("0") ? " " : ddlSeguimiento.SelectedValue.Trim());
                    objCliente.recAEC = (ddlRecomendacion.SelectedValue.Trim().Equals("0") ? " " : ddlRecomendacion.SelectedValue.Trim());

                    //Concatenar con '|' los valores del checkboxlist
                    string razonCalificacion = "";
                    foreach (ListItem item in ddlRazon.Items)
                    {
                        if (item.Selected)
                        {
                            razonCalificacion += item.Text + "|";
                        }
                    }


                    //Asignar razón calificacion concatenado con '|' al objeto cliente
                    objCliente.razCaliInt = razonCalificacion;

                    //Asignar tipo cliente
                    objCliente.tipoCliente = ddlTipoCliente.SelectedValue.Trim().Equals("0") ? " " : ddlTipoCliente.SelectedValue.Trim();

                    //Asignar Utilizo EEFF
                    objCliente.utilizoEEFF = ddlUtilizoEEFF.SelectedValue.Trim().Equals("0") ? " " : ddlUtilizoEEFF.SelectedValue.Trim();
                    // PMO27494
                    objCliente.estadoCalificacion = (ddlEstadoCal.SelectedValue.Trim().Equals("0") ? " " : ddlEstadoCal.SelectedValue.Trim());
                    objCliente.causalDisolucion = (lblCausalDisulocionResp.Text.Trim().Equals(string.Empty) ? " " : lblCausalDisulocionResp.Text);
                    objCliente.comentarioRiesgos = (txtComentariosRiesgos.Text.Trim()).Equals(string.Empty) ? " " : txtComentariosRiesgos.Text.Trim();

                    respuesta = Fachada.CalificacionCartera.ActualizarCliente(objCliente, ((Usuario)Session["Rol"]).usuario, "21").Trim();
                    if (!respuesta.Contains("0000"))
                        throw new Exception("ERRORSP@" + respuesta);

                    List<PropiedadesCalificacion> propiedades = new List<PropiedadesCalificacion>();

                    propiedades = guardarPropiedadesComunes(propiedades);


                    PropiedadesCalificacion propiedad5 = new PropiedadesCalificacion();
                    propiedad5.nombrePripiedad = ddlCalifExternaP.ID;
                    propiedad5.valorPropiedad = ddlCalifExternaP.SelectedValue.Trim();

                    propiedades.Add(propiedad5);

                    PropiedadesCalificacion propiedad6 = new PropiedadesCalificacion();
                    propiedad6.nombrePripiedad = ddlSeguimiento.ID;
                    propiedad6.valorPropiedad = ddlSeguimiento.SelectedValue.Trim();

                    propiedades.Add(propiedad6);

                    PropiedadesCalificacion propiedad7 = new PropiedadesCalificacion();
                    propiedad7.nombrePripiedad = ddlRecomendacion.ID;
                    propiedad7.valorPropiedad = ddlRecomendacion.SelectedValue.Trim();

                    propiedades.Add(propiedad7);

                    PropiedadesCalificacion propiedad8 = new PropiedadesCalificacion();
                    propiedad8.nombrePripiedad = ddlRazon.ID;
                    propiedad8.valorPropiedad = ddlRazon.SelectedValue.Trim();

                    propiedades.Add(propiedad8);

                    PropiedadesCalificacion propiedad9 = new PropiedadesCalificacion();
                    propiedad9.nombrePripiedad = txtComentariosRiesgos.ID;
                    propiedad9.valorPropiedad = txtComentariosRiesgos.Text.Trim();

                    propiedades.Add(propiedad9);

                    PropiedadesCalificacion propiedad10 = new PropiedadesCalificacion();
                    propiedad10.nombrePripiedad = ddlCalifInternaRNR.ID;
                    propiedad10.valorPropiedad = ddlCalifInternaRNR.SelectedValue.Trim();

                    propiedades.Add(propiedad10);

                    PropiedadesCalificacion propiedad12 = new PropiedadesCalificacion();
                    propiedad12.nombrePripiedad = ddlEstadoCal.ID;
                    propiedad12.valorPropiedad = ddlEstadoCal.SelectedValue.Trim();

                    propiedades.Add(propiedad12);

                    ViewState["Cambios"] = propiedades;

                    //hide your modal popup manually
                    mpeValidacionCal.Hide();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0469").Texto + "')", true);
                }
            }
        }


        // Validación de campos de preguntas
        public bool validarPreguntas()
        {
            string pregunta1 = "";
            string pregunta2 = "";
            string pregunta3 = "";
            string pregunta4 = "";
            string pregunta5 = "";
            string pregunta6 = "";
            string pregunta7 = "";
            string pregunta8 = "";
            string pregunta9 = "";
            string pregunta10 = "";
            string pregunta11 = "";
            string pregunta12 = "";
            string pregunta13 = "";
            string pregunta14 = "";
            
            string txtPregunta1 = "";
            string txtPregunta2 = "";
            string txtPregunta3 = "";
            string txtPregunta4 = "";
            string txtPregunta5 = "";
            string txtPregunta6 = "";
            string txtPregunta7 = "";
            string txtPregunta8 = "";
            string txtPregunta9 = "";
            string txtPregunta10 = "";
            string txtPregunta11 = "";
            string txtPregunta12 = "";
            string txtPregunta13 = "";
            string txtPregunta14 = "";
            string txtPregunta15 = "";
            string txtPregunta16 = "";
            string txtPregunta17 = "";
            string txtPregunta18 = "";

            int contador = 0;
            int contadorTxt = 0;
            bool contador10 = false;
            bool contadorTxt10 = false;
            int contadorTxtFinm = 0;


            if (ddlPregunta1.SelectedValue.Trim().Equals("0"))
            {
                pregunta1 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.76").Texto;
                contador = contador + 1;
            }
            if (txtRespuestaPregunta1.Text.Trim().Equals(string.Empty) && (ddlPregunta1.SelectedValue.Trim().Equals("SI")))
            {
                txtPregunta1 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.76").Texto;
                contadorTxt = contadorTxt + 1;
            }
            if (ddlPregunta2.SelectedValue.Trim().Equals("0"))
            {
                pregunta2 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.77").Texto;
                contador = contador + 1;
            }
            if (txtRespuestaPregunta2.Text.Trim().Equals(string.Empty) && (ddlPregunta2.SelectedValue.Trim().Equals("SI")))
            {
                txtPregunta2 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.77").Texto;
                contadorTxt = contadorTxt + 1;
            }
            if (ddlPregunta3.SelectedValue.Trim().Equals("0"))
            {
                pregunta3 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.78").Texto;
                contador = contador + 1;
            }
            if (txtRespuestaPregunta3.Text.Trim().Equals(string.Empty) && (ddlPregunta3.SelectedValue.Trim().Equals("SI")))
            {
                txtPregunta3 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.78").Texto;
                contadorTxt = contadorTxt + 1;
            }
            if (ddlPregunta4.SelectedValue.Trim().Equals("0"))
            {
                pregunta4 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.79").Texto;
                contador = contador + 1;
            }
            if (txtRespuestaPregunta4.Text.Trim().Equals(string.Empty) && (ddlPregunta4.SelectedValue.Trim().Equals("SI")))
            {
                txtPregunta4 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.79").Texto;
                contadorTxt = contadorTxt + 1;
            }
            if (ddlPregunta5.SelectedValue.Trim().Equals("0"))
            {
                pregunta5 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.80").Texto;
                contador = contador + 1;
            }
            if (txtRespuestaPregunta5.Text.Trim().Equals(string.Empty) && (ddlPregunta5.SelectedValue.Trim().Equals("SI")))
            {
                txtPregunta5 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.80").Texto;
                contadorTxt = contadorTxt + 1;
            }
            if (ddlPregunta6.SelectedValue.Trim().Equals("0"))
            {
                pregunta6 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.81").Texto;
                contador = contador + 1;
            }
            if (txtRespuestaPregunta6.Text.Trim().Equals(string.Empty) && (ddlPregunta6.SelectedValue.Trim().Equals("SI")))
            {
                txtPregunta6 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.81").Texto;
                contadorTxt = contadorTxt + 1;
            }
            if (ddlPregunta7.SelectedValue.Trim().Equals("0"))
            {
                pregunta7 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.82").Texto;
                contador = contador + 1;
            }
            if (txtRespuestaPregunta7.Text.Trim().Equals(string.Empty) && (ddlPregunta7.SelectedValue.Trim().Equals("SI")))
            {
                txtPregunta7 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.82").Texto;
                contadorTxt = contadorTxt + 1;
            }
            if (ddlPregunta8.SelectedValue.Trim().Equals("0"))
            {
                pregunta8 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.83").Texto;
                contador = contador + 1;
            }
            if (txtRespuestaPregunta8.Text.Trim().Equals(string.Empty) && (ddlPregunta8.SelectedValue.Trim().Equals("SI")))
            {
                txtPregunta8 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.83").Texto;
                contadorTxt = contadorTxt + 1;
            }
            if (ddlPregunta9.SelectedValue.Trim().Equals("0"))
            {
                pregunta9 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.84").Texto;
                contador = contador + 1;
            }
            if (txtRespuestaPregunta9.Text.Trim().Equals(string.Empty) && (ddlPregunta9.SelectedValue.Trim().Equals("SI")))
            {
                txtPregunta9 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.84").Texto;
                contadorTxt = contadorTxt + 1;
            }
            if (ddlPregunta10.SelectedValue.Trim().Equals("0"))
            {
                pregunta10 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.85").Texto;
                contador = contador + 1;
            }
            if (txtRespuestaPregunta10.Text.Trim().Equals(string.Empty) && (ddlPregunta10.SelectedValue.Trim().Equals("SI")))
            {
                txtPregunta10 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.85").Texto;
                contadorTxt = contadorTxt + 1;
            }
            if (ddlPregunta11.SelectedValue.Trim().Equals("0"))
            {
                pregunta11 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.86").Texto;
                contador = contador + 1;
            }
            if (txtRespuestaPregunta11.Text.Trim().Equals(string.Empty) && (ddlPregunta11.SelectedValue.Trim().Equals("SI")))
            {
                txtPregunta11 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.86").Texto;
                contadorTxt = contadorTxt + 1;
            }
            if (ddlPregunta12.SelectedValue.Trim().Equals("0"))
            {
                pregunta12 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.87").Texto;
                contador = contador + 1;
            }
            if (txtRespuestaPregunta12.Text.Trim().Equals(string.Empty) && (ddlPregunta12.SelectedValue.Trim().Equals("SI")))
            {
                txtPregunta12 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.87").Texto;
                contadorTxt = contadorTxt + 1;
            }
            if (ddlPregunta13.SelectedValue.Trim().Equals("0"))
            {
                pregunta13 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.88").Texto;
                contador = contador + 1;
            }
            if (txtRespuestaPregunta13.Text.Trim().Equals(string.Empty) && (ddlPregunta13.SelectedValue.Trim().Equals("SI")))
            {
                txtPregunta13 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.88").Texto;
                contadorTxt = contadorTxt + 1;
            }
            if (ddlPregunta14.SelectedValue.Trim().Equals("0"))
            {
                pregunta14 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.89").Texto;
                contador = contador + 1;
            }
            if (txtRespuestaPregunta14.Text.Trim().Equals(string.Empty) && (ddlPregunta14.SelectedValue.Trim().Equals("SI")))
            {
                txtPregunta14 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.89").Texto;
                contadorTxt = contadorTxt + 1;
            }

            // Preguntas para clientes tipo Fondos Inmobiliarios:

            if (txtRespuestaPregunta15.Text.Trim().Equals(string.Empty))
            {
                txtPregunta15 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.76").Texto;
                contadorTxtFinm = contadorTxtFinm + 1;
            }

            if (txtRespuestaPregunta16.Text.Trim().Equals(string.Empty))
            {
                txtPregunta16 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.77").Texto;
                contadorTxtFinm = contadorTxtFinm + 1;
            }

            if (txtRespuestaPregunta17.Text.Trim().Equals(string.Empty))
            {
                txtPregunta17 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.78").Texto;
                contadorTxtFinm = contadorTxtFinm + 1;
            }

            if (txtRespuestaPregunta18.Text.Trim().Equals(string.Empty))
            {
                txtPregunta18 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.79").Texto;
                contadorTxtFinm = contadorTxtFinm + 1;
            }


            //BLOQUE PARA LOS COMBOBOX DE SI | NO
            if ((pregunta1 != "") || (pregunta2 != "") || (pregunta3 != "") || (pregunta4 != "") || (pregunta5 != "") || (pregunta6 != "") || (pregunta7 != "") || (pregunta8 != "") || (pregunta9 != ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "GuardarComercial", "javascript:alert('" +
                    Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.91").Texto + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.92").Texto +
                    pregunta1 + pregunta2 + pregunta3 + pregunta4 + pregunta5 + pregunta6 + pregunta7 + pregunta8 + pregunta9 + "')", true);
                return false;
            }
            if ((pregunta10 != "") || (pregunta11 != "") || (pregunta12 != "") || (pregunta13 != "") || (pregunta14 != ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "GuardarComercial", "javascript:alert('" +
                    Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.94").Texto + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.92").Texto +
                    pregunta10 + pregunta11 + pregunta12 + pregunta13 + pregunta14 + "')", true);
                return false;
            }
            //BLOQUE PARA LOS TEXTBOX DE RESPUESTAS
            if ((txtPregunta1 != "") || (txtPregunta2 != "") || (txtPregunta3 != "") || (txtPregunta4 != "") || (txtPregunta5 != "") || (txtPregunta6 != "") || (txtPregunta7 != "") || (txtPregunta8 != "") || (txtPregunta9 != ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "GuardarComercial", "javascript:alert('" +
                    Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.91").Texto + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.93").Texto +
                    txtPregunta1 + txtPregunta2 + txtPregunta3 + txtPregunta4 + txtPregunta5 + txtPregunta6 + txtPregunta7 + txtPregunta8 + txtPregunta9 + "')", true);
                return false;
            }
            if ((txtPregunta10 != "") || (txtPregunta11 != "") || (txtPregunta12 != "") || (txtPregunta13 != "") || (txtPregunta14 != ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "GuardarComercial", "javascript:alert('" +
                    Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.94").Texto + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.93").Texto +
                    txtPregunta10 + txtPregunta11 + txtPregunta12 + txtPregunta13 + txtPregunta14 + "')", true);
                return false;
            }

            if ((txtPregunta15 != "") || (txtPregunta16 != "") || (txtPregunta17 != "") || (txtPregunta18 != ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "GuardarComercial", "javascript:alert('" +
                    Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.96").Texto + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.93").Texto +
                    txtPregunta15 + txtPregunta16 + txtPregunta17 + txtPregunta18 + "')", true);
                return false;
            }

            
            return true;
        }

    
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Parametros[] param = (Parametros[])Session["parametros"];

                if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
                {
                    cmdOk_Click(sender, e);
                }
                else
                {
                    string calificacionInternaRNRSel = ddlCalifInternaRNR.SelectedValue;

                    IEnumerable<Parametros> listaNuevoRating = param.Where(p => p.paramName.Trim().Equals("LCALNR"));

                    List<String> valores2 = new List<string>();
                    List<String> valores3 = new List<string>();
                    foreach (var nuevoRating in listaNuevoRating)
                    {
                        if (calificacionInternaRNRSel.Equals(nuevoRating.param7))
                        {
                            valores2.Add(nuevoRating.param2.Trim());
                            valores3.Add(nuevoRating.param3.Trim());
                        }
                    }

                    string calificacionExternaSel = ddlCalifExternaP.SelectedValue;
                    bool esMismaCalificacion = false;

                    for (int i = 0; i < valores2.Count; i++)
                    {
                        Parametros calExternaEsperada = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals(valores2[i])
                                                && p.paramSeq == int.Parse(valores3[i])).ElementAt(0);

                        if (calExternaEsperada.param7.Trim().Equals(calificacionExternaSel))
                        {
                            esMismaCalificacion = true;
                            break;
                        }
                    }

                    if (!esMismaCalificacion)
                    {
                        mpeValidacionCal.Show();
                    }
                    else
                    {
                        cmdOk_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CalificacionCliente";
                if (ex.Message.ToString().Contains("ERRORSP@"))
                    errEnt.Error = "ERRORSP@";
                else
                    errEnt.Error = "MEN.0408";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");

            }
        }

        protected void btnBuscarNuevo_Click(object sender, EventArgs e)
        {
            bool cambian = false;
            List<PropiedadesCalificacion> propiedades = new List<PropiedadesCalificacion>();
            propiedades = (List<PropiedadesCalificacion>)ViewState["Cambios"];
           
            if (!(((Usuario)Session["Rol"]).rol.Equals("Comercial") || ((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC")))
            {
                Response.Redirect("CalificacionSemCartera.aspx");
            }
            else
            {

                if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
                {
                    if (propiedades.Find(p => p.nombrePripiedad == ddlCalNRRecom.ID).valorPropiedad.Trim() != ddlCalNRRecom.SelectedValue.Trim())
                    {
                        cambian = true;
                    }
                }
                else if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC"))
                {
                    if (propiedades.Find(p => p.nombrePripiedad == ddlCalifInternaRNR.ID).valorPropiedad.Trim() != ddlCalifInternaRNR.SelectedValue.Trim())
                    {
                        cambian = true;
                    }
                    if (propiedades.Find(p => p.nombrePripiedad == txtComentariosRiesgos.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtComentariosRiesgos.Text.Trim().Replace(Environment.NewLine, "\n"))
                    {
                        cambian = true;
                    }
                    if (propiedades.Find(p => p.nombrePripiedad == ddlCalifExternaP.ID).valorPropiedad.Trim() != ddlCalifExternaP.SelectedValue.Trim())
                    {
                        cambian = true;
                    }
                    if (propiedades.Find(p => p.nombrePripiedad == ddlSeguimiento.ID).valorPropiedad.Trim() != ddlSeguimiento.SelectedValue.Trim())
                    {
                        cambian = true;
                    }
                    if (propiedades.Find(p => p.nombrePripiedad == ddlRecomendacion.ID).valorPropiedad.Trim() != ddlRecomendacion.SelectedValue.Trim())
                    {
                        cambian = true;
                    }
                    if (propiedades.Find(p => p.nombrePripiedad == ddlRazon.ID).valorPropiedad.Trim() != ddlRazon.SelectedValue.Trim())
                    {
                        cambian = true;
                    }
                    if (propiedades.Find(p => p.nombrePripiedad == ddlCalifInternaRNR.ID).valorPropiedad.Trim() != ddlCalifInternaRNR.SelectedValue.Trim())
                    {
                        cambian = true;
                    }

                    if (propiedades.Find(p => p.nombrePripiedad == ddlEstadoCal.ID).valorPropiedad.Trim() != ddlEstadoCal.SelectedValue.Trim())
                    {
                        cambian = true;
                    }
                }
            }

            
            if (propiedades.Find(p => p.nombrePripiedad == txtSustentacionCalRec.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtSustentacionCalRec.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == ddlPregunta1.ID).valorPropiedad.Trim() != ddlPregunta1.SelectedValue.Trim())
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta1.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta1.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == ddlPregunta2.ID).valorPropiedad.Trim() != ddlPregunta2.SelectedValue.Trim())
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta2.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta2.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == ddlPregunta3.ID).valorPropiedad.Trim() != ddlPregunta3.SelectedValue.Trim())
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta3.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta3.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == ddlPregunta4.ID).valorPropiedad.Trim() != ddlPregunta4.SelectedValue.Trim())
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta4.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta4.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == ddlPregunta5.ID).valorPropiedad.Trim() != ddlPregunta5.SelectedValue.Trim())
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta5.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta5.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == ddlPregunta6.ID).valorPropiedad.Trim() != ddlPregunta6.SelectedValue.Trim())
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta6.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta6.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == ddlPregunta7.ID).valorPropiedad.Trim() != ddlPregunta7.SelectedValue.Trim())
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta7.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta7.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == ddlPregunta8.ID).valorPropiedad.Trim() != ddlPregunta8.SelectedValue.Trim())
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta8.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta8.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == ddlPregunta9.ID).valorPropiedad.Trim() != ddlPregunta9.SelectedValue.Trim())
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta9.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta9.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == ddlPregunta10.ID).valorPropiedad.Trim() != ddlPregunta10.SelectedValue.Trim())
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta10.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta10.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == ddlPregunta11.ID).valorPropiedad.Trim() != ddlPregunta11.SelectedValue.Trim())
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta11.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta11.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == ddlPregunta12.ID).valorPropiedad.Trim() != ddlPregunta12.SelectedValue.Trim())
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta12.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta12.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == ddlPregunta13.ID).valorPropiedad.Trim() != ddlPregunta13.SelectedValue.Trim())
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta13.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta13.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == ddlPregunta14.ID).valorPropiedad.Trim() != ddlPregunta14.SelectedValue.Trim())
            {
                cambian = true;
            }
            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta14.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta14.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }
            

            if (!cambian)
            {
                Response.Redirect("CalificacionSemCartera.aspx");
            }
            else
            {
                String js = "if(confirm('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0413").Texto + "')==true)window.location='CalificacionSemCartera.aspx';";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", js, true);
            }
        }


        protected void btnComiteAnterior_Click(object sender, EventArgs e)
        {
            try
            {
               
                Cliente objCliente = (Cliente)ViewState["Cliente"];
                ObjetosCalificacion objCalificacion = Fachada.CalificacionCartera.comiteAnterior(objCliente.nit.Trim(), objCliente.tipDoc.Trim(), objCliente.fecProc.Trim());

                if (objCalificacion.objComiteAnterior != null)
                {
                    Parametros[] param = (Parametros[])Session["parametros"];
                    Parametros parametros = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("OPARMADI")
                       && p.paramSeq == 7).ElementAt(0);
                    int fechaComite = Int32.Parse(parametros.param1);
                    if (Int32.Parse(objCliente.fecProc.Trim()) > fechaComite)
                    {
                       
                        pintarNuevoComite(objCalificacion.objComiteAnterior);
                      
                        mpeNuevoComiteAnterior.Show();
                    }
                    else
                    {
                  
                        pintarComite(objCalificacion.objComiteAnterior);
                     
                        mpeComiteAnterior.Show();
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0440").Texto + "')", true);
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CalificacionCliente";
                errEnt.Error = "MEN.0441";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }

        protected void btnPEC_Click(object sender, EventArgs e)
        {
            try
            {
               
                Cliente objCliente = (Cliente)ViewState["Cliente"];
                ObjetosCalificacion objCalificacion = Fachada.CalificacionCartera.CentralExterna(objCliente.nit.Trim());
                if (objCalificacion.objCentralExterna != null)
                {
                    
                    pintarPEC(objCalificacion.objCentralExterna);
                    pnlPEC.Visible = true;
                    pnlCovenants.Visible = false;
                    pnlProrrogas.Visible = false;
                    pnlValidacionCal.Visible = false;
                    mpePopPups.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0440").Texto + "')", true);
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CalificacionCliente";
                errEnt.Error = "MEN.0441";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }

     
        protected void btnCovenants_Click(object sender, EventArgs e)
        {
            try
            {
               
                Cliente objCliente = (Cliente)ViewState["Cliente"];
                ObjetosCalificacion objCalificacion = Fachada.CalificacionCartera.Covenants(objCliente.nit.Trim());
                if (objCalificacion.objCovenants != null)
                {
                  
                    pintarCovenants(objCalificacion.objCovenants);
                    pnlPEC.Visible = false;
                    pnlCovenants.Visible = true;
                    pnlProrrogas.Visible = false;
                    pnlPopPups.Style.Add("height", "260");
                    mpePopPups.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0440").Texto + "')", true);
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CalificacionCliente";
                errEnt.Error = "MEN.0441";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }

       
        protected void btnProrrogas_Click(object sender, EventArgs e)
        {
            try
            {
              
                Cliente objCliente = (Cliente)ViewState["Cliente"];
                ObjetosCalificacion objCalificacion = Fachada.CalificacionCartera.Prorrogas(objCliente.nit.Trim());
                if (objCalificacion.objProrrogas != null)
                {
                  
                    pintarProrrogas(objCalificacion.objProrrogas);
                    pnlPEC.Visible = false;
                    pnlCovenants.Visible = false;
                    pnlProrrogas.Visible = true;
                    pnlPopPups.Style.Add("height", "500");
                    mpePopPups.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0440").Texto + "')", true);
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CalificacionCliente";
                errEnt.Error = "MEN.0471";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }

       
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            bool cambian = false;
            List<PropiedadesCalificacion> propiedades = new List<PropiedadesCalificacion>();
            propiedades = (List<PropiedadesCalificacion>)ViewState["Cambios"];
            if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
            {
                if (propiedades.Find(p => p.nombrePripiedad == ddlCalNRRecom.ID).valorPropiedad.Trim() != ddlCalNRRecom.SelectedValue.Trim())
                {
                    cambian = true;
                }
            }
            else if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC"))
            {
                if (propiedades.Find(p => p.nombrePripiedad == ddlCalifExternaP.ID).valorPropiedad.Trim() != ddlCalifExternaP.SelectedValue.Trim())
                {
                    cambian = true;
                }
                if (propiedades.Find(p => p.nombrePripiedad == ddlSeguimiento.ID).valorPropiedad.Trim() != ddlSeguimiento.SelectedValue.Trim())
                {
                    cambian = true;
                }
                if (propiedades.Find(p => p.nombrePripiedad == ddlRecomendacion.ID).valorPropiedad.Trim() != ddlRecomendacion.SelectedValue.Trim())
                {
                    cambian = true;
                }
                if (propiedades.Find(p => p.nombrePripiedad == ddlRazon.ID).valorPropiedad.Trim() != ddlRazon.SelectedValue.Trim())
                {
                    cambian = true;
                }
                if (propiedades.Find(p => p.nombrePripiedad == ddlCalifInternaRNR.ID).valorPropiedad.Trim() != ddlCalifInternaRNR.SelectedValue.Trim())
                {
                    cambian = true;
                }

                if (propiedades.Find(p => p.nombrePripiedad == ddlEstadoCal.ID).valorPropiedad.Trim() != ddlEstadoCal.SelectedValue.Trim())
                {
                    cambian = true;
                }
            }
            if (!cambian)
            {
                string categoriaCliente = "FI";

                Response.Redirect("Reporte/PDF.aspx?Cliente=" + lblCliente.Text.Trim() + "&categoriaCliente=" + categoriaCliente);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0444").Texto + "')", true);
            }
        }

        #endregion


        #region "Metodos"

      
        protected void llenarCliente(string codigo)
        {
            
            ObjetosCalificacion objCalificacion = new ObjetosCalificacion();

            Parametros[] param = null;
            if (Session["parametros"] != null)
            {
                param = (Parametros[])Session["parametros"];
            }
            try
            {
                if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
                {
                    objCalificacion = Fachada.CalificacionCartera.ConsultarCliente("10", codigo);

                    //Obtiene la información necesaria para luego usarla en la actualización del log.
                    var data = objCalificacion.objCliente;
                    //Actualiza la tabla de logs insertando una nueva actividad de consulta.
                    if (data != null)
                        Fachada.CalificacionCartera.ActualizarLogCSC(data.fecProc, "10",
                            ((Usuario)Session["Rol"]).usuario, "", codigo, "", "", "", "");

                }
                else if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC"))
                {
                    objCalificacion = Fachada.CalificacionCartera.ConsultarCliente("20", codigo);

                   
                    var data = objCalificacion.objCliente;
                   
                    if (data != null)
                        Fachada.CalificacionCartera.ActualizarLogCSC(data.fecProc, "13",
                            ((Usuario)Session["Rol"]).usuario, "", codigo, "", "", "", "");
                }
                else
                {
                    objCalificacion = Fachada.CalificacionCartera.ConsultarCliente("30", codigo);

                    if (((Usuario)Session["Rol"]).rol.Equals("Superfinanciera"))
                    {
                      
                        var data = objCalificacion.objCliente;
                     
                        if (data != null)
                            Fachada.CalificacionCartera.ActualizarLogCSC(data.fecProc, "16",
                                ((Usuario)Session["Rol"]).usuario, "", codigo, "", "", "", "");
                    }
                    else if (((Usuario)Session["Rol"]).rol.Equals("Consulta"))
                    {
                       
                        var data = objCalificacion.objCliente;
                       
                        if (data != null)
                            Fachada.CalificacionCartera.ActualizarLogCSC(data.fecProc, "19",
                                ((Usuario)Session["Rol"]).usuario, "", codigo, "", "", "", "");
                    }
                }

                if (objCalificacion.setDatos != null && objCalificacion.objCliente != null)
                {
                    
                    cargarPreguntasPresencial(param);

                    ViewState["Cliente"] = objCalificacion.objCliente;
                    lblTipoComite.Text = objCalificacion.objCliente.tipCom.Trim();

                  
                    pnlCliente.Visible = true;

                    lblCliente.Text = codigo;
                    lblNombreCliente.Text = objCalificacion.objCliente.nombre.Trim();
                    lblFechaComite.Text = objCalificacion.objCliente.fecProc.Trim();

                   
                    if (!string.IsNullOrEmpty(objCalificacion.objCliente.nitRelacion) && (!objCalificacion.objCliente.nit.Equals(objCalificacion.objCliente.nitRelacion)))
                    {
                        lblNitRelacionamiento.Text = objCalificacion.objCliente.nitRelacion;
                        btnRelacionamiento.Visible = true;
                    }
                    else
                    {
                        lblNitRelacionamiento.Text = "No tiene";
                    }

                  
                    var nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ",";
                    nfi.NumberGroupSeparator = ".";
                    nfi.CurrencyDecimalSeparator = ",";
                    nfi.CurrencyGroupSeparator = ".";

                  
                    if (((Usuario)Session["Rol"]).rol.Equals("Superfinanciera"))
                    {
                        lblCausalDisolucion.Visible = false;
                        lblCausalDisulocionResp.Visible = false;
                        lblLocal.Visible = false;
                    }
                    else
                    {
                        if (((Usuario)Session["Rol"]).rol.Equals("Consulta"))
                        {
                            lblLocal.Visible = false;
                        }
                        int tipoCodigo = int.MinValue;
                        lblCausalDisolucion.Visible = true;
                        lblCausalDisulocionResp.Visible = true;
                        if (int.TryParse(objCalificacion.objCliente.tipDoc.Trim(), out tipoCodigo))
                        {
                            if (tipoCodigo != 1 && tipoCodigo != 2 && tipoCodigo != 4 && tipoCodigo != 9)
                            {
                                IEnumerable<Parametros> causal;
                                if (param != null)
                                {
                                   
                                    if (param.Length == 0)
                                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0422").Texto);

                                    
                                    causal = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("VPROAUTO") && p.paramSeq == 1);
                                    if (causal.ToArray().Length == 0 || causal.ToArray()[0].param1.Trim().Equals(string.Empty))
                                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0422").Texto);
                                }
                                else
                                {
                                    throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0422").Texto);
                                }
                                if (objCalificacion.objCliente.capSocialPerAct.Trim().Equals(string.Empty) || objCalificacion.objCliente.patPerAct.Trim().Equals(string.Empty))
                                {
                                    lblCausalDisulocionResp.Text = "NO";
                                }
                                else
                                {
                                    double capital = double.MinValue;
                                    double patrimonio = double.MinValue;
                                    if (double.TryParse(objCalificacion.objCliente.capSocialPerAct, NumberStyles.Float | NumberStyles.AllowParentheses | NumberStyles.AllowThousands, nfi, out capital) && double.TryParse(objCalificacion.objCliente.patPerAct, NumberStyles.Float | NumberStyles.AllowParentheses | NumberStyles.AllowThousands, nfi, out patrimonio))
                                    {
                                        if (((patrimonio / capital)) * 100 < double.Parse(causal.ToArray()[0].param1.Trim(), nfi))
                                        {
                                            lblCausalDisulocionResp.Text = "SI";
                                        }
                                        else
                                        {
                                            lblCausalDisulocionResp.Text = "NO";
                                        }
                                    }
                                    else
                                    {
                                        lblCausalDisulocionResp.Text = "NO";
                                        pnlErrores.Visible = true;
                                        lblErrores.Text = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0432").Texto;
                                    }
                                }
                            }
                            else
                            {
                                lblCausalDisulocionResp.Text = "No por ID";
                                pnlErrores.Visible = true;
                            }
                        }
                        else
                        {
                            lblCausalDisulocionResp.Text = "No por ID";
                            pnlErrores.Visible = true;
                            lblErrores.Text = lblErrores.Text + "<br />" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0439").Texto;
                        }
                    }

                    lblSector.Text = objCalificacion.objCliente.sectEcon.Trim();
                    lblSegmento.Text = objCalificacion.objCliente.segmento.Trim();
                    lblRegional.Text = objCalificacion.objCliente.regional.Trim();
                    lblSeguimiento.Text = objCalificacion.objCliente.segRecComAnt.Trim();
                    lblGrupoR.Text = objCalificacion.objCliente.grupoRiesgo.Trim();
                    lblRiesgoAEC.Text = objCalificacion.objCliente.aec.Trim();

                    //LOCAL

                    //llenar saldo capital
                    lblSCBanc.Text = objCalificacion.objCliente.salgoKBanco.Trim();
                    lblSCFact.Text = objCalificacion.objCliente.saldoKFactoring.Trim();
                    lblSCSuf.Text = objCalificacion.objCliente.salgoKSufi.Trim();
                    lblSCLea.Text = objCalificacion.objCliente.saldoKLeasing.Trim();
                    lblSCAnt.Text = objCalificacion.objCliente.leasingAnticipos.Trim();

                    //llenar dias mora
                    lblDMBanc.Text = objCalificacion.objCliente.diasMoraBanco.Trim();
                    lblDMFact.Text = objCalificacion.objCliente.diasMoraFactoring.Trim();
                    lblDMSuf.Text = objCalificacion.objCliente.diasMoraSufi.Trim();
                    lblDMLea.Text = objCalificacion.objCliente.diasMoraLeasing.Trim();
                    //llenar reestructurado
                    lblRBanc.Text = objCalificacion.objCliente.reestrucBanco.Trim();
                    lblRFact.Text = objCalificacion.objCliente.reestrucFactoring.Trim();
                    lblRSuf.Text = objCalificacion.objCliente.reestrucSufi.Trim();
                    lblRLea.Text = objCalificacion.objCliente.reestructLeasing.Trim();

                    //llenar calificacion externa modelo
                    lblCEMBanco.Text = objCalificacion.objCliente.calExternaModeloBanco.Trim();
                    lblCEMFactoring.Text = objCalificacion.objCliente.calExternaModeloFactoring.Trim();
                    lblCEMLeasing.Text = objCalificacion.objCliente.calExternaModeloLeasing.Trim();
                    lblCEMSufi.Text = objCalificacion.objCliente.calExternaModeloSufi.Trim();

                    //llenar calificacion externa actual
                    lblCEBanc.Text = objCalificacion.objCliente.calEBanco.Trim();
                    lblCEFact.Text = objCalificacion.objCliente.calEFactoring.Trim();
                    lblCESuf.Text = objCalificacion.objCliente.calESufi.Trim();
                    lblCELea.Text = objCalificacion.objCliente.calELeasing.Trim();

                    //EXTERIOR
                    //SK
                    // Se quita formatNumber(objCalificacion.objCliente.saldoKPanama.Trim()) la función formatNumber se elimina ya que el valor viene con el formto de . y ,
                    lblSKPan.Text = objCalificacion.objCliente.saldoKPanama.Trim();
                    lblSKPR.Text = objCalificacion.objCliente.saldoKPuertoRico.Trim();

                    //Panama
                    lblDiasMoraPan.Text = objCalificacion.objCliente.diasMoraPanama.Trim();
                    lblReestructPan.Text = objCalificacion.objCliente.reestructuradoPanama.Trim();
                    lblCalExtPan.Text = objCalificacion.objCliente.calExternaActualPanama.Trim();

                    //Puerto rico
                    lblDiasMoraPR.Text = objCalificacion.objCliente.diasMoraPuertoRico.Trim();
                    lblReestructPR.Text = objCalificacion.objCliente.reestructuradoPuertoRico.Trim();
                    lblCalExtPR.Text = objCalificacion.objCliente.calExternaActualPuertoRico.Trim();

                    //SI
                    lblSIPan.Text = objCalificacion.objCliente.saldoIPanama.Trim();
                    lblSIPR.Text = objCalificacion.objCliente.saldoIPuertoRico.Trim();

                    if (objCalificacion.objCliente.EEFFUti != null)
                    {
                        txtEEFFUti.Text = objCalificacion.objCliente.EEFFUti.Trim();
                    }
                    if (objCalificacion.objCliente.FteEEFF != null)
                    {
                        txtFteEEFF.Text = objCalificacion.objCliente.FteEEFF.Trim();
                    }

                    if (((Usuario)Session["Rol"]).rol.Equals("Comercial") || ((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC")
                        || ((Usuario)Session["Rol"]).rol.Equals("Superfinanciera") || ((Usuario)Session["Rol"]).rol.Equals("Consulta"))
                    {
                        lblCalificado.Visible = true;
                        lblCalifPor.Visible = true;

                        pnlExterior.Visible = true;
                        pnlLocal.Visible = true;
                        pnlInfoEndeudamiento.Visible = true;

                        lblMora30.Text = objCalificacion.objCliente.nroVecesMora30.Trim();
                        lblMora60.Text = objCalificacion.objCliente.nroVecesMora60.Trim();
                        lblMoraMax.Text = objCalificacion.objCliente.moraMaxima.Trim();

                        //llenar
                        lblCalifPor.Text = objCalificacion.objCliente.calPor.Trim();

                        //Se busca el valor para el CAMPO PARAMÉTRICO 1
                        Parametros campoParametrico = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("OPAR")
                            && p.paramSeq == 1).ElementAt(0);
                        lblCampoParametrico1.Text = campoParametrico.param7;
                    }
                    else
                    {
                        pnlExterior.Visible = true;
                        pnlLocal.Visible = true;
                        pnlInfoEndeudamiento.Visible = false;

                        lblCalificado.Visible = false;
                        lblCalifPor.Visible = false;
                    }

                    //información Financiera

                    //if fecha de info vacia mostrar alerta, sino, mostrar todo
                    int fecha = int.MinValue;

                    if (int.TryParse(objCalificacion.objCliente.fechaEstFros.Trim(), out fecha) && fecha.ToString().Length == 6)
                    {
                        pnlInformacionFinanciera.Visible = true;
                        pnlInformacionFinancieraNo.Visible = false;
                        //si es visible la info financiera mostrar:
                        lblFechaInfoFinanciera.Text = objCalificacion.objCliente.fechaEstFros.Trim();
                        lblCorteEstFros.Text = objCalificacion.objCliente.corteEstFros.Trim();
                        lblVentas.Text = objCalificacion.objCliente.ventasPerAct.Trim();
                        lblUtilidadOp.Text = objCalificacion.objCliente.utilPerdidaOpPerAct.Trim();
                        lblCostoFin.Text = objCalificacion.objCliente.intPagadosPerAct.Trim();
                        lblUtilidadNe.Text = objCalificacion.objCliente.utilPerNetaPerAct.Trim();
                        lblUtilidadBru.Text = objCalificacion.objCliente.utilBrutaPerAct.Trim();//ojoooooooooooooooooooooooo

                        if (((Usuario)Session["Rol"]).rol.Equals("Comercial") || ((Usuario)Session["Rol"]).rol.Equals("Riesgos") || ((Usuario)Session["Rol"]).rol.Equals("Consulta"))
                        {
                            double utilidadNeta = double.MinValue;

                            if (double.TryParse(objCalificacion.objCliente.utilPerNetaPerAct, NumberStyles.Float | NumberStyles.AllowParentheses | NumberStyles.AllowParentheses | NumberStyles.AllowThousands, nfi, out utilidadNeta))
                            {
                                if (utilidadNeta < 0)
                                    lblUtilidadNe.ForeColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                pnlErrores.Visible = true;
                                lblErrores.Text = lblErrores.Text + "<br />" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0434").Texto;
                            }
                        }

                        lblTotalActivo.Text = objCalificacion.objCliente.activosPerAct.Trim();
                        lblTotalPasivo.Text = objCalificacion.objCliente.pasivoPerAct.Trim();
                        lblEbidtaPesos.Text = objCalificacion.objCliente.ebitdaPerAct.Trim();
                        lblSuperavit.Text = objCalificacion.objCliente.superCapPerAct.Trim();
                        lblReservas.Text = objCalificacion.objCliente.reservasPerAct.Trim();
                        lblPatrimonio.Text = objCalificacion.objCliente.patPerAct.Trim();
                        lblCicloFinanciero.Text = objCalificacion.objCliente.cicloFinPerAct.Trim();

                       
                        double PasivoFCP = double.MinValue;
                        double PasivoFLP = double.MinValue;
                        double deuda = double.MinValue;
                        double ebitdapesos = double.MinValue;
                        double ebitda = double.MinValue;

                        if (objCalificacion.objCliente.pasivoFroCP.Trim().Equals(string.Empty))
                            objCalificacion.objCliente.pasivoFroCP = "0";
                        if (objCalificacion.objCliente.pasivoFroLP.Trim().Equals(string.Empty))
                            objCalificacion.objCliente.pasivoFroLP = "0";
                        if (double.TryParse(objCalificacion.objCliente.pasivoFroCP.Trim(), NumberStyles.Float | NumberStyles.AllowParentheses | NumberStyles.AllowParentheses | NumberStyles.AllowThousands, nfi, out PasivoFCP) && double.TryParse(objCalificacion.objCliente.pasivoFroLP, NumberStyles.Float | NumberStyles.AllowParentheses | NumberStyles.AllowParentheses | NumberStyles.AllowThousands, nfi, out PasivoFLP))
                        {
                            deuda = PasivoFLP + PasivoFCP;
                            if (double.TryParse(objCalificacion.objCliente.ebitdaPerAct.Trim(), NumberStyles.Float | NumberStyles.AllowParentheses | NumberStyles.AllowParentheses | NumberStyles.AllowThousands, nfi, out ebitdapesos))
                            {
                                //validar ebidta
                                int mes = int.Parse(fecha.ToString().Substring(4, 2));
                                if (mes <= 0 || mes > 12)
                                    lblDeudaEbitda.Text = "0,00";
                                else
                                {
                                    ebitda = (ebitdapesos * 12) / mes;
                                    if (ebitda != 0)
                                    {
                                        lblDeudaEbitda.Text = Math.Round((deuda / ebitda), 2).ToString(nfi);
                                    }
                                    else
                                    {
                                        lblDeudaEbitda.Text = "0,00";
                                    }
                                }
                            }
                            else
                            {
                                lblDeudaEbitda.Text = "0,00";
                            }
                            if (!lblDeudaEbitda.Text.Contains(","))
                            {
                                lblDeudaEbitda.Text = lblDeudaEbitda.Text + nfi.NumberDecimalSeparator + "00";
                            }
                            else
                            {
                                string[] nums = lblDeudaEbitda.Text.Split(',');
                                if (nums[1].Length == 1)
                                {
                                    lblDeudaEbitda.Text = nums[0] + nfi.NumberDecimalSeparator + nums[1].PadRight(2, '0');
                                }
                            }
                        }
                        else
                        {
                            pnlErrores.Visible = true;
                            lblErrores.Text = lblErrores.Text + "<br />" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0442").Texto;
                        }

                        lblRotacionCart.Text = objCalificacion.objCliente.rotacionCartera.Trim();
                        lblRotacionInv.Text = objCalificacion.objCliente.rotacionInventarios.Trim();
                        lblEndCP.Text = objCalificacion.objCliente.pasivoFroCP.Trim();
                        lblEndLP.Text = objCalificacion.objCliente.pasivoFroLP.Trim();
                        lblCapitalSocial.Text = objCalificacion.objCliente.capSocialPerAct.Trim();

                        lblVarVentas.Text = objCalificacion.objCliente.porcentVarVentas.Trim();
                        double pasivo = double.MinValue;
                        double activo = double.MinValue;
                        if (double.TryParse(objCalificacion.objCliente.pasivoPerAct, NumberStyles.Float | NumberStyles.AllowParentheses | NumberStyles.AllowThousands, nfi, out pasivo) && double.TryParse(objCalificacion.objCliente.activosPerAct, NumberStyles.Float | NumberStyles.AllowParentheses | NumberStyles.AllowThousands, nfi, out activo))
                        {
                            string end = (pasivo / activo * 100).ToString(nfi);
                            if (end.Contains(','))
                            {
                                string num = end.Substring(0, end.IndexOf(','));
                                string[] aux = end.Split(',');
                                
                                if (aux[1].Length.Equals(1))
                                    end += "0";
                                string dec = end.Substring(end.IndexOf(',') + 1, 2);
                                lblEnd.Text = num + nfi.NumberDecimalSeparator + dec + "%";
                            }
                            else
                            {
                                if (activo == 0)
                                {
                                    lblEnd.Text = "Infinity";
                                }
                                else
                                {
                                    lblEnd.Text = end + nfi.NumberDecimalSeparator + "00%";
                                }
                            }
                        }
                        else
                        {
                            lblEnd.Text = "No se puede calcular";
                            pnlErrores.Visible = true;
                            lblErrores.Text = lblErrores.Text + "<br />" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0435").Texto;
                        }
                        lblMargenEB.Text = objCalificacion.objCliente.margenEbitda.Trim();
                        lblCobertEB.Text = objCalificacion.objCliente.coberturaEbitda.Trim();
                        lblMargNeto.Text = objCalificacion.objCliente.margenNeto.Trim();
                    }
                    else
                    {
                        pnlInformacionFinancieraNo.Visible = true;
                        pnlInformacionFinanciera.Visible = false;
                        lblFechaInfoFinanciera.Text = "";
                    }

                    //valida que sección mostrar de calificación
                    if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
                    {
                        //habilitar calificación comercial
                        pnlCalificacionComercial.Visible = true;
                        pnlCalificacionPIC.Visible = false;
                        pnlCalificacionSuper.Visible = false;
                        pnlCalificacionConsulta.Visible = false;

                        //pintar controles de calif comercial
                        lblCalificacionINRatingCom.Text = objCalificacion.objCliente.CalIntNRating.Trim();
                        lblFechaCalifC.Text = objCalificacion.objCliente.fecCalInt.Trim();
                        lblCovenantC.Text = objCalificacion.objCliente.segCovenants.Trim();
                        lblCalMAFNuevoRatingCom.Text = objCalificacion.objCliente.CalMAFNRating.Trim();
                        lblListasdeControlCom.Text = objCalificacion.objCliente.ListasDeControl.Trim();

                        //Llena la lista de calificacion nuevo rating recomendada 
                        ddlCalNRRecom.DataSource = objCalificacion.setDatos.Tables[2];
                        ddlCalNRRecom.DataTextField = "APCVALCHA7";
                        ddlCalNRRecom.DataValueField = "APCVALCHA7";
                        ddlCalNRRecom.DataBind();
                        // Ini PMO27494
                        inicializarLista(ddlCalNRRecom, "0", false);

                        ddlCalNRRecom.SelectedValue = objCalificacion.objCliente.calNuevoRatRecom.Trim();

                        List<PropiedadesCalificacion> propiedades = new List<PropiedadesCalificacion>();

                        //cargar respuesta a preguntas comercial para edición
                        txtSustentacionCalRec.Text = objCalificacion.objCliente.SustentacionCalRecCom.Trim();
                        txtRespuestaPregunta1.Text = objCalificacion.objCliente.RespPregunta1Com.Trim();
                        txtRespuestaPregunta2.Text = objCalificacion.objCliente.RespPregunta2Com.Trim();
                        txtRespuestaPregunta3.Text = objCalificacion.objCliente.RespPregunta3Com.Trim();
                        txtRespuestaPregunta4.Text = objCalificacion.objCliente.RespPregunta4Com.Trim();
                        txtRespuestaPregunta5.Text = objCalificacion.objCliente.RespPregunta5Com.Trim();
                        txtRespuestaPregunta6.Text = objCalificacion.objCliente.RespPregunta6Com.Trim();
                        txtRespuestaPregunta7.Text = objCalificacion.objCliente.RespPregunta7Com.Trim();
                        txtRespuestaPregunta8.Text = objCalificacion.objCliente.RespPregunta8Com.Trim();
                        txtRespuestaPregunta9.Text = objCalificacion.objCliente.RespPregunta9Com.Trim();
                        txtRespuestaPregunta10.Text = objCalificacion.objCliente.RespPregunta10Com.Trim();
                        txtRespuestaPregunta11.Text = objCalificacion.objCliente.RespPregunta11Com.Trim();
                        txtRespuestaPregunta12.Text = objCalificacion.objCliente.RespPregunta12Com.Trim();
                        txtRespuestaPregunta13.Text = objCalificacion.objCliente.RespPregunta13Com.Trim();
                        txtRespuestaPregunta14.Text = objCalificacion.objCliente.RespPregunta14Com.Trim();
                        txtRespuestaPregunta15.Text = objCalificacion.objCliente.RespPregunta15Com.Trim();
                        txtRespuestaPregunta16.Text = objCalificacion.objCliente.RespPregunta16Com.Trim();
                        txtRespuestaPregunta17.Text = objCalificacion.objCliente.RespPregunta17Com.Trim();
                        txtRespuestaPregunta18.Text = objCalificacion.objCliente.RespPregunta18Com.Trim();

                        //cargar los valores de las listas asociadas a las preguntas
                        InicializarListaPreguntas(objCalificacion);

                        //cargar valor seleccionado para cada una de las listas de las preguntas
                        ddlPregunta1.SelectedValue = objCalificacion.objCliente.RespListaPregunta1Com.Trim();
                        ddlPregunta2.SelectedValue = objCalificacion.objCliente.RespListaPregunta2Com.Trim();
                        ddlPregunta3.SelectedValue = objCalificacion.objCliente.RespListaPregunta3Com.Trim();
                        ddlPregunta4.SelectedValue = objCalificacion.objCliente.RespListaPregunta4Com.Trim();
                        ddlPregunta5.SelectedValue = objCalificacion.objCliente.RespListaPregunta5Com.Trim();
                        ddlPregunta6.SelectedValue = objCalificacion.objCliente.RespListaPregunta6Com.Trim();
                        ddlPregunta7.SelectedValue = objCalificacion.objCliente.RespListaPregunta7Com.Trim();
                        ddlPregunta8.SelectedValue = objCalificacion.objCliente.RespListaPregunta8Com.Trim();
                        ddlPregunta9.SelectedValue = objCalificacion.objCliente.RespListaPregunta9Com.Trim();
                        ddlPregunta10.SelectedValue = objCalificacion.objCliente.RespListaPregunta10Com.Trim();
                        ddlPregunta11.SelectedValue = objCalificacion.objCliente.RespListaPregunta11Com.Trim();
                        ddlPregunta12.SelectedValue = objCalificacion.objCliente.RespListaPregunta12Com.Trim();
                        ddlPregunta13.SelectedValue = objCalificacion.objCliente.RespListaPregunta13Com.Trim();
                        ddlPregunta14.SelectedValue = objCalificacion.objCliente.RespListaPregunta14Com.Trim();

                        //Fin PMO27494
                        // PMO18879 - El comercial no debe visualizar los campos (Comentarios Riesgos y Cliente pertenece IFRS)
                        lblComentariosRiesgos.Visible = false;
                        txtComentariosRiesgos.Visible = false;

                        btnGuardar.Visible = true;
                        btnCovenants.Visible = false;
                        btnProrrogas.Visible = false;
                        btnPEC.Visible = false;

                        propiedades = guardarPropiedadesComunes(propiedades);

                        //Fin PMO27494

                        PropiedadesCalificacion propiedad6 = new PropiedadesCalificacion();
                        propiedad6.nombrePripiedad = ddlCalNRRecom.ID;
                        propiedad6.valorPropiedad = ddlCalNRRecom.SelectedValue;

                        propiedades.Add(propiedad6);

                        ViewState["Cambios"] = propiedades;

                        btnBuscarNuevo.Visible = true;

                    }
                    else if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC"))
                    {
                        //habilitar calificación comercial
                        pnlCalificacionComercial.Visible = false;
                        pnlCalificacionPIC.Visible = true;
                        pnlCalificacionSuper.Visible = false;
                        pnlCalificacionConsulta.Visible = false;

                        //pintar controles de calif PIC
                        lblCalificacionINRatingPIC.Text = objCalificacion.objCliente.CalIntNRating.Trim();
                        lblFechaCalifP.Text = objCalificacion.objCliente.fecCalInt.Trim();
                        lblCovenantP.Text = objCalificacion.objCliente.segCovenants.Trim();
                        lblCalMAFNuevoRatingPic.Text = objCalificacion.objCliente.CalMAFNRating.Trim();
                        lblCalifRecPEC.Text = objCalificacion.objCliente.calSugeridaPEC.Trim();

                        lblCalNuevoRRecom.Text = objCalificacion.objCliente.calNuevoRatRecom.Trim();
                        lblListasdeControl.Text = objCalificacion.objCliente.ListasDeControl.Trim();
                        lblCalifRecomenGrupo.Text = objCalificacion.objCliente.CalifRecomenGrupo.Trim();

                        //ADICIONAR CALIFICACION INTERNA RATIFICADA NUEVO RATING
                        IEnumerable<ListItem> CalifInterna = param.Where(p => p.paramName.Trim().Equals("LCALNR") && p.paramSeq != 0 && p.param6.Trim() != "0").OrderBy(p => p.paramSeq).Select(x =>
                                   new ListItem()
                                   {
                                       Text = x.param7.ToString().Trim()
                                   }).Distinct();

                        ddlCalifInternaRNR.DataSource = CalifInterna;
                        ddlCalifInternaRNR.DataBind();
                        inicializarLista(ddlCalifInternaRNR, objCalificacion.objCliente.calIntRatNRating, false);

                        ddlCalifExternaP.DataSource = objCalificacion.setDatos.Tables[2];
                        ddlCalifExternaP.DataTextField = "APCVALCHA7";
                        ddlCalifExternaP.DataValueField = "APCVALCHA7";
                        ddlCalifExternaP.DataBind();
                        inicializarLista(ddlCalifExternaP, objCalificacion.objCliente.calExtRat, false);

                        ddlRazon.DataSource = objCalificacion.setDatos.Tables[9];
                        ddlRazon.DataTextField = "APCVALCHA7";
                        ddlRazon.DataValueField = "APCVALCHA7";
                        ddlRazon.DataBind();
                        inicializarLista(ddlRazon, objCalificacion.objCliente.razCaliInt, true);

                        //Tipo cliente
                        ddlTipoCliente.DataSource = objCalificacion.setDatos.Tables[11];
                        ddlTipoCliente.DataTextField = "APCVALCHA7";
                        ddlTipoCliente.DataValueField = "APCVALCHA7";
                        ddlTipoCliente.DataBind();
                        inicializarLista(ddlTipoCliente, objCalificacion.objCliente.tipoCliente, false);

                        //Utilizo EEFF
                        ddlUtilizoEEFF.DataSource = objCalificacion.setDatos.Tables[3];
                        ddlUtilizoEEFF.DataTextField = "APCVALCHA7";
                        ddlUtilizoEEFF.DataValueField = "APCVALCHA7";
                        ddlUtilizoEEFF.DataBind();
                        inicializarLista(ddlUtilizoEEFF, objCalificacion.objCliente.utilizoEEFF, false);

                        ddlSeguimiento.DataSource = objCalificacion.setDatos.Tables[3];
                        ddlSeguimiento.DataTextField = "APCVALCHA7";
                        ddlSeguimiento.DataValueField = "APCVALCHA7";
                        ddlSeguimiento.DataBind();
                        inicializarLista(ddlSeguimiento, objCalificacion.objCliente.segProxComite, false);

                        ddlRecomendacion.DataSource = objCalificacion.setDatos.Tables[3];
                        ddlRecomendacion.DataTextField = "APCVALCHA7";
                        ddlRecomendacion.DataValueField = "APCVALCHA7";
                        ddlRecomendacion.DataBind();
                        inicializarLista(ddlRecomendacion, objCalificacion.objCliente.recAEC, false);


                        ddlEstadoCal.DataSource = objCalificacion.setDatos.Tables[10];
                        ddlEstadoCal.DataTextField = "APCVALCHA7";
                        ddlEstadoCal.DataValueField = "APCVALCHA7";
                        ddlEstadoCal.DataBind();
                        inicializarLista(ddlEstadoCal, objCalificacion.objCliente.estadoCalificacion, false);

                        //PMO27494
                        InicializarListaPreguntas(objCalificacion);

                        //poner comentarios riesgos para edición
                        if (objCalificacion.objCliente.calIntRatNRating.Trim().Length > 0)
                        {
                            txtSustentacionCalRec.Text = objCalificacion.objCliente.SustentacionCalRecPIC;
                            txtRespuestaPregunta1.Text = objCalificacion.objCliente.RespPregunta1PIC;
                            txtRespuestaPregunta2.Text = objCalificacion.objCliente.RespPregunta2PIC;
                            txtRespuestaPregunta3.Text = objCalificacion.objCliente.RespPregunta3PIC;
                            txtRespuestaPregunta4.Text = objCalificacion.objCliente.RespPregunta4PIC;
                            txtRespuestaPregunta5.Text = objCalificacion.objCliente.RespPregunta5PIC;
                            txtRespuestaPregunta6.Text = objCalificacion.objCliente.RespPregunta6PIC;
                            txtRespuestaPregunta7.Text = objCalificacion.objCliente.RespPregunta7PIC;
                            txtRespuestaPregunta8.Text = objCalificacion.objCliente.RespPregunta8PIC;
                            txtRespuestaPregunta9.Text = objCalificacion.objCliente.RespPregunta9PIC;
                            txtRespuestaPregunta10.Text = objCalificacion.objCliente.RespPregunta10PIC;
                            txtRespuestaPregunta11.Text = objCalificacion.objCliente.RespPregunta11PIC;
                            txtRespuestaPregunta12.Text = objCalificacion.objCliente.RespPregunta12PIC;
                            txtRespuestaPregunta13.Text = objCalificacion.objCliente.RespPregunta13PIC;
                            txtRespuestaPregunta14.Text = objCalificacion.objCliente.RespPregunta14PIC;
                            txtRespuestaPregunta15.Text = objCalificacion.objCliente.RespPregunta15PIC;
                            txtRespuestaPregunta16.Text = objCalificacion.objCliente.RespPregunta16PIC;
                            txtRespuestaPregunta17.Text = objCalificacion.objCliente.RespPregunta17PIC;
                            txtRespuestaPregunta18.Text = objCalificacion.objCliente.RespPregunta18PIC;

                            //asignar valores seleccionados a las listas
                            ddlPregunta1.SelectedValue = objCalificacion.objCliente.RespListaPregunta1PIC.Trim();
                            ddlPregunta2.SelectedValue = objCalificacion.objCliente.RespListaPregunta2PIC.Trim();
                            ddlPregunta3.SelectedValue = objCalificacion.objCliente.RespListaPregunta3PIC.Trim();
                            ddlPregunta4.SelectedValue = objCalificacion.objCliente.RespListaPregunta4PIC.Trim();
                            ddlPregunta5.SelectedValue = objCalificacion.objCliente.RespListaPregunta5PIC.Trim();
                            ddlPregunta6.SelectedValue = objCalificacion.objCliente.RespListaPregunta6PIC.Trim();
                            ddlPregunta7.SelectedValue = objCalificacion.objCliente.RespListaPregunta7PIC.Trim();
                            ddlPregunta8.SelectedValue = objCalificacion.objCliente.RespListaPregunta8PIC.Trim();
                            ddlPregunta9.SelectedValue = objCalificacion.objCliente.RespListaPregunta9PIC.Trim();
                            ddlPregunta10.SelectedValue = objCalificacion.objCliente.RespListaPregunta10PIC.Trim();
                            ddlPregunta11.SelectedValue = objCalificacion.objCliente.RespListaPregunta11PIC.Trim();
                            ddlPregunta12.SelectedValue = objCalificacion.objCliente.RespListaPregunta12PIC.Trim();
                            ddlPregunta13.SelectedValue = objCalificacion.objCliente.RespListaPregunta13PIC.Trim();
                            ddlPregunta14.SelectedValue = objCalificacion.objCliente.RespListaPregunta14PIC.Trim();

                            //Fin PMO27494

                            // PMO18879
                            txtComentariosRiesgos.Text = objCalificacion.objCliente.comentarioRiesgos.Trim();
                            ddlCalifExternaP.SelectedValue = objCalificacion.objCliente.calExtRat.Trim();
                            ddlEstadoCal.SelectedValue = objCalificacion.objCliente.estadoCalificacion.Trim();

                            //ddlRazon.SelectedValue = objCalificacion.objCliente.razCaliInt.Trim();
                            ddlUtilizoEEFF.SelectedValue = objCalificacion.objCliente.utilizoEEFF.Trim();
                            ddlTipoCliente.SelectedValue = objCalificacion.objCliente.tipoCliente.Trim();

                            ddlSeguimiento.SelectedValue = objCalificacion.objCliente.segProxComite.Trim();
                            ddlRecomendacion.SelectedValue = objCalificacion.objCliente.recAEC.Trim();
                            ddlCalifInternaRNR.SelectedValue = objCalificacion.objCliente.calIntRatNRating.Trim();

                        }
                        //poner comentarios comercial para edicion
                        else if (objCalificacion.objCliente.calNuevoRatRecom.Trim().Length > 0)
                        {
                            txtSustentacionCalRec.Text = objCalificacion.objCliente.SustentacionCalRecCom;
                            txtRespuestaPregunta1.Text = objCalificacion.objCliente.RespPregunta1Com;
                            txtRespuestaPregunta2.Text = objCalificacion.objCliente.RespPregunta2Com;
                            txtRespuestaPregunta3.Text = objCalificacion.objCliente.RespPregunta3Com;
                            txtRespuestaPregunta4.Text = objCalificacion.objCliente.RespPregunta4Com;
                            txtRespuestaPregunta5.Text = objCalificacion.objCliente.RespPregunta5Com;
                            txtRespuestaPregunta6.Text = objCalificacion.objCliente.RespPregunta6Com;
                            txtRespuestaPregunta7.Text = objCalificacion.objCliente.RespPregunta7Com;
                            txtRespuestaPregunta8.Text = objCalificacion.objCliente.RespPregunta8Com;
                            txtRespuestaPregunta9.Text = objCalificacion.objCliente.RespPregunta9Com;
                            txtRespuestaPregunta10.Text = objCalificacion.objCliente.RespPregunta10Com;
                            txtRespuestaPregunta11.Text = objCalificacion.objCliente.RespPregunta11Com;
                            txtRespuestaPregunta12.Text = objCalificacion.objCliente.RespPregunta12Com;
                            txtRespuestaPregunta13.Text = objCalificacion.objCliente.RespPregunta13Com;
                            txtRespuestaPregunta14.Text = objCalificacion.objCliente.RespPregunta14Com;
                            txtRespuestaPregunta15.Text = objCalificacion.objCliente.RespPregunta15Com;
                            txtRespuestaPregunta16.Text = objCalificacion.objCliente.RespPregunta16Com;
                            txtRespuestaPregunta17.Text = objCalificacion.objCliente.RespPregunta17Com;
                            txtRespuestaPregunta18.Text = objCalificacion.objCliente.RespPregunta18Com;

                            txtComentariosRiesgos.Text = objCalificacion.objCliente.comentarioRiesgos.Trim();

                            ddlPregunta1.SelectedValue = objCalificacion.objCliente.RespListaPregunta1Com.Trim();
                            ddlPregunta2.SelectedValue = objCalificacion.objCliente.RespListaPregunta2Com.Trim();
                            ddlPregunta3.SelectedValue = objCalificacion.objCliente.RespListaPregunta3Com.Trim();
                            ddlPregunta4.SelectedValue = objCalificacion.objCliente.RespListaPregunta4Com.Trim();
                            ddlPregunta5.SelectedValue = objCalificacion.objCliente.RespListaPregunta5Com.Trim();
                            ddlPregunta6.SelectedValue = objCalificacion.objCliente.RespListaPregunta6Com.Trim();
                            ddlPregunta7.SelectedValue = objCalificacion.objCliente.RespListaPregunta7Com.Trim();
                            ddlPregunta8.SelectedValue = objCalificacion.objCliente.RespListaPregunta8Com.Trim();
                            ddlPregunta9.SelectedValue = objCalificacion.objCliente.RespListaPregunta9Com.Trim();
                            ddlPregunta10.SelectedValue = objCalificacion.objCliente.RespListaPregunta10Com.Trim();
                            ddlPregunta11.SelectedValue = objCalificacion.objCliente.RespListaPregunta11Com.Trim();
                            ddlPregunta12.SelectedValue = objCalificacion.objCliente.RespListaPregunta12Com.Trim();
                            ddlPregunta13.SelectedValue = objCalificacion.objCliente.RespListaPregunta13Com.Trim();
                            ddlPregunta14.SelectedValue = objCalificacion.objCliente.RespListaPregunta14Com.Trim();

                            //Fin PMO27494
                        }

                        btnGuardar.Visible = true;

                        List<PropiedadesCalificacion> propiedades = new List<PropiedadesCalificacion>();
                        //Ini PMO27494
                        propiedades = guardarPropiedadesComunes(propiedades);

                        PropiedadesCalificacion propiedadComPic = new PropiedadesCalificacion();
                        propiedadComPic.nombrePripiedad = txtComentariosRiesgos.ID;
                        propiedadComPic.valorPropiedad = txtComentariosRiesgos.Text.Trim();

                        propiedades.Add(propiedadComPic);

                        //FIN PMO27494

                        PropiedadesCalificacion propiedad5 = new PropiedadesCalificacion();
                        propiedad5.nombrePripiedad = ddlCalifExternaP.ID;
                        propiedad5.valorPropiedad = ddlCalifExternaP.SelectedValue;

                        propiedades.Add(propiedad5);

                        PropiedadesCalificacion propiedad6 = new PropiedadesCalificacion();
                        propiedad6.nombrePripiedad = ddlSeguimiento.ID;
                        propiedad6.valorPropiedad = ddlSeguimiento.SelectedValue;

                        propiedades.Add(propiedad6);

                        PropiedadesCalificacion propiedad7 = new PropiedadesCalificacion();
                        propiedad7.nombrePripiedad = ddlRecomendacion.ID;
                        propiedad7.valorPropiedad = ddlRecomendacion.SelectedValue;

                        propiedades.Add(propiedad7);

                        PropiedadesCalificacion propiedad8 = new PropiedadesCalificacion();
                        propiedad8.nombrePripiedad = ddlRazon.ID;
                        propiedad8.valorPropiedad = ddlRazon.SelectedValue;

                        propiedades.Add(propiedad8);

                        PropiedadesCalificacion propiedad9 = new PropiedadesCalificacion();
                        propiedad9.nombrePripiedad = ddlCalifInternaRNR.ID;
                        propiedad9.valorPropiedad = ddlCalifInternaRNR.SelectedValue;

                        propiedades.Add(propiedad9);

                        PropiedadesCalificacion propiedad10 = new PropiedadesCalificacion();
                        propiedad10.nombrePripiedad = ddlCalifInternaRNR.ID;
                        propiedad10.valorPropiedad = ddlCalifInternaRNR.SelectedValue;

                        propiedades.Add(propiedad10);

                        PropiedadesCalificacion propiedad12 = new PropiedadesCalificacion();
                        propiedad12.nombrePripiedad = ddlEstadoCal.ID;
                        propiedad12.valorPropiedad = ddlEstadoCal.SelectedValue;

                        propiedades.Add(propiedad12);

                        ViewState["Cambios"] = propiedades;

                        btnCovenants.Visible = true;
                        btnPEC.Visible = true;
                        btnProrrogas.Visible = true;

                        btnBuscarNuevo.Visible = true; ;
                        if (!objCalificacion.objCliente.calSugeridaPEC.Trim().Equals(string.Empty))
                        {
                            btnPEC.Enabled = true;
                        }
                        else
                        {
                            btnPEC.Enabled = false;
                        }

                        if (objCalificacion.objCliente.segCovenants.Trim().ToUpper().Equals("SI"))
                        {
                            btnCovenants.Enabled = true;
                        }
                        else
                        {
                            btnCovenants.Enabled = false;
                        }
                    }
                    // PMO19939 - RF048: En el perfil “Consulta” y “Superfinanciera” para el comité presencial replicar la vista del Perfil “Riesgos” sin posibilidad de modificarla
                    else if (((Usuario)Session["Rol"]).rol.Equals("Superfinanciera") || ((Usuario)Session["Rol"]).rol.Equals("Consulta"))
                    {
                        //habilitar calificación comercial
                        pnlCalificacionComercial.Visible = false;
                        pnlCalificacionPIC.Visible = false;
                        pnlCalificacionSuper.Visible = false;
                        pnlCalificacionConsulta.Visible = true;

                        //pintar controles de calif Superfinanciera y Consulta
                        lblCalificacionINRatingCon.Text = objCalificacion.objCliente.CalIntNRating.Trim();
                        lblCalifInternaRNRCon.Text = objCalificacion.objCliente.calIntRatNRating.Trim().Equals("0") ? "" : objCalificacion.objCliente.calIntRatNRating.Trim();
                        lblFechaCalifCon.Text = objCalificacion.objCliente.fecCalInt.Trim();
                        lblCalifExternaCon.Text = objCalificacion.objCliente.calExtRat.Trim().Equals("0") ? "" : objCalificacion.objCliente.calExtRat.Trim();
                        lblCovenantCon.Text = objCalificacion.objCliente.segCovenants.Trim();
                        lblSeguimientoCon.Text = objCalificacion.objCliente.segProxComite.Trim().Equals("0") ? "" : objCalificacion.objCliente.segProxComite.Trim();
                        lblCalMAFNuevoRatingCon.Text = objCalificacion.objCliente.CalMAFNRating.Trim();
                        lblRecomendacionCon.Text = objCalificacion.objCliente.recAEC.Trim().Equals("0") ? "" : objCalificacion.objCliente.recAEC.Trim();
                        lblCalifRecPECCon.Text = objCalificacion.objCliente.calSugeridaPEC.Trim();
                        lblRazCalifExtCon.Text = processoRazCaliInt(objCalificacion.objCliente.razCaliInt);
                        lblTipoClienteCon.Text = objCalificacion.objCliente.tipoCliente.Trim().Equals("0") ? "" : objCalificacion.objCliente.tipoCliente.Trim();
                        lblCalNuevoRRecomCon.Text = objCalificacion.objCliente.calNuevoRatRecom.Trim().Equals("0") ? "" : objCalificacion.objCliente.calNuevoRatRecom.Trim();
                        lblUtilizoEEFF.Text = objCalificacion.objCliente.utilizoEEFF.Trim().Equals("0") ? "" : objCalificacion.objCliente.utilizoEEFF.Trim();

                        //Ini PMO27494
                        lblCalifRecomenGrupoCons.Text = objCalificacion.objCliente.CalifRecomenGrupo.Trim();
                        lblListasdeControlCons.Text = objCalificacion.objCliente.ListasDeControl.Trim();

                        // se dashabilitan los TextBox para consulta
                        txtSustentacionCalRec.ReadOnly = true;
                        txtRespuestaPregunta1.ReadOnly = true;
                        txtRespuestaPregunta2.ReadOnly = true;
                        txtRespuestaPregunta3.ReadOnly = true;
                        txtRespuestaPregunta4.ReadOnly = true;
                        txtRespuestaPregunta5.ReadOnly = true;
                        txtRespuestaPregunta6.ReadOnly = true;
                        txtRespuestaPregunta7.ReadOnly = true;
                        txtRespuestaPregunta8.ReadOnly = true;
                        txtRespuestaPregunta9.ReadOnly = true;
                        txtRespuestaPregunta10.ReadOnly = true;
                        txtRespuestaPregunta11.ReadOnly = true;
                        txtRespuestaPregunta12.ReadOnly = true;
                        txtRespuestaPregunta13.ReadOnly = true;
                        txtRespuestaPregunta14.ReadOnly = true;
                        txtRespuestaPregunta15.ReadOnly = true;
                        txtRespuestaPregunta16.ReadOnly = true;
                        txtRespuestaPregunta17.ReadOnly = true;
                        txtRespuestaPregunta18.ReadOnly = true;
                        txtComentariosRiesgos.ReadOnly = true;
                        txtRazonCalificacion.ReadOnly = true;

                        // se dashabilitan los dropdownlist para consulta
                        ddlPregunta1.Attributes.Add("disabled", "disabled");
                        ddlPregunta2.Attributes.Add("disabled", "disabled");
                        ddlPregunta3.Attributes.Add("disabled", "disabled");
                        ddlPregunta4.Attributes.Add("disabled", "disabled");
                        ddlPregunta5.Attributes.Add("disabled", "disabled");
                        ddlPregunta6.Attributes.Add("disabled", "disabled");
                        ddlPregunta7.Attributes.Add("disabled", "disabled");
                        ddlPregunta8.Attributes.Add("disabled", "disabled");
                        ddlPregunta9.Attributes.Add("disabled", "disabled");
                        ddlPregunta10.Attributes.Add("disabled", "disabled");
                        ddlPregunta11.Attributes.Add("disabled", "disabled");
                        ddlPregunta12.Attributes.Add("disabled", "disabled");
                        ddlPregunta13.Attributes.Add("disabled", "disabled");
                        ddlPregunta14.Attributes.Add("disabled", "disabled");

                        if (!objCalificacion.objCliente.SustentacionCalRecPIC.Trim().Equals(string.Empty))

                        {
                            txtSustentacionCalRec.Text = objCalificacion.objCliente.SustentacionCalRecPIC.Trim();
                            txtRespuestaPregunta1.Text = objCalificacion.objCliente.RespPregunta1PIC.Trim();
                            txtRespuestaPregunta2.Text = objCalificacion.objCliente.RespPregunta2PIC.Trim();
                            txtRespuestaPregunta3.Text = objCalificacion.objCliente.RespPregunta3PIC.Trim();
                            txtRespuestaPregunta4.Text = objCalificacion.objCliente.RespPregunta4PIC.Trim();
                            txtRespuestaPregunta5.Text = objCalificacion.objCliente.RespPregunta5PIC.Trim();
                            txtRespuestaPregunta6.Text = objCalificacion.objCliente.RespPregunta6PIC.Trim();
                            txtRespuestaPregunta7.Text = objCalificacion.objCliente.RespPregunta7PIC.Trim();
                            txtRespuestaPregunta8.Text = objCalificacion.objCliente.RespPregunta8PIC.Trim();
                            txtRespuestaPregunta9.Text = objCalificacion.objCliente.RespPregunta9PIC.Trim();
                            txtRespuestaPregunta10.Text = objCalificacion.objCliente.RespPregunta10PIC.Trim();
                            txtRespuestaPregunta11.Text = objCalificacion.objCliente.RespPregunta11PIC.Trim();
                            txtRespuestaPregunta12.Text = objCalificacion.objCliente.RespPregunta12PIC.Trim();
                            txtRespuestaPregunta13.Text = objCalificacion.objCliente.RespPregunta13PIC.Trim();
                            txtRespuestaPregunta14.Text = objCalificacion.objCliente.RespPregunta14PIC.Trim();
                            txtRespuestaPregunta15.Text = objCalificacion.objCliente.RespPregunta15PIC.Trim();
                            txtRespuestaPregunta16.Text = objCalificacion.objCliente.RespPregunta16PIC.Trim();
                            txtRespuestaPregunta17.Text = objCalificacion.objCliente.RespPregunta17PIC.Trim();
                            txtRespuestaPregunta18.Text = objCalificacion.objCliente.RespPregunta18PIC.Trim();
                            txtComentariosRiesgos.Text = objCalificacion.objCliente.comentarioRiesgos.Trim();

                            InicializarListaPreguntas(objCalificacion);

                            // se anexan las listas
                            ddlPregunta1.SelectedValue = objCalificacion.objCliente.RespListaPregunta1PIC.Trim();
                            ddlPregunta2.SelectedValue = objCalificacion.objCliente.RespListaPregunta2PIC.Trim();
                            ddlPregunta3.SelectedValue = objCalificacion.objCliente.RespListaPregunta3PIC.Trim();
                            ddlPregunta4.SelectedValue = objCalificacion.objCliente.RespListaPregunta4PIC.Trim();
                            ddlPregunta5.SelectedValue = objCalificacion.objCliente.RespListaPregunta5PIC.Trim();
                            ddlPregunta6.SelectedValue = objCalificacion.objCliente.RespListaPregunta6PIC.Trim();
                            ddlPregunta7.SelectedValue = objCalificacion.objCliente.RespListaPregunta7PIC.Trim();
                            ddlPregunta8.SelectedValue = objCalificacion.objCliente.RespListaPregunta8PIC.Trim();
                            ddlPregunta9.SelectedValue = objCalificacion.objCliente.RespListaPregunta9PIC.Trim();
                            ddlPregunta10.SelectedValue = objCalificacion.objCliente.RespListaPregunta10PIC.Trim();
                            ddlPregunta11.SelectedValue = objCalificacion.objCliente.RespListaPregunta11PIC.Trim();
                            ddlPregunta12.SelectedValue = objCalificacion.objCliente.RespListaPregunta12PIC.Trim();
                            ddlPregunta13.SelectedValue = objCalificacion.objCliente.RespListaPregunta13PIC.Trim();
                            ddlPregunta14.SelectedValue = objCalificacion.objCliente.RespListaPregunta14PIC.Trim();
                        }

                        btnGuardar.Visible = false;
                        btnCovenants.Visible = false;
                        btnProrrogas.Visible = false;
                        btnPEC.Visible = false;
                        btnBuscarNuevo.Visible = true;
                    }
                    else
                    {
                        //habilitar calificación comercial
                        pnlCalificacionComercial.Visible = false;
                        pnlCalificacionPIC.Visible = false;
                        pnlCalificacionSuper.Visible = true;
                        pnlCalificacionConsulta.Visible = false;

                        lblCalifExtRatif.Text = (objCalificacion.objCliente.calExtRat.Trim().Equals("0") ? "" : objCalificacion.objCliente.calExtRat.Trim());
                        lblRazonCalifInternaSuper.Text = processoRazCaliInt(objCalificacion.objCliente.razCaliInt);
                        lblCalMAFNuevoRatingSup.Text = objCalificacion.objCliente.CalMAFNRating.Trim();
                        lblCalificacionIRNR.Text = (objCalificacion.objCliente.calIntRatNRating.Trim().Equals("0") ? "" : objCalificacion.objCliente.calIntRatNRating.Trim());

                        txtComentariosRiesgos.ReadOnly = true;

                        if (!objCalificacion.objCliente.actClientePIC.Trim().Equals(string.Empty))
                        {
                            txtComentariosRiesgos.Text = objCalificacion.objCliente.comentarioRiesgos.Trim();
                        }

                        btnGuardar.Visible = false;
                        btnCovenants.Visible = false;
                        btnProrrogas.Visible = false;
                        btnPEC.Visible = false;
                        btnBuscarNuevo.Visible = true;
                    }
                }
                else
                {
                    //habilitar controles de cliente
                    pnlCliente.Visible = true;

                    //habilitar calificación comercial
                    pnlCalificacionComercial.Visible = false;
                    pnlCalificacionPIC.Visible = true;
                    pnlCalificacionSuper.Visible = false;
                    pnlCalificacionConsulta.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CalificacionCliente";
                errEnt.Error = "MEN.0409";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }

       

        private void inicializarLista(ListControl lista, string itemSeleccionado, Boolean split)
        {
            if (split)
            {
                //Se separan los valores concatenados por '|'
                string[] items = itemSeleccionado.Split('|');
                foreach (string item in items)
                {
                    ListItem itemList = lista.Items.FindByValue(item);
                    if (itemList != null)
                    {
                        itemList.Selected = true;
                    }
                }
            }
            else
            {
                //Agregar seleccione a Dropdownlist
                ListItem seleccione = new ListItem("Seleccione...", "0");
                lista.Items.Insert(0, seleccione);

                //Dar tooltip a los items
                foreach (ListItem li in lista.Items)
                {
                    li.Attributes.Add("title", li.Text);
                }
            }
        }
        ///Inicializar lista de preguntas PMO27494
        private void InicializarListaPreguntas(ObjetosCalificacion objCalificacion)
        {

            ddlPregunta1.DataSource = objCalificacion.setDatos.Tables[3];
            ddlPregunta1.DataTextField = "APCVALCHA7";
            ddlPregunta1.DataValueField = "APCVALCHA7";
            ddlPregunta1.DataBind();
            inicializarLista(ddlPregunta1, "0", false);

            ddlPregunta2.DataSource = objCalificacion.setDatos.Tables[3];
            ddlPregunta2.DataTextField = "APCVALCHA7";
            ddlPregunta2.DataValueField = "APCVALCHA7";
            ddlPregunta2.DataBind();
            inicializarLista(ddlPregunta2, "0", false);

            ddlPregunta3.DataSource = objCalificacion.setDatos.Tables[3];
            ddlPregunta3.DataTextField = "APCVALCHA7";
            ddlPregunta3.DataValueField = "APCVALCHA7";
            ddlPregunta3.DataBind();
            inicializarLista(ddlPregunta3, "0", false);

            ddlPregunta4.DataSource = objCalificacion.setDatos.Tables[3];
            ddlPregunta4.DataTextField = "APCVALCHA7";
            ddlPregunta4.DataValueField = "APCVALCHA7";
            ddlPregunta4.DataBind();
            inicializarLista(ddlPregunta4, "0", false);

            ddlPregunta5.DataSource = objCalificacion.setDatos.Tables[3];
            ddlPregunta5.DataTextField = "APCVALCHA7";
            ddlPregunta5.DataValueField = "APCVALCHA7";
            ddlPregunta5.DataBind();
            inicializarLista(ddlPregunta5, "0", false);

            ddlPregunta6.DataSource = objCalificacion.setDatos.Tables[3];
            ddlPregunta6.DataTextField = "APCVALCHA7";
            ddlPregunta6.DataValueField = "APCVALCHA7";
            ddlPregunta6.DataBind();
            inicializarLista(ddlPregunta6, "0", false);

            ddlPregunta7.DataSource = objCalificacion.setDatos.Tables[3];
            ddlPregunta7.DataTextField = "APCVALCHA7";
            ddlPregunta7.DataValueField = "APCVALCHA7";
            ddlPregunta7.DataBind();
            inicializarLista(ddlPregunta7, "0", false);

            ddlPregunta8.DataSource = objCalificacion.setDatos.Tables[3];
            ddlPregunta8.DataTextField = "APCVALCHA7";
            ddlPregunta8.DataValueField = "APCVALCHA7";
            ddlPregunta8.DataBind();
            inicializarLista(ddlPregunta8, "0", false);

            ddlPregunta9.DataSource = objCalificacion.setDatos.Tables[3];
            ddlPregunta9.DataTextField = "APCVALCHA7";
            ddlPregunta9.DataValueField = "APCVALCHA7";
            ddlPregunta9.DataBind();
            inicializarLista(ddlPregunta9, "0", false);

            ddlPregunta10.DataSource = objCalificacion.setDatos.Tables[3];
            ddlPregunta10.DataTextField = "APCVALCHA7";
            ddlPregunta10.DataValueField = "APCVALCHA7";
            ddlPregunta10.DataBind();
            inicializarLista(ddlPregunta10, "0", false);

            ddlPregunta11.DataSource = objCalificacion.setDatos.Tables[3];
            ddlPregunta11.DataTextField = "APCVALCHA7";
            ddlPregunta11.DataValueField = "APCVALCHA7";
            ddlPregunta11.DataBind();
            inicializarLista(ddlPregunta11, "0", false);

            ddlPregunta12.DataSource = objCalificacion.setDatos.Tables[3];
            ddlPregunta12.DataTextField = "APCVALCHA7";
            ddlPregunta12.DataValueField = "APCVALCHA7";
            ddlPregunta12.DataBind();
            inicializarLista(ddlPregunta12, "0", false);

            ddlPregunta13.DataSource = objCalificacion.setDatos.Tables[3];
            ddlPregunta13.DataTextField = "APCVALCHA7";
            ddlPregunta13.DataValueField = "APCVALCHA7";
            ddlPregunta13.DataBind();
            inicializarLista(ddlPregunta13, "0", false);

            ddlPregunta14.DataSource = objCalificacion.setDatos.Tables[3];
            ddlPregunta14.DataTextField = "APCVALCHA7";
            ddlPregunta14.DataValueField = "APCVALCHA7";
            ddlPregunta14.DataBind();
            inicializarLista(ddlPregunta14, "0", false);

        }

        
        protected void pintarComite(ComiteAnterior objComA)
        {

            lblCANit.Text = objComA.nit;
            lblCANombre.Text = objComA.nombre;
            lblCAComite.Text = objComA.comite;
            lblCASegmento.Text = objComA.segmento;
            lblCAOficina.Text = objComA.oficina;
            lblCAGerente.Text = objComA.codGerente;
            lblCARegion.Text = objComA.region;
            lblCAZona.Text = objComA.zona;
            lblCAFechaMAF.Text = objComA.fecMaf;
            lblCAPuntajeMAF.Text = objComA.puntajeMaf;
            lblCACalificacionMAF.Text = objComA.calMaf;
            lblCACalificacionInterna.Text = objComA.calInterna;
            lblCACalificacionExterna.Text = objComA.calExterna;
            txtRazonCalificacion.Text = objComA.razCalInt.Trim().Replace("|", "\r\n");
            txtCAActividadCliente.Text = objComA.actCliente;
            txtCAAnalisisBalance.Text = objComA.analisisBalPic;
            txtCAAnalisisPyG.Text = objComA.analisisPyGPic;
            txtCAComAdicionales.Text = objComA.comAdPic;
            txtCAComentariosRiesgos.Text = objComA.comentariosRiesgos.Trim();
            lblCAClientePerteneceIFRS.Text = objComA.perteneceIFRS.Trim();
        }
        //PMO27494 Pintar Nuevo comite
        protected void pintarNuevoComite(ComiteAnterior objComA)
        {
            lblCANNit.Text = objComA.nit;
            lblCANNombre.Text = objComA.nombre;
            lblCANComite.Text = objComA.comite;
            lblCANSegmento.Text = objComA.segmento;
            lblCANOficina.Text = objComA.oficina;
            lblCANGerente.Text = objComA.codGerente;
            lblCANRegion.Text = objComA.region;
            lblCANZona.Text = objComA.zona;
            lblCANCalificacionMAF.Text = objComA.calMaf;
            lblCANCalificacionExterna.Text = objComA.calExterna;
            lblCANCalificacionInterna.Text = objComA.calInterna;
            txtCANRazonCalificacion.Text = objComA.razCalInt.Trim().Replace("|", "\r\n");
            txtCANSustentacionCalRec.Text = objComA.JustificacionCalificacion;
            lblCANPregunta1.Text = objComA.TextoPregunta1;
            lblJustPregunta1.Text = objComA.JustPregunta1;
            lblCANComplementoPregunta1.Text = objComA.ComplementoPregunta1;
            txtCANRespuestaPregunta1.Text = objComA.RespPregunta1;
            lblCANPregunta2.Text = objComA.TextoPregunta2;
            lblJustPregunta2.Text = objComA.JustPregunta2;
            lblCANComplementoPregunta2.Text = objComA.ComplementoPregunta2;
            txtCANRespuestaPregunta2.Text = objComA.RespPregunta2;
            lblCANPregunta3.Text = objComA.TextoPregunta3;
            lblJustPregunta3.Text = objComA.JustPregunta3;
            lblCANComplementoPregunta3.Text = objComA.ComplementoPregunta3;
            txtCANRespuestaPregunta4.Text = objComA.RespPregunta4;
            lblCANPregunta4.Text = objComA.TextoPregunta4;
            lblJustPregunta4.Text = objComA.JustPregunta4;
            lblCANComplementoPregunta4.Text = objComA.ComplementoPregunta4;
            txtCANRespuestaPregunta4.Text = objComA.RespPregunta4;
            lblCANPregunta5.Text = objComA.TextoPregunta5;
            lblJustPregunta5.Text = objComA.JustPregunta5;
            lblCANComplementoPregunta5.Text = objComA.ComplementoPregunta5;
            txtCANRespuestaPregunta5.Text = objComA.RespPregunta5;
            lblCANPregunta6.Text = objComA.TextoPregunta6;
            lblJustPregunta6.Text = objComA.JustPregunta6;
            lblCANComplementoPregunta6.Text = objComA.ComplementoPregunta6;
            txtCANRespuestaPregunta6.Text = objComA.RespPregunta6;
            lblCANPregunta7.Text = objComA.TextoPregunta7;
            lblJustPregunta7.Text = objComA.JustPregunta7;
            lblCANComplementoPregunta7.Text = objComA.ComplementoPregunta7;
            txtCANRespuestaPregunta7.Text = objComA.RespPregunta7;
            lblCANPregunta8.Text = objComA.TextoPregunta8;
            lblJustPregunta8.Text = objComA.JustPregunta8;
            lblCANComplementoPregunta8.Text = objComA.ComplementoPregunta8;
            txtCANRespuestaPregunta8.Text = objComA.RespPregunta8;
            lblCANPregunta9.Text = objComA.TextoPregunta9;
            lblJustPregunta9.Text = objComA.JustPregunta9;
            lblCANComplementoPregunta9.Text = objComA.ComplementoPregunta9;
            txtCANRespuestaPregunta9.Text = objComA.RespPregunta9;
            lblCANPregunta10.Text = objComA.TextoPregunta10;
            lblJustPregunta10.Text = objComA.JustPregunta10;
            lblCANComplementoPregunta10.Text = objComA.ComplementoPregunta10;
            txtCANRespuestaPregunta10.Text = objComA.RespPregunta10;
            lblCANPregunta11.Text = objComA.TextoPregunta11;
            lblJustPregunta11.Text = objComA.JustPregunta11;
            lblCANComplementoPregunta11.Text = objComA.ComplementoPregunta11;
            txtCANRespuestaPregunta11.Text = objComA.RespPregunta11;
            lblCANPregunta12.Text = objComA.TextoPregunta12;
            lblJustPregunta12.Text = objComA.JustPregunta12;
            lblCANComplementoPregunta12.Text = objComA.ComplementoPregunta12;
            txtCANRespuestaPregunta12.Text = objComA.RespPregunta12;
            lblCANPregunta13.Text = objComA.TextoPregunta13;
            lblJustPregunta13.Text = objComA.JustPregunta13;
            lblCANComplementoPregunta13.Text = objComA.ComplementoPregunta13;
            txtCANRespuestaPregunta13.Text = objComA.RespPregunta13;
            lblCANPregunta14.Text = objComA.TextoPregunta14;
            lblJustPregunta14.Text = objComA.JustPregunta14;
            lblCANComplementoPregunta14.Text = objComA.ComplementoPregunta14;
            txtCANRespuestaPregunta14.Text = objComA.RespPregunta14;
            TxtNuevoComentarioRiesgos.Text = objComA.comentariosRiesgos;
        }


        
        protected void pintarPEC(CentralExterna central)
        {
            lblNitPEC.Text = central.Nit.Trim();
            lblNombrePEC.Text = central.Nombre.Trim();
            lblSegmentoPEC.Text = central.Segmento.Trim();

            lblCalSug.Text = central.CalSugPEC.Trim();
            lblRazon.Text = central.RazonPEC.Trim();

            lblArrComCalSec.Text = central.CalArrCom.Trim();
            if (central.PorSalArr.Trim().Equals(string.Empty))
            {
                lblArrComPorSal.Text = "";
            }
            else
            {
                lblArrComPorSal.Text = central.PorSalArr.Trim() + "%";
            }
            lblArrComEnt1.Text = central.ArrComEnt1.Trim();
            lblArrComEnt2.Text = central.ArrComEnt2.Trim();

            lblReeComCalSec.Text = central.CalReeCom.Trim();
            if (central.PorSalRee.Trim().Equals(string.Empty))
            {
                lblReeComPorSal.Text = "";
            }
            else
            {
                lblReeComPorSal.Text = central.PorSalRee.Trim() + "%";
            }
            lblReeComEnt1.Text = central.ReeComEnt1.Trim();
            lblReeComEnt2.Text = central.ReeComEnt2.Trim();

            lblCasComCalSec.Text = central.CalCasCom.Trim();
            if (central.PorSalCas.Trim().Equals(string.Empty))
            {
                lblCasComPorSal.Text = "";
            }
            else
            {
                lblCasComPorSal.Text = central.PorSalCas.Trim() + "%";
            }
            lblCasComEnt1.Text = central.CasComEnt1.Trim();
            lblCasComEnt2.Text = central.CasComEnt2.Trim();

            lblCalAsig.Text = central.CalSugAli.Trim();
            lblCorInfo.Text = central.CorInfo.Trim();

            lblEntCal1.Text = central.CalEnt1.Trim();
            lblEntCal2.Text = central.CalEnt2.Trim();
            lblEntCal3.Text = central.CalEnt3.Trim();
            lblEntCal4.Text = central.CalEnt4.Trim();
            lblEntCal5.Text = central.CalEnt5.Trim();
            lblEntCal6.Text = central.CalEnt6.Trim();
            lblEntCal7.Text = central.CalEnt7.Trim();
            lblEntCal8.Text = central.CalEnt8.Trim();

            lblEntSal1.Text = central.SalEnt1.Trim();
            lblEntSal2.Text = central.SalEnt2.Trim();
            lblEntSal3.Text = central.SalEnt3.Trim();
            lblEntSal4.Text = central.SalEnt4.Trim();
            lblEntSal5.Text = central.SalEnt5.Trim();
            lblEntSal6.Text = central.SalEnt6.Trim();
            lblEntSal7.Text = central.SalEnt7.Trim();
            lblEntSal8.Text = central.SalEnt8.Trim();
            lblObservacion.Text = central.Observacion.Trim();
        }

        
        protected void pintarCovenants(Covenants covenants)
        {
            lblNitCov.Text = covenants.nit.Trim();
            lblGerenteCov.Text = covenants.gteCuenta.Trim();
            lblFrecRevCov.Text = covenants.frecRev.Trim();
            lblOficinaCov.Text = covenants.oficina.Trim();
            lblSegCov.Text = covenants.segmento.Trim();
            txtCovenantsCov.Text = covenants.covenant.Trim('"');
            lblGarantiaCov.Text = covenants.garantia.Trim();
            lblFecUltGesCov.Text = covenants.FecUltGest.Trim();
            lblNombreCov.Text = covenants.nombre.Trim();
            txtRespSegCov.Text = covenants.respSeg.Trim('"');
        }

        protected void pintarProrrogas(List<Prorrogas> prorrogas)
        {
            foreach (Prorrogas pr in prorrogas)
            {
                TableCell id = new TableCell();
                id.Text = pr.id;
                TableCell tipo = new TableCell();
                tipo.Text = pr.tipo;
                TableCell obl = new TableCell();
                obl.Text = pr.obl;
                TableCell numMan = new TableCell();
                numMan.Text = pr.numMan;
                TableCell desc = new TableCell();
                desc.Text = pr.desc;
                TableRow fila = new TableRow();
                fila.Controls.Add(id);
                fila.Controls.Add(tipo);
                fila.Controls.Add(obl);
                fila.Controls.Add(numMan);
                fila.Controls.Add(desc);
                tblProrrogas.Controls.Add(fila);
            }
        }

        
        protected void pintarIndicadores(IndicadoresCartera indicadores)
        {
            double valor;

            if (indicadores.dtIndicadores.Rows[0]["INDTIPO"].ToString().ToUpper() == "CONSTRUCTOR")
            {
                gvIndConstructor.DataSource = indicadores.dtIndicadores;
                gvIndConstructor.DataBind();

                pnlIndicadoresConstructor.Style.Add("height", "425");
                mpeIndicadoresConstructor.Show();
            }
            else if (indicadores.dtIndicadores.Rows[0]["INDTIPO"].ToString().ToUpper() == "GOBIERNO")
            {
                lblIDGobiernoValue.Text = indicadores.dtIndicadores.Rows[0]["INDNITCLI"].ToString();
                lblTipoIdGobiernoValue.Text = indicadores.dtIndicadores.Rows[0]["INDTNITCLI"].ToString();
                lblTipoGobiernoValue.Text = indicadores.dtIndicadores.Rows[0]["INDTIPO"].ToString();

                Double.TryParse(indicadores.dtIndicadores.Rows[0]["INDIADMCTR"].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out valor);
                lblIndicadorAdminCentral.Text = valor.ToString("#0.##%");

                Double.TryParse(indicadores.dtIndicadores.Rows[0]["INDLADMCTR"].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out valor);
                lblLimiteAdminCentral.Text = valor.ToString("#0.##%");

                lblIndAsam.Text = indicadores.dtIndicadores.Rows[0]["INDIAESTUD"].ToString();
                lblLimIndAsam.Text = indicadores.dtIndicadores.Rows[0]["INDLAESTUD"].ToString();
                lblIndContraloria.Text = indicadores.dtIndicadores.Rows[0]["INDICESTDO"].ToString();
                lblLimIndContraloria.Text = indicadores.dtIndicadores.Rows[0]["INDLCESTDO"].ToString();

                Double.TryParse(indicadores.dtIndicadores.Rows[0]["INDSOLVECI"].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out valor);
                lblSolvencia.Text = valor.ToString("#0.##%");

                Double.TryParse(indicadores.dtIndicadores.Rows[0]["INDSOSTENI"].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out valor);
                lblSostenibilidad.Text = valor.ToString("#0.##%");

                Double.TryParse(indicadores.dtIndicadores.Rows[0]["INDCOBERTU"].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out valor);
                lblCobertura.Text = valor.ToString("#0.##%");

                lblIndConsejo.Text = indicadores.dtIndicadores.Rows[0]["INDICESTUD"].ToString();
                lblLimIndConsejo.Text = indicadores.dtIndicadores.Rows[0]["INDLCESTUD"].ToString();
                lblIndPer.Text = indicadores.dtIndicadores.Rows[0]["INDIPESTUD"].ToString();
                lblLimIndPer.Text = indicadores.dtIndicadores.Rows[0]["INDLPESTUD"].ToString();

                pnlIndicadoresGobierno.Style.Add("height", "380");
                mpeIndicadoresGobierno.Show();
            }
            else if (indicadores.dtIndicadores.Rows[0]["INDTIPO"].ToString().ToUpper() == "FINANCIERO")
            {
                lblIdFinancieroValue.Text = indicadores.dtIndicadores.Rows[0]["INDNITCLI"].ToString();
                lblTipoIdFinancieroValue.Text = indicadores.dtIndicadores.Rows[0]["INDTNITCLI"].ToString();
                lblTipoFinancieroValue.Text = indicadores.dtIndicadores.Rows[0]["INDTIPO"].ToString();

                Double.TryParse(indicadores.dtIndicadores.Rows[0]["INDCCPVIVC"].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out valor);
                lblCalidCar.Text = valor.ToString("#0.##%");

                Double.TryParse(indicadores.dtIndicadores.Rows[0]["INDCCVENCI"].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out valor);
                lblCobCar.Text = valor.ToString("#0.##%");

                Double.TryParse(indicadores.dtIndicadores.Rows[0]["INDRSOLVEN"].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out valor);
                lblRelSol.Text = valor.ToString("#0.##%");

                Double.TryParse(indicadores.dtIndicadores.Rows[0]["INDEFINACI"].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out valor);
                lblEfiFin.Text = valor.ToString("#0.##%");

                lblUtiNet.Text = indicadores.dtIndicadores.Rows[0]["INDUNETA"].ToString();

                Double.TryParse(indicadores.dtIndicadores.Rows[0]["INDROE"].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out valor);
                lblROE.Text = valor.ToString("#0.##%");

                btnCerrarIndicadoresFinanciero.Visible = true;
                pnlIndicadoresFinanciero.Style.Add("height", "275");
                mpeIndicadoresFinanciero.Show();
            }
            else
            {
                divIndicadoresConstructor.Visible = true;
                divIndicadoresConstructor.Visible = false;
                pnlIndicadoresGobierno.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0440").Texto + "')", true);
            }

        }

       
        protected DataTable RangoFechaMAF()
        {
            DataTable dtFechas = new DataTable();
            dtFechas.Columns.Add("Fecha", typeof(string));
            DateTime FechaActual = DateTime.Now;
            int anio = FechaActual.Year;
            int mes = FechaActual.Month;

            if (mes < 7)
            {
                dtFechas.Rows.Add("jun-" + (anio - 1).ToString().Substring(2, 2));
                dtFechas.Rows.Add("jul-" + (anio - 1).ToString().Substring(2, 2));
                dtFechas.Rows.Add("ago-" + (anio - 1).ToString().Substring(2, 2));
                dtFechas.Rows.Add("sep-" + (anio - 1).ToString().Substring(2, 2));
                dtFechas.Rows.Add("oct-" + (anio - 1).ToString().Substring(2, 2));
                dtFechas.Rows.Add("nov-" + (anio - 1).ToString().Substring(2, 2));
                dtFechas.Rows.Add("dic-" + (anio - 1).ToString().Substring(2, 2));
                dtFechas.Rows.Add("ene-" + anio.ToString().Substring(2, 2));
                dtFechas.Rows.Add("feb-" + anio.ToString().Substring(2, 2));
                dtFechas.Rows.Add("mar-" + anio.ToString().Substring(2, 2));
                dtFechas.Rows.Add("abr-" + anio.ToString().Substring(2, 2));
                dtFechas.Rows.Add("may-" + anio.ToString().Substring(2, 2));
            }
            else if (mes >= 7)
            {
                dtFechas.Rows.Add("dic-" + (anio - 1).ToString().Substring(2, 2));
                dtFechas.Rows.Add("ene-" + anio.ToString().Substring(2, 2));
                dtFechas.Rows.Add("feb-" + anio.ToString().Substring(2, 2));
                dtFechas.Rows.Add("mar-" + anio.ToString().Substring(2, 2));
                dtFechas.Rows.Add("abr-" + anio.ToString().Substring(2, 2));
                dtFechas.Rows.Add("may-" + anio.ToString().Substring(2, 2));
                dtFechas.Rows.Add("jun-" + anio.ToString().Substring(2, 2));
                dtFechas.Rows.Add("jul-" + anio.ToString().Substring(2, 2));
                dtFechas.Rows.Add("ago-" + anio.ToString().Substring(2, 2));
                dtFechas.Rows.Add("sep-" + anio.ToString().Substring(2, 2));
                dtFechas.Rows.Add("oct-" + anio.ToString().Substring(2, 2));
                dtFechas.Rows.Add("nov-" + anio.ToString().Substring(2, 2));
            }
            return dtFechas;
        }

       


        #endregion

        protected void btnIndicadores_Click(object sender, EventArgs e)
        {
            try
            {
                //Proxy.CalificacionCartera proxyCartera = new Proxy.CalificacionCartera();
                Cliente objCliente = (Cliente)ViewState["Cliente"];
                ObjetosCalificacion objCalificacion = Fachada.CalificacionCartera.Indicadores(objCliente.nit.Trim());
                if (objCalificacion.objIndicadoresCartera != null)
                {
                    //pintarIndicadores
                    pintarIndicadores(objCalificacion.objIndicadoresCartera);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0440").Texto + "')", true);
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.ToString() + "//ApplicationPage:CalificacionCliente";
                errEnt.Error = "MEN.0440";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }

        protected void ddlCalificacionExternaChanged(object sender, EventArgs e)
        {
            if (ddlCalifExternaP.SelectedValue.Equals("AA")
                && lblFechaInfoFinanciera.Text.Equals(string.Empty)
                && ddlUtilizoEEFF.SelectedValue.Equals("SI"))
            {
                cvTxtEEFFUti.Enabled = true;
                plnCorteFteEEFF.Visible = true;
                mpeCorteFteEEFF.Show();
            }
        }


        protected void ddlUtilizoEEFFChanged(object sender, EventArgs e)
        {
            if ((ddlUtilizoEEFF.SelectedValue.Equals("NO"))
                || (ddlUtilizoEEFF.SelectedValue.Equals("SI")
                && ddlCalifExternaP.SelectedValue.Equals("AA")
                && lblFechaInfoFinanciera.Text.Equals(string.Empty)))
            {
                cvTxtEEFFUti.Enabled = true;
                plnCorteFteEEFF.Visible = true;
                mpeCorteFteEEFF.Show();
            }
        }

        protected void btnCerrarPlnCorteFteEEFF_Click(object sender, EventArgs e)
        {
            Cliente objCliente = (Cliente)ViewState["Cliente"];
            objCliente.EEFFUti = txtEEFFUti.Text.Trim();
            objCliente.FteEEFF = txtFteEEFF.Text.Trim();
            if (objCliente.EEFFUti.Equals(string.Empty) || objCliente.FteEEFF.Equals(string.Empty))
            {
                string msj = "La información debe ser diligenciada obligatoriamente";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + msj + "')", true);
            }
            else
            {
                ViewState["Cliente"] = objCliente;
                cvTxtEEFFUti.Enabled = false;
            }
        }

      
        private string processoRazCaliInt(string razCaliInt)
        {
            if (razCaliInt.Trim().Equals("0"))
            {
                return "";
            }
            else
            {
                if (razCaliInt.Trim().Contains("|"))
                {
                    return razCaliInt.Trim().Replace("|", "<BR/>");
                }
                else
                {
                    return (razCaliInt.Trim());
                }
            }
        }



       
        private void establecerEncodingCamposTexto()
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            txtSustentacionCalRec.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtSustentacionCalRec.Text.Trim()))));
            txtRespuestaPregunta1.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta1.Text.Trim()))));
            txtRespuestaPregunta2.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta2.Text.Trim()))));
            txtRespuestaPregunta3.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta3.Text.Trim()))));
            txtRespuestaPregunta4.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta4.Text.Trim()))));
            txtRespuestaPregunta5.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta5.Text.Trim()))));
            txtRespuestaPregunta6.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta6.Text.Trim()))));
            txtRespuestaPregunta7.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta7.Text.Trim()))));
            txtRespuestaPregunta8.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta8.Text.Trim()))));
            txtRespuestaPregunta9.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta9.Text.Trim()))));
            txtRespuestaPregunta10.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta10.Text.Trim()))));
            txtRespuestaPregunta11.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta11.Text.Trim()))));
            txtRespuestaPregunta12.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta12.Text.Trim()))));
            txtRespuestaPregunta13.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta13.Text.Trim()))));
            txtRespuestaPregunta14.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta14.Text.Trim()))));
            txtRespuestaPregunta15.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta15.Text.Trim()))));
            txtRespuestaPregunta16.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta16.Text.Trim()))));
            txtRespuestaPregunta17.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta17.Text.Trim()))));
            txtRespuestaPregunta18.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta18.Text.Trim()))));
            txtComentariosRiesgos.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtComentariosRiesgos.Text.Trim()))));
        }

       
        private List<PropiedadesCalificacion> guardarPropiedadesComunes(List<PropiedadesCalificacion> propiedades)
        {

            PropiedadesCalificacion propiedad0 = new PropiedadesCalificacion();
            propiedad0.nombrePripiedad = txtSustentacionCalRec.ID;
            propiedad0.valorPropiedad = txtSustentacionCalRec.Text.Trim();

            propiedades.Add(propiedad0);

            PropiedadesCalificacion propiedadPr1 = new PropiedadesCalificacion();
            propiedadPr1.nombrePripiedad = txtRespuestaPregunta1.ID;
            propiedadPr1.valorPropiedad = txtRespuestaPregunta1.Text.Trim();

            propiedades.Add(propiedadPr1);

            PropiedadesCalificacion propiedadListPr1 = new PropiedadesCalificacion();
            propiedadListPr1.nombrePripiedad = ddlPregunta1.ID;
            propiedadListPr1.valorPropiedad = ddlPregunta1.SelectedValue.Trim();

            propiedades.Add(propiedadListPr1);

            PropiedadesCalificacion propiedadPr2 = new PropiedadesCalificacion();
            propiedadPr2.nombrePripiedad = txtRespuestaPregunta2.ID;
            propiedadPr2.valorPropiedad = txtRespuestaPregunta2.Text.Trim();

            propiedades.Add(propiedadPr2);

            PropiedadesCalificacion propiedadListPr2 = new PropiedadesCalificacion();
            propiedadListPr2.nombrePripiedad = ddlPregunta2.ID;
            propiedadListPr2.valorPropiedad = ddlPregunta2.SelectedValue.Trim();

            propiedades.Add(propiedadListPr2);

            PropiedadesCalificacion propiedadPr3 = new PropiedadesCalificacion();
            propiedadPr3.nombrePripiedad = txtRespuestaPregunta3.ID;
            propiedadPr3.valorPropiedad = txtRespuestaPregunta3.Text.Trim();

            propiedades.Add(propiedadPr3);

            PropiedadesCalificacion propiedadListPr3 = new PropiedadesCalificacion();
            propiedadListPr3.nombrePripiedad = ddlPregunta3.ID;
            propiedadListPr3.valorPropiedad = ddlPregunta3.SelectedValue.Trim();

            propiedades.Add(propiedadListPr3);

            PropiedadesCalificacion propiedadPr4 = new PropiedadesCalificacion();
            propiedadPr4.nombrePripiedad = txtRespuestaPregunta4.ID;
            propiedadPr4.valorPropiedad = txtRespuestaPregunta4.Text.Trim();

            propiedades.Add(propiedadPr4);

            PropiedadesCalificacion propiedadListPr4 = new PropiedadesCalificacion();
            propiedadListPr4.nombrePripiedad = ddlPregunta4.ID;
            propiedadListPr4.valorPropiedad = ddlPregunta4.SelectedValue.Trim();

            propiedades.Add(propiedadListPr4);

            PropiedadesCalificacion propiedadPr5 = new PropiedadesCalificacion();
            propiedadPr5.nombrePripiedad = txtRespuestaPregunta5.ID;
            propiedadPr5.valorPropiedad = txtRespuestaPregunta5.Text.Trim();

            propiedades.Add(propiedadPr5);

            PropiedadesCalificacion propiedadListPr5 = new PropiedadesCalificacion();
            propiedadListPr5.nombrePripiedad = ddlPregunta5.ID;
            propiedadListPr5.valorPropiedad = ddlPregunta5.SelectedValue.Trim();

            propiedades.Add(propiedadListPr5);

            PropiedadesCalificacion propiedadPr6 = new PropiedadesCalificacion();
            propiedadPr6.nombrePripiedad = txtRespuestaPregunta6.ID;
            propiedadPr6.valorPropiedad = txtRespuestaPregunta6.Text.Trim();

            propiedades.Add(propiedadPr6);

            PropiedadesCalificacion propiedadListPr6 = new PropiedadesCalificacion();
            propiedadListPr6.nombrePripiedad = ddlPregunta6.ID;
            propiedadListPr6.valorPropiedad = ddlPregunta6.SelectedValue.Trim();

            propiedades.Add(propiedadListPr6);

            PropiedadesCalificacion propiedadPr7 = new PropiedadesCalificacion();
            propiedadPr7.nombrePripiedad = txtRespuestaPregunta7.ID;
            propiedadPr7.valorPropiedad = txtRespuestaPregunta7.Text.Trim();

            propiedades.Add(propiedadPr7);

            PropiedadesCalificacion propiedadListPr7 = new PropiedadesCalificacion();
            propiedadListPr7.nombrePripiedad = ddlPregunta7.ID;
            propiedadListPr7.valorPropiedad = ddlPregunta7.SelectedValue.Trim();

            propiedades.Add(propiedadListPr7);

            PropiedadesCalificacion propiedadPr8 = new PropiedadesCalificacion();
            propiedadPr8.nombrePripiedad = txtRespuestaPregunta8.ID;
            propiedadPr8.valorPropiedad = txtRespuestaPregunta8.Text.Trim();

            propiedades.Add(propiedadPr8);

            PropiedadesCalificacion propiedadListPr8 = new PropiedadesCalificacion();
            propiedadListPr8.nombrePripiedad = ddlPregunta8.ID;
            propiedadListPr8.valorPropiedad = ddlPregunta8.SelectedValue.Trim();

            propiedades.Add(propiedadListPr8);

            PropiedadesCalificacion propiedadPr9 = new PropiedadesCalificacion();
            propiedadPr9.nombrePripiedad = txtRespuestaPregunta9.ID;
            propiedadPr9.valorPropiedad = txtRespuestaPregunta9.Text;

            propiedades.Add(propiedadPr9);

            PropiedadesCalificacion propiedadListPr9 = new PropiedadesCalificacion();
            propiedadListPr9.nombrePripiedad = ddlPregunta9.ID;
            propiedadListPr9.valorPropiedad = ddlPregunta9.SelectedValue.Trim();

            propiedades.Add(propiedadListPr9);

            PropiedadesCalificacion propiedadPr10 = new PropiedadesCalificacion();
            propiedadPr10.nombrePripiedad = txtRespuestaPregunta10.ID;
            propiedadPr10.valorPropiedad = txtRespuestaPregunta10.Text;

            propiedades.Add(propiedadPr10);

            PropiedadesCalificacion propiedadListPr10 = new PropiedadesCalificacion();
            propiedadListPr10.nombrePripiedad = ddlPregunta10.ID;
            propiedadListPr10.valorPropiedad = ddlPregunta10.SelectedValue.Trim();

            propiedades.Add(propiedadListPr10);

            PropiedadesCalificacion propiedadPr11 = new PropiedadesCalificacion();
            propiedadPr11.nombrePripiedad = txtRespuestaPregunta11.ID;
            propiedadPr11.valorPropiedad = txtRespuestaPregunta11.Text;

            propiedades.Add(propiedadPr11);

            PropiedadesCalificacion propiedadListPr11 = new PropiedadesCalificacion();
            propiedadListPr11.nombrePripiedad = ddlPregunta11.ID;
            propiedadListPr11.valorPropiedad = ddlPregunta11.SelectedValue.Trim();

            propiedades.Add(propiedadListPr11);

            PropiedadesCalificacion propiedadPr12 = new PropiedadesCalificacion();
            propiedadPr12.nombrePripiedad = txtRespuestaPregunta12.ID;
            propiedadPr12.valorPropiedad = txtRespuestaPregunta12.Text;

            propiedades.Add(propiedadPr12);

            PropiedadesCalificacion propiedadListPr12 = new PropiedadesCalificacion();
            propiedadListPr12.nombrePripiedad = ddlPregunta12.ID;
            propiedadListPr12.valorPropiedad = ddlPregunta12.SelectedValue.Trim();

            propiedades.Add(propiedadListPr12);

            PropiedadesCalificacion propiedadPr13 = new PropiedadesCalificacion();
            propiedadPr13.nombrePripiedad = txtRespuestaPregunta13.ID;
            propiedadPr13.valorPropiedad = txtRespuestaPregunta13.Text;

            propiedades.Add(propiedadPr13);

            PropiedadesCalificacion propiedadListPr13 = new PropiedadesCalificacion();
            propiedadListPr13.nombrePripiedad = ddlPregunta13.ID;
            propiedadListPr13.valorPropiedad = ddlPregunta13.SelectedValue.Trim();

            propiedades.Add(propiedadListPr13);

            PropiedadesCalificacion propiedadPr14 = new PropiedadesCalificacion();
            propiedadPr14.nombrePripiedad = txtRespuestaPregunta14.ID;
            propiedadPr14.valorPropiedad = txtRespuestaPregunta14.Text;

            propiedades.Add(propiedadPr14);

            PropiedadesCalificacion propiedadListPr14 = new PropiedadesCalificacion();
            propiedadListPr14.nombrePripiedad = ddlPregunta14.ID;
            propiedadListPr14.valorPropiedad = ddlPregunta14.SelectedValue.Trim();

            propiedades.Add(propiedadListPr14);

            //PMO30491
            PropiedadesCalificacion propiedadPr15 = new PropiedadesCalificacion();
            propiedadPr15.nombrePripiedad = txtRespuestaPregunta15.ID;
            propiedadPr15.valorPropiedad = txtRespuestaPregunta15.Text;

            propiedades.Add(propiedadPr15);

            PropiedadesCalificacion propiedadPr16 = new PropiedadesCalificacion();
            propiedadPr16.nombrePripiedad = txtRespuestaPregunta16.ID;
            propiedadPr16.valorPropiedad = txtRespuestaPregunta16.Text;

            propiedades.Add(propiedadPr16);

            PropiedadesCalificacion propiedadPr17 = new PropiedadesCalificacion();
            propiedadPr17.nombrePripiedad = txtRespuestaPregunta17.ID;
            propiedadPr17.valorPropiedad = txtRespuestaPregunta17.Text;

            propiedades.Add(propiedadPr17);

            PropiedadesCalificacion propiedadPr18 = new PropiedadesCalificacion();
            propiedadPr18.nombrePripiedad = txtRespuestaPregunta18.ID;
            propiedadPr18.valorPropiedad = txtRespuestaPregunta18.Text;

            propiedades.Add(propiedadPr18);

            return propiedades;
        }

        private void cargarPreguntasPresencial(Parametros[] param)
        {
            //Pregunta 1
            Parametros pregunta1 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 1).ElementAt(0);
            lblPregunta1.Text = "1) " + pregunta1.param7;
            lblComplementoPregunta1.Text = pregunta1.param8;

            //Pregunta 2
            Parametros pregunta2 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 2).ElementAt(0);
            lblPregunta2.Text = "2) " + pregunta2.param7;
            lblComplementoPregunta2.Text = pregunta2.param8;


            //Pregunta 3
            Parametros pregunta3 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 3).ElementAt(0);
            lblPregunta3.Text = "3) " + pregunta3.param7;
            lblComplementoPregunta3.Text = pregunta3.param8;

            //Pregunta 4
            Parametros pregunta4 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 4).ElementAt(0);
            lblPregunta4.Text = "4) " + pregunta4.param7;
            lblComplementoPregunta4.Text = pregunta4.param8;

            //Pregunta 5
            Parametros pregunta5 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 5).ElementAt(0);
            lblPregunta5.Text = "5) " + pregunta5.param7;
            lblComplementoPregunta5.Text = pregunta5.param8;

            //Pregunta 6
            Parametros pregunta6 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 6).ElementAt(0);
            lblPregunta6.Text = "6) " + pregunta6.param7;
            lblComplementoPregunta6.Text = pregunta6.param8;

            //Pregunta 7
            Parametros pregunta7 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 7).ElementAt(0);
            lblPregunta7.Text = "7) " + pregunta7.param7;
            lblComplementoPregunta7.Text = pregunta7.param8;

            //Pregunta 8
            Parametros pregunta8 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 8).ElementAt(0);
            lblPregunta8.Text = "8) " + pregunta8.param7;
            lblComplementoPregunta8.Text = pregunta8.param8;

            //Pregunta 9
            Parametros pregunta9 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 9).ElementAt(0);
            lblPregunta9.Text = "9) " + pregunta9.param7;
            lblComplementoPregunta9.Text = pregunta9.param8;

            //Pregunta 10
            Parametros pregunta10 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 10).ElementAt(0);
            lblPregunta10.Text = "1) " + pregunta10.param7;
            lblComplementoPregunta10.Text = pregunta10.param8;

            //Pregunta 11
            Parametros pregunta11 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 11).ElementAt(0);
            lblPregunta11.Text = "2) " + pregunta11.param7;
            lblComplementoPregunta11.Text = pregunta11.param8;

            //Pregunta 12
            Parametros pregunta12 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 12).ElementAt(0);
            lblPregunta12.Text = "3) " + pregunta12.param7;
            lblComplementoPregunta12.Text = pregunta12.param8;

            //Pregunta 13
            Parametros pregunta13 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 13).ElementAt(0);
            lblPregunta13.Text = "4) " + pregunta13.param7;
            lblComplementoPregunta13.Text = pregunta13.param8;

            //Pregunta 14
            Parametros pregunta14 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 14).ElementAt(0);
            lblPregunta14.Text = "5) " + pregunta14.param7;
            lblComplementoPregunta14.Text = pregunta14.param8;

            //Pregunta 14
            Parametros pregunta15 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 15).ElementAt(0);
            lblPregunta15.Text = "1) " + pregunta15.param7;
            lblComplementoPregunta15.Text = pregunta15.param8;

            //Pregunta 14
            Parametros pregunta16 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 16).ElementAt(0);
            lblPregunta16.Text = "2) " + pregunta16.param7;
            lblComplementoPregunta16.Text = pregunta16.param8;

            //Pregunta 14
            Parametros pregunta17 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 17).ElementAt(0);
            lblPregunta17.Text = "3) " + pregunta17.param7;
            lblComplementoPregunta17.Text = pregunta17.param8;

            //Pregunta 14
            Parametros pregunta18 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 18).ElementAt(0);
            lblPregunta18.Text = "4) " + pregunta18.param7;
            lblComplementoPregunta18.Text = pregunta18.param8;
        }

        protected void btnRelacionamiento_Click(object sender, EventArgs e)
        {
            string nitRelacionamiento = lblNitRelacionamiento.Text.Trim();

            if (!string.IsNullOrEmpty(nitRelacionamiento))
            {
                llenarClienteNitRelacion(nitRelacionamiento);

            }
        }

        protected void llenarClienteNitRelacion(string codigo)
        {

            {
                
                ObjetosCalificacion objCalificacion = new ObjetosCalificacion();

                Parametros[] param = null;
                if (Session["parametros"] != null)
                {
                    param = (Parametros[])Session["parametros"];
                }
                try
                {
                    if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
                    {
                        objCalificacion = Fachada.CalificacionCartera.ConsultarCliente("10", codigo);

                        
                        var data = objCalificacion.objCliente;
                      
                        if (data != null)
                            Fachada.CalificacionCartera.ActualizarLogCSC(data.fecProc, "10",
                                ((Usuario)Session["Rol"]).usuario, "", codigo, "", "", "", "");

                    }
                    else if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC"))
                    {
                        objCalificacion = Fachada.CalificacionCartera.ConsultarCliente("20", codigo);

                       
                        var data = objCalificacion.objCliente;
                        
                        if (data != null)
                            Fachada.CalificacionCartera.ActualizarLogCSC(data.fecProc, "13",
                                ((Usuario)Session["Rol"]).usuario, "", codigo, "", "", "", "");
                    }


                    if (objCalificacion.setDatos != null && objCalificacion.objCliente != null)
                    {
                        if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
                        {
                            ddlCalNRRecom.DataSource = objCalificacion.setDatos.Tables[2];
                            ddlCalNRRecom.DataTextField = "APCVALCHA7";
                            ddlCalNRRecom.DataValueField = "APCVALCHA7";
                            ddlCalNRRecom.DataBind();
                            
                            inicializarLista(ddlCalNRRecom, "0", false);

                            ddlCalNRRecom.SelectedValue = objCalificacion.objCliente.calNuevoRatRecom.Trim();

                            List<PropiedadesCalificacion> propiedades = new List<PropiedadesCalificacion>();
                          
                            txtSustentacionCalRec.Text = objCalificacion.objCliente.SustentacionCalRecCom.Trim();
                            txtRespuestaPregunta1.Text = objCalificacion.objCliente.RespPregunta1Com.Trim();
                            txtRespuestaPregunta2.Text = objCalificacion.objCliente.RespPregunta2Com.Trim();
                            txtRespuestaPregunta3.Text = objCalificacion.objCliente.RespPregunta3Com.Trim();
                            txtRespuestaPregunta4.Text = objCalificacion.objCliente.RespPregunta4Com.Trim();
                            txtRespuestaPregunta5.Text = objCalificacion.objCliente.RespPregunta5Com.Trim();
                            txtRespuestaPregunta6.Text = objCalificacion.objCliente.RespPregunta6Com.Trim();
                            txtRespuestaPregunta7.Text = objCalificacion.objCliente.RespPregunta7Com.Trim();
                            txtRespuestaPregunta8.Text = objCalificacion.objCliente.RespPregunta8Com.Trim();
                            txtRespuestaPregunta9.Text = objCalificacion.objCliente.RespPregunta9Com.Trim();
                            txtRespuestaPregunta10.Text = objCalificacion.objCliente.RespPregunta10Com.Trim();
                            txtRespuestaPregunta11.Text = objCalificacion.objCliente.RespPregunta11Com.Trim();
                            txtRespuestaPregunta12.Text = objCalificacion.objCliente.RespPregunta12Com.Trim();
                            txtRespuestaPregunta13.Text = objCalificacion.objCliente.RespPregunta13Com.Trim();
                            txtRespuestaPregunta14.Text = objCalificacion.objCliente.RespPregunta14Com.Trim();
                            txtRespuestaPregunta15.Text = objCalificacion.objCliente.RespPregunta15Com.Trim();
                            txtRespuestaPregunta16.Text = objCalificacion.objCliente.RespPregunta16Com.Trim();
                            txtRespuestaPregunta17.Text = objCalificacion.objCliente.RespPregunta17Com.Trim();
                            txtRespuestaPregunta18.Text = objCalificacion.objCliente.RespPregunta18Com.Trim();

                 

                            InicializarListaPreguntas(objCalificacion);

         
                            ddlPregunta1.SelectedValue = objCalificacion.objCliente.RespListaPregunta1Com.Trim();
                            ddlPregunta2.SelectedValue = objCalificacion.objCliente.RespListaPregunta2Com.Trim();
                            ddlPregunta3.SelectedValue = objCalificacion.objCliente.RespListaPregunta3Com.Trim();
                            ddlPregunta4.SelectedValue = objCalificacion.objCliente.RespListaPregunta4Com.Trim();
                            ddlPregunta5.SelectedValue = objCalificacion.objCliente.RespListaPregunta5Com.Trim();
                            ddlPregunta6.SelectedValue = objCalificacion.objCliente.RespListaPregunta6Com.Trim();
                            ddlPregunta7.SelectedValue = objCalificacion.objCliente.RespListaPregunta7Com.Trim();
                            ddlPregunta8.SelectedValue = objCalificacion.objCliente.RespListaPregunta8Com.Trim();
                            ddlPregunta9.SelectedValue = objCalificacion.objCliente.RespListaPregunta9Com.Trim();
                            ddlPregunta10.SelectedValue = objCalificacion.objCliente.RespListaPregunta10Com.Trim();
                            ddlPregunta11.SelectedValue = objCalificacion.objCliente.RespListaPregunta11Com.Trim();
                            ddlPregunta12.SelectedValue = objCalificacion.objCliente.RespListaPregunta12Com.Trim();
                            ddlPregunta13.SelectedValue = objCalificacion.objCliente.RespListaPregunta13Com.Trim();
                            ddlPregunta14.SelectedValue = objCalificacion.objCliente.RespListaPregunta14Com.Trim();

                            propiedades = guardarPropiedadesComunes(propiedades);




                            PropiedadesCalificacion propiedad6 = new PropiedadesCalificacion();
                            propiedad6.nombrePripiedad = ddlCalNRRecom.ID;
                            propiedad6.valorPropiedad = ddlCalNRRecom.SelectedValue;

                            propiedades.Add(propiedad6);

                            ViewState["Cambios"] = propiedades;

                        }
                        else if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC"))
                        {
                            IEnumerable<ListItem> CalifInterna = param.Where(p => p.paramName.Trim().Equals("LCALNR") && p.paramSeq != 0 && p.param6.Trim() != "0").OrderBy(p => p.paramSeq).Select(x =>
                                       new ListItem()
                                       {
                                           Text = x.param7.ToString().Trim()
                                       }).Distinct();

                            ddlCalifInternaRNR.DataSource = CalifInterna;
                            ddlCalifInternaRNR.DataBind();
                            inicializarLista(ddlCalifInternaRNR, objCalificacion.objCliente.calIntRatNRating, false);

                            ddlCalifExternaP.DataSource = objCalificacion.setDatos.Tables[2];
                            ddlCalifExternaP.DataTextField = "APCVALCHA7";
                            ddlCalifExternaP.DataValueField = "APCVALCHA7";
                            ddlCalifExternaP.DataBind();
                            inicializarLista(ddlCalifExternaP, objCalificacion.objCliente.calExtRat, false);

                            lblCovenantP.Text = objCalificacion.objCliente.segCovenants.Trim();

                            ddlSeguimiento.DataSource = objCalificacion.setDatos.Tables[3];
                            ddlSeguimiento.DataTextField = "APCVALCHA7";
                            ddlSeguimiento.DataValueField = "APCVALCHA7";
                            ddlSeguimiento.DataBind();
                            inicializarLista(ddlSeguimiento, objCalificacion.objCliente.segProxComite, false);

                            ddlRecomendacion.DataSource = objCalificacion.setDatos.Tables[3];
                            ddlRecomendacion.DataTextField = "APCVALCHA7";
                            ddlRecomendacion.DataValueField = "APCVALCHA7";
                            ddlRecomendacion.DataBind();
                            inicializarLista(ddlRecomendacion, objCalificacion.objCliente.recAEC, false);

                            lblCalifRecomenGrupo.Text = objCalificacion.objCliente.CalifRecomenGrupo.Trim();

                            ddlRazon.DataSource = objCalificacion.setDatos.Tables[9];
                            ddlRazon.DataTextField = "APCVALCHA7";
                            ddlRazon.DataValueField = "APCVALCHA7";
                            ddlRazon.DataBind();
                            inicializarLista(ddlRazon, objCalificacion.objCliente.razCaliInt, true);

                            lblCalifRecPEC.Text = objCalificacion.objCliente.calSugeridaPEC.Trim();
                            lblListasdeControl.Text = objCalificacion.objCliente.ListasDeControl.Trim();

                            ddlTipoCliente.DataSource = objCalificacion.setDatos.Tables[11];
                            ddlTipoCliente.DataTextField = "APCVALCHA7";
                            ddlTipoCliente.DataValueField = "APCVALCHA7";
                            ddlTipoCliente.DataBind();
                            inicializarLista(ddlTipoCliente, objCalificacion.objCliente.tipoCliente, false);

                            lblCalNuevoRRecom.Text = objCalificacion.objCliente.calNuevoRatRecom.Trim();

                            ddlEstadoCal.DataSource = objCalificacion.setDatos.Tables[10];
                            ddlEstadoCal.DataTextField = "APCVALCHA7";
                            ddlEstadoCal.DataValueField = "APCVALCHA7";
                            ddlEstadoCal.DataBind();
                            inicializarLista(ddlEstadoCal, objCalificacion.objCliente.estadoCalificacion, false);

                            ddlUtilizoEEFF.DataSource = objCalificacion.setDatos.Tables[3];
                            ddlUtilizoEEFF.DataTextField = "APCVALCHA7";
                            ddlUtilizoEEFF.DataValueField = "APCVALCHA7";
                            ddlUtilizoEEFF.DataBind();
                            inicializarLista(ddlUtilizoEEFF, objCalificacion.objCliente.utilizoEEFF, false);

                            InicializarListaPreguntas(objCalificacion);

                            if (objCalificacion.objCliente.calIntRatNRating.Trim().Length > 0)
                            {
                                txtSustentacionCalRec.Text = objCalificacion.objCliente.SustentacionCalRecPIC;
                                txtRespuestaPregunta1.Text = objCalificacion.objCliente.RespPregunta1PIC;
                                txtRespuestaPregunta2.Text = objCalificacion.objCliente.RespPregunta2PIC;
                                txtRespuestaPregunta3.Text = objCalificacion.objCliente.RespPregunta3PIC;
                                txtRespuestaPregunta4.Text = objCalificacion.objCliente.RespPregunta4PIC;
                                txtRespuestaPregunta5.Text = objCalificacion.objCliente.RespPregunta5PIC;
                                txtRespuestaPregunta6.Text = objCalificacion.objCliente.RespPregunta6PIC;
                                txtRespuestaPregunta7.Text = objCalificacion.objCliente.RespPregunta7PIC;
                                txtRespuestaPregunta8.Text = objCalificacion.objCliente.RespPregunta8PIC;
                                txtRespuestaPregunta9.Text = objCalificacion.objCliente.RespPregunta9PIC;
                                txtRespuestaPregunta10.Text = objCalificacion.objCliente.RespPregunta10PIC;
                                txtRespuestaPregunta11.Text = objCalificacion.objCliente.RespPregunta11PIC;
                                txtRespuestaPregunta12.Text = objCalificacion.objCliente.RespPregunta12PIC;
                                txtRespuestaPregunta13.Text = objCalificacion.objCliente.RespPregunta13PIC;
                                txtRespuestaPregunta14.Text = objCalificacion.objCliente.RespPregunta14PIC;
                                txtRespuestaPregunta15.Text = objCalificacion.objCliente.RespPregunta15PIC;
                                txtRespuestaPregunta16.Text = objCalificacion.objCliente.RespPregunta16PIC;
                                txtRespuestaPregunta17.Text = objCalificacion.objCliente.RespPregunta17PIC;
                                txtRespuestaPregunta18.Text = objCalificacion.objCliente.RespPregunta18PIC;


                                //asignar valores seleccionados a las listas
                                ddlPregunta1.SelectedValue = (objCalificacion.objCliente.RespListaPregunta1PIC.Trim().Equals(string.Empty) ? "0" : objCalificacion.objCliente.RespListaPregunta1PIC.Trim());
                                ddlPregunta2.SelectedValue = (objCalificacion.objCliente.RespListaPregunta2PIC.Trim().Equals(string.Empty) ? "0" : objCalificacion.objCliente.RespListaPregunta2PIC.Trim());
                                ddlPregunta3.SelectedValue = (objCalificacion.objCliente.RespListaPregunta3PIC.Trim().Equals(string.Empty) ? "0" : objCalificacion.objCliente.RespListaPregunta3PIC.Trim());
                                ddlPregunta4.SelectedValue = (objCalificacion.objCliente.RespListaPregunta4PIC.Trim().Equals(string.Empty) ? "0" : objCalificacion.objCliente.RespListaPregunta4PIC.Trim()); 
                                ddlPregunta5.SelectedValue = (objCalificacion.objCliente.RespListaPregunta5PIC.Trim().Equals(string.Empty) ? "0" : objCalificacion.objCliente.RespListaPregunta5PIC.Trim());
                                ddlPregunta6.SelectedValue = (objCalificacion.objCliente.RespListaPregunta6PIC.Trim().Equals(string.Empty) ? "0" : objCalificacion.objCliente.RespListaPregunta6PIC.Trim());
                                ddlPregunta7.SelectedValue = (objCalificacion.objCliente.RespListaPregunta7PIC.Trim().Equals(string.Empty) ? "0" : objCalificacion.objCliente.RespListaPregunta7PIC.Trim());
                                ddlPregunta8.SelectedValue = (objCalificacion.objCliente.RespListaPregunta8PIC.Trim().Equals(string.Empty) ? "0" : objCalificacion.objCliente.RespListaPregunta8PIC.Trim());
                                ddlPregunta9.SelectedValue = (objCalificacion.objCliente.RespListaPregunta9PIC.Trim().Equals(string.Empty) ? "0" : objCalificacion.objCliente.RespListaPregunta9PIC.Trim());
                                ddlPregunta10.SelectedValue = (objCalificacion.objCliente.RespListaPregunta10PIC.Trim().Equals(string.Empty) ? "0" : objCalificacion.objCliente.RespListaPregunta10PIC.Trim());
                                ddlPregunta11.SelectedValue = (objCalificacion.objCliente.RespListaPregunta11PIC.Trim().Equals(string.Empty) ? "0" : objCalificacion.objCliente.RespListaPregunta11PIC.Trim());
                                ddlPregunta12.SelectedValue = (objCalificacion.objCliente.RespListaPregunta12PIC.Trim().Equals(string.Empty) ? "0" : objCalificacion.objCliente.RespListaPregunta12PIC.Trim()); 
                                ddlPregunta13.SelectedValue = (objCalificacion.objCliente.RespListaPregunta13PIC.Trim().Equals(string.Empty) ? "0" : objCalificacion.objCliente.RespListaPregunta13PIC.Trim());
                                ddlPregunta14.SelectedValue = (objCalificacion.objCliente.RespListaPregunta14PIC.Trim().Equals(string.Empty) ? "0" : objCalificacion.objCliente.RespListaPregunta14PIC.Trim()); 

                               
                                txtComentariosRiesgos.Text = objCalificacion.objCliente.comentarioRiesgos.Trim();
                                ddlCalifExternaP.SelectedValue = objCalificacion.objCliente.calExtRat.Trim();
                                ddlEstadoCal.SelectedValue = objCalificacion.objCliente.estadoCalificacion.Trim();

                               
                                ddlUtilizoEEFF.SelectedValue = objCalificacion.objCliente.utilizoEEFF.Trim();
                                ddlTipoCliente.SelectedValue = objCalificacion.objCliente.tipoCliente.Trim();

                                ddlSeguimiento.SelectedValue = objCalificacion.objCliente.segProxComite.Trim();
                                ddlRecomendacion.SelectedValue = objCalificacion.objCliente.recAEC.Trim();
                                ddlCalifInternaRNR.SelectedValue = objCalificacion.objCliente.calIntRatNRating.Trim();

                            }
                          
                            else if (objCalificacion.objCliente.calNuevoRatRecom.Trim().Length > 0)
                            {
                                txtSustentacionCalRec.Text = objCalificacion.objCliente.SustentacionCalRecCom;
                                txtRespuestaPregunta1.Text = objCalificacion.objCliente.RespPregunta1Com;
                                txtRespuestaPregunta2.Text = objCalificacion.objCliente.RespPregunta2Com;
                                txtRespuestaPregunta3.Text = objCalificacion.objCliente.RespPregunta3Com;
                                txtRespuestaPregunta4.Text = objCalificacion.objCliente.RespPregunta4Com;
                                txtRespuestaPregunta5.Text = objCalificacion.objCliente.RespPregunta5Com;
                                txtRespuestaPregunta6.Text = objCalificacion.objCliente.RespPregunta6Com;
                                txtRespuestaPregunta7.Text = objCalificacion.objCliente.RespPregunta7Com;
                                txtRespuestaPregunta8.Text = objCalificacion.objCliente.RespPregunta8Com;
                                txtRespuestaPregunta9.Text = objCalificacion.objCliente.RespPregunta9Com;
                                txtRespuestaPregunta10.Text = objCalificacion.objCliente.RespPregunta10Com;
                                txtRespuestaPregunta11.Text = objCalificacion.objCliente.RespPregunta11Com;
                                txtRespuestaPregunta12.Text = objCalificacion.objCliente.RespPregunta12Com;
                                txtRespuestaPregunta13.Text = objCalificacion.objCliente.RespPregunta13Com;
                                txtRespuestaPregunta14.Text = objCalificacion.objCliente.RespPregunta14Com;
                                txtRespuestaPregunta15.Text = objCalificacion.objCliente.RespPregunta15Com;
                                txtRespuestaPregunta16.Text = objCalificacion.objCliente.RespPregunta16Com;
                                txtRespuestaPregunta17.Text = objCalificacion.objCliente.RespPregunta17Com;
                                txtRespuestaPregunta18.Text = objCalificacion.objCliente.RespPregunta18Com;

                                txtComentariosRiesgos.Text = objCalificacion.objCliente.comentarioRiesgos.Trim();

                                ddlPregunta1.SelectedValue = objCalificacion.objCliente.RespListaPregunta1Com.Trim();
                                ddlPregunta2.SelectedValue = objCalificacion.objCliente.RespListaPregunta2Com.Trim();
                                ddlPregunta3.SelectedValue = objCalificacion.objCliente.RespListaPregunta3Com.Trim();
                                ddlPregunta4.SelectedValue = objCalificacion.objCliente.RespListaPregunta4Com.Trim();
                                ddlPregunta5.SelectedValue = objCalificacion.objCliente.RespListaPregunta5Com.Trim();
                                ddlPregunta6.SelectedValue = objCalificacion.objCliente.RespListaPregunta6Com.Trim();
                                ddlPregunta7.SelectedValue = objCalificacion.objCliente.RespListaPregunta7Com.Trim();
                                ddlPregunta8.SelectedValue = objCalificacion.objCliente.RespListaPregunta8Com.Trim();
                                ddlPregunta9.SelectedValue = objCalificacion.objCliente.RespListaPregunta9Com.Trim();
                                ddlPregunta10.SelectedValue = objCalificacion.objCliente.RespListaPregunta10Com.Trim();
                                ddlPregunta11.SelectedValue = objCalificacion.objCliente.RespListaPregunta11Com.Trim();
                                ddlPregunta12.SelectedValue = objCalificacion.objCliente.RespListaPregunta12Com.Trim();
                                ddlPregunta13.SelectedValue = objCalificacion.objCliente.RespListaPregunta13Com.Trim();
                                ddlPregunta14.SelectedValue = objCalificacion.objCliente.RespListaPregunta14Com.Trim();

                            }

                            btnGuardar.Visible = true;

                            List<PropiedadesCalificacion> propiedades = new List<PropiedadesCalificacion>();
                        
                            propiedades = guardarPropiedadesComunes(propiedades);

                            PropiedadesCalificacion propiedadComPic = new PropiedadesCalificacion();
                            propiedadComPic.nombrePripiedad = txtComentariosRiesgos.ID;
                            propiedadComPic.valorPropiedad = txtComentariosRiesgos.Text.Trim();

                            propiedades.Add(propiedadComPic);

                     

                            PropiedadesCalificacion propiedad5 = new PropiedadesCalificacion();
                            propiedad5.nombrePripiedad = ddlCalifExternaP.ID;
                            propiedad5.valorPropiedad = ddlCalifExternaP.SelectedValue;

                            propiedades.Add(propiedad5);

                            PropiedadesCalificacion propiedad6 = new PropiedadesCalificacion();
                            propiedad6.nombrePripiedad = ddlSeguimiento.ID;
                            propiedad6.valorPropiedad = ddlSeguimiento.SelectedValue;

                            propiedades.Add(propiedad6);

                            PropiedadesCalificacion propiedad7 = new PropiedadesCalificacion();
                            propiedad7.nombrePripiedad = ddlRecomendacion.ID;
                            propiedad7.valorPropiedad = ddlRecomendacion.SelectedValue;

                            propiedades.Add(propiedad7);

                            PropiedadesCalificacion propiedad8 = new PropiedadesCalificacion();
                            propiedad8.nombrePripiedad = ddlRazon.ID;
                            propiedad8.valorPropiedad = ddlRazon.SelectedValue;

                            propiedades.Add(propiedad8);

                            PropiedadesCalificacion propiedad9 = new PropiedadesCalificacion();
                            propiedad9.nombrePripiedad = ddlCalifInternaRNR.ID;
                            propiedad9.valorPropiedad = ddlCalifInternaRNR.SelectedValue;

                            propiedades.Add(propiedad9);

                            PropiedadesCalificacion propiedad10 = new PropiedadesCalificacion();
                            propiedad10.nombrePripiedad = ddlCalifInternaRNR.ID;
                            propiedad10.valorPropiedad = ddlCalifInternaRNR.SelectedValue;

                            propiedades.Add(propiedad10);

                            PropiedadesCalificacion propiedad12 = new PropiedadesCalificacion();
                            propiedad12.nombrePripiedad = ddlEstadoCal.ID;
                            propiedad12.valorPropiedad = ddlEstadoCal.SelectedValue;

                            propiedades.Add(propiedad12);

                            ViewState["Cambios"] = propiedades;

                        }
                    
                    }

                }
                catch (Exception ex)
                {
                    ErroresEntity errEnt = new ErroresEntity();
                    errEnt.Url = Request.UrlReferrer.PathAndQuery;
                    errEnt.Log = ex.Message + "//ApplicationPage:CalificacionCliente";
                    errEnt.Error = "MEN.0409";
                    Session["Error"] = errEnt;
                    Response.Redirect("Errores/Error.aspx");
                }
            }
        }
    }

}

