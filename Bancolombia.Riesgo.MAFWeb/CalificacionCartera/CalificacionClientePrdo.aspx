<%@ Page Title="" Language="C#" MasterPageFile="~/CalificacionCartera/Master/General.Master" AutoEventWireup="true" CodeBehind="CalificacionClientePrdo.aspx.cs" Inherits="Bancolombia.Riesgo.MAFWeb.CalificacionCartera.CalificacionClientePrdo" %>
<%@ MasterType VirtualPath="~/CalificacionCartera/Master/General.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
	

function format(input) {
    var num = input.value.replace(/\,/g, '');
    if (!isNaN(num)) {
        if (num.indexOf('.') > -1) {
            num = num.split('.');
            num[0] = num[0].toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1,').split('').reverse().join('').replace(/^[\,]/, '');
            if (num[1].length > 2) {
                //alert('You may only enter two decimals!');
                num[1] = num[1].substring(0, num[1].length - 1);
                return false;
            } input.value = num[0] + '.' + num[1];
        } else {
            input.value = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1,').split('').reverse().join('').replace(/^[\,]/, '')
        };
    } else {
        //alert('You may enter only numbers in this field!');
    input.value = input.value.substring(0, input.value.length - 1);
    return false;
    }
}

function ondalikSayiKontrol(alan) {

    var msg = alan.value;
    var w;
    var nokta = msg.indexOf(",");
    var ind;

    for (w = 0; w < msg.length; w++) {

        ind = msg.substring(w, w + 1);
        if (ind < "0" || ind > "9") {

            if (nokta > 0)
                if (w == nokta) continue;

            msg = msg.substring(0, w);
            alan.value = msg;
            break;
        }

    }

}

