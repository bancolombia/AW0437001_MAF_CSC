using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Bancolombia.Riesgo.MAFWeb.Entidades.CalificacionCartera;
//using Proxy;
using Bancolombia.Riesgo.MAF.Entidades.CalificacionCartera;
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Fachada = Bancolombia.Riesgo.MAF.Negocio.Fachadas.CalificacionCartera;

namespace Bancolombia.Riesgo.MAFWeb.CalificacionCartera
{
    public partial class CalificacionSemCartera : System.Web.UI.Page
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
                    lblIntroduccion.Text = "Por favor ingrese sus comentarios para los siguientes clientes:";
                    pnlRPySup.Visible = false;
                    pnlFiltros.Visible = false;
                    lblUsuario.Text = ((Usuario)Session["Rol"]).usuario;
                    ObjetosCalificacion objCalificacion = new ObjetosCalificacion();
                    List<Cliente> clientes;
                    //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
                    objCalificacion = Fachada.CalificacionCartera.ConsultarClientesAsesor(((Usuario)Session["Rol"]).usuario, "1", "P");
                    clientes = objCalificacion.cliente;
                    ViewState["ClientesBusqueda"] = objCalificacion.cliente;
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
                else if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC") || ((Usuario)Session["Rol"]).rol.Equals("Consulta"))
                {
                    Master.mpTitulo = string.Format("Calificación Semestral de Cartera - {0}", ((Usuario)Session["Rol"]).rol);
                    pnlComercial.Visible = false;
                    pnlRPySup.Visible = true;
                    pnlFiltros.Visible = true;
                    lblIntroduccion.Text = "Por favor digite la identificación del cliente a consultar, el código del gerente o seleccione los filtros deseados:";
                    lblUsuario.Text = ((Usuario)Session["Rol"]).usuario;

                    //llenar listas
                    //Proxy.CalificacionCartera cartera = new Proxy.CalificacionCartera();

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


                }
                else if (((Usuario)Session["Rol"]).rol.Equals("Superfinanciera"))
                {
                    Master.mpTitulo = "Calificación Semestral de Cartera - Superintendencia Financiera";
                    pnlComercial.Visible = false;
                    pnlRPySup.Visible = true;
                    pnlFiltros.Visible = false;
                    lblIntroduccion.Text = "Por favor digite la identificación del cliente a consultar:";
                    lblUsuario.Text = ((Usuario)Session["Rol"]).usuario;
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

        /// <summary>
        /// Carga los clientes temporales
        /// </summary>
        protected List<Cliente> tempCargarClientes()
        {

            List<Cliente> clientes = new List<Cliente>();

            for (int i = 0; i < 15; i++)
            {
                Cliente cl = new Cliente();
                cl.nit = "00" + i.ToString();
                cl.nombre = "Cliente" + i.ToString();
                cl.banca = "PERSONAS Y PYMES" + i.ToString();
                cl.zona = "ZONA 3 ZONA GBNO E INSTITUCIONAL BCA EMP BGTA" + i.ToString();
                cl.entidad = "LEASING INTERNACIONAL" + i.ToString();
                cl.regional = "CONSTRUCTOR OTRAS CIUDADES" + i.ToString();

                clientes.Add(cl);
            }
            return clientes;


        }


        /// <summary>
        /// Consulta los clientes por el asesor
        /// </summary>
        /// <returns></returns>
        protected List<Cliente> CargarClientes()
        {
            ObjetosCalificacion objCalificacion = new ObjetosCalificacion();
            List<Cliente> cliente;
            //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
            try
            {
                objCalificacion = Fachada.CalificacionCartera.ConsultarClientesAsesor(((Usuario)Session["Rol"]).usuario, "1", "P");
                cliente = objCalificacion.cliente;
                ViewState["ClientesBusqueda"] = objCalificacion.cliente;
                if (!objCalificacion.codigo.Equals("n") && !objCalificacion.codigo.Equals("0000"))
                    throw new Exception("ERRORSP@" + objCalificacion.codigo + "/" + objCalificacion.descripcion);
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
                cliente = null;
            }
            return cliente;
        }

        protected List<Cliente> CargarClientesFiltros()
        {
            ObjetosCalificacion objCalificacion = new ObjetosCalificacion();
            List<Cliente> cliente;
            //Proxy.CalificacionCartera proxyCalificacion = new Proxy.CalificacionCartera();
            try
            {
                objCalificacion = Fachada.CalificacionCartera.ConsultarClientesAsesor(((Usuario)Session["Rol"]).usuario, "1", "P");
                cliente = objCalificacion.cliente;
                if (!objCalificacion.codigo.Equals("n") && !objCalificacion.codigo.Equals("0000"))
                    throw new Exception("ERRORSP@" + objCalificacion.codigo + "/" + objCalificacion.descripcion);
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
                cliente = null;
            }
            return cliente;
        }

        /// <summary>
        /// realiza la búsqueda de los clientes para riesgos/PIC y superfinanciera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string categoriaCliente = null;
            if (txtCodigoCliente.Text.Trim().Equals(string.Empty))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0398").Texto + "')", true);
            }
            else
            {
                string codigo = txtCodigoCliente.Text.Trim();
                DataSet dsCliente = null;
                ObjetosCalificacion objCalificacion = new ObjetosCalificacion();
                try
                {
                    string rol = " ";
                    if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC"))
                        rol = "20";
                    else
                        rol = "30";

                    //Verificar Existencia del cliente
                    //.CalificacionCartera proxyCartera = new Proxy.CalificacionCartera();
                    objCalificacion = Fachada.CalificacionCartera.ConsultarCliente(rol, codigo);
                    categoriaCliente = objCalificacion.objCliente.categoriaCliente;

                    if (!objCalificacion.codigo.Equals("n") && !objCalificacion.codigo.Equals("0000"))
                        throw new Exception("ERRORSP@" + objCalificacion.codigo + "/" + objCalificacion.descripcion);

                    
                }
                catch (Exception ex)
                {
                    ErroresEntity errEnt = new ErroresEntity();
                    errEnt.Url = Request.UrlReferrer.PathAndQuery;
                    errEnt.Log = ex.Message + "//ApplicationPage:CalificacionSemCartera";
                    if (ex.Message.ToString().Contains("ERRORSP@"))
                        errEnt.Error = "ERRORSP@";
                    else
                        errEnt.Error = "MEN.0409";
                    Session["Error"] = errEnt;
                    Response.Redirect("Errores/Error.aspx");
                }
                if (objCalificacion.setDatos == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0412").Texto + "')", true);
                }
                else
                {
                    if (categoriaCliente != null)
                    {
                        if (categoriaCliente.Equals("FI"))
                        {
                            Response.Redirect("CalificacionClienteFinm.aspx?Codigo=" + HttpUtility.UrlEncode(codigo));
                        }

                        else if (categoriaCliente.Equals("GB"))
                        {
                            Response.Redirect("CalificacionClienteGob.aspx?Codigo=" + HttpUtility.UrlEncode(codigo));
                        }
                        else
                        {
                            Response.Redirect("CalificacionCliente.aspx?Codigo=" + HttpUtility.UrlEncode(codigo));
                        }
                    }
                    else
                    {
                        Response.Redirect("CalificacionCliente.aspx?Codigo=" + HttpUtility.UrlEncode(codigo));
                    }

                }
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

                List<Cliente> clientes = (List<Cliente>)ViewState["ClientesBusqueda"];

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
        /// Realiza la búsqueda por los filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (ddlBanca.SelectedValue.Equals("-1") && ddlEntidad.SelectedValue.Equals("-1") && ddlRegion.SelectedValue.Equals("-1") && ddlZona.SelectedValue.Equals("-1"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0443").Texto + "')", true);
            }
            else
            {
                try
                {
                    gvClientes2.DataSource = null;
                    gvClientes2.DataBind();
                    lblMensBusqueda.Text = "";
                    //proxy
                    //Proxy.CalificacionCartera proxyCartera = new Proxy.CalificacionCartera();
                    ObjetosCalificacion objCalificacion;
                    string entidad = string.Empty;
                    string banca = string.Empty;
                    string region = string.Empty;
                    string zona = string.Empty;

                    entidad = (ddlEntidad.SelectedValue.Equals("-1") ? " " : ddlEntidad.SelectedValue);
                    banca = (ddlBanca.SelectedValue.Equals("-1") ? " " : ddlBanca.SelectedValue);
                    region = (ddlRegion.SelectedValue.Equals("-1") ? " " : ddlRegion.SelectedValue);
                    zona = (ddlZona.SelectedValue.Equals("-1") ? " " : ddlZona.SelectedValue);
                    string rol = " ";
                    if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC"))
                        rol = "20";
                    else
                        rol = "30";
                    objCalificacion = Fachada.CalificacionCartera.ConsultarClientesFiltros(rol, entidad, banca, region, zona);
                    if (!objCalificacion.codigo.Equals("n") && !objCalificacion.codigo.Equals("0000"))
                        throw new Exception("ERRORSP@" + objCalificacion.codigo + "/" + objCalificacion.descripcion);

                    List<Cliente> clientes = objCalificacion.cliente;
                    if (clientes.Count > 0)
                    {
                        ViewState["ClientesBusqueda"] = clientes;
                        lblIntroduccion.Visible = true;
                        gvClientes2.DataSource = clientes;
                        gvClientes2.DataBind();
                        int limite = (((gvClientes2.PageIndex + 1) * gvClientes2.PageSize) > clientes.Count) ? clientes.Count : ((gvClientes2.PageIndex + 1) * gvClientes2.PageSize);
                        if (clientes.Count < gvClientes2.PageSize)
                            lblMensBusqueda.Text = "En pantalla se observan los clientes del " + (gvClientes2.PageIndex + 1) + " al " + limite.ToString() +
                                " de un total de " + clientes.Count + " clientes encontrados";
                        else
                            lblMensBusqueda.Text = "En pantalla se observan los clientes del " + (gvClientes2.PageIndex + 1) + " al " + limite.ToString() +
                                                " de un total de " + clientes.Count + " clientes encontrados, para observar más clientes por favor navegue entre las páginas";
                    }
                    else
                    {
                        lblIntroduccion.Visible = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0451").Texto + "')", true);
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
                        errEnt.Error = "MEN.0452";
                    Session["Error"] = errEnt;
                    Response.Redirect("Errores/Error.aspx");
                }

            }
        }

