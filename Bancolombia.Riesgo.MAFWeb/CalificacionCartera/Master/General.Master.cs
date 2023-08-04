using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Bancolombia.Riesgo.MAF.Entidades;
using Bancolombia.Riesgo.MAFWeb.Entidades;
using Bancolombia.Riesgo.MAFWeb.Entidades.CalificacionCartera;
using Fachada = Bancolombia.Riesgo.MAF.Negocio.Fachadas.CalificacionCartera;

namespace Bancolombia.Riesgo.MAFWeb.CalificacionCartera.Master
{
    public partial class General : System.Web.UI.MasterPage
    {

        public string mpTitulo
        {
            get { return lblTitulo.Text; }
            set { lblTitulo.Text = value; }
        }

        public string mpRol
        {
            get { return lblUsuario.Text; }
            set { lblUsuario.Text = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Page.Header.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["Rol"] == null)
            {
                Response.Redirect("Login.aspx");
                Response.End();
            }
            else
            {
                if (!IsPostBack)
                {

                    Analista analista = (Analista)Session["_an"];

                    lblUsuario.Text = ((Usuario)Session["Rol"]).rol;
                    DataTable dtMenu = determinarMenu();
                    if (lblUsuario.Text.Equals("Admin"))
                    {
                        DataRow drMenu = dtMenu.NewRow();
                        drMenu["Nombre"] = "Cargar Clientes";
                        drMenu["Url"] = "../Cargar.aspx";

                        DataRow drMenu1 = dtMenu.NewRow();
                        drMenu1["Nombre"] = "Exportar Calificación";
                        drMenu1["Url"] = "../ExportarCalificacion.aspx";

                        DataRow drMenu2 = dtMenu.NewRow();
                        drMenu2["Nombre"] = "Parametrizar Aplicación";
                        drMenu2["Url"] = "../Parametrizacion.aspx";

                        DataRow drMenu3 = dtMenu.NewRow();
                        drMenu3["Nombre"] = "Otros Procesos";
                        drMenu3["Url"] = "../OtrosProcesos.aspx";

                        dtMenu.Rows.Add(drMenu);
                        dtMenu.Rows.Add(drMenu1);
                        dtMenu.Rows.Add(drMenu2);
                        dtMenu.Rows.Add(drMenu3);

                       
                        lblPeriodos.Visible = false;
                        lsvPeriodos.Visible = false;
                    }
                    else if (lblUsuario.Text.Equals("Superfinanciera") || lblUsuario.Text.Equals("Consulta"))
                    {
                        DataRow drMenu = dtMenu.NewRow();
                        drMenu["Nombre"] = "Presencial";
                        drMenu["Url"] = "../CalificacionSemCartera.aspx";
                        dtMenu.Rows.Add(drMenu);
                        drMenu = dtMenu.NewRow();
                        drMenu["Nombre"] = "Masivo";
                        drMenu["Url"] = "../CalificacionMasCartera.aspx";
                        dtMenu.Rows.Add(drMenu);
                        drMenu = dtMenu.NewRow();
                        drMenu["Nombre"] = "Generar Reporte";
                        drMenu["Url"] = "../Reporte/GenerarReporte.aspx";
                        dtMenu.Rows.Add(drMenu);

                    }
                    else if (lblUsuario.Text.Equals("Riesgos - PIC"))
                    {
                        ValidarPerfilCalificacion(analista, dtMenu);

                        DataRow drMenu = dtMenu.NewRow();
                        drMenu["Nombre"] = "Generar Reporte";
                        drMenu["Url"] = "../Reporte/GenerarReporte.aspx";
                        dtMenu.Rows.Add(drMenu);
                    }
                    else
                    {
                        ValidarPerfilCalificacion(analista, dtMenu);
                    }
                    lsvMenu.DataSource = dtMenu;
                    lsvMenu.DataBind();

                  
                }
            }
        }

        private void ValidarPerfilCalificacion(Analista analista, DataTable dtMenu)
        {
           

            if (analista != null)
            {
                DataRow drMenu = dtMenu.NewRow();

                if (analista.PuedeCalificarPresencial)
                {
                    drMenu["Nombre"] = "Presencial";
                    drMenu["Url"] = "../CalifiCartera.aspx";
                    dtMenu.Rows.Add(drMenu);
                }


                if (analista.PuedeCalificarMasivo)
                {
                    drMenu = dtMenu.NewRow();
                    drMenu["Nombre"] = "Masivo";
                    drMenu["Url"] = "../CalificaCartera.aspx";
                    dtMenu.Rows.Add(drMenu);
                }
            }
        }

        protected DataTable determinarMenu()
        {
            DataTable dtMenu = new DataTable();
            DataColumn dcNombre = new DataColumn("Nombre", typeof(string));
            DataColumn dcUrl = new DataColumn("Url", typeof(string));
            dtMenu.Columns.Add(dcNombre);
            dtMenu.Columns.Add(dcUrl);

            return dtMenu;
        }

        protected void lbSalir_Click(object sender, EventArgs e)
        {
            string pagina = Request.Url.Segments[Request.Url.Segments.Count() - 1];
            if (Session["Reporte"] != null)
            {
                if (File.Exists(Session["Reporte"].ToString()))
                {
                    File.Delete(Session["Reporte"].ToString());
                }
            }
            Session.RemoveAll();
            if (pagina.Equals("GenerarReporte.aspx"))
            {
                Response.Redirect("../inicio");
            }
            else
            {
                Response.Redirect("Inicio");
            }

        }
    }
}