</script>
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" />
<asp:UpdatePanel ID="upCliente" runat="server">
<ContentTemplate>
    <asp:Panel ID="pnlErrores" runat="server" Visible="false">
    <table border="0" width="100%" class="lblNormal">
        <tr>
            <td class="lblError">
                <asp:Label ID="lblErrores" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="pnlCliente" runat="server" Visible="False">
        <asp:Panel ID="pnlEncabezadoCliente" runat="server">
            <table class="lblNormal" border="0" width="100%">
                <tr>
                    <td class="lblLinea">
                        USUARIO:
                    </td>
                    <td align="left">
                        <asp:Label ID="lblUsuario" runat="server" CssClass="lblNormalBold"></asp:Label>
                    </td>
                    <td class="lblSubTitInfoDerecha">
                        &nbsp;</td>
                    <td class="lblSubTitInfoDerecha">
                        Comité de:</td>
                    <td>
                        <asp:Label ID="lblFechaComite" runat="server" CssClass="lblNormal"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblLinea" colspan="5">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblLinea" colspan="5">
                        DATOS DEL CLIENTE</td>
                </tr>
                <tr>
                    <td class="lblLinea" colspan="5">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Nit:</td>
                    <td align="left">
                        <asp:Label ID="lblCliente" runat="server"></asp:Label>
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                    &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Nombre:</td>
                    <td align="left">
                        <asp:Label ID="lblNombreCliente" runat="server"></asp:Label>
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Tipo de Comité:</td>
                    <td align="left">
                        <asp:Label ID="lblTipoComite" runat="server"></asp:Label>
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr /></td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblLinea" colspan="5">
                        INFORMACIÓN DEL CLIENTE</td>
                </tr>
                <tr>
                    <td class="lblLinea" colspan="5">
                        &nbsp;</td>
                </tr>
                <tr class="lblNormal">
                    <td class="lblSubTitInfo">
                        Sector económico:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblSector" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        Seguimiento recomendado en el comité anterior:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblSeguimiento" runat="server"></asp:Label>
                    </td>
                    <td class="lblNormal">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Segmento:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblSegmento" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        Grupo de Riesgo:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblGrupoR" runat="server"></asp:Label>
                    </td>
                    <td class="lblNormal">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Regional:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblRegional" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        Nivel de Riesgo AEC:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblRiesgoAEC" runat="server"></asp:Label>
                    </td>
                    <td class="lblNormal">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        <asp:Label ID="lblCalificado" runat="server" Text="Calificado por:"></asp:Label>
                    </td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalifPor" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblSubTitInfo">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo" colspan="5">
                        <hr /></td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblSubTitInfo">
                        &nbsp;</td>
                    <td class="lblNormal" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblLinea" colspan="5">
                        INFORMACIÓN DE ENDEUDAMIENTO</td>
                </tr>
                <tr>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal" colspan="2">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlInfoEndeudamiento" runat="server">
            <table>
                <tr>
                    <td class="lblSubTitInfo">
                        <asp:Label ID="lblCampoParametrico1" runat="server"></asp:Label>
                    </td>
                    <td class="lblNormal">
                        &nbsp;
                    </td>
                    <td class="lblNormal">
                        &nbsp;
                    </td>
                    <td class="lblNormal">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        #VECES MORA 30: </td>
                    <td class="lblNormal">
                        &nbsp;
                    </td>
                    <td class="lblNormal">
                    <asp:Label ID="lblMora30" runat="server"></asp:Label></td>
                    <td class="lblNormal">
                    &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        #VECES MORA 60: </td>
                    <td class="lblNormal">
                        &nbsp;
                    </td>
                    <td class="lblNormal">
                    <asp:Label ID="lblMora60" runat="server"></asp:Label></td>
                    <td class="lblNormal">
                    &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        MORA MÁXIMA: </td>
                    <td class="lblNormal">
                      &nbsp;  
                    </td>
                    <td class="lblNormal">
                        <asp:Label ID="lblMoraMax" runat="server"></asp:Label></td>
                    <td class="lblNormal">
                    &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4">
                    &nbsp;</td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlLocal" runat="server">
            <table class="lblNormal" width="100%">
                <tr>
                    <td class="lblLinea" >
                        <asp:Label ID="lblLocal" runat="server" Text="LOCAL"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    &nbsp;</td>
                </tr>
            </table>
            <table class="lblNormal" width="35%">
                <tr>
                    <td class="lblSubTitInfo">
                        Anticipos de leasing:</td>
                    <td >
                        <asp:Label ID="lblSCAnt" runat="server" CssClass="lblNormal"></asp:Label>
                    </td>
                    <td >
                        &nbsp;</td>
                    <td >
                        &nbsp;</td>
                    <td  >
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
            <table class="Separadores" width="100%">
                <tr>
                    <td class="TituloPrincipal">
                    &nbsp;</td>
                    <td class="TituloPrincipalSeparadores">
                    Saldo Capital</td>
                    <td class="TituloPrincipalSeparadores">
                    Días de Mora</td>
                    <td class="TituloPrincipalSeparadores">
                    Reestructurado</td>
                    <td class="TituloPrincipalSeparadores">
                        Calif. Externa Modelo SFC</td>
                    <td class="TituloPrincipalSeparadores">
                    Calif. Externa Actual</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfoSeparador">
                    Bancolombia</td>
                    <td class="Separadores" align="right">
                        <asp:Label ID="lblSCBanc" runat="server" CssClass="lblNormalDerecha"></asp:Label>
                    </td>
                    <td class="Separadores" align="center">
                        &nbsp;<asp:Label ID="lblDMBanc" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                    </td>
                    <td class="Separadores" align="center">
                        <asp:Label ID="lblRBanc" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                    </td>
                    <td class="Separadores" align="center">
                        <asp:Label ID="lblCEMBanco" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                    </td>
                    <td class="Separadores" align="center">
                        <asp:Label ID="lblCEBanc" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td class="lblSubTitInfoSeparador">
                    Factoring</td>
                    <td class="Separadores" align="right" >
                        <asp:Label ID="lblSCFact" runat="server" CssClass="lblNormalDerecha"></asp:Label>
                    </td>
                    <td class="Separadores" align="center"">
                        &nbsp;<asp:Label ID="lblDMFact" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                    </td>
                    <td class="Separadores" align="center">
                        <asp:Label ID="lblRFact" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                    </td>
                    <td align="center" class="Separadores">
                        <asp:Label ID="lblCEMFactoring" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                    </td>
                    <td class="Separadores" align="center">
                        <asp:Label ID="lblCEFact" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfoSeparador">
                    Sufi</td>
                    <td class="Separadores" align="right" >
                        <asp:Label ID="lblSCSuf" runat="server" CssClass="lblNormal"></asp:Label>
                    </td>
                    <td class="Separadores" align="center">
                        <asp:Label ID="lblDMSuf" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                    </td>
                    <td class="Separadores" align="center" >
                        <asp:Label ID="lblRSuf" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                    </td>
                    <td align="center" class="Separadores">
                        <asp:Label ID="lblCEMSufi" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                    </td>
                    <td class="Separadores" align="center" >
                        <asp:Label ID="lblCESuf" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfoSeparador">
                    Leasing</td>
                    <td class="Separadores" align="right" >
                        <asp:Label ID="lblSCLea" runat="server" CssClass="lblNormal"></asp:Label>
                    </td>
                    <td class="Separadores" align="center">
                        <asp:Label ID="lblDMLea" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                    </td>
                    <td class="Separadores" align="center">
                        <asp:Label ID="lblRLea" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                    </td>
                    <td align="center" class="Separadores">
                        <asp:Label ID="lblCEMLeasing" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                    </td>
                    <td class="Separadores" align="center">
                        <asp:Label ID="lblCELea" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                    </td>
                </tr>
            </table>
            
            &nbsp;</asp:Panel>
        <asp:Panel ID="pnlExterior" runat="server">
            <table class="lblNormal" width="50%">
                <tr>
                    <td class="lblLinea">
                    EXTERIOR
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
            <table class="Separadores" width="100%"> <%--PMO19939 - RF061: Modificar visualización “Consulta Proceso Anteriores” del comité presencial--%>
                <tr>
                    <td class="TituloPrincipal">
                        &nbsp;
                    </td>
                    <td class="TituloPrincipalSeparadores">
                        Saldo K
                    </td>
                    <td class="TituloPrincipalSeparadores">
                        Saldo I
                    </td>
                    <td class="TituloPrincipalSeparadores">
                        Días de Mora
                    </td>
                    <td class="TituloPrincipalSeparadores">
                        Reestructurado <%--PMO19939 - RF061: Modificar visualización “Consulta Proceso Anteriores” del comité presencial--%>
                    </td>
                    <td class="TituloPrincipalSeparadores">
                        Calif. Externa Actual <%--PMO19939 - RF061: Modificar visualización “Consulta Proceso Anteriores” del comité presencial--%>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfoSeparador">
                        Panamá
                    </td>
                    <td class="Separadores" align="right">
                        <asp:Label ID="lblSKPan" runat="server" CssClass="lblNormal"></asp:Label>
                    </td>
                    <td class="Separadores" align="right">
                        <asp:Label ID="lblSIPan" runat="server" CssClass="lblNormal"></asp:Label>
                    </td>
                     <%--PMO19939 - RF061: Modificar visualización “Consulta Proceso Anteriores” del comité presencial--%>
                    <td align="center" class="Separadores">
                        <asp:Label ID="lblDiasMoraPan" runat="server"></asp:Label>
                    </td>
                    <td align="center" class="Separadores">
                        <asp:Label ID="lblReestructPan" runat="server"></asp:Label>
                    </td>
                    <td align="center" class="Separadores">
                        <asp:Label ID="lblCalExtPan" runat="server"></asp:Label>
                    </td>
                     <%----%>
                </tr>
