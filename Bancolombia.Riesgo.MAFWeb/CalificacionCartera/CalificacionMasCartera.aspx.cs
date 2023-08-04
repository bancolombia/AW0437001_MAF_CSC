using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Bancolombia.Riesgo.MAFWeb.Entidades.CalificacionCartera;
//using Proxy;
using Fachada = Bancolombia.Riesgo.MAF.Negocio.Fachadas.CalificacionCartera;
using Bancolombia.Riesgo.MAF.Entidades.CalificacionCartera;
using System.Linq.Expressions;
using System.Globalization;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Bancolombia.Riesgo.MAFWeb.CalificacionCartera
{
    public partial class CalificacionMasCartera : System.Web.UI.Page
    {

        //Proxy.CalificacionCartera _wsCalificacionCartera = new Proxy.CalificacionCartera();
        /// <summary>
        /// Método de refresco de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //si la sesión es nula redirige a la página del login, si es d perfil adminsitrador muestra error
            //porque no tiene permisos para ingresar, de lo contrario, realiza las cargas segun el perfil
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
                pintarCampos();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            }
        }

        /// <summary>
        /// dependiendo del rol que ingrese a la pantalla, pinta u oculta los páneles correspondientes a este
        /// </summary>
        protected void pintarCampos()
        {
            try
            {
                if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
                {

                    Master.mpTitulo = "Calificación Semestral de Cartera - Fuerza Comercial";
                    lblIntroduccion.Text = "";
                    pnlComercial.Visible = true;
                    pnlRiesgos.Visible = false;
                    lblIntroduccion.Text = "Por favor ingrese sus comentarios para los siguientes clientes:";
                    lblUsuario.Text = ((Usuario)Session["Rol"]).usuario;

                    ObjetosCalificacion objCalificacion = new ObjetosCalificacion();
                    List<ClienteMasivo> clientes;
                    //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
                    objCalificacion = Fachada.CalificacionCartera.ConsultarClientesAsesor(((Usuario)Session["Rol"]).usuario, "1", "M");
                    clientes = objCalificacion.clienteMasivo;
                    ViewState["clienteMasivoBusqueda"] = objCalificacion.clienteMasivo;
                    if (!objCalificacion.codigo.Equals("n") && !objCalificacion.codigo.Equals("0000"))
                        throw new Exception("ERRORSP@" + objCalificacion.codigo + "/" + objCalificacion.descripcion);
                    if (clientes.Count > 0)
                    {
                        lblIntroduccion.Visible = true;
                        //gvClientes.DataSource = dtClientes;
                        gvClientes.DataSource = clientes;
                        gvClientes.DataBind();
                        int limite = (((gvClientes.PageIndex + 1) * gvClientes.PageSize) > clientes.Count) ? clientes.Count : ((gvClientes.PageIndex + 1) * gvClientes.PageSize);
                        if (clientes.Count < gvClientes.PageSize)
                            lblCantidadClientes.Text = "En pantalla se observan los clientes del " + (gvClientes.PageIndex + 1) + " al " + limite.ToString() +
                                " de un total de " + clientes.Count + " clientes asignados";
                        else
                            lblCantidadClientes.Text = "En pantalla se observan los clientes del " + (gvClientes.PageIndex + 1) + " al " + limite.ToString() +
                                                " de un total de " + clientes.Count + " clientes asignados, para observar más clientes por favor navegue entre las páginas";
                    }
                    else
                    {
                        lblIntroduccion.Visible = false;
                        lblCantidadClientes.Text = "No existen clientes relacionados a su usuario";
                    }
                }
                else if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC"))
                {
                    Master.mpTitulo = "Calificación Semestral de Cartera - Riesgos - PIC";
                    lblIntroduccion.Text = "";
                    pnlComercial.Visible = false;
                    pnlRiesgos.Visible = true;
                    lblIntroduccion.Text = "Por favor ingrese sus comentarios para los siguientes clientes:";
                    lblUsuario.Text = ((Usuario)Session["Rol"]).usuario;

                    ObjetosCalificacion objCalificacion = new ObjetosCalificacion();
                    List<ClienteMasivo> clientes;
                    //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
                    objCalificacion = Fachada.CalificacionCartera.ConsultarClientesxRiesgos(((Usuario)Session["Rol"]).usuario, "15");
                    clientes = objCalificacion.clienteMasivo;
                    ViewState["clienteMasivoBusqueda"] = objCalificacion.clienteMasivo;
                    if (!objCalificacion.codigo.Equals("n") && !objCalificacion.codigo.Equals("0000"))
                        throw new Exception("ERRORSP@" + objCalificacion.codigo + "/" + objCalificacion.descripcion);
                    if (clientes.Count > 0)
                    {
                        lblIntroduccion.Visible = true;

                        gvClientesRiesgos.DataSource = clientes;
                        gvClientesRiesgos.DataBind();

                        int limite = (((gvClientesRiesgos.PageIndex + 1) * gvClientesRiesgos.PageSize) > clientes.Count) ? clientes.Count : ((gvClientesRiesgos.PageIndex + 1) * gvClientesRiesgos.PageSize);
                        if (clientes.Count < gvClientesRiesgos.PageSize)
                            lblCantidadClientesR.Text = "En pantalla se observan los clientes del " + (gvClientesRiesgos.PageIndex + 1) + " al " + limite.ToString() +
                                " de un total de " + clientes.Count + " clientes asignados";
                        else
                            lblCantidadClientesR.Text = "En pantalla se observan los clientes del " + (gvClientesRiesgos.PageIndex + 1) + " al " + limite.ToString() +
                                                " de un total de " + clientes.Count + " clientes asignados, para observar más clientes por favor navegue entre las páginas";
                    }
                    else
                    {
                        lblIntroduccion.Visible = false;
                        lblCantidadClientesR.Text = "No existen clientes relacionados a su usuario";
                    }

                }
                //Bug 1352 - Removed gridview in consulta role
                else if (((Usuario)Session["Rol"]).rol.Equals("Consulta"))
                {
                    Master.mpTitulo = "Calificación Semestral de Cartera - Consulta";
                    lblIntroduccion.Text = "";
                    pnlComercial.Visible = false;
                    pnlRiesgos.Visible = true;
                    lblIntroduccion.Text = "Por favor digite la identificación del cliente a consultar:";
                    lblUsuario.Text = ((Usuario)Session["Rol"]).usuario;
                }
                else if (((Usuario)Session["Rol"]).rol.Equals("Superfinanciera"))
                {
                    Master.mpTitulo = "Calificación Semestral de Cartera - Superintendencia Financiera";
                    pnlComercial.Visible = false;
                    lblIntroduccion.Text = "Por favor digite la identificación del cliente a consultar:";
                    lblUsuario.Text = ((Usuario)Session["Rol"]).usuario;
                }
                if (((Usuario)Session["Rol"]).rol.Equals("Superfinanciera") || ((Usuario)Session["Rol"]).rol.Equals("Consulta"))
                {
                    tblProcesoMasivo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CalificacionMasCartera";
                if (ex.Message.ToString().Contains("ERRORSP@"))
                    errEnt.Error = "ERRORSP@";
                else
                    errEnt.Error = "MEN.0411";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }



        /// <summary>
        /// cambia la paginación del grid de clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // actualizar el indice del grid
                gvClientes.PageIndex = e.NewPageIndex;

                List<ClienteMasivo> clientes = (List<ClienteMasivo>)ViewState["clienteMasivoBusqueda"];

                // realiza nuevamente la búsqueda
                gvClientes.DataSource = clientes;//temp cargar clientes
                gvClientes.DataBind();
                int limite = (((gvClientes.PageIndex + 1) * gvClientes.PageSize) > clientes.Count) ? clientes.Count : ((gvClientes.PageIndex + 1) * gvClientes.PageSize);
                lblCantidadClientes.Text = "En pantalla se observan los clientes del " + (gvClientes.PageIndex * gvClientes.PageSize + 1) + " al " + limite.ToString() +
                        " de un total de " + clientes.Count + " clientes asignados, para observar más clientes por favor navegue entre las páginas";
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CalificacionSemCartera";
                errEnt.Error = "MEN.0411";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }


        /// <summary>
        /// cambia la paginación del grid de clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvClientesRiesgos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // actualizar el indice del grid
                gvClientesRiesgos.PageIndex = e.NewPageIndex;

                List<ClienteMasivo> clientes = (List<ClienteMasivo>)ViewState["clienteMasivoBusqueda"];

                // realiza nuevamente la búsqueda
                gvClientesRiesgos.DataSource = clientes;//temp cargar clientes
                gvClientesRiesgos.DataBind();
                int limite = (((gvClientesRiesgos.PageIndex + 1) * gvClientesRiesgos.PageSize) > clientes.Count) ? clientes.Count : ((gvClientesRiesgos.PageIndex + 1) * gvClientesRiesgos.PageSize);
                lblCantidadClientesR.Text = "En pantalla se observan los clientes del " + (gvClientesRiesgos.PageIndex * gvClientesRiesgos.PageSize + 1) + " al " + limite.ToString() +
                        " de un total de " + clientes.Count + " clientes asignados, para observar más clientes por favor navegue entre las páginas";
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CalificacionSemCartera";
                errEnt.Error = "MEN.0411";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }



        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;

                return (SortDirection)ViewState["sortDirection"];
            }
            set { ViewState["sortDirection"] = value; }
        }

        public string GridViewSortField
        {
            get
            {
                if (ViewState["sortField"] == null)
                    ViewState["sortField"] = "nombre";

                return (string)ViewState["sortField"];
            }
            set { ViewState["sortField"] = value; }
        }

        protected void gvClientes_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<ClienteMasivo> cl = (List<ClienteMasivo>)ViewState["clienteMasivoBusqueda"];

            if (cl != null)
            {
                var param = Expression.Parameter(typeof(ClienteMasivo), e.SortExpression);
                var sortExpression = Expression.Lambda<Func<ClienteMasivo, object>>(Expression.Convert(Expression.Property(param, e.SortExpression), typeof(object)), param);

                if (!param.ToString().Equals(GridViewSortField))
                {
                    GridViewSortDirection = SortDirection.Ascending;
                }

                if (GridViewSortDirection == SortDirection.Ascending)
                {
                    gvClientes.DataSource = cl.AsQueryable<ClienteMasivo>().OrderBy(sortExpression).ToList();
                    ViewState["clienteMasivoBusqueda"] = cl.AsQueryable<ClienteMasivo>().OrderBy(sortExpression).ToList();
                    GridViewSortField = param.Name;
                    GridViewSortDirection = SortDirection.Descending;
                }
                else
                {
                    gvClientes.DataSource = cl.AsQueryable<ClienteMasivo>().OrderByDescending(sortExpression).ToList();
                    ViewState["clienteMasivoBusqueda"] = cl.AsQueryable<ClienteMasivo>().OrderByDescending(sortExpression).ToList();
                    GridViewSortField = param.Name;
                    GridViewSortDirection = SortDirection.Ascending;
                }

                gvClientes.DataBind();
            }

        }

        protected void gvClientesRiesgos_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<ClienteMasivo> cl = (List<ClienteMasivo>)ViewState["clienteMasivoBusqueda"];

            if (cl != null)
            {
                var param = Expression.Parameter(typeof(ClienteMasivo), e.SortExpression);
                var sortExpression = Expression.Lambda<Func<ClienteMasivo, object>>(Expression.Convert(Expression.Property(param, e.SortExpression), typeof(object)), param);

                if (!param.ToString().Equals(GridViewSortField))
                {
                    GridViewSortDirection = SortDirection.Ascending;
                }

                if (GridViewSortDirection == SortDirection.Ascending)
                {
                    gvClientesRiesgos.DataSource = cl.AsQueryable<ClienteMasivo>().OrderBy(sortExpression).ToList();
                    ViewState["clienteMasivoBusqueda"] = cl.AsQueryable<ClienteMasivo>().OrderBy(sortExpression).ToList();
                    GridViewSortField = param.Name;
                    GridViewSortDirection = SortDirection.Descending;
                }
                else
                {
                    gvClientesRiesgos.DataSource = cl.AsQueryable<ClienteMasivo>().OrderByDescending(sortExpression).ToList();
                    ViewState["clienteMasivoBusqueda"] = cl.AsQueryable<ClienteMasivo>().OrderByDescending(sortExpression).ToList();
                    GridViewSortField = param.Name;
                    GridViewSortDirection = SortDirection.Ascending;
                }

                gvClientesRiesgos.DataBind();
            }

        }

        /// <summary>
        /// dependiendo del rol que ingrese a la pantalla, pinta u oculta los páneles correspondientes a este
        /// </summary>
        protected void btnConsultarClienteMasAsesor_Click(string nitCliente)
        {
            try
            {
                if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
                {
                    lblUsuario.Text = ((Usuario)Session["Rol"]).usuario;
                    ObjetosCalificacion objCalificacion = new ObjetosCalificacion();
                    List<ClienteMasivo> clientes;
                    //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
                    objCalificacion = Fachada.CalificacionCartera.ConsultarClienteMasRiesgos(nitCliente, "10");

                    clientes = objCalificacion.clienteMasivo;

                    ClienteMasivo cliente = objCalificacion.clienteMasivo[0];

                    //Obtiene la información necesaria para luego usarla en la actualización del log.
                    var data = objCalificacion.clienteMasivo[0];
                    //Actualiza la tabla de logs insertando una nueva actividad de consulta.
                    if (data != null)
                        Fachada.CalificacionCartera.ActualizarLogCSC(data.fechaProceso, "11",
                            ((Usuario)Session["Rol"]).usuario, "", nitCliente, "", "", "", "");

                    bool deshabilitaCampos = false;

                    DateTime fechaComercial;
                    if (DateTime.TryParseExact(cliente.fecMaxComercial, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaComercial))
                    {
                        if (fechaComercial < DateTime.Now)
                        {
                            deshabilitaCampos = true;
                        }
                    }
                    if (!deshabilitaCampos)
                    {
                        ObjetosCalificacion objCalificacion2 = Fachada.CalificacionCartera.ConsultarClienteMasRiesgos(nitCliente, "20");

                        ////Obtiene la información necesaria para luego usarla en la actualización del log.
                        //var data2 = cliente;
                        ////Actualiza la tabla de logs insertando una nueva actividad de consulta.
                        //if (data2 != null)
                        //    proxyCalificacion.ActualizarLogCSC(data2.fechaProceso, "11",
                        //        ((Usuario)Session["Rol"]).usuario, "", nitCliente, "", "", "", "");

                        if (objCalificacion2.clienteMasivo.Count > 0)
                        {
                            ClienteMasivo cliente2 = objCalificacion2.clienteMasivo[0];
                            if (!cliente2.calInRatR.Trim().Equals(string.Empty) || !cliente2.calExRatR.Trim().Equals(string.Empty))
                            {
                                deshabilitaCampos = true;
                            }
                        }
                    }

                    if (deshabilitaCampos)
                    {
                        txtJustifiComercial.Disabled = true;
                        dplCalifInterComercial.Enabled = false;
                        dplCalifExterComercial.Enabled = false;
                        btnGrabar.Enabled = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ConsultarComercial", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0468").Texto + "')", true);
                    }
                    else
                    {
                        txtJustifiComercial.Disabled = false;
                        dplCalifInterComercial.Enabled = true;
                        dplCalifExterComercial.Enabled = true;
                        btnGrabar.Enabled = true;
                    }

                    lblIdCliente.Text = cliente.idCliente;
                    lblNombre.Text = cliente.nombreCliete;
                    lblEntidad.Text = cliente.entidad;
                    lblBanca.Text = cliente.banca;

                    lblCalifInterActual.Text = cliente.calIntAct;
                    lblCalifExterActual.Text = cliente.calExAct;
                    lblCalifInterRiesgo.Text = cliente.calInRecR;
                    lblCalifExterRiesgo.Text = cliente.calExRecR;

                    txtJustifiRiesgo.Value = cliente.custRatRie.Trim();
                    txtJustifiComercial.Value = cliente.justCalCom.Trim();
                    lblFechaProceso.Text = cliente.fechaProceso;

                    pnlCalMasivoComercial.Visible = true;
                    pnlCalMasivoRiesgos.Style["display"] = "none";
                    Parametros[] parametros = null;
                    if (Session["parametros"] != null)
                    {
                        parametros = (Parametros[])Session["parametros"];
                    }

                    IEnumerable<ListItem> CalifInterna = parametros.Where(p => p.paramName.Trim().Equals("LCALNR") && p.paramSeq != 0 && p.param6.Trim() != "0").OrderBy(p => p.paramSeq).Select(x =>
                                 new ListItem()
                                 {
                                     Text = x.param7.ToString().Trim()
                                 }).Distinct();
                    List<ListItem> CalifInternaList = CalifInterna.ToList();
                    CalifInternaList.Insert(0, new ListItem() { Text = "-- Seleccionar--" });


                    IEnumerable<ListItem> CalifExterna = parametros.Where(p => p.paramName.Trim().Equals("LCALEXT") && p.paramSeq != 0 && p.param6.Trim() != "0").OrderBy(p => p.paramSeq).Select(x =>
                                 new ListItem()
                                 {
                                     Text = x.param7.ToString().Trim()

                                 }).Distinct();

                    List<ListItem> CalifExternaList = CalifExterna.ToList();
                    CalifExternaList.Insert(0, new ListItem() { Text = "-- Seleccionar--" });

                    dplCalifInterComercial.AppendDataBoundItems = false;
                    //Cargar calificacion interna
                    dplCalifInterComercial.DataSource = CalifInternaList;
                    dplCalifInterComercial.DataBind();
                    if (!cliente.calInRecC.Trim().Equals(string.Empty))
                    {
                        try
                        {
                            dplCalifInterComercial.SelectedValue = cliente.calInRecC.Trim();
                        }
                        catch
                        {
                        }
                    }

                    dplCalifExterComercial.AppendDataBoundItems = false;
                    //Cargar calificacion extrena
                    dplCalifExterComercial.DataSource = CalifExternaList;
                    dplCalifExterComercial.DataBind();
                    if (!cliente.calExRecC.Trim().Equals(string.Empty))
                    {
                        try
                        {
                            dplCalifExterComercial.SelectedValue = cliente.calExRecC.Trim();
                        }
                        catch
                        {
                        }
                    }

                }

                if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC") || ((Usuario)Session["Rol"]).rol.Equals("Consulta") || ((Usuario)Session["Rol"]).rol.Equals("Superfinanciera"))
                {
                    lblUsuario.Text = ((Usuario)Session["Rol"]).usuario;
                    ObjetosCalificacion objCalificacion = new ObjetosCalificacion();
                    List<ClienteMasivo> clientes;
                    //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
                    objCalificacion = Fachada.CalificacionCartera.ConsultarClienteMasRiesgos(nitCliente, "20");
                    clientes = objCalificacion.clienteMasivo;

                    //Obtiene la información necesaria para luego usarla en la actualización del log.
                    var data = clientes[0];
                    //Actualiza la tabla de logs insertando una nueva actividad de consulta.
                    if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC"))
                    {
                        if (data != null)
                            Fachada.CalificacionCartera.ActualizarLogCSC(data.fechaProceso, "14",
                                ((Usuario)Session["Rol"]).usuario, "", nitCliente, "", "", "", "");
                    }
                    else if (((Usuario)Session["Rol"]).rol.Equals("Consulta"))
                    {
                        if (data != null)
                            Fachada.CalificacionCartera.ActualizarLogCSC(data.fechaProceso, "20",
                                ((Usuario)Session["Rol"]).usuario, "", nitCliente, "", "", "", "");
                    }
                    else if (((Usuario)Session["Rol"]).rol.Equals("Superfinanciera"))
                    {
                        if (data != null)
                            Fachada.CalificacionCartera.ActualizarLogCSC(data.fechaProceso, "17",
                                ((Usuario)Session["Rol"]).usuario, "", nitCliente, "", "", "", "");
                    }


                    lblIdClienteR.Text = objCalificacion.clienteMasivo[0].idCliente;
                    lblNombreR.Text = objCalificacion.clienteMasivo[0].nombreCliete;
                    lblEntidadR.Text = objCalificacion.clienteMasivo[0].entidad;
                    lblBancaR.Text = objCalificacion.clienteMasivo[0].banca;

                    lblUsuarioComercial.Text = objCalificacion.clienteMasivo[0].usrComercial;
                    lblUsuarioRiesgos.Text = objCalificacion.clienteMasivo[0].usrRiesgos;

                    lblCalifInterActualR.Text = objCalificacion.clienteMasivo[0].calIntAct;
                    lblCalifInterRiesgoR.Text = objCalificacion.clienteMasivo[0].calInRecR;
                    lblCalifInterComercial.Text = objCalificacion.clienteMasivo[0].calInRecC;

                    lblCalifExterActualR.Text = objCalificacion.clienteMasivo[0].calExAct;
                    lblCalifExterRiesgoR.Text = objCalificacion.clienteMasivo[0].calExRecR;
                    lblCalifExterComercial.Text = objCalificacion.clienteMasivo[0].calExRecC;

                    txtJustifiRiesgoR.Value = objCalificacion.clienteMasivo[0].custRatRie.Trim();
                    txtJustifiComercialR.Value = objCalificacion.clienteMasivo[0].justCalCom.Trim();
                    txtJustifiRatificada.Value = objCalificacion.clienteMasivo[0].justRatR.Trim();
                    lblFechaProcesoR.Text = objCalificacion.clienteMasivo[0].fechaProceso;

                    pnlCalMasivoComercial.Style["display"] = "none";
                    pnlCalMasivoRiesgos.Visible = true;

                    Parametros[] parametros = null;
                    if (Session["parametros"] != null)
                    {
                        parametros = (Parametros[])Session["parametros"];
                    }

                    IEnumerable<ListItem> CalifInterna = parametros.Where(p => p.paramName.Trim().Equals("LCALNR") && p.paramSeq != 0 && p.param6.Trim() != "0").OrderBy(p => p.paramSeq).Select(x =>
                                 new ListItem()
                                 {
                                     Text = x.param7.ToString().Trim()
                                 }).Distinct();
                    List<ListItem> CalifInternaList = CalifInterna.ToList();
                    CalifInternaList.Insert(0, new ListItem() { Text = "-- Seleccionar--" });


                    IEnumerable<ListItem> CalifExterna = parametros.Where(p => p.paramName.Trim().Equals("LCALEXT") && p.paramSeq != 0 && p.param6.Trim() != "0").OrderBy(p => p.paramSeq).Select(x =>
                                 new ListItem()
                                 {
                                     Text = x.param7.ToString().Trim()

                                 }).Distinct();

                    List<ListItem> CalifExternaList = CalifExterna.ToList();
                    CalifExternaList.Insert(0, new ListItem() { Text = "-- Seleccionar--" });

                    dplCalifInterRatificada.AppendDataBoundItems = false;
                    //Cargar calificacion interna
                    dplCalifInterRatificada.DataSource = CalifInternaList;
                    dplCalifInterRatificada.DataBind();
                    if (!objCalificacion.clienteMasivo[0].calInRatR.Trim().Equals(string.Empty))
                    {
                        dplCalifInterRatificada.SelectedValue = objCalificacion.clienteMasivo[0].calInRatR.Trim();
                    }

                    dplCalifExterRatificada.AppendDataBoundItems = false;
                    //Cargar calificacion extrena
                    dplCalifExterRatificada.DataSource = CalifExternaList;
                    dplCalifExterRatificada.DataBind();
                    if (!objCalificacion.clienteMasivo[0].calInRatR.Trim().Equals(string.Empty))
                    {
                        dplCalifExterRatificada.SelectedValue = objCalificacion.clienteMasivo[0].calExRatR.Trim();
                    }

                }

            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CalificacionMasCartera";
                if (ex.Message.ToString().Contains("ERRORSP@"))
                    errEnt.Error = "ERRORSP@";
                else
                    errEnt.Error = "MEN.0411";
                Session["Error"] = errEnt;
                Response.Redirect("Errores/Error.aspx");
            }
        }


        /// <summary>
        /// Guarda la información ingresada del cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnValidarGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Parametros[] param = (Parametros[])Session["parametros"];

                string calificacionInternaRNRSel = dplCalifInterRatificada.SelectedValue;

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

                string calificacionExternaSel = dplCalifExterRatificada.SelectedValue;
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
                    btnGrabarR_Click(sender, e);
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
        /// Guarda la información  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            Respuesta respuesta = new Respuesta();
            ClienteMasivo peticion = new ClienteMasivo();

            peticion.usrComercial = ((Usuario)Session["Rol"]).usuario;
            peticion.idOperacion = "11";
            peticion.idCliente = lblIdCliente.Text;
            peticion.calInRecC = dplCalifInterComercial.SelectedValue;
            peticion.calExRecC = dplCalifExterComercial.SelectedValue;
            peticion.justCalCom = txtJustifiComercial.Value.Trim();
            peticion.fechaProceso = lblFechaProceso.Text.Trim();
            peticion.retAuto = "NO";

            if (dplCalifInterComercial.SelectedIndex == 0 || dplCalifInterComercial.SelectedIndex == 0
                || peticion.justCalCom.Equals(string.Empty))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "GrabarComercial", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0467").Texto + "')", true);
            }
            else
            {
                //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
                respuesta = Fachada.CalificacionCartera.CalificarMasComercial(peticion);
                if (respuesta.codResp == "0000")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "GrabarComercial", "alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0469").Texto + "');location.reload();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "GrabarComercial", "alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0470").Texto + "');", true);
                }
            }

        }

        /// <summary>
        /// Guarda la información  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGrabarR_Click(object sender, EventArgs e)
        {
            Respuesta respuesta = new Respuesta();
            ClienteMasivo peticion = new ClienteMasivo();

            peticion.usrComercial = ((Usuario)Session["Rol"]).usuario;
            peticion.idOperacion = "21";
            peticion.idCliente = lblIdClienteR.Text.Trim();
            peticion.calInRatR = dplCalifInterRatificada.SelectedValue;
            peticion.calExRatR = dplCalifExterRatificada.SelectedValue;
            peticion.justRatR = txtJustifiRatificada.Value.Trim();
            peticion.fechaProceso = lblFechaProcesoR.Text.Trim();

            if (dplCalifInterRatificada.SelectedIndex == 0 || dplCalifExterRatificada.SelectedIndex == 0
                || peticion.justRatR.Equals(string.Empty))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "GrabarRiesgos", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0467").Texto + "')", true);
            }
            else
            {
                //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
                respuesta = Fachada.CalificacionCartera.CalificarMasRiesgos(peticion);
                if (respuesta.codResp == "0000")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "GrabarComercial", "alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0469").Texto + "');location.reload();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "GrabarComercial", "alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0470").Texto + "');", true);
                }
            }
        }

        /// <summary>
        /// Retorna a la lista de clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Response.Redirect("CalificacionMasCartera.aspx");
        }

        /// <summary>
        /// Generar PDF 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            var usrSession = (Usuario)Session["Rol"];
            //Switch depending on Role
            switch (usrSession.rol)
            {
                case "Comercial":
                    Response.Redirect("Reporte/PDFMasivo.aspx?Cliente=" + lblIdCliente.Text.Trim());
                    break;
                case "Riesgos - PIC":
                case "Consulta":
                case "Superfinanciera":
                    Response.Redirect("Reporte/PDFMasivo.aspx?Cliente=" + lblIdClienteR.Text.Trim());
                    break;
                default:
                    break;
            }

        }


        /// <summary>
        /// Realiza un enconde de la url para el hyperlink
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyname"></param>
        /// <returns></returns>
        public void GetUrl(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            switch (btn.CommandName)
            {
                case "Comercial":
                    mpePnlCalMasivoCom.Show();
                    break;
                case "Riesgos":
                    mpePnlCalMasivoRies.Show();
                    break;
            }
            btnConsultarClienteMasAsesor_Click(btn.CommandArgument.ToString().Trim());
        }

        /// <summary>
        /// Realiza un enconde de la url para el linkButton.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyname"></param>
        /// <returns></returns>
        //public void GetUrlLinkButton(object sender, EventArgs e)
        //{
        //    LinkButton lnkBtn = (LinkButton)sender;

        //    switch (lnkBtn.CommandName)
        //    {
        //        case "Comercial":
        //            mpePnlCalMasivoCom.Show();
        //            break;
        //        case "Riesgos":
        //            mpePnlCalMasivoRies.Show();
        //            break;
        //    }
        //    try
        //    {
        //        BuscarClienteMasivo(Convert.ToInt32(lnkBtn.CommandArgument));
        //    }
        //    catch (Exception)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0398").Texto + "')", true);
        //    }
        //}

        protected void btnCancelarGrabarR_Click(object sender, EventArgs e)
        {
            mpePnlCalMasivoRies.Show();
        }
        /// <summary>
        /// RF047 CHANGE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarClienteMasivo_Click(object sender, EventArgs e)
        {
            Int64 nitCliMasivo;
            bool isNumeric = Int64.TryParse(txtNitCliMasivo.Text.Trim(), out nitCliMasivo);

            if (txtNitCliMasivo.Text.Trim().Equals(string.Empty) || !isNumeric)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0398").Texto + "')", true);
            }
            else
            {
                try
                {
                    lblUsuario.Text = ((Usuario)Session["Rol"]).usuario;
                    ObjetosCalificacion objCalificacion = new ObjetosCalificacion();
                    

                    //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
                    objCalificacion = Fachada.CalificacionCartera.ConsultarClienteMasRiesgos(txtNitCliMasivo.Text, "30");

                    //Obtiene la información necesaria para luego usarla en la actualización del log.
                    var data = objCalificacion.clienteMasivo[0];
                    //Actualiza la tabla de logs insertando una nueva actividad de consulta.
                    if (((Usuario)Session["Rol"]).rol.Equals("Superfinanciera"))
                    {
                        if (data != null)
                            Fachada.CalificacionCartera.ActualizarLogCSC(data.fechaProceso, "17",
                                ((Usuario)Session["Rol"]).usuario, "", txtNitCliMasivo.Text, "", "", "", "");
                    }
                    else if (((Usuario)Session["Rol"]).rol.Equals("Consulta"))
                    {
                        if (data != null)
                            Fachada.CalificacionCartera.ActualizarLogCSC(data.fechaProceso, "20",
                                ((Usuario)Session["Rol"]).usuario, "", txtNitCliMasivo.Text, "", "", "", "");
                    }


                    if (objCalificacion.clienteMasivo == null || objCalificacion.clienteMasivo.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0412").Texto + "')", true);
                        //Mensajes.Mensajes.MostrarMensaje("MAF", "MEN.0412", this);
                    }
                    else
                    {
                        lblIdClienteR2.Text = objCalificacion.clienteMasivo[0].idCliente;
                        lblNombreR2.Text = objCalificacion.clienteMasivo[0].nombreCliete;
                        lblEntidadR2.Text = objCalificacion.clienteMasivo[0].entidad;
                        lblBancaR2.Text = objCalificacion.clienteMasivo[0].banca;
                        lblCalifInterActualR2.Text = objCalificacion.clienteMasivo[0].calIntAct; //Calificación Interna Actual
                        lblCalifInterRiesgoR2.Text = objCalificacion.clienteMasivo[0].calInRecR; //Calificación Interna Recomendada Riesgos
                        lblCalifInterComercial2.Text = objCalificacion.clienteMasivo[0].calInRecC; //Calificación Interna Recomendada Comercial

                        lblCalifExterActualR2.Text = objCalificacion.clienteMasivo[0].calExAct; //Calificación Externa Actual
                        lblCalifExterRiesgoR2.Text = objCalificacion.clienteMasivo[0].calExRecR; //Calificación Externa Recomendada Riesgos
                        lblCalifExterComercial2.Text = objCalificacion.clienteMasivo[0].calExRecC; //Calificación Externa Recomendada Comercial

                        txtJustifiRiesgoR2.Value = objCalificacion.clienteMasivo[0].custRatRie.Trim(); //Justificacion Calificación Recomendada Riesgos
                        txtJustifiComercialR2.Value = objCalificacion.clienteMasivo[0].justCalCom.Trim(); //Justificacion Calificación Recomendada Riesgos

                        if (((Usuario)Session["Rol"]).rol.Equals("Superfinanciera") || ((Usuario)Session["Rol"]).rol.Equals("Consulta"))
                        {
                            lblUsuarioComercial2.Text = objCalificacion.clienteMasivo[0].usrComercial; //Usuario Comercial
                            lblUsuarioRiesgos2.Text = objCalificacion.clienteMasivo[0].usrRiesgos; //Usuario Riesgos
                            txtJustifiRatificada2.Value = objCalificacion.clienteMasivo[0].justRatR.Trim(); //Justificacion Calificación Ratificada
                            lblCalifInterRatificada2.Text = objCalificacion.clienteMasivo[0].calInRatR.Trim();
                            lblCalifExterRatificada2.Text = objCalificacion.clienteMasivo[0].calExRatR.Trim();
                        }
                        lblFechaProcesoR2.Text = objCalificacion.clienteMasivo[0].fechaProceso;

                        mpePnlCliMasivo.Show();
                        pnlClienteMasivo.Visible = true;
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
                        errEnt.Error = "MEN.0411";
                    Session["Error"] = errEnt;
                    Response.Redirect("Errores/Error.aspx");
                }
            }


        }

        /// <summary>
        /// Consulta clientes de la opción Masivo.
        /// </summary>
        /// <param name="codeNumber"></param>
        //private void BuscarClienteMasivo(int codeNumber)
        //{
        //    try
        //    {
        //        lblUsuario.Text = ((Usuario)Session["Rol"]).usuario;
        //        ObjetosCalificacion objCalificacion = new ObjetosCalificacion();
        //        List<ClienteMasivo> clientes;
        //        Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
        //        objCalificacion = proxyCalificacion.ConsultarClienteMasRiesgos(codeNumber.ToString(), "30");

        //        //Obtiene la información necesaria para luego usarla en la actualización del log.
        //        var data = objCalificacion.clienteMasivo[0];
        //        //Actualiza la tabla de logs insertando una nueva actividad de consulta.
        //        if (((Usuario)Session["Rol"]).rol.Equals("Superfinanciera"))
        //        {
        //            if (data != null)
        //                proxyCalificacion.ActualizarLogCSC(data.fechaProceso, "17",
        //                    ((Usuario)Session["Rol"]).usuario, "", codeNumber.ToString(), "", "", "", "");
        //        }
        //        else if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC"))
        //        {
        //            if (data != null)
        //                proxyCalificacion.ActualizarLogCSC(data.fechaProceso, "14",
        //                    ((Usuario)Session["Rol"]).usuario, "", codeNumber.ToString(), "", "", "", "");
        //        }
        //        else if (((Usuario)Session["Rol"]).rol.Equals("Comercial"))
        //        {
        //            if (data != null)
        //                proxyCalificacion.ActualizarLogCSC(data.fechaProceso, "11",
        //                    ((Usuario)Session["Rol"]).usuario, "", codeNumber.ToString(), "", "", "", "");
        //        }


        //        if (objCalificacion.clienteMasivo == null || objCalificacion.clienteMasivo.Count == 0)
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0412").Texto + "')", true);
        //            //Mensajes.Mensajes.MostrarMensaje("MAF", "MEN.0412", this);
        //        }
        //        else
        //        {
        //            lblIdClienteR2.Text = objCalificacion.clienteMasivo[0].idCliente;
        //            lblNombreR2.Text = objCalificacion.clienteMasivo[0].nombreCliete;
        //            lblEntidadR2.Text = objCalificacion.clienteMasivo[0].entidad;
        //            lblBancaR2.Text = objCalificacion.clienteMasivo[0].banca;
        //            lblCalifInterActualR2.Text = objCalificacion.clienteMasivo[0].calIntAct; //Calificación Interna Actual
        //            lblCalifInterRiesgoR2.Text = objCalificacion.clienteMasivo[0].calInRecR; //Calificación Interna Recomendada Riesgos
        //            lblCalifInterComercial2.Text = objCalificacion.clienteMasivo[0].calInRecC; //Calificación Interna Recomendada Comercial

        //            lblCalifExterActualR2.Text = objCalificacion.clienteMasivo[0].calExAct; //Calificación Externa Actual
        //            lblCalifExterRiesgoR2.Text = objCalificacion.clienteMasivo[0].calExRecR; //Calificación Externa Recomendada Riesgos
        //            lblCalifExterComercial2.Text = objCalificacion.clienteMasivo[0].calExRecC; //Calificación Externa Recomendada Comercial

        //            txtJustifiRiesgoR2.Value = objCalificacion.clienteMasivo[0].custRatRie.Trim(); //Justificacion Calificación Recomendada Riesgos
        //            txtJustifiComercialR2.Value = objCalificacion.clienteMasivo[0].justCalCom.Trim(); //Justificacion Calificación Recomendada Riesgos

        //            if (((Usuario)Session["Rol"]).rol.Equals("Superfinanciera") || ((Usuario)Session["Rol"]).rol.Equals("Consulta"))
        //            {
        //                lblUsuarioComercial2.Text = objCalificacion.clienteMasivo[0].usrComercial; //Usuario Comercial
        //                lblUsuarioRiesgos2.Text = objCalificacion.clienteMasivo[0].usrRiesgos; //Usuario Riesgos
        //                txtJustifiRatificada2.Value = objCalificacion.clienteMasivo[0].justRatR.Trim(); //Justificacion Calificación Ratificada
        //                lblCalifInterRatificada2.Text = objCalificacion.clienteMasivo[0].calInRatR.Trim();
        //                lblCalifExterRatificada2.Text = objCalificacion.clienteMasivo[0].calExRatR.Trim();
        //            }
        //            lblFechaProcesoR2.Text = objCalificacion.clienteMasivo[0].fechaProceso;

        //            mpePnlCliMasivo.Show();
        //            pnlClienteMasivo.Visible = true;
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        ErroresEntity errEnt = new ErroresEntity();
        //        errEnt.Url = Request.UrlReferrer.PathAndQuery;
        //        errEnt.Log = ex.Message + "//ApplicationPage:CalificacionSemCartera";
        //        if (ex.Message.ToString().Contains("ERRORSP@"))
        //            errEnt.Error = "ERRORSP@";
        //        else
        //            errEnt.Error = "MEN.0411";
        //        Session["Error"] = errEnt;
        //        Response.Redirect("Errores/Error.aspx");
        //    }
        //}

        /// <summary>
        /// RF047 CHANGE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdfR2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Reporte/PDFMasivo.aspx?Cliente=" + lblIdClienteR2.Text.Trim());

        }
        /// <summary>
        /// RF047 CHANGE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bntCancelarR2_Click(object sender, EventArgs e)
        {
            Response.Redirect("CalificacionMasCartera.aspx");
        }

    }
}
