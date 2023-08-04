<%@ Page Title="" Language="C#" MasterPageFile="~/CalificacionCartera/Reporte/GenerarInformes.Master"
    AutoEventWireup="true" CodeBehind="GenerarReporte.aspx.cs" Inherits="Bancolombia.Riesgo.MAFWeb.CalificacionCartera.Reporte.GenerarReporte" %>

<%@ MasterType VirtualPath="~/CalificacionCartera/Reporte/GenerarInformes.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" AsyncPostBackTimeout="720" />
    <link href="../../css/CalificacionCartera/analisisFinanciero.css" rel="stylesheet" type="text/css" />
    <asp:UpdatePanel ID="upCliente" runat="server">
        
        <ContentTemplate>
            <asp:Panel ID="pnlUsuario" runat="server">
                <table width="100%">
                    <tr>
                        <td class="lblLinea">
                            USUARIO:
                            <asp:Label ID="lblUsuario" runat="server" CssClass="lblNormalBold"></asp:Label>
                        </td>
                        <td class="lblNormal">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="right">
                            <asp:Label ID="lblComite" runat="server" CssClass="lblSubTitInfoDerecha" Text="Comité de:"></asp:Label>:&nbsp;<asp:Label
                                ID="lblFechaComite" runat="server" CssClass="lblNormalDerecha"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="lblLinea" style="white-space:nowrap" colspan="2">
                            GENERACIÓN REPORTE PRESENCIAL
                        </td>
                        
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="lblNormal">
                            <asp:Label ID="Label1" runat="server" CssClass="lblNormal" Text="Entidad"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblRegion" runat="server" CssClass="lblNormal" Text="Región"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblZona" runat="server" CssClass="lblNormal" Text="Zona"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblBanca" runat="server" CssClass="lblNormal" Text="Banca"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblEstado" runat="server" CssClass="lblNormal" Text="Estado"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblUsrRiesgos" runat="server" CssClass="lblNormal" Text="Usuario Riesgos"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlEntidad" runat="server" CssClass="listCampo">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRegion" runat="server" CssClass="listCampo">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlZona" runat="server" CssClass="listCampo">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBanca" runat="server" CssClass="listCampo">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="listCampo">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUsrRiesgos" runat="server" CssClass="txtCampo117" 
                                MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnGenerar" runat="server" CssClass="boton" OnClick="btnFiltrar_Click"
                                Text="Generar" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    </table>
                    <table runat="server" id="tblReporteMasivo"> 
                    <tr>
                        <td class="lblLinea">
                            GENERACIÓN REPORTE MASIVO
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnGenerarRepMasivo" runat="server" CssClass="boton" 
                                Text="Generar" onclick="btnGenerarRepMasivo_Click" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <%--Start--PMO19939-RF010 Descargar el archivo “Log de cambios” a Excel desde el perfil de Riesgos--%>
                <table width="42%" ID="tblExportarLog" runat="server">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="lblLinea">
                            EXPORTAR LOG
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style5">
                            &nbsp;
                        </td>
                        <td class="lblNormalCentrado">
                            Para realizar la exportación del log de Calificación, dar clic en la opcion exportar
                            plano log para el proceso actual o digte el Nit puntual y dar clic en generar
                        </td>
                        <td class="txtCeldaCampo">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style5">
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:RadioButton ID="rdoLogPresencial" Text="Log Presencial" GroupName="rbgExportarLog"
                                runat="server" Checked="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style5">
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:RadioButton ID="rdoLogMasivo" Text="Log Masivo" GroupName="rbgExportarLog" runat="server"
                                Checked="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style5">
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:RadioButton ID="rdoLogSobreUnCliente" Text="Log Sobre un cliente" GroupName="rbgExportarLog"
                                runat="server" Checked="false" />
                            <asp:TextBox ID="txtNitCliente" Text="" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style5">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnExportarLog" runat="server" CssClass="boton" Text="Exportar Log"
                                OnClick="btnExportarLog_Click" ValidationGroup="carga" Width="130px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style6">
                            &nbsp;
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style6" colspan="3">
                             &nbsp;
                        </td>
                    </tr>
                </table>
                <%--End--PMO19939-RF010 Descargar el archivo “Log de cambios” a Excel desde el perfil de Riesgos--%></asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGenerar" />
             <%--PMO19939_REQ001_Exportar Log y Resultado del proceso Masivo.--%>
            <asp:PostBackTrigger ControlID="btnGenerarRepMasivo" />
            <asp:PostBackTrigger ControlID="btnExportarLog" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlProgress" runat="server" CssClass="updateProgress" HorizontalAlign="Center"
        Style="display: none;">
    </asp:Panel>
    <asp:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="pnlProgress"
        PopupControlID="pnlProgress" DropShadow="false" BackgroundCssClass="modalBackgroundProgress" />
    <asp:Panel ID="pnlProgressImg" runat="server" HorizontalAlign="Center" Style="display: none;">
        <img alt="" src="../../img/CalificacionCartera/ajax-loader.gif" style="top: 400px;" />
        <br />
        <strong><span style="font-size: 15px; font-family: Tahoma; text-align: center;">Cargando...</span>
        </strong>
    </asp:Panel>
    <asp:ModalPopupExtender ID="mpeProgressImg" runat="server" TargetControlID="pnlProgressImg"
        PopupControlID="pnlProgressImg" DropShadow="false" BackgroundCssClass="modalBackgroundProgress" />

    <script type="text/javascript">
    /// <summary>
    /// operacion que permite adicional el evento onInvoke a la página
    /// </summary>
    Sys.Net.WebRequestManager.add_invokingRequest(onInvoke);
    /// <summary>
    /// operacion que permite adicionar el evento onComplete a la página
    /// </summary>
    Sys.Net.WebRequestManager.add_completedRequest(onComplete);
    /// <summary>
    /// Esta función manejará el evento onInvoke
    /// </summary>
    /// <param name="sender">objeto que desencadena el evento</param>
    /// <param name="args">parametros del evento que se desencadena</param>
    function onInvoke(sender, args) {
        $find('<%= mpeProgress.ClientID %>').show();
        $find('<%= mpeProgressImg.ClientID %>').show();
    }
    /// <summary>
    /// Esta función manejará el evento onComplete
    /// </summary>
    /// <param name="sender">objeto que desencadena el evento</param>
    /// <param name="args">parametros del evento que se desencadena</param>
    function onComplete(sender, args) {
        $find('<%= mpeProgress.ClientID %>').hide();
        $find('<%= mpeProgressImg.ClientID %>').hide();
    }
    /// <summary>
    /// Método que se ejecuta cuando la página está Unload
    /// </summary>
    function pageUnload() {
        Sys.Net.WebRequestManager.remove_invokingRequest(onInvoke);
        Sys.Net.WebRequestManager.remove_completedRequest(onComplete);
    }
    </script>

</asp:Content>