        protected void btnBuscarGte_Click(object sender, EventArgs e)
        {
            if (txtCodGte.Text.Equals(string.Empty))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0449").Texto + "')", true);
            }
            else
            {
                //proxy
                //Proxy.CalificacionCartera proxyCartera = new Proxy.CalificacionCartera();
                ObjetosCalificacion objCalificacion;

                try
                {
                    gvClientes2.DataSource = null;
                    gvClientes2.DataBind();
                    lblMensBusqueda.Text = "";
                    string rol = " ";
                    if (((Usuario)Session["Rol"]).rol.Equals("Riesgos - PIC"))
                        rol = "20";
                    else
                        rol = "30";
                    objCalificacion = Fachada.CalificacionCartera.ConsultarClientesGerente(rol, txtCodGte.Text.Trim());

                    if (!objCalificacion.codigo.Equals("n") && !objCalificacion.codigo.Equals("0000"))
                        throw new Exception("ERRORSP@" + objCalificacion.codigo + "/" + objCalificacion.descripcion);

                    List<Cliente> clientes = objCalificacion.cliente;
                    if (clientes.Count > 0)
                    {
                        ViewState["ClientesBusqueda"] = clientes;
                        lblIntroduccion.Visible = true;
                        //gvClientes.DataSource = dtClientes;
                        gvClientes2.DataSource = clientes;
                        gvClientes2.DataBind();
                        int limite = (((gvClientes2.PageIndex + 1) * gvClientes2.PageSize) > clientes.Count) ? clientes.Count : ((gvClientes2.PageIndex + 1) * gvClientes2.PageSize);
                        if (clientes.Count < gvClientes2.PageSize)
                            lblMensBusqueda.Text = "En pantalla se observan los clientes del " + (gvClientes2.PageIndex + 1) + " al " + limite.ToString() +
                                " de un total de " + clientes.Count + " clientes encontrados";
                        else
                            lblMensBusqueda.Text = "En pantalla se observan los clientes del " + (gvClientes2.PageIndex + 1) + " al " + limite.ToString() +
                                                " de un total de " + clientes.Count + " clientes encontrados, para observar más clientes por favor navegue entre las páginas";
                    }
                    else
                    {
                        lblIntroduccion.Visible = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", "javascript:alert('" + Mensajes.Mensajes.ObtenerMensaje("MAF", "MEN.0450").Texto + "')", true);
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
                        errEnt.Error = "MEN.0452";
                    Session["Error"] = errEnt;
                    Response.Redirect("Errores/Error.aspx");
                }
            }
        }

