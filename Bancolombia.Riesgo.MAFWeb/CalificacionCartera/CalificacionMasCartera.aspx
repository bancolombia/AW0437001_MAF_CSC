<%@ Page Title="Búsqueda Clientes para Calificación" Language="C#" MasterPageFile="~/CalificacionCartera/Master/General.Master"
    AutoEventWireup="true" CodeBehind="CalificacionMasCartera.aspx.cs" Inherits="Bancolombia.Riesgo.MAFWeb.CalificacionCartera.CalificacionMasCartera" %>

<%@ MasterType VirtualPath="~/CalificacionCartera/Master/General.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style4
        {
            width: 100%;
        }
        #txaJustifiRiesgo
        {
            width: 715px;
            height: 156px;
        }
        #txaJustifiRiesgo0
        {
            width: 715px;
            height: 156px;
        }
        #txaJJustifiComercial
        {
            width: 713px;
            height: 167px;
        }
        .style6
        {
            width: 130px;
        }
        .style7
        {
            width: 130px;
            height: 20px;
        }
        .style8
        {
            height: 20px;
        }
        #txtJustifiRiesgo
        {
            width: 684px;
            height: 118px;
        }
        #txtJustifiComercial
        {
            width: 685px;
            height: 132px;
        }
        #txtJustifiRiesgoR
        {
            width: 740px;
        }
        #txtJustifiComercialR
        {
            width: 741px;
        }
        #txtJustifiRatificada
        {
            width: 734px;
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
                        <td colspan="4">
                            <table width="100%" id="tblProcesoMasivo" runat="server" visible="false">
                                <tr>
                                    <td class="lblNormal">
                                        <asp:Label ID="lblProcMasivo" runat="server" CssClass="lblNormalBold" Text="CONSULTA PROCESO MASIVO"></asp:Label>
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
                                        <asp:Label ID="lblNitCliMasivo" runat="server" CssClass="lblNormal" Text="NIT Cliente"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblNormal">
                                        <asp:TextBox ID="txtNitCliMasivo" runat="server" CssClass="txtCampo117" MaxLength="15"></asp:TextBox>
                                        <asp:Button ID="btnBuscarClienteMasivo" runat="server" class="boton" Text="Buscar"
                                            OnClick="btnBuscarClienteMasivo_Click" />
                                    </td>
                                </tr>
                            </table>
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
                        <td colspan="4">
                            <asp:Label ID="lblIntroduccion" runat="server" CssClass="lblNormal"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Panel ID="pnlComercial" runat="server" Visible="false">
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
                                                CssClass="gridCell" AllowSorting="True" OnPageIndexChanging="gvClientes_PageIndexChanging"
                                                OnSorting="gvClientes_Sorting">
                                                
                                                
                                                <Columns>
                                                
                                                
                                                    <%--<asp:TemplateField HeaderText="Identificación Cliente" SortExpression="idCliente">
                                                        <HeaderStyle Width="10%" />
                                                        
                                                        <ItemTemplate>
                                                        
                                                            <asp:LinkButton ID="btnPopUpComCliente"  runat="server" CommandName="Comercial" Text='<%# Eval("idCliente", "{0}") %>'
                                                                CommandArgument='<%#Eval("idCliente")%>' OnCommand="GetUrlLinkButton"></asp:LinkButton>
                                                          
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>--%>
                                                    
                                                    <asp:BoundField DataField="idCliente" HeaderText=" Identificación Cliente " SortExpression="idCliente">
                                                        <ItemStyle Font-Bold="false" CssClass="lblNormal" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="nombreCliete" HeaderText=" Nombre " SortExpression="nombreCliete">
                                                        <ItemStyle Font-Bold="false" CssClass="lblNormal" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="entidad" HeaderText=" Entidad" SortExpression="entidad">
                                                        <ItemStyle Font-Bold="false" CssClass="lblNormal" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="banca" HeaderText="Banca" SortExpression="banca">
                                                        <ItemStyle Font-Bold="false" CssClass="lblNormal" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="calIntAct" HeaderText="Calificación Interna Actual" SortExpression="calIntAct">
                                                        <ItemStyle Font-Bold="false" CssClass="lblNormal" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="calInRecR" HeaderText="Calificación Interna Recomendada Riesgos"
                                                        SortExpression="calInRecR" />
                                                    <asp:BoundField DataField="calExAct" HeaderText="Calificación Externa Actual" SortExpression="calExAct" />
                                                    <asp:BoundField DataField="calExRecR" HeaderText="Calificación Externa Recomendada Riesgos"
                                                        SortExpression="calExRecR" />
                                                    <asp:BoundField DataField="calInRecC" HeaderText="Calificación Interna Recomendada Comercial"
                                                        SortExpression="calInRecC" />
                                                    <asp:BoundField DataField="calExRecC" HeaderText="Calificación Externa Recomendada Comercial"
                                                        SortExpression="calExRecC" />
                                                        
                                                        
                                                    <asp:TemplateField HeaderText="¿Retroalimentar la calificación? - Ver" SortExpression="idCliente"
                                                        ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="10%" />
                                                        
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnPopUpCom" runat="server" Text="Ir" CommandName="Comercial" CommandArgument='<%#Eval("idCliente")%>'
                                                                OnCommand="GetUrl" />
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>
                                                    
                                                    
                                                    <asp:BoundField DataField="retAuto" HeaderText=" Retroalimentación Automatica" SortExpression="retAuto" />
                                                </Columns>
                                                
                                                
                                                <HeaderStyle CssClass="gridHeader" Width="200px" />
                                                <FooterStyle CssClass="gridFooter" Width="200px" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlCalMasivoComercial" ScrollBars="Auto" runat="server" Width="50%" CssClass="popUpStyle"
                                Style="display: none; height: 55%; overflow-x: hidden" HorizontalAlign="Center">
                                <table border="0" width="100%" class="lblNormal">
                                    <tr>
                                        <td class="TituloPrincipal" align="center" colspan="5">
                                            COMENTARIOS CALIFICACIÓN PROCESO MASIVO
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6" style="font-weight: bold;">
                                            Identificación Cliente:<b> </b>
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblIdCliente" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="font-weight: bold;">
                                            Nombre:
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblNombre" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblFechaProceso" runat="server" Visible="false"></asp:Label>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6" style="font-weight: bold;">
                                            Entidad:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEntidad" runat="server"></asp:Label>
                                        </td>
                                        <td style="font-weight: bold;">
                                            Banca:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBanca" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="5">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            &nbsp;
                                        </td>
                                        <td class="lblSubTitInfo" style="font-weight: bold; text-align: center;">
                                            Calificación Actual
                                        </td>
                                        <td class="lblSubTitInfo" style="font-weight: bold; text-align: center;">
                                            Calificación Recomendada Riesgo
                                        </td>
                                        <td class="lblSubTitInfo" style="font-weight: bold; text-align: center;">
                                            Calificación Recomendada Comercial
                                        </td>
                                        <td class="lblNormal">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style7" style="font-weight: bold;">
                                            Calificación Interna:
                                        </td>
                                        <td class="style8" align="center">
                                            <asp:Label ID="lblCalifInterActual" runat="server"></asp:Label>
                                        </td>
                                        <td class="style8" align="center">
                                            <asp:Label ID="lblCalifInterRiesgo" runat="server"></asp:Label>
                                        </td>
                                        <td class="style8" align="center">
                                            <asp:DropDownList ID="dplCalifInterComercial" runat="server" Width="115px">
                                                <asp:ListItem Value="0" Text="-- Seleccionar--"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="style8">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6" style="font-weight: bold;">
                                            Calificación Externa:
                                        </td>
                                        <td class="style8" align="center">
                                            <asp:Label ID="lblCalifExterActual" runat="server"></asp:Label>
                                        </td>
                                        <td class="style8" align="center">
                                            <asp:Label ID="lblCalifExterRiesgo" runat="server"></asp:Label>
                                        </td>
                                        <td class="style8" align="center">
                                            <asp:DropDownList ID="dplCalifExterComercial" runat="server" Width="115px">
                                                <asp:ListItem Value="0" Text="-- Seleccionar--"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="lblNormal">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="5">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" style="font-weight: bold;" colspan="5">
                                            Justificación Calificación Riesgo
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="5">
                                            <textarea id="txtJustifiRiesgo" cols="20" name="S1" rows="2" runat="server" style="width: 95%;
                                                height: 70px" disabled="disabled"></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" style="font-weight: bold;" colspan="5">
                                            Justificación Calificación Comercial
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="5">
                                            <textarea id="txtJustifiComercial" cols="20" rows="2" runat="server" style="width: 95%;
                                                height: 70px"></textarea>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" align="center" colspan="5" style="text-align: center">
                                            <asp:Button ID="btnGrabar" runat="server" CssClass="boton" OnClientClick="return guardarComercial()"
                                                OnClick="btnGrabar_Click" Text="Grabar" Width="149px" />
                                            <asp:Button ID="bntCancelar" runat="server" CssClass="boton" OnClick="btnCancelar_Click"
                                                Text="Cancelar" Width="149px" />
                                            <asp:Button ID="btnPdf" runat="server" CssClass="boton" OnClick="btnPdf_Click" Text="Generar PDF"
                                                Width="149px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="5">
                                            <hr />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Panel ID="pnlRiesgos" runat="server" Visible="false">
                                <table class="style4">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCantidadClientesR" runat="server" CssClass="lblNormal"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="gvClientesRiesgos" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="gridCell" OnPageIndexChanging="gvClientesRiesgos_PageIndexChanging"
                                                OnSorting="gvClientesRiesgos_Sorting" Width="100%">
                                                <Columns>
                                                
                                                
                                                <%--<asp:TemplateField HeaderText="Identificación Cliente" SortExpression="idCliente">
                                                        <HeaderStyle Width="10%" />
                                                        
                                                        <ItemTemplate>
                                                        
                                                            <asp:LinkButton ID="btnPopUpComClienteRiesgos"  runat="server" CommandName="Comercial" Text='<%# Eval("idCliente", "{0}") %>'
                                                                CommandArgument='<%#Eval("idCliente")%>' OnCommand="GetUrlLinkButton"></asp:LinkButton>
                                                          
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>--%>
                                                
                                                    <asp:BoundField DataField="idCliente" HeaderText=" Identificación Cliente " SortExpression="idCliente">
                                                        <ItemStyle Font-Bold="false" CssClass="lblNormal" />
                                                    </asp:BoundField>
                                                    
                                                    
                                                    <asp:BoundField DataField="nombreCliete" HeaderText=" Nombre " SortExpression="nombreCliete">
                                                        <ItemStyle CssClass="lblNormal" Font-Bold="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="entidad" HeaderText=" Entidad" SortExpression="entidad">
                                                        <ItemStyle CssClass="lblNormal" Font-Bold="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="banca" HeaderText="Banca" SortExpression="banca">
                                                        <ItemStyle CssClass="lblNormal" Font-Bold="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="calInRecR" HeaderText="Calificación Interna Recomendada Riesgos"
                                                        SortExpression="calInRecR" />
                                                    <asp:BoundField DataField="calExRecR" HeaderText="Calificación Externa Recomendada Riesgos"
                                                        SortExpression="calExRecR" />
                                                    <asp:BoundField DataField="calInRecC" HeaderText="Calificación Interna Recomendada Comercial"
                                                        SortExpression="calInRecC" />
                                                    <asp:BoundField DataField="calExRecC" HeaderText="Calificación Externa Recomendada Comercial"
                                                        SortExpression="calExRecC" />
                                                    <asp:BoundField DataField="calInRatR" HeaderText="Calificación Interna Ratificada"
                                                        SortExpression="calInRatR" />
                                                    <asp:BoundField DataField="calExRatR" HeaderText="Calificación Externa Ratificada"
                                                        SortExpression="calExRatR" />
                                                    <asp:TemplateField HeaderText="Ratificar calificación" SortExpression="idCliente"
                                                        ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="10%" />
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnPopUpCom" runat="server" Text="Ir" CommandName="Riesgos" CommandArgument='<%#Eval("idCliente")%>'
                                                                OnCommand="GetUrl" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="gridHeader" Width="200px" />
                                                <FooterStyle CssClass="gridFooter" Width="200px" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlCalMasivoRiesgos" ScrollBars="Auto" runat="server" Width="60%" CssClass="popUpStyle"
                                Style="display: none; height: 70%; overflow-x: hidden" HorizontalAlign="Center">
                                <table border="0" width="100%" class="lblNormal">
                                    <tr>
                                        <td class="TituloPrincipal" align="center" colspan="7">
                                            COMENTARIOS CALIFICACION PROCESO MASIVO
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6" style="font-weight: bold;">
                                            Identificación Cliente:<b> </b>
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblIdClienteR" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="font-weight: bold;">
                                            Nombre:
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblNombreR" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="font-weight: bold;">
                                            Usuario Comercial:
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblUsuarioComercial" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblFechaProcesoR" runat="server" Visible="false"></asp:Label>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6" style="font-weight: bold;">
                                            Entidad:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEntidadR" runat="server"></asp:Label>
                                        </td>
                                        <td style="font-weight: bold;">
                                            Banca:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBancaR" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="font-weight: bold;">
                                            Usuario Riesgos:
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblUsuarioRiesgos" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="7">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            &nbsp;
                                        </td>
                                        <td class="lblSubTitInfo" style="font-weight: bold; text-align: center;">
                                            Calificación Actual
                                        </td>
                                        <td class="lblSubTitInfo" style="font-weight: bold; text-align: center;">
                                            Calificación Recomendada Riesgos
                                        </td>
                                        <td class="lblSubTitInfo" style="font-weight: bold; text-align: center;">
                                            Calificación Recomendada Comercial
                                        </td>
                                        <td class="lblSubTitInfo" style="font-weight: bold; text-align: center;">
                                            Calificación Ratificada
                                        </td>
                                        <td class="lblNormal">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style7" style="font-weight: bold;">
                                            Calificación Interna:
                                        </td>
                                        <td class="style8" style="text-align: center;">
                                            <asp:Label ID="lblCalifInterActualR" runat="server"></asp:Label>
                                        </td>
                                        <td class="style8" style="text-align: center;">
                                            <asp:Label ID="lblCalifInterRiesgoR" runat="server"></asp:Label>
                                        </td>
                                        <td class="style8" style="text-align: center;">
                                            <asp:Label ID="lblCalifInterComercial" runat="server"></asp:Label>
                                        </td>
                                        <td class="style8" style="text-align: center;">
                                            <asp:DropDownList ID="dplCalifInterRatificada" runat="server" Width="115px">
                                                <asp:ListItem Value="0" Text="-- Seleccionar--"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="style8">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6" style="font-weight: bold;">
                                            Calificación Externa:
                                        </td>
                                        <td class="lblNormal" style="text-align: center;">
                                            <asp:Label ID="lblCalifExterActualR" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="text-align: center;">
                                            <asp:Label ID="lblCalifExterRiesgoR" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="text-align: center;">
                                            <asp:Label ID="lblCalifExterComercial" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="text-align: center;">
                                            <asp:DropDownList ID="dplCalifExterRatificada" runat="server" Width="115px">
                                                <asp:ListItem Value="0" Text="-- Seleccionar--"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="lblNormal">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="7">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="7">
                                            Justificación Calificación Recomendada Riesgos
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="7">
                                            <textarea id="txtJustifiRiesgoR" cols="20" name="S1" rows="2" runat="server" style="width: 95%;
                                                height: 70px" disabled="disabled"></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="7">
                                            Justificación Calificación Comercial
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="7">
                                            <textarea id="txtJustifiComercialR" cols="20" rows="2" runat="server" style="width: 95%;
                                                height: 70px" disabled="disabled"></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="7">
                                            Justificación Calificación Ratificada
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="7">
                                            <textarea id="txtJustifiRatificada" cols="20" rows="2" style="width: 95%; height: 70px"
                                                runat="server"></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" align="center" colspan="5" style="text-align: center">
                                            <asp:Button ID="btnGrabarR" runat="server" CssClass="boton" OnClientClick="return guardarRiesgos()"
                                                OnClick="btnValidarGuardar_Click" Text="Grabar" Width="149px" />
                                            <asp:Button ID="bntCancelarR" runat="server" CssClass="boton" OnClick="btnCancelar_Click"
                                                Text="Cancelar" Width="149px" />
                                            <asp:Button ID="btnPdfR" CssClass="boton" runat="server" OnClick="btnPdf_Click" Text="Generar PDF"
                                                Width="149px" />
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="7">
                                            <hr />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlClienteMasivo" runat="server" Width="50%" CssClass="popUpStyle" ScrollBars="Auto"
                                Style="display: none; height: 70%; overflow-x: hidden" HorizontalAlign="Center">
                                <table border="0" width="100%" class="lblNormal" >
                                    <tr>
                                        <td class="TituloPrincipal" align="center" colspan="7">
                                            COMENTARIOS CALIFICACION PROCESO MASIVO
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6" style="font-weight: bold;">
                                            Identificación Cliente:<b> </b>
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblIdClienteR2" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="font-weight: bold;">
                                            Nombre:
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblNombreR2" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="font-weight: bold;">
                                            <asp:Label ID="lblUsuarioComercialDisplay" runat="server">Usuario Comercial:</asp:Label>
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblUsuarioComercial2" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblFechaProcesoR2" runat="server" Visible="false"></asp:Label>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6" style="font-weight: bold;">
                                            Entidad:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEntidadR2" runat="server"></asp:Label>
                                        </td>
                                        <td style="font-weight: bold;">
                                            Banca:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBancaR2" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="font-weight: bold;">
                                            <asp:Label ID="lblUsuarioRiesgosDisplay" runat="server">Usuario Riesgos:</asp:Label>
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblUsuarioRiesgos2" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="7">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6">
                                            &nbsp;
                                        </td>
                                        <td class="lblSubTitInfo" style="font-weight: bold; text-align: center;">
                                            Calificación Actual
                                        </td>
                                        <td class="lblSubTitInfo" style="font-weight: bold; text-align: center;">
                                            Calificación Recomendada Riesgos
                                        </td>
                                        <td class="lblSubTitInfo" style="font-weight: bold; text-align: center;">
                                            Calificación Recomendada Comercial
                                        </td>
                                        <td class="lblSubTitInfo" style="font-weight: bold; text-align: center;">
                                            <asp:Label ID="lblCalifRat" runat="server">Calificación Ratificada</asp:Label>
                                        </td>
                                        <td class="lblNormal">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style7" style="font-weight: bold;">
                                            Calificación Interna:
                                        </td>
                                        <td class="style8" style="text-align: center;">
                                            <asp:Label ID="lblCalifInterActualR2" runat="server"></asp:Label>
                                        </td>
                                        <td class="style8" style="text-align: center;">
                                            <asp:Label ID="lblCalifInterRiesgoR2" runat="server"></asp:Label>
                                        </td>
                                        <td class="style8" style="text-align: center;">
                                            <asp:Label ID="lblCalifInterComercial2" runat="server"></asp:Label>
                                        </td>
                                        <td class="style8" style="text-align: center;">
                                            <asp:Label ID="lblCalifInterRatificada2" runat="server"></asp:Label>
                                        </td>
                                        <td class="style8">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style6" style="font-weight: bold;">
                                            Calificación Externa:
                                        </td>
                                        <td class="lblNormal" style="text-align: center;">
                                            <asp:Label ID="lblCalifExterActualR2" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="text-align: center;">
                                            <asp:Label ID="lblCalifExterRiesgoR2" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="text-align: center;">
                                            <asp:Label ID="lblCalifExterComercial2" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="text-align: center;">
                                            <asp:Label ID="lblCalifExterRatificada2" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="7">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="7">
                                            Justificación Calificación Recomendada Riesgos
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="7">
                                            <textarea id="txtJustifiRiesgoR2" cols="20" name="S1" rows="2" runat="server" style="width: 95%;
                                                height: 70px" disabled="disabled"></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="7">
                                            Justificación Calificación Comercial
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="7">
                                            <textarea id="txtJustifiComercialR2" cols="20" rows="2" runat="server" style="width: 95%;
                                                height: 70px" disabled="disabled"></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="7">
                                            <asp:Label ID="lblJustifiRatificada" runat="server">Justificación Calificación Ratificada</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="7">
                                            <textarea id="txtJustifiRatificada2" cols="20" rows="2" style="width: 95%; height: 70px"
                                                runat="server" disabled="disabled"></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" align="center" colspan="5" style="text-align: center">
                                            <asp:Button ID="btnPdfR2" CssClass="boton" runat="server" Text="Generar PDF" Width="149px"
                                                OnClick="btnPdfR2_Click" />
                                            <asp:Button ID="bntCancelarR2" runat="server" CssClass="boton" Text="Cerrar" Width="149px"
                                                OnClick="bntCancelarR2_Click" />
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="7">
                                            <hr />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlValidacionCal" runat="server" Width="725px" CssClass="popUpStyle" ScrollBars="Auto"
                Style="display: none; height: 12%; overflow-x: hidden" HorizontalAlign="Center">
                <table>
                    <tr>
                        <td>
                            <h1 style="height: 15%; padding-top: 5px; font-size: 10pt; width: 100%;">
                                La Calificación Interna Ratificada Nuevo Rating y la Calificación Externa Ratificada,
                                no guardan coherencia. ¿Desea continuar con la grabación?</h1>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <asp:Button ID="btnAceptaValidacion" runat="server" Text="Si" Width="60px" CssClass="boton"
                                        OnClick="btnGrabarR_Click" />
                                    <asp:Button ID="btnCancelaValidacion" runat="server" Text="No" Width="60px" CssClass="boton"
                                        OnClick="btnCancelarGrabarR_Click" />
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button ID="btnPopUpComercial" runat="server" Style="display: none;" />
            <asp:Button ID="btnPopUpRiesgos" runat="server" Style="display: none;" />
            <asp:Button ID="btnValidacionGuardar" runat="server" Style="display: none;" />
            <asp:Button ID="btnPopUpMasivo" runat="server" Style="display: none;" />
            <asp:ModalPopupExtender ID="mpePnlCliMasivo" runat="server" TargetControlID="btnPopUpMasivo"
                PopupControlID="pnlClienteMasivo" DropShadow="false" Drag="true" BackgroundCssClass="modalBackground" />
                
            <asp:ModalPopupExtender ID="mpePnlCalMasivoCom" runat="server" TargetControlID="btnPopUpComercial"
                PopupControlID="pnlCalMasivoComercial" DropShadow="false" Drag="true" BackgroundCssClass="modalBackground" />
            <asp:ModalPopupExtender ID="mpePnlCalMasivoRies" runat="server" TargetControlID="btnPopUpRiesgos"
                PopupControlID="pnlCalMasivoRiesgos" DropShadow="false" Drag="true" BackgroundCssClass="modalBackground" />
            <asp:ModalPopupExtender ID="mpeValidacionCal" runat="server" PopupControlID="pnlValidacionCal"
                TargetControlID="btnValidacionGuardar" DropShadow="false" Drag="true" BackgroundCssClass="modalBackground" />
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


        function guardarComercial() {
            panelStr = "ctl00_ContentPlaceHolder1_";
            dplCalIntComer = $("#" + panelStr + "dplCalifInterComercial").val();
            dplCalExtComer = $("#" + panelStr + "dplCalifExterComercial").val();
            txtJustComer = $("#" + panelStr + "txtJustifiComercial").val().trim();
            if (dplCalIntComer == "0" || dplCalExtComer == "0" || txtJustComer=="") {
                alert("Debe diligenciar todos los campos");
                return false;
            }
            return true;
        }

        function guardarRiesgos() {
            panelStr = "ctl00_ContentPlaceHolder1_";
            dplCalIntRies = $("#" + panelStr + "dplCalifInterRatificada").val();
            dplCalExtRies = $("#" + panelStr + "dplCalifExterRatificada").val();
            txtJustRies = $("#" + panelStr + "txtJustifiRatificada").val().trim();
            if (dplCalIntRies == "0" || dplCalExtRies == "0" || txtJustRies == "") {
                alert("Debe diligenciar todos los campos");
                return false;
            }
            return true;
        }
    </script>

</asp:Content>
