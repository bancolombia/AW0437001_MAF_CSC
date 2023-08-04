<%@ Page Title="Búsqueda Clientes para Calificación" Language="C#" MasterPageFile="~/CalificacionCartera/Master/General.Master"
    AutoEventWireup="true" CodeBehind="CalificacionSemCartera.aspx.cs" Inherits="Bancolombia.Riesgo.MAFWeb.CalificacionCartera.CalificacionSemCartera" %>

<%@ MasterType VirtualPath="~/CalificacionCartera/Master/General.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style4
        {
            width: 100%;
        }
        .gridCell {
            margin-top: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" />
    <asp:UpdatePanel ID="upCliente" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlUsuario" runat="server">
                <table width="100%">
                    <tr>
                        <td colspan="4">
                           
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblIntroduccion" runat="server" CssClass="lblNormal"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Panel ID="pnlComercial" runat="server">
                                <table class="style4">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCantidadClientes" runat="server" CssClass="lblNormal"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView Width="100%" ID="gvClientes" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                CssClass="gridCell" AllowSorting="true" OnPageIndexChanging="gvClientes_PageIndexChanging"
                                                OnSorting="gvClientes_Sorting">
                                                <Columns>
                                                
                                                    <asp:TemplateField HeaderText="Identificación Cliente" SortExpression="nit" >
                                                        <HeaderStyle Width="10%" />
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="hlCodigo" runat="server" CssClass="lblSimple" NavigateUrl='<%# GetUrl(Eval("nit"),Eval("categoriaCliente"))%>'
                                                                Text='<%# Eval("nit", "{0}") %>'></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    
                                                    <asp:BoundField DataField="nombre" HeaderText="Nombre" SortExpression="nombre">
                                                        <ItemStyle Font-Bold="false" CssClass="lblNormal" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="entidad" HeaderText="Entidad" SortExpression="entidad">
                                                        <ItemStyle Font-Bold="false" CssClass="lblNormal" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="banca" HeaderText="Banca" SortExpression="banca">
                                                        <ItemStyle Font-Bold="false" CssClass="lblNormal" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="regional" HeaderText="Región" SortExpression="regional">
                                                        <ItemStyle Font-Bold="false" CssClass="lblNormal" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="zona" HeaderText="Zona" SortExpression="zona">
                                                        <ItemStyle Font-Bold="false" CssClass="lblNormal" />
                                                    </asp:BoundField>
                                                     <asp:BoundField DataField="categoriaCliente" HeaderText="Categoria Cliente" SortExpression="categoriaCliente"  Visible="false">
                                                        <ItemStyle Font-Bold="false" CssClass="lblNormal" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <HeaderStyle CssClass="gridHeader" Width="200px" />
                                                <FooterStyle CssClass="gridFooter" Width="200px" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlRPySup" runat="server">
                                <table class="style4">
                                    <tr>
                                        <td class="lblNormal">
                                            Bienvenido Usuario:
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
                                    </tr>
                                    <tr>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblProcPresencial" runat="server" CssClass="lblNormalBold" Text="CONSULTA PROCESO PRESENCIAL"></asp:Label>
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
                                    </tr>
                                    <tr>
                                        <td class="lblNormal">
                                            <asp:Label ID="Label1" runat="server" CssClass="lblNormal" Text="NIT Cliente"></asp:Label>
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
                                    </tr>
                                    <tr>
                                        <td class="lblNormal">
                                            <asp:TextBox ID="txtCodigoCliente" runat="server" CssClass="txtCampo117"></asp:TextBox>
                                            <asp:Button ID="btnBuscar" runat="server" CssClass="boton" OnClick="btnBuscar_Click"
                                                Text="Buscar" ValidationGroup="buscar" />
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
                                    </tr>
                                </table>
                            </asp:Panel>
                          
                            <asp:Panel ID="pnlFiltros" runat="server">
                                <table width="100%">
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
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal">
                                            Entidad
                                        </td>
                                        <td class="lblNormal">
                                            Banca
                                        </td>
                                        <td class="lblNormal">
                                            Región
                                        </td>
                                        <td class="lblNormal" colspan="2">
                                            Zona
                                        </td>                               
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlEntidad" runat="server" CssClass="listCampo">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBanca" runat="server" CssClass="listCampo">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlRegion" runat="server" CssClass="listCampo" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlZona" runat="server" CssClass="listCampo" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnFiltrar" runat="server" CssClass="boton" Text="Filtrar" OnClick="btnFiltrar_Click" />
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
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table class="style4">
                                    <tr>
                                        <td class="lblNormal">
                                            Usuario
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal">
                                            <asp:TextBox ID="txtCodGte" runat="server" CssClass="txtCampo117"></asp:TextBox>
                                            <asp:Button ID="btnBuscarGte" runat="server" CssClass="boton" Text="Buscar" ValidationGroup="buscarGte"
                                                OnClick="btnBuscarGte_Click" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td colspan="2">
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
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" class="lblNormal">
                                            <asp:Label ID="lblMensBusqueda" runat="server" CssClass="lblNormal"></asp:Label>
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
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" align="center">
                                            <asp:GridView Width="100%" ID="gvClientes2" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                CssClass="gridCell" OnPageIndexChanging="gvClientes2_PageIndexChanging" AllowSorting="True"
                                                OnSorting="gvClientes2_Sorting" EnableSortingAndPagingCallbacks="true">
                                                <Columns>
                                                   <asp:TemplateField HeaderText="Identificación Cliente" SortExpression="nit">
                                                        <HeaderStyle Width="10%" />
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="hlCodigo" runat="server" CssClass="lblSimple" NavigateUrl='<%# GetUrl(Eval("nit"),Eval("categoriaCliente")) %>'
                                                                Text='<%# Eval("nit", "{0}") %>'></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="nombre" HeaderText="Nombre" SortExpression="nombre">
                                                        <ItemStyle CssClass="lblNormal" Font-Bold="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="entidad" HeaderText="Entidad" SortExpression="entidad">
                                                        <ItemStyle CssClass="lblNormal" Font-Bold="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="banca" HeaderText="Banca" SortExpression="banca">
                                                        <ItemStyle CssClass="lblNormal" Font-Bold="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="regional" HeaderText="Región" SortExpression="regional">
                                                        <ItemStyle CssClass="lblNormal" Font-Bold="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="zona" HeaderText="Zona" SortExpression="zona">
                                                        <ItemStyle CssClass="lblNormal" Font-Bold="false" />
                                                    </asp:BoundField>
                                                     <asp:BoundField DataField="categoriaCliente" HeaderText="Categoría Cliente" SortExpression="categoriaCliente" Visible="false">
                                                        <ItemStyle CssClass="lblNormal" Font-Bold="false" />
                                                    </asp:BoundField>
                                                   
                                                </Columns>
                                                <HeaderStyle CssClass="gridHeader" Width="200px" />
                                                <FooterStyle CssClass="gridFooter" Width="200px" />
                                            </asp:GridView>
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
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlProgress" runat="server" CssClass="updateProgress" HorizontalAlign="Center"
        Style="display: none;">
    </asp:Panel>
    <asp:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="pnlProgress"
        PopupControlID="pnlProgress" DropShadow="false" BackgroundCssClass="modalBackgroundProgress" />
    <asp:Panel ID="pnlProgressImg" runat="server" HorizontalAlign="Center" Style="display: none;">
        <img alt="" src="../img/CalificacionCartera/ajax-loader.gif" style="top: 400px;" />
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
