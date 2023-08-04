<%@ Page Title="Carga de Archivo de Clientes" Language="C#" MasterPageFile="~/CalificacionCartera/Master/General.Master"
    AutoEventWireup="true" CodeBehind="CargarArchivo.aspx.cs" Inherits="Bancolombia.Riesgo.MAFWeb.CalificacionCartera.CargarArchivo" %>

<%@ MasterType VirtualPath="~/CalificacionCartera/Master/General.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style6
        {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" />
    <asp:UpdatePanel ID="upExportar" runat="server">
        <ContentTemplate>
            <div>
                <table width="100%">
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="lblNormalBold" valign="top" colspan="2">
                            Seleccione la opción del archivo que desea cargar
                        </td>
                    </tr>
                    <tr>
                        <td class="style6">
                            &nbsp;
                        </td>
                        <td align="left">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style6" style="text-align: center">
                            <asp:Button ID="btnCargar" runat="server" CssClass="boton" OnClick="btnCargar_Click"
                                Text="Cargar Clientes Proceso Presencial" Width="210px" ValidationGroup="carga" />
                            &nbsp;<asp:Button ID="btnCargarCric" runat="server" CssClass="boton" OnClick="btnCargarCric_Click"
                                Text="Cargue Masivo Proceso Presencial" Width="210px" ValidationGroup="carga" />
                            &nbsp;<asp:Button ID="btnCargarPEC" runat="server" CssClass="boton" Width="140px"
                                Text="Cargar Central Externa" OnClick="btnCargarPEC_Click" />
                            &nbsp;<asp:Button ID="btnCovenants" runat="server" CssClass="boton" Width="110px"
                                Text="Cargar Covenants" OnClick="btnCovenants_Click" />
                            &nbsp;<asp:Button ID="btnProrrogas" runat="server" CssClass="boton" Width="110px"
                                Text="Cargar Prorrogas" OnClick="btnProrrogas_Click" />
                            &nbsp;<asp:Button ID="btnIndicadores" runat="server" CssClass="boton" Width="120px"
                                Text="Cargar Indicadores" OnClick="btnIndicadores_Click" />
                            &nbsp;<asp:Button ID="Button1" runat="server" CssClass="boton" Width="210px" 
                                Text="Cargar Clientes Proceso Masivo" OnClick="btnCargarPrMasivo_Click" />
                        </td>
                        <td align="left">
                            &nbsp;
                        </td>
                    </tr>
                    <td class="style6" colspan="2">
                        <asp:Label ID="lblExito" runat="server" CssClass="lblAyudas"></asp:Label>
                        <asp:Label ID="lblMensaje" runat="server" CssClass="lblError"></asp:Label>
                    </td>
                    </tr>
                </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlProgress" runat="server" CssClass="updateProgress" HorizontalAlign="Center"
        Style="display: none;">
    </asp:Panel>
    <asp:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="pnlProgress"
        PopupControlID="pnlProgress" DropShadow="false" BackgroundCssClass="modalBackgroundProgress" />
    <asp:Panel ID="pnlProgressImg" runat="server" HorizontalAlign="Center" Style="display: none;">
        <img alt="" src="../img/CalificacionCartera/ajax-loader.gif" style="top: 400px" />
        <br />
        <strong><span style="font-size: 15px; font-family: Tahoma; text-align: center">Cargando...</span>
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