<%--                PMO19939 - RF061: Modificar visualización “Consulta Proceso Anteriores” del comité presencial
                <tr>
                    <td class="lblSubTitInfoSeparador">
                    Miami</td>
                    <td class="Separadores" align="right">
                        <asp:Label ID="lblSKMia" runat="server" CssClass="lblNormal"></asp:Label>
                    </td>
                    <td class="Separadores" align="right">
                        <asp:Label ID="lblSIMia" runat="server" CssClass="lblNormal"></asp:Label>
                    </td>
                    
                </tr>--%>
                <tr>
                    <td class="lblSubTitInfoSeparador">
                        Puerto Rico
                    </td>
                    <td class="Separadores" align="right">
                        <asp:Label ID="lblSKPR" runat="server" CssClass="lblNormal"></asp:Label>
                    </td>
                    <td class="Separadores" align="right">
                        <asp:Label ID="lblSIPR" runat="server" CssClass="lblNormal"></asp:Label>
                    </td>
                    <%--PMO19939 - RF061: Modificar visualización “Consulta Proceso Anteriores” del comité presencial--%>
                    <td align="center" class="Separadores">
                        <asp:Label ID="lblDiasMoraPR" runat="server"></asp:Label>
                    </td>
                    <td align="center" class="Separadores">
                        <asp:Label ID="lblReestructPR" runat="server"></asp:Label>
                    </td>
                    <td align="center" class="Separadores">
                        <asp:Label ID="lblCalExtPR" runat="server"></asp:Label>
                    </td>
                    <%----%>
                </tr>
                
            </table>
            &nbsp;<br />
            <asp:Label ID="lblCifras" runat="server" CssClass="lblNormalBold" 
                Text="Nota: Cifras en Pesos Colombianos"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnlInformacionFinancieraNo" runat="server">
            <table class="lblNormal" border="0" width="100%">
                <tr>
                    <td class="lblLinea">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblLinea">
                        <hr /></td>
                </tr>
                <tr>
                    <td class="lblLinea">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblLinea">
                        INFORMACIÓN FINANCIERA:
                    </td>
                </tr>
                <tr>
                    <td class="lblLinea">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblInformacionFinanciera" runat="server" 
                        Text="Información Financiera aún no disponible en el sistema" 
                        CssClass="lblErrorBold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="6">
                        <hr /></td>
                </tr>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlInformacionFinanciera" runat="server">
            <table class="lblNormal" border="0" width="100%">
                <tr>
                    <td colspan="2" class="lblLinea">
                        &nbsp;</td>
                    <td colspan="4">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblLinea" colspan="6">
                        <hr /></td>
                </tr>
                <tr>
                    <td class="lblLinea" colspan="2">
                        &nbsp;</td>
                    <td colspan="4">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblLinea" colspan="3">
                        <asp:Label ID="Label1" runat="server" Text="INFORMACIÓN FINANCIERA: "></asp:Label>
                        <asp:Label ID="lblFechaInfoFinanciera" runat="server" CssClass="lblNormalBold"></asp:Label>
                    </td>
                    <td class="lblLinea" colspan="3">
                        <asp:Label ID="Label3" runat="server" Text="INFORMACIÓN FINANCIERA SOLICITADA: "></asp:Label>
                        <asp:Label ID="lblFinancieraSolicitada" runat="server" CssClass="lblNormalBold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td colspan="3">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfoBolderCentrado" colspan="2">
                        PyG</td>
                    <td class="lblSubTitInfoBolderCentrado" colspan="2">
                        Balance</td>
                    <td class="lblSubTitInfoBolderCentrado" colspan="2">
                        Indicadores</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Ventas:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblVentas" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfoPadding">
                        Pasivo Financiero CP:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblEndCP" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfoPadding">
                        %Var. Ventas:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblVarVentas" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Ebitda en pesos:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblEbidtaPesos" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfoPadding">
                        Pasivo Financiero LP:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblEndLP" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfoPadding">
                        Endeudamiento:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblEnd" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="lblNormal">
                    <td class="lblSubTitInfo">
                        Utilidad Operacional:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblUtilidadOp" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfoPadding">
                        Capital Social:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblCapitalSocial" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfoPadding">
                        Margen EBITDA:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblMargenEB" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Intereses Pagados:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblCostoFin" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfoPadding">
                        Total Activo:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblTotalActivo" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfoPadding">
                        Cobertura Ebitda:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblCobertEB" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Utilidad Neta:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblUtilidadNe" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfoPadding">
                        Total Pasivo:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblTotalPasivo" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfoPadding">
                        Margen Neto:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblMargNeto" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Utilidad Bruta:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblUtilidadBru" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfoPadding">
                        Superávit de Capital:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblSuperavit" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfoPadding">
                        Rotación de Cartera:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblRotacionCart" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblSubTitInfoPadding">
                        Reservas:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblReservas" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfoPadding">
                        Rotación de Inventarios:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblRotacionInv" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblSubTitInfoPadding">
                        Patrimonio:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblPatrimonio" runat="server"></asp:Label></td>
                        <td class="lblSubTitInfoPadding">
                            Ciclo Financiero:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblCicloFinanciero" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblSubTitInfo">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                        <td class="lblSubTitInfoPadding">
                            Deuda/Ebitda:</td>
                    <td class="lblNormalDerecha">
                        <asp:Label ID="lblDeudaEbitda" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                        <td class="lblNormal">
                        &nbsp;
                    </td>
                    <td class="lblNormal">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="lblNormalBold">
                        <asp:Label ID="lblCausalDisolucion" runat="server" 
                            Text="Cumple Causal de disolución:"></asp:Label>
                    </td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCausalDisulocionResp" runat="server"></asp:Label>
                    </td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="6">
                        <asp:Label ID="lblCifras2" runat="server" CssClass="lblNormalBold" 
                            Text="Nota: Cifras en Pesos Colombianos"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="6">
                        <hr /></td>
                </tr>
                <tr>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal" colspan="3">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlCalificacionPIC" runat="server">
            <table class="lblNormal" border="0" width="100%">
                <tr>
                    <td colspan="2" class="lblLinea">
                        CALIFICACIÓN</td>
                    <td colspan="2" class="lblLinea">
                        CALIFICACIONES A INGRESAR</td>
                </tr>
                <tr>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td class="lblLinea">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
				<%-- 
                <tr>
                    <td class="lblSubTitInfo">
                        Calificación Interna Actual:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalifIntP" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        Calificación Interna Ratificada:
                    </td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalifInternaRPBHis" runat="server" ></asp:Label>
                    </td>
                </tr> --%>
                <tr>
                    <td class="lblSubTitInfo">
                        Calificación Interna Actual Nuevo Rating:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalificacionINRatingPIC" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        Calificación Interna Ratificada Nuevo Rating:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalifInternaRNRHis" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Fecha Calificación Interna Actual:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblFechaCalifP" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        Calificación Externa Ratificada:
                    </td class="lblNormal">
                    <td>
                        <asp:Label ID="lblCalifExternaPHis" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Seguimiento de Covenants:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCovenantP" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        Seguimiento en próximo Comité:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblSeguimientoHis" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="lblNormal">
