using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fachada = Bancolombia.Riesgo.MAF.Negocio.Fachadas.CalificacionCartera;
using Bancolombia.Riesgo.MAFWeb.Entidades.CalificacionCartera;
using Bancolombia.Riesgo.MAF.Entidades.CalificacionCartera;
using System.Globalization;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Bancolombia.Riesgo.MAFWeb.CalificacionCartera
{
    public partial class CalificacionClientePrdo : System.Web.UI.Page
    {
        #region "Eventos"

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
                string prdo = Request.QueryString["Prdo"];
                //Proxy.CalificacionCartera proxy = new Proxy.CalificacionCartera();
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
                ViewState["Cliente"] = null;
                ViewState["Cambios"] = null;
                Master.mpTitulo = "Consulta histórica periodo " + prdo;
                lblUsuario.Text = ((Usuario)Session["Rol"]).usuario;
                llenarCliente(Request.QueryString["Codigo"].ToString());

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            }
        }
        /// <summary>
        /// Retorna a la búsqueda de clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConsultarClientePrdo.aspx?Prdo=" + Request.QueryString["Prdo"]);
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
            ObjetosCalificacion objCalificacion;
            //RF017
            Parametros[] param = null;
            if (Session["parametros"] != null)
            {
                param = (Parametros[])Session["parametros"];
            }
            //END
            try
            {
                txtBalance.ReadOnly = true;
                txtCliente.ReadOnly = true;
                txtComentariosAdicionales.ReadOnly = true;
                txtPyG.ReadOnly = true;
                txtRespuestaPregunta1Pdo.ReadOnly = true;
                txtRespuestaPregunta2Pdo.ReadOnly = true;
                txtRespuestaPregunta3Pdo.ReadOnly = true;
                txtRespuestaPregunta4Pdo.ReadOnly = true;
                txtRespuestaPregunta5Pdo.ReadOnly = true;
                txtRespuestaPregunta6Pdo.ReadOnly = true;
                txtRespuestaPregunta7Pdo.ReadOnly = true;
                txtRespuestaPregunta8Pdo.ReadOnly = true;
                txtRespuestaPregunta9Pdo.ReadOnly = true;
                txtRespuestaPregunta10Pdo.ReadOnly = true;
                txtRespuestaPregunta11Pdo.ReadOnly = true;
                txtRespuestaPregunta12Pdo.ReadOnly = true;
                txtRespuestaPregunta13Pdo.ReadOnly = true;
                txtRespuestaPregunta14Pdo.ReadOnly = true;
                txtRespuestaPregunta15Pdo.ReadOnly = true;
                txtRespuestaPregunta16Pdo.ReadOnly = true;
                txtRespuestaPregunta17Pdo.ReadOnly = true;
                txtRespuestaPregunta18Pdo.ReadOnly = true;
                txtRespuestaPregunta19Pdo.ReadOnly = true;
                txtRespuestaPregunta20Pdo.ReadOnly = true;
                txtRespuestaPregunta21Pdo.ReadOnly = true;
                txtRespuestaPregunta22Pdo.ReadOnly = true;
                txtSustentacionCalRecPdo.ReadOnly = true;
                txtComentariosRiesgosPdo.ReadOnly = true;
                ddlPregunta1Pdo.Attributes.Add("disabled", "disabled");
                ddlPregunta2Pdo.Attributes.Add("disabled", "disabled");
                ddlPregunta3Pdo.Attributes.Add("disabled", "disabled");
                ddlPregunta4Pdo.Attributes.Add("disabled", "disabled");
                ddlPregunta5Pdo.Attributes.Add("disabled", "disabled");
                ddlPregunta6Pdo.Attributes.Add("disabled", "disabled");
                ddlPregunta7Pdo.Attributes.Add("disabled", "disabled");
                ddlPregunta8Pdo.Attributes.Add("disabled", "disabled");
                ddlPregunta9Pdo.Attributes.Add("disabled", "disabled");
                ddlPregunta10Pdo.Attributes.Add("disabled", "disabled");
                ddlPregunta11Pdo.Attributes.Add("disabled", "disabled");
                ddlPregunta12Pdo.Attributes.Add("disabled", "disabled");
                ddlPregunta13Pdo.Attributes.Add("disabled", "disabled");
                ddlPregunta14Pdo.Attributes.Add("disabled", "disabled");
                ddlCalifInternaRNRPdo.Attributes.Add("disabled", "disabled");
                ddlCalifExternaPPdo.Attributes.Add("disabled", "disabled");
                ddlSeguimientoPdo.Attributes.Add("disabled", "disabled");
                ddlRecomendacionPdo.Attributes.Add("disabled", "disabled");
                ddlTipoClientePdo.Attributes.Add("disabled", "disabled");
                ddlEstadoCalPdo.Attributes.Add("disabled", "disabled");
                ddlUtilizoEEFFPdo.Attributes.Add("disabled", "disabled");

                objCalificacion = Fachada.CalificacionCartera.ConsultarNITPrdo(codigo, Request.QueryString["Prdo"]);

                //Obtiene la información necesaria para luego usarla en la actualización del log.
                var data = objCalificacion.objCliente;

                //Actualiza la tabla de logs insertando una nueva actividad de consulta.
                if (data != null)
                    if (((Usuario)Session["Rol"]).rol == "Comercial")
                    {
                        Fachada.CalificacionCartera.ActualizarLogCSC(data.fecProc, "12",
                        ((Usuario)Session["Rol"]).usuario, "", codigo, "", "", "", "");
                    }
                    else if (((Usuario)Session["Rol"]).rol == "Riesgos - PIC")
                    {
                        Fachada.CalificacionCartera.ActualizarLogCSC(data.fecProc, "15",
                        ((Usuario)Session["Rol"]).usuario, "", codigo, "", "", "", "");
                    }
                    else if (((Usuario)Session["Rol"]).rol == "Superfinanciera")
                    {
                        Fachada.CalificacionCartera.ActualizarLogCSC(data.fecProc, "18",
                        ((Usuario)Session["Rol"]).usuario, "", codigo, "", "", "", "");
                    }
                    else if (((Usuario)Session["Rol"]).rol == "Consulta")
                    {
                        Fachada.CalificacionCartera.ActualizarLogCSC(data.fecProc, "21",
                        ((Usuario)Session["Rol"]).usuario, "", codigo, "", "", "", "");
                    }

                if (objCalificacion.objCliente != null)
                {
                    ViewState["Cliente"] = objCalificacion.objCliente;
                    lblTipoComite.Text = objCalificacion.objCliente.tipCom.Trim();

                    //habilitar controles de cliente
                    pnlCliente.Visible = true;

                    lblCliente.Text = codigo;
                    lblNombreCliente.Text = objCalificacion.objCliente.nombre.Trim();
                    lblFechaComite.Text = objCalificacion.objCliente.fecProc.Trim();

                    //establece cuál será el separador de decimales
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
                        if (((Usuario)Session["Rol"]).rol.Equals("Consulta") || ((Usuario)Session["Rol"]).rol.Equals("Comercial"))
                        {
                            lblLocal.Visible = false;
                        }
                        //Bug 1361
                        string cumple = "";
                        if (!objCalificacion.objCliente.causalDisolucion.Trim().Equals(string.Empty))
                        {
                            cumple = objCalificacion.objCliente.causalDisolucion.Trim();
                        }
                        else
                        {
                            //Cumple o no causal de disolucion
                            if (objCalificacion.objCliente.capSocialPerAct.Trim().Equals(string.Empty) || objCalificacion.objCliente.patPerAct.Trim().Equals(string.Empty))
                            {
                                cumple = "NO";
                            }
                            else
                            {
                                IEnumerable<Parametros> causal;
                                if (int.TryParse(objCalificacion.objCliente.tipDoc.Trim(), out int tipoCodigo))
                                {
                                    if (tipoCodigo != 1 && tipoCodigo != 2 && tipoCodigo != 4 && tipoCodigo != 9)
                                    {
                                        causal = param.Where(p => p.paramName.Trim().Equals("VPROAUTO") && p.paramSeq == 1);
                                        double capital = double.MinValue;
                                        double patrimonio = double.MinValue;
                                        double prima = double.MinValue;
                                        if (double.TryParse(objCalificacion.objCliente.capSocialPerAct, NumberStyles.Float | NumberStyles.AllowThousands, nfi, out capital) && double.TryParse(objCalificacion.objCliente.patPerAct, NumberStyles.Float | NumberStyles.AllowThousands, nfi, out patrimonio)
                                            && double.TryParse(objCalificacion.objCliente.primaColAcPerAct, NumberStyles.Float | NumberStyles.AllowThousands, nfi, out prima))
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
                        lblCausalDisulocionResp.Text = cumple;
                        //End Bug 1361.
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

                    lblSKPan.Text = objCalificacion.objCliente.saldoKPanama.Trim();
                    lblSKPR.Text = objCalificacion.objCliente.saldoKPuertoRico.Trim();

                    //Se adiciona dias de mora, reestructurados y Calif Externa para Puerto Rico y panama
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

                    // End - PMO19939 - RF061
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

                    //RF017
                    //Se busca el valor para el CAMPO PARAMÉTRICO 1
                    if (Session["parametros"] != null)
                    {
                        Parametros campoParametrico = param.Where(p => p.paramName.Trim().Equals("OPARMADI")
                            && p.paramSeq == 1).ElementAt(0);
                        lblCampoParametrico1.Text = campoParametrico.param7;
                    }
                    //información Financiera

                    int fecha = int.MinValue;
                    if (int.TryParse(objCalificacion.objCliente.fechaEstFros.Trim(), out fecha) && fecha.ToString().Length == 6)
                    {
                        pnlInformacionFinanciera.Visible = true;
                        pnlInformacionFinancieraNo.Visible = false;
                        //si es visible la info financiera mostrar:
                        lblFechaInfoFinanciera.Text = objCalificacion.objCliente.fechaEstFros.Trim();
                        lblFinancieraSolicitada.Text = objCalificacion.objCliente.corteEstFros.Trim();
                        lblVentas.Text = objCalificacion.objCliente.ventasPerAct.Trim();

                        lblUtilidadOp.Text = objCalificacion.objCliente.utilPerdidaOpPerAct.Trim();
                        lblCostoFin.Text = objCalificacion.objCliente.intPagadosPerAct.Trim();
                        lblUtilidadNe.Text = objCalificacion.objCliente.utilPerNetaPerAct.Trim();
                        lblUtilidadBru.Text = objCalificacion.objCliente.utilBrutaPerAct.Trim();
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
                        //Bug 1361
                        lblFinancieraSolicitada.Text = "";
                    }

                    if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC") || ((Usuario)Session["Rol"]).rol.Equals("Superfinanciera") || ((Usuario)Session["Rol"]).rol.Equals("Consulta") || ((Usuario)Session["Rol"]).rol.Equals("Comercial"))
                    {
                        //habilitar calificación comercial
                        pnlCalificacionSuper.Visible = false;
                        PanelCalificacionComercialPdo.Visible = false;
                        if (Convert.ToInt32(data.fecProc.ToString()) >= 201812 && (Convert.ToInt32(data.fecProc.ToString()) <= 201906)) //   //201806
                        {
                            pnlCalificacionPIC.Visible = false;
                            pnlComentarios.Visible = false;
                            pnlComentariosPdoFI.Visible = false;
                            pnlComentariosPdoGB.Visible = false;
                            CargarPreguntasNvoPdo(codigo);
                            pnlCalificacionPICPdo.Visible = true;
                        }
                        else if (Convert.ToInt32(data.fecProc.ToString()) >= 201912)
                        {

                            if (data.categoriaCliente == "FI")
                            {
                                pnlCalificacionPIC.Visible = false;
                                pnlComentarios.Visible = false;
                                pnlComentariosPdo.Visible = true;
                                pnlComentariosPdoFI.Visible = true;
                                pnlComentariosPdoGB.Visible = false;
                                CargarPreguntasNvoPdo(codigo);
                                pnlCalificacionPICPdo.Visible = true;
                            }
                            else if (data.categoriaCliente == "GB")
                            {
                                pnlCalificacionPIC.Visible = false;
                                pnlComentarios.Visible = false;
                                pnlComentariosPdo.Visible = false;
                                pnlComentariosPdoFI.Visible = false;
                                pnlComentariosPdoGB.Visible = true;
                                CargarPreguntasNvoPdo(codigo);
                                pnlCalificacionPICPdo.Visible = true;
                            }
                            else
                            {
                                pnlCalificacionPIC.Visible = false;
                                pnlComentarios.Visible = false;
                                pnlComentariosPdo.Visible = true;
                                pnlComentariosPdoFI.Visible = false;
                                pnlComentariosPdoGB.Visible = false;
                                CargarPreguntasNvoPdo(codigo);
                                pnlCalificacionPICPdo.Visible = true;
                            }
                        }

                        else
                        {
                            pnlCalificacionPIC.Visible = true;
                            pnlComentariosPdo.Visible = false;
                            pnlComentariosPdoFI.Visible = false;
                            pnlComentariosPdoGB.Visible = false;
                            pnlComentariosGeneralPdo.Visible = false;
                            pnlCalificacionPICPdo.Visible = false;
                        }

                        //pintar controles de calif PIC
                        lblCalificacionINRatingPIC.Text = objCalificacion.objCliente.CalIntNRating.Trim();
                        lblFechaCalifP.Text = objCalificacion.objCliente.fecCalInt.Trim();
                        lblCovenantP.Text = objCalificacion.objCliente.segCovenants.Trim();

                        lblCalifRecPEC.Text = objCalificacion.objCliente.calSugeridaPEC.Trim();

                        txtCliente.Text = objCalificacion.objCliente.actClientePIC.Trim();
                        txtBalance.Text = objCalificacion.objCliente.analisisBalPIC.Trim();
                        txtPyG.Text = objCalificacion.objCliente.analisisPyGPIC.Trim();
                        txtComentariosAdicionales.Text = objCalificacion.objCliente.comAdPIC.Trim();
                        txtComentariosRiesgos.Text = objCalificacion.objCliente.comentarioRiesgos.Trim();
                        lblCalifInternaRNRHis.Text = objCalificacion.objCliente.calIntRat.Trim();
                        lblCalifExternaPHis.Text = objCalificacion.objCliente.calExtRat.Trim();

                        // PMO19939 - RF061: Modificar visualización “Consulta Proceso Anteriores” del comité presencial
                        lblTipoCliente.Text = objCalificacion.objCliente.tipoCliente.Trim();
                        lblUtilizoEEFF.Text = objCalificacion.objCliente.utilizoEEFF.Trim();

                        lblRazonHis.Text = processoRazCaliInt(objCalificacion.objCliente.razCaliInt);
                        lblSeguimientoHis.Text = (objCalificacion.objCliente.segProxComite.Trim().Equals("0") ? "" : objCalificacion.objCliente.segProxComite.Trim());
                        lblRecomendacionHis.Text = (objCalificacion.objCliente.recAEC.Trim().Equals("0") ? "" : objCalificacion.objCliente.recAEC.Trim());
                        lblPuntajeHis.Text = (objCalificacion.objCliente.puntajeMAF.Trim().Equals("0") ? "" : objCalificacion.objCliente.puntajeMAF.Trim());
                        lblFechaMAFHis.Text = (objCalificacion.objCliente.fechaMAF.Trim().Equals("0") ? "" : objCalificacion.objCliente.fechaMAF.Trim());
                        lblCalifInternaRNRHis.Text = (objCalificacion.objCliente.calIntRatNRating.Trim().Equals("0") ? "" : objCalificacion.objCliente.calIntRatNRating.Trim());
                        lblEstadoCalificacionHis.Text = (objCalificacion.objCliente.estadoCalificacion.Trim().Equals("0") ? "" : objCalificacion.objCliente.estadoCalificacion.Trim());
                        lblCalNuevoRRecomHis.Text = (objCalificacion.objCliente.calNuevoRatRecom.Trim().Equals("0") ? "" : objCalificacion.objCliente.calNuevoRatRecom.Trim());
                        //Bug 1361
                        lblMafNuevo.Text = objCalificacion.objCliente.CalMAFNRating.Trim();

                        //PMO27494

                        lblCalificacionINRatingPICPdo.Text = objCalificacion.objCliente.CalIntNRating.Trim();
                        ddlCalifInternaRNRPdo.Items.Insert(0, objCalificacion.objCliente.calIntRatNRating.Trim());
                        lblFechaCalifPPdo.Text = objCalificacion.objCliente.fecCalInt.Trim();
                        ddlCalifExternaPPdo.Items.Insert(0, objCalificacion.objCliente.calExtRat.Trim());
                        lblCovenantPPdo.Text = objCalificacion.objCliente.segCovenants.Trim();
                        ddlSeguimientoPdo.Items.Insert(0, objCalificacion.objCliente.segProxComite.Trim());
                        lblCalMAFNuevoRatingPicPdo.Text = objCalificacion.objCliente.CalMAFNRating.Trim();
                        lblRazonPdo.Text = processoRazCaliInt(objCalificacion.objCliente.razCaliInt).Trim();
                        lblCalifRecPECPdo.Text = objCalificacion.objCliente.calSugeridaPEC.Trim();
                        lblListasdeControlPdo.Text = objCalificacion.objCliente.ListasDeControl.Trim();
                        ddlTipoClientePdo.Items.Insert(0, objCalificacion.objCliente.tipoCliente.Trim());
                        lblCalNuevoRRecomPdo.Text = objCalificacion.objCliente.calNuevoRatRecom.Trim();
                        ddlEstadoCalPdo.Items.Insert(0, objCalificacion.objCliente.estadoCalificacion.Trim());
                        ddlUtilizoEEFFPdo.Items.Insert(0, objCalificacion.objCliente.EEFFUti.Trim());


                    }
                    else
                    {
                        pnlCalificacionPIC.Visible = false;
                        pnlCalificacionSuper.Visible = true;
                        pnlCalificacionPICPdo.Visible = false;

                        if (Convert.ToInt32(data.fecProc.ToString()) >= 201812)
                        {
                            pnlCalificacionSuper.Visible = true;
                            pnlComentarios.Visible = false;
                            PanelCalificacionComercialPdo.Visible = false;
                            CargarPreguntasNvoPdo(codigo);

                        }
                        else
                        {
                            PanelCalificacionComercialPdo.Visible = false;
                            pnlComentariosPdo.Visible = false;
                            pnlComentariosGeneralPdo.Visible = false;
                        }

                        lblCalMAFNuevoRatingSup.Text = objCalificacion.objCliente.CalMAFNRating.Trim();

                        lblCalificacionINRatingPICSuper.Text = objCalificacion.objCliente.CalIntNRating.Trim();
                        lblCalNuevoRRecomHisSuper.Text = (objCalificacion.objCliente.calNuevoRatRecom.Trim().Equals("0") ? "" : objCalificacion.objCliente.calNuevoRatRecom.Trim());
                        lblFechaCalifPSuper.Text = objCalificacion.objCliente.fecCalInt.Trim();
                        lblCovenantPSuper.Text = objCalificacion.objCliente.segCovenants.Trim();

                        txtCliente.Text = objCalificacion.objCliente.actClientePIC.Trim();
                        txtBalance.Text = objCalificacion.objCliente.analisisBalPIC.Trim();
                        txtPyG.Text = objCalificacion.objCliente.analisisPyGPIC.Trim();
                        txtComentariosAdicionales.Text = objCalificacion.objCliente.comAdPIC.Trim();
                        txtComentariosRiesgos.Text = objCalificacion.objCliente.comentarioRiesgos.Trim();

                        //Adicionar el contenido de las preguntas del nuevo periodo PMO 27494
                        lblCalificacionInRatingPdoCom.Text = objCalificacion.objCliente.CalIntNRating.Trim();
                        lblCalNRRecomPdo.Text = (objCalificacion.objCliente.calNuevoRatRecom.Trim().Equals("0") ? "" : objCalificacion.objCliente.calNuevoRatRecom.Trim());
                        lblFechaCalifCPdo.Text = objCalificacion.objCliente.fecCalInt.Trim();
                        lblCovenantCPdo.Text = objCalificacion.objCliente.segCovenants.Trim();
                        lblListadeControlComPdo.Text = objCalificacion.objCliente.ListasDeControl.Trim();




                        btnBuscarNuevo.Visible = true;
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

        #endregion

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            //Proxy.CalificacionCartera proxyCartera = new Proxy.CalificacionCartera();
            ObjetosCalificacion objCalificacion;

            objCalificacion = Fachada.CalificacionCartera.ConsultarNITPrdo(Request.QueryString["Codigo"].ToString(), Request.QueryString["Prdo"]);

            var data = objCalificacion.objCliente;
            var categoriaCliente = data.categoriaCliente;

            Response.Redirect("Reporte/PDFHistorico.aspx?Cliente=" + lblCliente.Text.Trim() + "&Prdo=" + lblFechaComite.Text.Trim() + "&categoriaCliente=" + categoriaCliente);

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
        // PMO 27494 se agregan las nuevas preguntas y respuestas para la consulta por periodos.
        private void CargarPreguntasNvoPdo(string codigo)
        {
            //consultarInformaciónCliente
            //Proxy.CalificacionCartera proxyCartera = new Proxy.CalificacionCartera();
            ObjetosCalificacion objCalificacion;

            objCalificacion = Fachada.CalificacionCartera.ConsultarNITPrdo(codigo, Request.QueryString["Prdo"]);

            //Obtiene la información necesaria para luego usarla en la actualización del log

            txtSustentacionCalRecPdo.Text = objCalificacion.objCliente.SustentacionCalRecPIC.Trim();

            if ((objCalificacion.objCliente.categoriaCliente != "GB"))
            {
                lblPregunta1Pdo.Text = "1) " + objCalificacion.objCliente.Pregunta1Pdo.Trim();
                lblComplementoPregunta1Pdo.Text = objCalificacion.objCliente.DesPregunta1Pdo.Trim();
                txtRespuestaPregunta1Pdo.Text = objCalificacion.objCliente.RespPregunta1PIC.Trim();
                ddlPregunta1Pdo.Items.Insert(0, objCalificacion.objCliente.RespListaPregunta1PIC.Trim());

                lblPregunta2Pdo.Text = "2) " + objCalificacion.objCliente.Pregunta2Pdo.Trim();
                lblComplementoPregunta2Pdo.Text = objCalificacion.objCliente.DesPregunta2Pdo.Trim();
                txtRespuestaPregunta2Pdo.Text = objCalificacion.objCliente.RespPregunta2PIC.Trim();
                ddlPregunta2Pdo.Items.Insert(0, objCalificacion.objCliente.RespListaPregunta2PIC.Trim());

                lblPregunta3Pdo.Text = "3) " + objCalificacion.objCliente.Pregunta3Pdo.Trim();
                lblComplementoPregunta3Pdo.Text = objCalificacion.objCliente.DesPregunta3Pdo.Trim();
                txtRespuestaPregunta3Pdo.Text = objCalificacion.objCliente.RespPregunta3PIC.Trim();
                ddlPregunta3Pdo.Items.Insert(0, objCalificacion.objCliente.RespListaPregunta3PIC.Trim());

                lblPregunta4Pdo.Text = "4) " + objCalificacion.objCliente.Pregunta4Pdo.Trim();
                lblComplementoPregunta4Pdo.Text = objCalificacion.objCliente.DesPregunta4Pdo.Trim();
                txtRespuestaPregunta4Pdo.Text = objCalificacion.objCliente.RespPregunta4PIC.Trim();
                ddlPregunta4Pdo.Items.Insert(0, objCalificacion.objCliente.RespListaPregunta4PIC.Trim());

                lblPregunta5Pdo.Text = "5) " + objCalificacion.objCliente.Pregunta5Pdo.Trim();
                lblComplementoPregunta5Pdo.Text = objCalificacion.objCliente.DesPregunta5Pdo.Trim();
                txtRespuestaPregunta5Pdo.Text = objCalificacion.objCliente.RespPregunta5PIC.Trim();
                ddlPregunta5Pdo.Items.Insert(0, objCalificacion.objCliente.RespListaPregunta5PIC.Trim());

                lblPregunta6Pdo.Text = "6) " + objCalificacion.objCliente.Pregunta6Pdo.Trim();
                lblComplementoPregunta6Pdo.Text = objCalificacion.objCliente.DesPregunta6Pdo.Trim();
                txtRespuestaPregunta6Pdo.Text = objCalificacion.objCliente.RespPregunta6PIC.Trim();
                ddlPregunta6Pdo.Items.Insert(0, objCalificacion.objCliente.RespListaPregunta6PIC.Trim());

                lblPregunta7Pdo.Text = "7) " + objCalificacion.objCliente.Pregunta7Pdo.Trim();
                lblComplementoPregunta7Pdo.Text = objCalificacion.objCliente.DesPregunta7Pdo.Trim();
                txtRespuestaPregunta7Pdo.Text = objCalificacion.objCliente.RespPregunta7PIC.Trim();
                ddlPregunta7Pdo.Items.Insert(0, objCalificacion.objCliente.RespListaPregunta7PIC.Trim());

                lblPregunta8Pdo.Text = "8) " + objCalificacion.objCliente.Pregunta8Pdo.Trim();
                lblComplementoPregunta8Pdo.Text = objCalificacion.objCliente.DesPregunta8Pdo.Trim();
                txtRespuestaPregunta8Pdo.Text = objCalificacion.objCliente.RespPregunta8PIC.Trim();
                ddlPregunta8Pdo.Items.Insert(0, objCalificacion.objCliente.RespListaPregunta8PIC.Trim());

                lblPregunta9Pdo.Text = "9) " + objCalificacion.objCliente.Pregunta9Pdo.Trim();
                lblComplementoPregunta9Pdo.Text = objCalificacion.objCliente.DesPregunta9Pdo.Trim();
                txtRespuestaPregunta9Pdo.Text = objCalificacion.objCliente.RespPregunta9PIC.Trim();
                ddlPregunta9Pdo.Items.Insert(0, objCalificacion.objCliente.RespListaPregunta9PIC.Trim());

                lblPregunta10Pdo.Text = "1) " + objCalificacion.objCliente.Pregunta10Pdo.Trim();
                lblComplementoPregunta10Pdo.Text = objCalificacion.objCliente.DesPregunta10Pdo.Trim();
                txtRespuestaPregunta10Pdo.Text = objCalificacion.objCliente.RespPregunta10PIC.Trim();
                ddlPregunta10Pdo.Items.Insert(0, objCalificacion.objCliente.RespListaPregunta10PIC.Trim());

                lblPregunta11Pdo.Text = "2) " + objCalificacion.objCliente.Pregunta11Pdo.Trim();
                lblComplementoPregunta11Pdo.Text = objCalificacion.objCliente.DesPregunta11Pdo.Trim();
                txtRespuestaPregunta11Pdo.Text = objCalificacion.objCliente.RespPregunta11PIC.Trim();
                ddlPregunta11Pdo.Items.Insert(0, objCalificacion.objCliente.RespListaPregunta11PIC.Trim());

                lblPregunta12Pdo.Text = "3) " + objCalificacion.objCliente.Pregunta12Pdo.Trim();
                lblComplementoPregunta12Pdo.Text = objCalificacion.objCliente.DesPregunta12Pdo.Trim();
                txtRespuestaPregunta12Pdo.Text = objCalificacion.objCliente.RespPregunta12PIC.Trim();
                ddlPregunta12Pdo.Items.Insert(0, objCalificacion.objCliente.RespListaPregunta12PIC.Trim());

                lblPregunta13Pdo.Text = "4) " + objCalificacion.objCliente.Pregunta13Pdo.Trim();
                lblComplementoPregunta13Pdo.Text = objCalificacion.objCliente.DesPregunta13Pdo.Trim();
                txtRespuestaPregunta13Pdo.Text = objCalificacion.objCliente.RespPregunta13PIC.Trim();
                ddlPregunta13Pdo.Items.Insert(0, objCalificacion.objCliente.RespListaPregunta13PIC.Trim());

                lblPregunta14Pdo.Text = "5) " + objCalificacion.objCliente.Pregunta14Pdo.Trim();
                lblComplementoPregunta14Pdo.Text = objCalificacion.objCliente.DesPregunta14Pdo.Trim();
                txtRespuestaPregunta14Pdo.Text = objCalificacion.objCliente.RespPregunta14PIC.Trim();
                ddlPregunta14Pdo.Items.Insert(0, objCalificacion.objCliente.RespListaPregunta14PIC.Trim());

            }
            //Extración de preguntas y respuestas de periodos anteriores

            if (objCalificacion.objCliente.categoriaCliente == "FI")
            {
                lblPregunta15Pdo.Text = "1) " + objCalificacion.objCliente.Pregunta15Pdo.Trim();
                lblComplementoPregunta15Pdo.Text = objCalificacion.objCliente.DesPregunta15Pdo.Trim();
                txtRespuestaPregunta15Pdo.Text = objCalificacion.objCliente.RespPregunta15PIC.Trim();

                lblPregunta16Pdo.Text = "2) " + objCalificacion.objCliente.Pregunta16Pdo.Trim();
                lblComplementoPregunta16Pdo.Text = objCalificacion.objCliente.DesPregunta16Pdo.Trim();
                txtRespuestaPregunta16Pdo.Text = objCalificacion.objCliente.RespPregunta16PIC.Trim();

                lblPregunta17Pdo.Text = "3) " + objCalificacion.objCliente.Pregunta17Pdo.Trim();
                lblComplementoPregunta17Pdo.Text = objCalificacion.objCliente.DesPregunta17Pdo.Trim();
                txtRespuestaPregunta17Pdo.Text = objCalificacion.objCliente.RespPregunta17PIC.Trim();

                lblPregunta18Pdo.Text = "4) " + objCalificacion.objCliente.Pregunta18Pdo.Trim();
                lblComplementoPregunta18Pdo.Text = objCalificacion.objCliente.DesPregunta18Pdo.Trim();
                txtRespuestaPregunta18Pdo.Text = objCalificacion.objCliente.RespPregunta18PIC.Trim();
            }

            if (objCalificacion.objCliente.categoriaCliente == "GB")
            {
                lblPregunta19Pdo.Text = "1) " + objCalificacion.objCliente.Pregunta19Pdo.Trim();
                lblComplementoPregunta19Pdo.Text = objCalificacion.objCliente.DesPregunta19Pdo.Trim();
                txtRespuestaPregunta19Pdo.Text = objCalificacion.objCliente.RespPregunta19PIC.Trim();

                lblPregunta20Pdo.Text = "2) " + objCalificacion.objCliente.Pregunta20Pdo.Trim();
                lblComplementoPregunta20Pdo.Text = objCalificacion.objCliente.DesPregunta20Pdo.Trim();
                txtRespuestaPregunta20Pdo.Text = objCalificacion.objCliente.RespPregunta20PIC.Trim();

                lblPregunta21Pdo.Text = "3) " + objCalificacion.objCliente.Pregunta21Pdo.Trim();
                lblComplementoPregunta21Pdo.Text = objCalificacion.objCliente.DesPregunta21Pdo.Trim();
                txtRespuestaPregunta21Pdo.Text = objCalificacion.objCliente.RespPregunta21PIC.Trim();

                lblPregunta22Pdo.Text = "4) " + objCalificacion.objCliente.Pregunta22Pdo.Trim();
                lblComplementoPregunta22Pdo.Text = objCalificacion.objCliente.DesPregunta22Pdo.Trim();
                txtRespuestaPregunta22Pdo.Text = objCalificacion.objCliente.RespPregunta22PIC.Trim();
            }


            txtComentariosRiesgosPdo.Text = objCalificacion.objCliente.comentarioRiesgos.Trim();

        }
    }
}
