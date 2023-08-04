using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bancolombia.Riesgo.MAFWeb.Entidades.CalificacionCartera;
using System.Data;
//using Proxy;
using Fachada = Bancolombia.Riesgo.MAF.Negocio.Fachadas.CalificacionCartera;
using System.Globalization;
using Bancolombia.Riesgo.MAFWeb.Clases.CalificacionCartera;
using Bancolombia.Riesgo.MAF.Entidades.CalificacionCartera;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Bancolombia.Riesgo.MAFWeb.CalificacionCartera
{
    public partial class CalificacionClienteGob : System.Web.UI.Page
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
                else if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC"))
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
            //Proxy.CalificacionCartera proxyCartera = new Proxy.CalificacionCartera();

            if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
            {
                if (validarPreguntas())
                {
                    establecerEncodingCamposTexto();
                    objCliente.calIntRec = " ";
                    objCliente.calNuevoRatRecom = (ddlCalNRRecom.SelectedValue.Trim().Equals("0") ? " " : ddlCalNRRecom.SelectedValue.Trim());

                    objCliente.SustentacionCalRecCom = (txtSustentacionCalRec.Text.Trim().Equals(string.Empty) ? " " : txtSustentacionCalRec.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta1Com = string.Empty;
                    objCliente.RespPregunta2Com = string.Empty;
                    objCliente.RespPregunta3Com = string.Empty;
                    objCliente.RespPregunta4Com = string.Empty;
                    objCliente.RespPregunta5Com = string.Empty;
                    objCliente.RespPregunta6Com = string.Empty;
                    objCliente.RespPregunta7Com = string.Empty;
                    objCliente.RespPregunta8Com = string.Empty;
                    objCliente.RespPregunta9Com = string.Empty;
                    objCliente.RespPregunta10Com = string.Empty;
                    objCliente.RespPregunta11Com = string.Empty;
                    objCliente.RespPregunta12Com = string.Empty;
                    objCliente.RespPregunta13Com = string.Empty;
                    objCliente.RespPregunta14Com = string.Empty;
                    objCliente.RespPregunta15Com = string.Empty;
                    objCliente.RespPregunta16Com = string.Empty;
                    objCliente.RespPregunta17Com = string.Empty;
                    objCliente.RespPregunta18Com = string.Empty;
                    objCliente.RespPregunta19Com = (txtRespuestaPregunta19.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta19.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta20Com = (txtRespuestaPregunta20.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta20.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta21Com = (txtRespuestaPregunta21.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta21.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta22Com = (txtRespuestaPregunta22.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta22.Text.Trim().Replace("|", " "));

                    //Fin PMO27494
                    objCliente.causalDisolucion = (lblCausalDisulocionResp.Text.Trim().Equals(string.Empty) ? " " : lblCausalDisulocionResp.Text);

                    // PMO18879
                    objCliente.comentarioRiesgos = (txtComentariosRiesgos.Text.Trim().Equals(string.Empty) ? " " : txtComentariosRiesgos.Text.Trim());
                    //PMO27494
                    respuesta = Fachada.CalificacionCartera.ActualizarCliente(objCliente, ((Usuario)Session["Rol"]).usuario, "11");
                    if (!respuesta.Contains("0000"))
                        throw new Exception("ERRORSP@" + respuesta);

                    List<PropiedadesCalificacion> propiedades = new List<PropiedadesCalificacion>();

                    //Ini PMO27494
                    //Guardar en propiedades campos de preguntas y respuestas
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
                    objCliente.RespPregunta1PIC = string.Empty;
                    objCliente.RespPregunta2PIC = string.Empty;
                    objCliente.RespPregunta3PIC = string.Empty;
                    objCliente.RespPregunta4PIC = string.Empty;
                    objCliente.RespPregunta5PIC = string.Empty;
                    objCliente.RespPregunta6PIC = string.Empty;
                    objCliente.RespPregunta7PIC = string.Empty;
                    objCliente.RespPregunta8PIC = string.Empty;
                    objCliente.RespPregunta9PIC = string.Empty;
                    objCliente.RespPregunta10PIC = string.Empty;
                    objCliente.RespPregunta11PIC = string.Empty;
                    objCliente.RespPregunta12PIC = string.Empty;
                    objCliente.RespPregunta13PIC = string.Empty;
                    objCliente.RespPregunta14PIC = string.Empty;
                    objCliente.RespPregunta15PIC = string.Empty;
                    objCliente.RespPregunta16PIC = string.Empty;
                    objCliente.RespPregunta17PIC = string.Empty;
                    objCliente.RespPregunta18PIC = string.Empty;
                    objCliente.RespPregunta19PIC = (txtRespuestaPregunta19.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta19.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta20PIC = (txtRespuestaPregunta20.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta20.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta21PIC = (txtRespuestaPregunta21.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta21.Text.Trim().Replace("|", " "));
                    objCliente.RespPregunta22PIC = (txtRespuestaPregunta22.Text.Trim().Equals(string.Empty) ? " " : txtRespuestaPregunta22.Text.Trim().Replace("|", " "));

                    objCliente.comentarioRiesgos = (txtComentariosRiesgos.Text.Trim()).Equals(string.Empty) ? " " : txtComentariosRiesgos.Text.Trim().Replace("|", " ");

                    //Valores seleccionados en las listas de respuestas.

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

        protected bool validarPreguntas()
        {
            string txtPregunta19 = "";
            string txtPregunta20 = "";
            string txtPregunta21 = "";
            string txtPregunta22 = "";

            int contadorTxt = 0;

            if (txtRespuestaPregunta19.Text.Trim().Equals(string.Empty))
            {
                txtPregunta19 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0476").Texto;
                contadorTxt = contadorTxt + 1;
            }

            if (txtRespuestaPregunta20.Text.Trim().Equals(string.Empty))
            {
                txtPregunta20 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0477").Texto;
                contadorTxt = contadorTxt + 1;
            }

            if (txtRespuestaPregunta21.Text.Trim().Equals(string.Empty))
            {
                txtPregunta21 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0478").Texto;
                contadorTxt = contadorTxt + 1;
            }

            if (txtRespuestaPregunta22.Text.Trim().Equals(string.Empty))
            {
                txtPregunta22 = Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0479").Texto;
                contadorTxt = contadorTxt + 1;
            }


            if (contadorTxt > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "GuardarComercial", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0495").Texto + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0493").Texto + txtPregunta19 + txtPregunta20 + txtPregunta21 + txtPregunta22 + "')", true);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Guarda la información ingresada del cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Retorna a la búsqueda de clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarNuevo_Click(object sender, EventArgs e)
        {
            bool cambian = false;
            List<PropiedadesCalificacion> propiedades = new List<PropiedadesCalificacion>();
            propiedades = (List<PropiedadesCalificacion>)ViewState["Cambios"];
            //Si usuario no pertenece a los roles Comercial o Riesgos-PIC redirecciona para realizar nueva busqueda.
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
                    //Ini PMO27494 
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
                }//verificar
            }

            //Ini PMO27494: Validar cambios en propiedades comunes
            if (propiedades.Find(p => p.nombrePripiedad == txtSustentacionCalRec.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtSustentacionCalRec.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }

            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta19.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta19.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }

            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta20.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta20.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }

            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta21.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta21.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }

            if (propiedades.Find(p => p.nombrePripiedad == txtRespuestaPregunta22.ID).valorPropiedad.Trim().Replace(Environment.NewLine, "\n") != txtRespuestaPregunta22.Text.Trim().Replace(Environment.NewLine, "\n"))
            {
                cambian = true;
            }

            //Fin PMO27494

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

        /// <summary>
        /// muestra el popup con la informacion de comité anterior
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnComiteAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                //Proxy.CalificacionCartera proxyCartera = new Proxy.CalificacionCartera();
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
                        //pintarComite Nuevo comite apartir del periodo 201812 PMO27494
                        pintarNuevoComite(objCalificacion.objComiteAnterior);
                        // metodo para abrir el popup de comite anterior PMO27494
                        mpeNuevoComiteAnterior.Show();
                    }
                    else
                    {
                        //pintarComite
                        pintarComite(objCalificacion.objComiteAnterior);
                        // metodo para abrir el popup de comite anterior
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

        /// <summary>
        /// muestra el popup con la información PEC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPEC_Click(object sender, EventArgs e)
        {
            try
            {
                //Proxy.CalificacionCartera proxyCartera = new Proxy.CalificacionCartera();
                Cliente objCliente = (Cliente)ViewState["Cliente"];
                ObjetosCalificacion objCalificacion = Fachada.CalificacionCartera.CentralExterna(objCliente.nit.Trim());
                if (objCalificacion.objCentralExterna != null)
                {
                    //pintarPEC
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

        /// <summary>
        /// muestra el popup con la informacion covenants
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void btnCovenants_Click(object sender, EventArgs e)
        {
            try
            {
                //Proxy.CalificacionCartera proxyCartera = new Proxy.CalificacionCartera();
                Cliente objCliente = (Cliente)ViewState["Cliente"];
                ObjetosCalificacion objCalificacion = Fachada.CalificacionCartera.Covenants(objCliente.nit.Trim());
                if (objCalificacion.objCovenants != null)
                {
                    //pintarPEC
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

        /// <summary>
        /// muestra el popup con la informacion prorrogas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProrrogas_Click(object sender, EventArgs e)
        {
            try
            {
                //Proxy.CalificacionCartera proxyCartera = new Proxy.CalificacionCartera();
                Cliente objCliente = (Cliente)ViewState["Cliente"];
                ObjetosCalificacion objCalificacion = Fachada.CalificacionCartera.Prorrogas(objCliente.nit.Trim());
                if (objCalificacion.objProrrogas != null)
                {
                    //pintarProrrogas
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

        /// <summary>
        /// reenvia la información de cliente para visualizarse por pdf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                string categoriaCliente = "GB";

                //Response.Redirect("Reporte/PDF.aspx?Cliente=" + lblCliente.Text.Trim());
                Response.Redirect("Reporte/PDF.aspx?Cliente=" + lblCliente.Text.Trim() + "&categoriaCliente=" + categoriaCliente);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0444").Texto + "')", true);
            }
        }

        #endregion


        #region "Metodos"

        /// <summary>
        /// Carga la información del cliente
        /// </summary>
        /// <param name="codigo"></param>
        protected void llenarCliente(string codigo)
        {
            //consultarInformaciónCliente

            //Proxy.CalificacionCartera proxyCartera = new Proxy.CalificacionCartera();
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

                    //Obtiene la información necesaria para luego usarla en la actualización del log.
                    var data = objCalificacion.objCliente;
                    //Actualiza la tabla de logs insertando una nueva actividad de consulta.
                    if (data != null)
                        Fachada.CalificacionCartera.ActualizarLogCSC(data.fecProc, "13",
                            ((Usuario)Session["Rol"]).usuario, "", codigo, "", "", "", "");
                }
                else
                {
                    objCalificacion = Fachada.CalificacionCartera.ConsultarCliente("30", codigo);

                    if (((Usuario)Session["Rol"]).rol.Equals("Superfinanciera"))
                    {
                        //Obtiene la información necesaria para luego usarla en la actualización del log.
                        var data = objCalificacion.objCliente;
                        //Actualiza la tabla de logs insertando una nueva actividad de consulta.
                        if (data != null)
                            Fachada.CalificacionCartera.ActualizarLogCSC(data.fecProc, "16",
                                ((Usuario)Session["Rol"]).usuario, "", codigo, "", "", "", "");
                    }
                    else if (((Usuario)Session["Rol"]).rol.Equals("Consulta"))
                    {
                        //Obtiene la información necesaria para luego usarla en la actualización del log.
                        var data = objCalificacion.objCliente;
                        //Actualiza la tabla de logs insertando una nueva actividad de consulta.
                        if (data != null)
                            Fachada.CalificacionCartera.ActualizarLogCSC(data.fecProc, "19",
                                ((Usuario)Session["Rol"]).usuario, "", codigo, "", "", "", "");
                    }
                }

                if (objCalificacion.setDatos != null && objCalificacion.objCliente != null)
                {
                    //PMO27494 - preguntas paramétricas revisar
                    cargarPreguntasPresencial(param);

                    ViewState["Cliente"] = objCalificacion.objCliente;
                    lblTipoComite.Text = objCalificacion.objCliente.tipCom.Trim();

                    //habilitar controles de cliente
                    pnlCliente.Visible = true;

                    lblCliente.Text = codigo;
                    lblNombreCliente.Text = objCalificacion.objCliente.nombre.Trim();
                    lblFechaComite.Text = objCalificacion.objCliente.fecProc.Trim();

                    //Nit relacionamiento
                    if (!string.IsNullOrEmpty(objCalificacion.objCliente.nitRelacion) && (!objCalificacion.objCliente.nit.Equals(objCalificacion.objCliente.nitRelacion)))
                    {
                        lblNitRelacionamiento.Text = objCalificacion.objCliente.nitRelacion;
                        btnRelacionamiento.Visible = true;
                    }
                    else
                    {
                        lblNitRelacionamiento.Text = "No tiene";
                    }

                    //establece cuál será el separador de decimales
                    var nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ",";
                    nfi.NumberGroupSeparator = ".";
                    nfi.CurrencyDecimalSeparator = ",";
                    nfi.CurrencyGroupSeparator = ".";

                    //Si es super financiera, oculta elmensaje de causal de disolucion
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
                                    //Valida la existencia de parámetros
                                    if (param.Length == 0)
                                        throw new Exception(Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0422").Texto);

                                    //verificar existencia de ruta y archivo definidos
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
                        Parametros campoParametrico = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("OPARMADI")
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

                        if (((Usuario)Session["Rol"]).rol.Equals("Comercial") || ((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC") || ((Usuario)Session["Rol"]).rol.Equals("Consulta"))
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

                        //deuda ebidta
                        //validar deuda:
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
                                //Si la cifra decimal es en longitud a 1 se 
                                //asigna un cero de más para completar el formato de 
                                //dos cifras decimales (solo se da cuando la variable 'end' en 
                                //la parte decimal de la cifra general 
                                //es igual a: xx,1 ; xx,2; xx,3; ..... ; xx,9).
                                //x es cualquier numero.
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
                        txtRespuestaPregunta19.Text = objCalificacion.objCliente.RespPregunta19Com.Trim();
                        txtRespuestaPregunta20.Text = objCalificacion.objCliente.RespPregunta20Com.Trim();
                        txtRespuestaPregunta21.Text = objCalificacion.objCliente.RespPregunta21Com.Trim();
                        txtRespuestaPregunta22.Text = objCalificacion.objCliente.RespPregunta22Com.Trim();

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


                        //poner comentarios riesgos para edición
                        if (objCalificacion.objCliente.calIntRatNRating.Trim().Length > 0)
                        {
                            txtSustentacionCalRec.Text = objCalificacion.objCliente.SustentacionCalRecPIC;
                            txtRespuestaPregunta19.Text = objCalificacion.objCliente.RespPregunta19PIC;
                            txtRespuestaPregunta20.Text = objCalificacion.objCliente.RespPregunta20PIC;
                            txtRespuestaPregunta21.Text = objCalificacion.objCliente.RespPregunta21PIC;
                            txtRespuestaPregunta22.Text = objCalificacion.objCliente.RespPregunta22PIC;


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
                            txtRespuestaPregunta19.Text = objCalificacion.objCliente.RespPregunta19Com;
                            txtRespuestaPregunta20.Text = objCalificacion.objCliente.RespPregunta20Com;
                            txtRespuestaPregunta21.Text = objCalificacion.objCliente.RespPregunta21Com;
                            txtRespuestaPregunta22.Text = objCalificacion.objCliente.RespPregunta22Com;

                            txtComentariosRiesgos.Text = objCalificacion.objCliente.comentarioRiesgos.Trim();

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
                        txtRespuestaPregunta19.ReadOnly = true;
                        txtRespuestaPregunta20.ReadOnly = true;
                        txtRespuestaPregunta21.ReadOnly = true;
                        txtRespuestaPregunta22.ReadOnly = true;

                        txtComentariosRiesgos.ReadOnly = true;
                        txtRazonCalificacion.ReadOnly = true;

                        if (!objCalificacion.objCliente.SustentacionCalRecPIC.Trim().Equals(string.Empty))

                        {
                            txtSustentacionCalRec.Text = objCalificacion.objCliente.SustentacionCalRecPIC.Trim();
                            txtRespuestaPregunta19.Text = objCalificacion.objCliente.RespPregunta19PIC.Trim();
                            txtRespuestaPregunta20.Text = objCalificacion.objCliente.RespPregunta20PIC.Trim();
                            txtRespuestaPregunta21.Text = objCalificacion.objCliente.RespPregunta21PIC.Trim();
                            txtRespuestaPregunta22.Text = objCalificacion.objCliente.RespPregunta22PIC.Trim();

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

        /// <summary>
        /// Muestra la información del comité anterior
        /// </summary>
        /// <param name="objComA"></param>
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


        /// <summary>
        /// llena la informacion de pec
        /// </summary>
        /// <param name="central"></param>
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

        /// <summary>
        /// llena la informacion de covenants
        /// </summary>
        /// <param name="covenants"></param>
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

        /// <summary>
        /// muestra la informacion de prorrogas
        /// </summary>
        /// <param name="prorrogas"></param>
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

        /// <summary>
        /// muestra la informacion de Indicadores
        /// </summary>
        /// <param name="prorrogas"></param>
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

        /// <summary>
        /// Crea los rangos de fechas
        /// </summary>
        /// <returns></returns>
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

       /*
        * private string formatNumber(string number)
        {
            string result = "";
            if (number != null)
            {
                number = number.Trim();
                if (number != String.Empty)
                {
                    number = number.Replace(".", "").Replace(",", ".");
                    if (!number.Contains("."))
                    {
                        number = number + ".00";
                    }
                    result = Double.Parse(number, CultureInfo.InvariantCulture).ToString("N2");
                    result = result.Replace(".", "!").Replace(",", "$");
                    result = result.Replace("!", ",").Replace("$", ".");
                }
            }

            return result;
        }
        */

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

        //PMO019939 - RF061
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

        /// <summary>
        /// PMO27494
        /// Establece encoding UTF8 para los campos de preguntas y respuesta tipo Texto
        /// </summary>
        /// <returns></returns>
        private void establecerEncodingCamposTexto()
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            txtSustentacionCalRec.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtSustentacionCalRec.Text.Trim()))));
            txtRespuestaPregunta19.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta19.Text.Trim()))));
            txtRespuestaPregunta20.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta20.Text.Trim()))));
            txtRespuestaPregunta21.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta21.Text.Trim()))));
            txtRespuestaPregunta22.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtRespuestaPregunta22.Text.Trim()))));

            txtComentariosRiesgos.Text = iso.GetString((Encoding.Convert(UTF8Encoding.UTF8, iso, UTF8Encoding.UTF8.GetBytes(txtComentariosRiesgos.Text.Trim()))));
        }

        /// <summary>
        /// PMO27494
        /// Retona un objeto List<PropiedadesCalificacion> que contendrá las propiedades que son comunes a Comercial y Riesgos
        /// </summary>
        /// <returns></returns>
        private List<PropiedadesCalificacion> guardarPropiedadesComunes(List<PropiedadesCalificacion> propiedades)
        {

            PropiedadesCalificacion propiedad0 = new PropiedadesCalificacion();
            propiedad0.nombrePripiedad = txtSustentacionCalRec.ID;
            propiedad0.valorPropiedad = txtSustentacionCalRec.Text.Trim();

            propiedades.Add(propiedad0);

            PropiedadesCalificacion propiedadPr1 = new PropiedadesCalificacion();
            propiedadPr1.nombrePripiedad = txtRespuestaPregunta19.ID;
            propiedadPr1.valorPropiedad = txtRespuestaPregunta19.Text.Trim();

            propiedades.Add(propiedadPr1);

            PropiedadesCalificacion propiedadPr2 = new PropiedadesCalificacion();
            propiedadPr2.nombrePripiedad = txtRespuestaPregunta20.ID;
            propiedadPr2.valorPropiedad = txtRespuestaPregunta20.Text.Trim();

            propiedades.Add(propiedadPr2);

            PropiedadesCalificacion propiedadPr3 = new PropiedadesCalificacion();
            propiedadPr3.nombrePripiedad = txtRespuestaPregunta21.ID;
            propiedadPr3.valorPropiedad = txtRespuestaPregunta21.Text.Trim();

            propiedades.Add(propiedadPr3);

            PropiedadesCalificacion propiedadPr4 = new PropiedadesCalificacion();
            propiedadPr4.nombrePripiedad = txtRespuestaPregunta22.ID;
            propiedadPr4.valorPropiedad = txtRespuestaPregunta22.Text.Trim();

            propiedades.Add(propiedadPr4);

            return propiedades;
        }

        private void cargarPreguntasPresencial(Parametros[] param)
        {

            //Pregunta 19
            Parametros pregunta19 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 19).ElementAt(0);
            lblPregunta19.Text = "1) " + pregunta19.param7;
            lblComplementoPregunta19.Text = pregunta19.param8;

            //Pregunta 20
            Parametros pregunta20 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 20).ElementAt(0);
            lblPregunta20.Text = "2) " + pregunta20.param7;
            lblComplementoPregunta20.Text = pregunta20.param8;

            //Pregunta 21
            Parametros pregunta21 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 21).ElementAt(0);
            lblPregunta21.Text = "3) " + pregunta21.param7;
            lblComplementoPregunta21.Text = pregunta21.param8;

            //Pregunta 22
            Parametros pregunta22 = param.Cast<Parametros>().Where(p => p.paramName.Trim().Equals("PRFORMPRE")
                && p.paramSeq == 22).ElementAt(0);
            lblPregunta22.Text = "4) " + pregunta22.param7;
            lblComplementoPregunta22.Text = pregunta22.param8;
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
                //consultarInformaciónCliente

                //Proxy.CalificacionCartera proxyCartera = new Proxy.CalificacionCartera();
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

                        //Obtiene la información necesaria para luego usarla en la actualización del log.
                        var data = objCalificacion.objCliente;
                        //Actualiza la tabla de logs insertando una nueva actividad de consulta.
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
                            // Ini PMO27494
                            inicializarLista(ddlCalNRRecom, "0", false);

                            ddlCalNRRecom.SelectedValue = objCalificacion.objCliente.calNuevoRatRecom.Trim();

                            List<PropiedadesCalificacion> propiedades = new List<PropiedadesCalificacion>();
                            //Ini PMO27494 - Preguntas
                            //cargar respuesta a preguntas comercial para edición
                            txtSustentacionCalRec.Text = objCalificacion.objCliente.SustentacionCalRecCom.Trim();                            
                            txtRespuestaPregunta19.Text = objCalificacion.objCliente.RespPregunta19Com.Trim();
                            txtRespuestaPregunta20.Text = objCalificacion.objCliente.RespPregunta20Com.Trim();
                            txtRespuestaPregunta21.Text = objCalificacion.objCliente.RespPregunta21Com.Trim();
                            txtRespuestaPregunta22.Text = objCalificacion.objCliente.RespPregunta22Com.Trim();

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

                            

                            //poner comentarios riesgos para edición

                            //poner comentarios riesgos para edición
                            if (objCalificacion.objCliente.calIntRatNRating.Trim().Length > 0)
                            {
                                txtSustentacionCalRec.Text = objCalificacion.objCliente.SustentacionCalRecPIC;
                               
                                txtRespuestaPregunta19.Text = objCalificacion.objCliente.RespPregunta19PIC;
                                txtRespuestaPregunta20.Text = objCalificacion.objCliente.RespPregunta20PIC;
                                txtRespuestaPregunta21.Text = objCalificacion.objCliente.RespPregunta21PIC;
                                txtRespuestaPregunta22.Text = objCalificacion.objCliente.RespPregunta22PIC;                                

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
                                txtRespuestaPregunta19.Text = objCalificacion.objCliente.RespPregunta19Com;
                                txtRespuestaPregunta20.Text = objCalificacion.objCliente.RespPregunta20Com;
                                txtRespuestaPregunta21.Text = objCalificacion.objCliente.RespPregunta21Com;
                                txtRespuestaPregunta22.Text = objCalificacion.objCliente.RespPregunta22Com;

                                txtComentariosRiesgos.Text = objCalificacion.objCliente.comentarioRiesgos;                                

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