<%--                    <td class="lblSubTitInfo">
                        Calificación MAF:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalifMAFP" runat="server"></asp:Label>
                    </td>--%>
                    <td class="lblSubTitInfo">
                        Calificación MAF Nuevo Rating:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblMafNuevo" runat="server"></asp:Label>
                    </td>
                    

                    <td class="lblSubTitInfo">
                        Recomendación AEC:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblRecomendacionHis" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
<%--                    <td class="lblSubTitInfo">
                        Calificación MAF Nuevo Rating:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalMAFNuevoRatingPic" runat="server"></asp:Label>
                    </td>--%>
                    <td class="lblSubTitInfo">
                        Calificación Recomendada (Alineación – PEC)</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalifRecPEC" runat="server"></asp:Label>
                    </td>                      
                    <td class="lblSubTitInfo">
                        Razón Calificación:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblRazonHis" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
<%--                    <td class="lblSubTitInfo">
                        Calificación Interna Recomendada:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalifIntRecom" runat="server"></asp:Label>
                    </td>--%>
                    <td class="lblSubTitInfo">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>                  
                     <%--PMO19939 - RF061: Modificar visualización “Consulta Proceso Anteriores” del comité presencial--%>
                    <td class="lblSubTitInfo">
                        Tipo Cliente:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblTipoCliente" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Calificación Nuevo Rating Recomendada:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalNuevoRRecomHis" runat="server"></asp:Label>
                    </td>
                     <%--end--%>
                    <td class="lblSubTitInfo">
                         Estado Calificación:</td>
                    <td class="lblNormal">
                         <asp:Label ID="lblEstadoCalificacionHis" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Fecha MAF:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblFechaMAFHis" runat="server"></asp:Label>
                    </td>
                    <%--PMO19939 - RF061: Modificar visualización “Consulta Proceso Anteriores” del comité presencial--%>
                    <td class="lblSubTitInfo">
                        Utilizó EEFF Cargados en la Herramienta:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblUtilizoEEFF" runat="server"></asp:Label></td>
                        <%--end--%>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Puntaje MAF:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblPuntajeHis" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Cliente Pertenece a IFRS:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblClientePerteneceIFRS" runat="server"></asp:Label></td>
                    <td class="lblSubTitInfo">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlCalificacionSuper" runat="server">
 <%--           <table class="lblNormal" border="0" >
                <tr>
                    <td colspan="6" class="lblLinea">
                        CALIFICACIÓN</td>
                </tr>
                <tr>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td class="lblLinea">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                        <td>
                        &nbsp;</td>
                        <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Calificación Interna Ratificada:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalifIintRatif" runat="server"></asp:Label>
                    </td>
                    <td class="lblLinea">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                        <td>
                        &nbsp;</td>
                        <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Calificación Interna Ratificada Nuevo Rating:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalificacionIRNR" runat="server"></asp:Label>
                    </td>
                    <td class="lblLinea">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                        <td>
                        &nbsp;</td>
                        <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Calificación Externa Ratificada:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalifExtRatif" runat="server"></asp:Label>
                    </td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                        <td>
                        &nbsp;</td>
                        <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Calificación Interna Actual:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalInternaActualSuper" runat="server"></asp:Label>
                    </td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                        <td>
                        &nbsp;</td>
                        <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Razón Calificación:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblRazonCalifInternaSuper" runat="server"></asp:Label>
                    </td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                        <td>
                        &nbsp;</td>
                        <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Calificación MAF:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalifMafSuper" runat="server"></asp:Label>
                    </td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                        <td>
                        &nbsp;</td>
                        <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Calificación MAF Nuevo Rating:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalMAFNuevoRatingSup" runat="server"></asp:Label>
                    </td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        <asp:Label ID="lblClientePerteneceIFRSLabel" Text="Cliente Pertenece a IFRS:" runat="server"></asp:Label></td>
                    <td class="lblNormal">
                        <asp:Label ID="lblClientePerteneceIFRSCampo" runat="server"></asp:Label></asp:Label>
                    </td>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>--%>
            <table class="lblNormal" border="0" width="100%">
                <tr>
                    <td colspan="2" class="lblLinea">
                        CALIFICACIÓN</td>
                    <td colspan="2" class="lblLinea">
                        CALIFICACIONES A INGRESAR</td>
                </tr>
                <tr>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td class="lblLinea">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Calificación Interna Actual Nuevo Rating:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalificacionINRatingPICSuper" runat="server"></asp:Label> 
                    </td>
                    <td class="lblSubTitInfo">
                        Calificación Nuevo Rating Recomendada:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalNuevoRRecomHisSuper" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Fecha Califi. Interna Actual:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblFechaCalifPSuper" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        <asp:Label ID="lblClientePerteneceIFRSLabel" Text="Cliente Pertenece a IFRS:" runat="server"></asp:Label></td>
                    <td class="lblNormal">
                        <asp:Label ID="lblClientePerteneceIFRSCampo" runat="server"></asp:Label></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Seguimiento de Covenants:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCovenantPSuper" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                </tr>
                <tr class="lblNormal">

                    <td class="lblSubTitInfo">
                        Calificación MAF Nuevo Rating:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalMAFNuevoRatingSup" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                </tr>
            </table>            
        </asp:Panel>
        <asp:Panel ID="pnlComentarios" runat="server">
            <table class="lblNormal" border="0" width="100%">
                <tr>
                    <td class="lblLinea">
                        &nbsp;</td>
                    <td class="lblLinea">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblLinea">
                        RECOMENDACIÓN</td>
                    <td class="lblLinea">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo" colspan="2">
                        Actividad del cliente</td>
                </tr>
                <tr>
                    <td class="lblDescripcion" colspan="2">
                        (Describir brevemente la actividad que realiza el cliente)</td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <asp:TextBox ID="txtCliente" runat="server" Height="100px" TextMode="MultiLine" 
                            Width="96%" CssClass="txtCampo117" onkeyup="Count(this,2000)" 
                            ReadOnly="True"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtCliente_FilteredTextBoxExtender" 
                                runat="server" TargetControlID="txtCliente"
                                FilterMode="InvalidChars" 
                                InvalidChars="'">
                            </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo" colspan="2">
                        Análisis de cifras financieras</td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <span class="lblNormal">Descripción de los hechos relevantes que ocasionan 
                        variaciones en las cifras del PyG </span><span class="lblDescripcion">(No es 
                        transcripción de cifras financieras)</span></td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <asp:TextBox ID="txtPyG" runat="server" Height="100px" TextMode="MultiLine" 
                            Width="96%" CssClass="txtCampo117" onkeyup="Count(this,2000)" 
                            ReadOnly="True"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtPyG_FilteredTextBoxExtender" 
                                runat="server" TargetControlID="txtPyG"
                                FilterMode="InvalidChars" 
                                InvalidChars="'">
                            </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <span class="lblNormal">Descripción de los hechos relevantes que ocasionan 
                        variaciones en las cifras del Balance </span><span class="lblDescripcion">(No es 
                        transcripción de cifras financieras)</span></td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <asp:TextBox ID="txtBalance" runat="server" Height="100px" TextMode="MultiLine" 
                            Width="96%" CssClass="txtCampo117" onkeyup="Count(this,2000)" 
                            ReadOnly="True"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtBalance_FilteredTextBoxExtender" 
                                runat="server" TargetControlID="txtBalance"
                                FilterMode="InvalidChars" 
                                InvalidChars="'">
                            </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo" colspan="2">
                        Hechos significativos que aporten a la calificación recomendada</td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <asp:TextBox ID="txtComentariosAdicionales" runat="server" Height="100px" 
                            TextMode="MultiLine" Width="96%" CssClass="txtCampo117" 
                            onkeyup="Count(this,2000)" ReadOnly="True"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtComentariosAdicionales_FilteredTextBoxExtender" 
                                runat="server" TargetControlID="txtComentariosAdicionales"
                                FilterMode="InvalidChars" 
                                InvalidChars="'">
                            </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo" colspan="4">
                        <asp:Label ID="lblComentariosRiesgos" runat="server" 
                            Text="Comentarios Riesgos - Sustentación Calificación">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="4">
                        <asp:TextBox ID="txtComentariosRiesgos" runat="server" CssClass="txtCampo117" 
                            Height="100px" ReadOnly="true" TextMode="MultiLine" Width="96%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <asp:ValidationSummary ID="vsErrores" runat="server" CssClass="lblError" 
                            DisplayMode="List" ValidationGroup="Guardar" Height="54px" 
                            ShowMessageBox="True" ShowSummary="False" 
                            HeaderText="" />
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:Panel>
        <%--Nuevo panel para consulta periodo apartir del historico 201812 PMO37494 --%>
        <asp:Panel ID="pnlCalificacionPICPdo" runat="server">
            <table class="lblNormal" border="0" width="100%">
                <tr>
                    <td colspan="2" class="lblLinea">
                        CALIFICACIÓN
                    </td>
                    <td colspan="2" class="lblLinea">
                        CALIFICACIONES A INGRESAR
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td class="lblLinea">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Calificación Interna Actual:
                    </td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalificacionINRatingPICPdo" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        Calificación Interna Ratificada:
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddlCalifInternaRNRPdo"
                            Operator="NotEqual" ValidationGroup="Guardar" ValueToCompare="0">*</asp:CompareValidator>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCalifInternaRNRPdo" runat="server" CssClass="listCampo">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Fecha Calificación Interna Actual:
                    </td>
                    <td class="lblNormal">
                        <asp:Label ID="lblFechaCalifPPdo" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        Calificación Externa Ratificada:<asp:CompareValidator ID="cvCalExtRat" runat="server"
                            ControlToValidate="ddlCalifExternaPPdo" Operator="NotEqual" ValidationGroup="Guardar"
                            ValueToCompare="0">*</asp:CompareValidator>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCalifExternaPPdo" runat="server" AutoPostBack="True" CssClass="listCampo" > <%--OnSelectedIndexChanged="ddlCalificacionExternaChanged" --%>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Seguimiento de Covenants:
                    </td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCovenantPPdo" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        Seguimiento en próximo Comité:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSeguimientoPdo" runat="server" CssClass="listCampo">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="lblNormal">
                    <td class="lblSubTitInfo">
                        Calificación Modelo Rating:
                    </td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalMAFNuevoRatingPicPdo" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        Recomendación AEC:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRecomendacionPdo" runat="server" CssClass="listCampo">
                        </asp:DropDownList>
                    </td>
                  </tr>
                    <tr class="lblNormal">
                        <td class="lblSubTitInfo">
                            Calificación Recomendada Alineación Grupo Bancolombia:
                        </td>
                        <td class="lblNormal">
                            <asp:Label ID="lblCalifRecomenGrupoPdo" runat="server"></asp:Label>
                        </td>
                        <td class="lblSubTitInfo">
                            Razón Calificación:
                        </td>
                        <td class="lblNormal">
                            <div style="overflow-y: scroll; width: 300px; height: 150px">
                                <%-- <asp:CheckBoxList ID="ddlRazonPdo" runat="server">
                                </asp:CheckBoxList>--%>
                                <asp:Label ID="lblRazonPdo" runat="server"></asp:Label>
                            </div>
                        </td>
                    </tr>
                     <tr>
                        <td class="lblSubTitInfo">
                            <!-- Calificación Recomendada PEC: -->
                            Calificación Recomendada PEC (Sector Financiero)
                        </td>
                        <td class="lblNormal">
                            <asp:Label ID="lblCalifRecPECPdo" runat="server"></asp:Label>
                        </td>
                      </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                El cliente se encuentra en lista de control:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblListasdeControlPdo" runat="server">
                                </asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Tipo Cliente:
                            </td>
                            <td class="lblNormal">
                                <asp:DropDownList ID="ddlTipoClientePdo" CssClass="listCampo" runat="server" 
                                    Height="16px" Width="182px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Calificación Recomendada:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblCalNuevoRRecomPdo" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Estado Calificación:
                                <asp:CompareValidator ID="cvEstadoCalificacionPdo" runat="server" ControlToValidate="ddlEstadoCalPdo"
                                    Operator="NotEqual" ValidationGroup="Guardar" ValueToCompare="0">*</asp:CompareValidator>
                            </td>
                            <td class="lblNormal">
                                <asp:DropDownList ID="ddlEstadoCalPdo" runat="server" CssClass="listCampo" 
                                    Height="16px" Width="182px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                         <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblSubTitInfo">
                                Utilizó EEFF Cargados en la Herramienta:
                            </td>
                            <td class="lblNormal">
                                <asp:DropDownList ID="ddlUtilizoEEFFPdo" AutoPostBack="True" 
                                    CssClass="listCampo" runat="server" Height="16px" Width="182px" > 
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                        </tr>
            </table>
        </asp:Panel>
        
        <asp:Panel ID="PanelCalificacionComercialPdo" runat="server">
            <table class="lblNormal" border="0" width="100%">
                <tr>
                    <td colspan="2" class="lblLinea">
                        CALIFICACIÓN</td>
                    <td colspan="2" class="lblLinea">
                        CALIFICACIONES A INGRESAR</td>
                </tr>
                <tr>
                    <td class="lblNormal">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td class="lblLinea">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Calificación Interna Actual:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalificacionInRatingPdoCom" runat="server"></asp:Label> 
                    </td>
                    <td class="lblSubTitInfo">
                        Calificación Interna Ratificada:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCalNRRecomPdo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Fecha Califiicación Interna Actual:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblFechaCalifCPdo" runat="server"></asp:Label>
                    </td>
                    <%-- <td class="lblSubTitInfo">
                        <asp:Label ID="lblPerteneceIFRSPdo" Text="Cliente Pertenece a IFRS:" runat="server"></asp:Label></td>
                    <td class="lblNormal">
                        <asp:Label ID="lblPerteneceIFRSPdoCampo" runat="server"></asp:Label></asp:Label>
                    </td>--%>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Seguimiento de Covenants:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblCovenantCPdo" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                </tr>
                <tr class="lblNormal">

                    <td class="lblSubTitInfo">
                        El cliente se encuentra en lista de control:</td>
                    <td class="lblNormal">
                        <asp:Label ID="lblListadeControlComPdo" runat="server"></asp:Label>
                    </td>
                    <td class="lblSubTitInfo">
                        &nbsp;</td>
                    <td class="lblNormal">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:Panel>
        
        <asp:Panel ID="pnlComentariosGeneralPdo" runat="server">
            <table class="lblNormal" border="0" width="100%">
                <tr>
                    <td class="lblLinea">
                        &nbsp;
                    </td>
                    <td class="lblLinea">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="lblLinea">
                        RECOMENDACIÓN
                    </td>
                    <td class="lblLinea">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo" align="center">
                        SUSTENTACIÓN DE CALIFICACIÓN RECOMENDADA
                    </td>
                </tr>
                <tr>
                    <td class="lblDescripcion" align="center">
                        (Describir actividad del cliente, hechos relevantes que generan variaciones en las cifras y argumentos que sustentan la calificación recomendada)
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <asp:TextBox ID="txtSustentacionCalRecPdo" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                            onkeyup="Count(this,3000)" MaxLength="3000" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
               </table>
           </asp:Panel>
            <asp:Panel ID="pnlComentariosPdo" runat="server">
            <table class="lblNormal" border="0" width="100%">
                <tr>
                    <td class="lblNormal" colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        Calificación General
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                    <asp:Label ID="lblPregunta1Pdo" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                    </td>
                    <td class="lblNormal">
                        <asp:DropDownList ID="ddlPregunta1Pdo" runat="server"  CssClass="listCampoPreguntas">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblDescripcion" colspan="2">
                    <asp:Label ID="lblComplementoPregunta1Pdo" runat="server"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <asp:TextBox ID="txtRespuestaPregunta1Pdo" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                        onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td class="lblSubTitInfo">
                        <asp:Label ID="lblPregunta2Pdo" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                    </td>
                    <td class="lblNormal">
                        <asp:DropDownList ID="ddlPregunta2Pdo" runat="server" CssClass="listCampoPreguntas">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblDescripcion" colspan="2">
                    <asp:Label ID="lblComplementoPregunta2Pdo" runat="server"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <asp:TextBox ID="txtRespuestaPregunta2Pdo" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                        onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td class="lblSubTitInfo">
                        <asp:Label ID="lblPregunta3Pdo" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                    </td>
                    <td class="lblNormal">
                        <asp:DropDownList ID="ddlPregunta3Pdo" runat="server" CssClass="listCampoPreguntas">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblDescripcion" colspan="2">
                    <asp:Label ID="lblComplementoPregunta3Pdo" runat="server"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <asp:TextBox ID="txtRespuestaPregunta3Pdo" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                            onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td class="lblSubTitInfo">
                        <asp:Label ID="lblPregunta4Pdo" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                    </td>
                    <td class="lblNormal">
                        <asp:DropDownList ID="ddlPregunta4Pdo" runat="server" CssClass="listCampoPreguntas">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblDescripcion" colspan="2">
                    <asp:Label ID="lblComplementoPregunta4Pdo" runat="server"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <asp:TextBox ID="txtRespuestaPregunta4Pdo" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                        onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td class="lblSubTitInfo">
                        <asp:Label ID="lblPregunta5Pdo" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                    </td>
                    <td class="lblNormal">
                        <asp:DropDownList ID="ddlPregunta5Pdo" runat="server" CssClass="listCampoPreguntas">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblDescripcion" colspan="2">
                    <asp:Label ID="lblComplementoPregunta5Pdo" runat="server"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <asp:TextBox ID="txtRespuestaPregunta5Pdo" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                        onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                  <tr>
                    <td class="lblSubTitInfo">
                        <asp:Label ID="lblPregunta6Pdo" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                    </td>
                    <td class="lblNormal">
                        <asp:DropDownList ID="ddlPregunta6Pdo" runat="server" CssClass="listCampoPreguntas">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblDescripcion" colspan="2">
                    <asp:Label ID="lblComplementoPregunta6Pdo" runat="server"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <asp:TextBox ID="txtRespuestaPregunta6Pdo" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                            onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td class="lblSubTitInfo">
                        <asp:Label ID="lblPregunta7Pdo" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                    </td>
                    <td class="lblNormal">
                        <asp:DropDownList ID="ddlPregunta7Pdo" runat="server" CssClass="listCampoPreguntas">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblDescripcion" colspan="2">
                    <asp:Label ID="lblComplementoPregunta7Pdo" runat="server"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <asp:TextBox ID="txtRespuestaPregunta7Pdo" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                        onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td class="lblSubTitInfo">
                        <asp:Label ID="lblPregunta8Pdo" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                    </td>
                    <td class="lblNormal">
                        <asp:DropDownList ID="ddlPregunta8Pdo" runat="server" CssClass="listCampoPreguntas">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblDescripcion" colspan="2">
                    <asp:Label ID="lblComplementoPregunta8Pdo" runat="server"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <asp:TextBox ID="txtRespuestaPregunta8Pdo" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                            onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="lblSubTitInfo">
                        <asp:Label ID="lblPregunta9Pdo" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                    </td>
                    <td class="lblNormal">
                        <asp:DropDownList ID="ddlPregunta9Pdo" runat="server" CssClass="listCampoPreguntas">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblDescripcion" colspan="2">
                    <asp:Label ID="lblComplementoPregunta9Pdo" runat="server"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="lblNormal" colspan="2">
                        <asp:TextBox ID="txtRespuestaPregunta9Pdo" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                        onkeyup="Count(this,1400)" MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                    <tr>
                        <td class="lblLinea" colspan="2">
                            <hr />
                        </td>
                        <tr>
                            <td align="center" align="center" class="lblErrorBoldPreguntas">
                                Calificación para proyectos o sociedades Propósito
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="lblLinea">
                                (Diligencie sólo esta información si la exposición del cliente a calificar 
                                corresponde únicamente al desarrollo de un proyecto específico)
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <tr>
                                <td class="lblSubTitInfo">
                                    <asp:Label ID="lblPregunta10Pdo" runat="server" CssClass="lblSubTitInfo">
                                    </asp:Label>
                                </td>
                                <td class="lblNormal">
                                    <asp:DropDownList ID="ddlPregunta10Pdo" runat="server" 
                                        CssClass="listCampoPreguntas">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblDescripcion" colspan="2">
                                    <asp:Label ID="lblComplementoPregunta10Pdo" runat="server"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblNormal" colspan="2">
                                    <asp:TextBox ID="txtRespuestaPregunta10Pdo" runat="server" 
                                        CssClass="txtCampo117" Height="50px" MaxLength="1000" 
                                        onkeyup="Count(this,1400)" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblSubTitInfo">
                                    <asp:Label ID="lblPregunta11Pdo" runat="server" CssClass="lblSubTitInfo">
                                    </asp:Label>
                                </td>
                                <td class="lblNormal">
                                    <asp:DropDownList ID="ddlPregunta11Pdo" runat="server" 
                                        CssClass="listCampoPreguntas">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblDescripcion" colspan="2">
                                    <asp:Label ID="lblComplementoPregunta11Pdo" runat="server"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblNormal" colspan="2">
                                    <asp:TextBox ID="txtRespuestaPregunta11Pdo" runat="server" 
                                        CssClass="txtCampo117" Height="50px" MaxLength="1000" 
                                        onkeyup="Count(this,1400)" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblSubTitInfo">
                                    <asp:Label ID="lblPregunta12Pdo" runat="server" CssClass="lblSubTitInfo">
                                    </asp:Label>
                                </td>
                                <td class="lblNormal">
                                    <asp:DropDownList ID="ddlPregunta12Pdo" runat="server" 
                                        CssClass="listCampoPreguntas">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblDescripcion" colspan="2">
                                    <asp:Label ID="lblComplementoPregunta12Pdo" runat="server"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblNormal" colspan="2">
                                    <asp:TextBox ID="txtRespuestaPregunta12Pdo" runat="server" 
                                        CssClass="txtCampo117" Height="50px" MaxLength="500" 
                                        onkeyup="Count(this,500)" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblSubTitInfo">
                                    <asp:Label ID="lblPregunta13Pdo" runat="server" CssClass="lblSubTitInfo">
                                    </asp:Label>
                                </td>
                                <td class="lblNormal">
                                    <asp:DropDownList ID="ddlPregunta13Pdo" runat="server" 
                                        CssClass="listCampoPreguntas">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblDescripcion" colspan="2">
                                    <asp:Label ID="lblComplementoPregunta13Pdo" runat="server"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblNormal" colspan="2">
                                    <asp:TextBox ID="txtRespuestaPregunta13Pdo" runat="server" 
                                        CssClass="txtCampo117" Height="50px" MaxLength="500" 
                                        onkeyup="Count(this,500)" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblSubTitInfo">
                                    <asp:Label ID="lblPregunta14Pdo" runat="server" CssClass="lblSubTitInfo">
                                    </asp:Label>
                                </td>
                                <td class="lblNormal">
                                    <asp:DropDownList ID="ddlPregunta14Pdo" runat="server" 
                                        CssClass="listCampoPreguntas">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblDescripcion" colspan="2">
                                    <asp:Label ID="lblComplementoPregunta14Pdo" runat="server"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblNormal" colspan="2">
                                    <asp:TextBox ID="txtRespuestaPregunta14Pdo" runat="server" 
                                        CssClass="txtCampo117" Height="50px" MaxLength="500" 
                                        onkeyup="Count(this,500)" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                </td>
                            </tr>
                            </tr>  
                            <asp:Panel ID="pnlComentariosPdoFI" runat="server">
                          <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="lblLinea" colspan="2">
                                <hr />
                            </td>
                            <tr>
                                <td align="center" align="center" class="lblErrorBoldPreguntas">
                                    Calificación para Fondos Inmobiliaros
                                </td>
                            </tr>
                                <td>
                                    &nbsp;
                                </td>

                                <tr>
                                <td class="lblSubTitInfo">
                                    <asp:Label ID="lblPregunta15Pdo" runat="server" CssClass="lblSubTitInfo">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblDescripcion" colspan="2">
                                    <asp:Label ID="lblComplementoPregunta15Pdo" runat="server"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblNormal" colspan="2">
                                    <asp:TextBox ID="txtRespuestaPregunta15Pdo" runat="server" 
                                        CssClass="txtCampo117" Height="50px" MaxLength="1000" 
                                        onkeyup="Count(this,1400)" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                </td>
                            </tr>

                             <tr>
                                <td class="lblSubTitInfo">
                                    <asp:Label ID="lblPregunta16Pdo" runat="server" CssClass="lblSubTitInfo">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblDescripcion" colspan="2">
                                    <asp:Label ID="lblComplementoPregunta16Pdo" runat="server"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblNormal" colspan="2">
                                    <asp:TextBox ID="txtRespuestaPregunta16Pdo" runat="server" 
                                        CssClass="txtCampo117" Height="50px" MaxLength="1000" 
                                        onkeyup="Count(this,1400)" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td class="lblSubTitInfo">
                                    <asp:Label ID="lblPregunta17Pdo" runat="server" CssClass="lblSubTitInfo">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblDescripcion" colspan="2">
                                    <asp:Label ID="lblComplementoPregunta17Pdo" runat="server"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblNormal" colspan="2">
                                    <asp:TextBox ID="txtRespuestaPregunta17Pdo" runat="server" 
                                        CssClass="txtCampo117" Height="50px" MaxLength="1000" 
                                        onkeyup="Count(this,1400)" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td class="lblSubTitInfo">
                                    <asp:Label ID="lblPregunta18Pdo" runat="server" CssClass="lblSubTitInfo">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblDescripcion" colspan="2">
                                    <asp:Label ID="lblComplementoPregunta18Pdo" runat="server"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblNormal" colspan="2">
                                    <asp:TextBox ID="txtRespuestaPregunta18Pdo" runat="server" 
                                        CssClass="txtCampo117" Height="50px" MaxLength="1000" 
                                        onkeyup="Count(this,1400)" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                </td>
                            </tr>
                             </tr>                
                             </asp:Panel> 

                            </table>
                          </asp:Panel> 

        <asp:Panel ID="pnlComentariosPdoGB" runat="server">
            <table class="lblNormal" border="0" width="100%">
                <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="lblLinea" colspan="2">
                                <hr />
                            </td>
                            <tr>
                                <td align="center" align="center" class="lblErrorBoldPreguntas">
                                    Calificación para Gobierno
                                </td>
                            </tr>
                                <td>
                                    &nbsp;
                                </td>

                                <tr>
                                <td class="lblSubTitInfo">
                                    <asp:Label ID="lblPregunta19Pdo" runat="server" CssClass="lblSubTitInfo">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblDescripcion" colspan="2">
                                    <asp:Label ID="lblComplementoPregunta19Pdo" runat="server"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblNormal" colspan="2">
                                    <asp:TextBox ID="txtRespuestaPregunta19Pdo" runat="server" 
                                        CssClass="txtCampo117" Height="50px" MaxLength="1000" 
                                        onkeyup="Count(this,1400)" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                </td>
                            </tr>

                             <tr>
                                <td class="lblSubTitInfo">
                                    <asp:Label ID="lblPregunta20Pdo" runat="server" CssClass="lblSubTitInfo">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblDescripcion" colspan="2">
                                    <asp:Label ID="lblComplementoPregunta20Pdo" runat="server"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblNormal" colspan="2">
                                    <asp:TextBox ID="txtRespuestaPregunta20Pdo" runat="server" 
                                        CssClass="txtCampo117" Height="50px" MaxLength="1000" 
                                        onkeyup="Count(this,1400)" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td class="lblSubTitInfo">
                                    <asp:Label ID="lblPregunta21Pdo" runat="server" CssClass="lblSubTitInfo">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblDescripcion" colspan="2">
                                    <asp:Label ID="lblComplementoPregunta21Pdo" runat="server"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblNormal" colspan="2">
                                    <asp:TextBox ID="txtRespuestaPregunta21Pdo" runat="server" 
                                        CssClass="txtCampo117" Height="50px" MaxLength="1000" 
                                        onkeyup="Count(this,1400)" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td class="lblSubTitInfo">
                                    <asp:Label ID="lblPregunta22Pdo" runat="server" CssClass="lblSubTitInfo">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblDescripcion" colspan="2">
                                    <asp:Label ID="lblComplementoPregunta22Pdo" runat="server"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblNormal" colspan="2">
                                    <asp:TextBox ID="txtRespuestaPregunta22Pdo" runat="server" 
                                        CssClass="txtCampo117" Height="50px" MaxLength="1000" 
                                        onkeyup="Count(this,1400)" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                </td>
                            </tr>

      </table>
        </asp:Panel> 
        
         <asp:Panel ID="pnlComentariosRiesgosPdo" runat="server">
            <table class="lblNormal" border="0" width="100%">
                            <tr>
                                <td align="center" class="lblSubTitInfo" colspan="2">
                                    <asp:Label ID="lblComentariosRiesgosPdo" runat="server" 
                                        CssClass="lblSubTitInfo" Text="COMENTARIOS RIESGOS - SUSTENTACIÓN CALIFICACIÓN">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblNormal" colspan="2">
                                    <asp:TextBox ID="txtComentariosRiesgosPdo" runat="server" 
                                        CssClass="txtCampo117" Height="100px" MaxLength="3000" 
                                        onkeyup="Count(this,3000)" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                </td>
                            </tr>
                                                 

            </table>
        </asp:Panel> 


        <asp:Panel ID="pnlBotones" runat="server">
            <table>
                <tr>
                    <td colspan="3">
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnBuscarNuevo" runat="server" CssClass="boton" 
                            onclick="btnBuscarNuevo_Click" Text="Buscar Nuevo Cliente" Width="149px" />
                    </td>
                    <td>
                        <asp:Button ID="btnPdf" CssClass="boton" runat="server" onclick="btnPdf_Click" 
                            Text="Generar PDF" />
                    </td>
                    <td>
                &nbsp;</td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
<asp:Panel ID="pnlProgress" runat="server" CssClass="updateProgress" HorizontalAlign="Center"
Style="display: none;">
</asp:Panel>
<asp:ModalPopupExtender ID="mpeProgress" runat="server" TargetControlID="pnlProgress"
PopupControlID="pnlProgress" DropShadow="false" BackgroundCssClass="modalBackgroundProgress"/>
<asp:Panel ID="pnlProgressImg" runat="server" HorizontalAlign="Center" Style="display: none;">
<img alt="" src="../img/CalificacionCartera/ajax-loader.gif" style="top: 400px;" />
<br />
<strong><span style="font-size: 15px; font-family: Tahoma; text-align: center;">Cargando...</span>
</strong>
</asp:Panel>
<asp:ModalPopupExtender ID="mpeProgressImg" runat="server" TargetControlID="pnlProgressImg"
PopupControlID="pnlProgressImg" DropShadow="false" BackgroundCssClass="modalBackgroundProgress"/>
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