        protected void gvClientes2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // actualizar el indice del grid
                gvClientes2.PageIndex = e.NewPageIndex;

                List<Cliente> clientes = (List<Cliente>)ViewState["ClientesBusqueda"];

                if (clientes.Count > 0)
                {

                    // realiza nuevamente la búsqueda
                    gvClientes2.DataSource = clientes;//temp cargar clientes
                    gvClientes2.DataBind();
                    int limite = (((gvClientes2.PageIndex + 1) * gvClientes2.PageSize) > clientes.Count) ? clientes.Count : ((gvClientes2.PageIndex + 1) * gvClientes2.PageSize);
                    lblMensBusqueda.Text = "En pantalla se observan los clientes del " + (gvClientes2.PageIndex * gvClientes2.PageSize + 1) + " al " + limite.ToString() +
                            " de un total de " + clientes.Count + " clientes encontrados, para observar más clientes por favor navegue entre las páginas";
                }
            }
            catch (Exception ex)
            {
                ErroresEntity errEnt = new ErroresEntity();
                errEnt.Url = Request.UrlReferrer.PathAndQuery;
                errEnt.Log = ex.Message + "//ApplicationPage:CalificacionSemCartera";
                errEnt.Error = "MEN.0452";
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
            List<Cliente> cl = (List<Cliente>)ViewState["ClientesBusqueda"];

            if (cl != null)
            {
                var param = Expression.Parameter(typeof(Cliente), e.SortExpression);
                var sortExpression = Expression.Lambda<Func<Cliente, object>>(Expression.Convert(Expression.Property(param, e.SortExpression), typeof(object)), param);

                if (!param.ToString().Equals(GridViewSortField))
                {
                    GridViewSortDirection = SortDirection.Ascending;
                }

                if (GridViewSortDirection == SortDirection.Ascending)
                {
                    gvClientes.DataSource = cl.AsQueryable<Cliente>().OrderBy(sortExpression).ToList();
                    ViewState["ClientesBusqueda"] = cl.AsQueryable<Cliente>().OrderBy(sortExpression).ToList();
                    GridViewSortField = param.Name;
                    GridViewSortDirection = SortDirection.Descending;
                }
                else
                {
                    gvClientes.DataSource = cl.AsQueryable<Cliente>().OrderByDescending(sortExpression).ToList();
                    ViewState["ClientesBusqueda"] = cl.AsQueryable<Cliente>().OrderByDescending(sortExpression).ToList();
                    GridViewSortField = param.Name;
                    GridViewSortDirection = SortDirection.Ascending;
                }

                gvClientes.DataBind();
            }

        }

        protected void gvClientes2_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<Cliente> cl = (List<Cliente>)ViewState["ClientesBusqueda"];

            if (cl != null)
            {
                var param = Expression.Parameter(typeof(Cliente), e.SortExpression);
                var sortExpression = Expression.Lambda<Func<Cliente, object>>(Expression.Convert(Expression.Property(param, e.SortExpression), typeof(object)), param);

                if (!param.ToString().Equals(GridViewSortField))
                {
                    GridViewSortDirection = SortDirection.Ascending;
                }

                if (GridViewSortDirection == SortDirection.Ascending)
                {
                    gvClientes2.DataSource = cl.AsQueryable<Cliente>().OrderBy(sortExpression).ToList();
                    ViewState["ClientesBusqueda"] = cl.AsQueryable<Cliente>().OrderBy(sortExpression).ToList();
                    GridViewSortField = param.Name;
                    GridViewSortDirection = SortDirection.Descending;
                }
                else
                {
                    gvClientes2.DataSource = cl.AsQueryable<Cliente>().OrderByDescending(sortExpression).ToList();
                    ViewState["ClientesBusqueda"] = cl.AsQueryable<Cliente>().OrderByDescending(sortExpression).ToList();
                    GridViewSortField = param.Name;
                    GridViewSortDirection = SortDirection.Ascending;
                };


                gvClientes2.DataBind();
            }
        }

        /// <summary>
        /// Realiza un enconde de la url para el hyperlink
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyname"></param>
        /// <returns></returns>
        public string GetUrl(object codigo, object categoriaCliente = null)
        {
            string url = string.Empty;
            if (categoriaCliente != null)
            {
                if (categoriaCliente.Equals("FI"))
                {
                    url = "CalificacionClienteFinm.aspx?Codigo=" + HttpUtility.UrlEncode(codigo.ToString());
                }

                else if (categoriaCliente.Equals("GB"))
                {
                    url = "CalificacionClienteGob.aspx?Codigo=" + HttpUtility.UrlEncode(codigo.ToString());
                }

                else
                {
                    url = "CalificacionCliente.aspx?Codigo=" + HttpUtility.UrlEncode(codigo.ToString());
                }
            }
            else
            {
                url = "CalificacionCliente.aspx?Codigo=" + HttpUtility.UrlEncode(codigo.ToString());
            }


            return url;
        }
    }
}
