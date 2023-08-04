<%@ Page Title="Calificación de Cliente" Language="C#" MasterPageFile="~/CalificacionCartera/Master/General.Master"
    AutoEventWireup="true" CodeBehind="CalificacionClienteFinm.aspx.cs" Inherits="Bancolombia.Riesgo.MAFWeb.CalificacionCartera.CalificacionClienteFinm" %>

<%@ MasterType VirtualPath="~/CalificacionCartera/Master/General.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style4
        {
        }
        .listCampoPreguntas
        {
            margin-left: 0px; 
        }
    </style>
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

     
        function validarComentarios(oSrc, args) {
            ddlSeguimiento = $("#ctl00_ContentPlaceHolder1_ddlSeguimiento").val();
            ddlRecomendacion = $("#ctl00_ContentPlaceHolder1_ddlRecomendacion").val();
            isValidar = (ddlSeguimiento != "0" || ddlRecomendacion != "0");
            txtComentariosRiesgos = $("#ctl00_ContentPlaceHolder1_txtComentariosRiesgos").val().trim().length;
            args.IsValid = isValidar ? txtComentariosRiesgos > 0 : true;
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
                                &nbsp;
                            </td>
                            <td class="lblSubTitInfoDerecha">
                                Comité de:
                            </td>
                            <td>
                                <asp:Label ID="lblFechaComite" runat="server" CssClass="lblNormal"></asp:Label>
                                &nbsp;
                            </td>

                        </tr>
                        <tr>
                            <td class="lblLinea" colspan="5">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblLinea" colspan="5">
                                DATOS DEL CLIENTE
                            </td>
                        </tr>
                        <tr>
                            <td class="lblLinea" colspan="5">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Nit:
                            </td>
                            <td align="left">
                                <asp:Label ID="lblCliente" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Nit Relacionamiento:
                                
                            </td>
                            <td align="left">
                                <asp:Label ID="lblNitRelacionamiento" runat="server" CssClass="lblNormal"> </asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                         </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Nombre:
                            </td>
                            <td align="left">
                                <asp:Label ID="lblNombreCliente" runat="server"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Button ID="btnRelacionamiento" runat="server" Text="Cargar calificación Nit de Relacionamiento" CssClass="boton" Style="width:250px" OnClick="btnRelacionamiento_Click" Visible="false" 
                                    OnClientClick="return confirm('Seguro desea cargar el Nit de relacionamiento, esto remplazará algunos campos?' );"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Tipo de Comité:
                            </td>
                            <td align="left">
                                <asp:Label ID="lblTipoComite" runat="server"></asp:Label>
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
                            <td class="lblSubTitInfo">
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
                            <td colspan="5">
                                <hr />
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
                        </tr>
                        <tr>
                            <td class="lblLinea" colspan="5">
                                INFORMACIÓN DEL CLIENTE
                            </td>
                        </tr>
                        <tr>
                            <td class="lblLinea" colspan="5">
                                &nbsp;
                            </td>
                        </tr>
                        <tr class="lblNormal">
                            <td class="lblSubTitInfo">
                                Sector económico:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblSector" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Seguimiento recomendado en el comité anterior:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblSeguimiento" runat="server"></asp:Label>
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Segmento:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblSegmento" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Grupo de Riesgo:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblGrupoR" runat="server"></asp:Label>
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Regional:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblRegional" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Nivel de Riesgo AEC:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblRiesgoAEC" runat="server"></asp:Label>
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                <asp:Label ID="lblCalificado" runat="server" Text="Calificado por:"></asp:Label>
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblCalifPor" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
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
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblSubTitInfo">
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
                            <td class="lblSubTitInfo" colspan="5">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblSubTitInfo">
                                &nbsp;
                            </td>
                            <td class="lblNormal" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblLinea" colspan="5">
                                INFORMACIÓN DE ENDEUDAMIENTO
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
                            <td class="lblNormal" colspan="2">
                                &nbsp;
                            </td>
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
                                #VECES MORA 30:
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblMora30" runat="server"></asp:Label>
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                #VECES MORA 60:
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblMora60" runat="server"></asp:Label>
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                MORA MÁXIMA:
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblMoraMax" runat="server"></asp:Label>
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlLocal" runat="server">
                    <table class="lblNormal" width="100%">
                        <tr>
                            <td class="lblLinea">
                                <asp:Label ID="lblLocal" runat="server" Text="LOCAL"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table class="lblNormal" width="35%">
                        <tr>
                            <td class="lblSubTitInfo">
                                Anticipos de leasing:
                            </td>
                            <td>
                                <asp:Label ID="lblSCAnt" runat="server" CssClass="lblNormal"></asp:Label>
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
                    <table class="Separadores" width="100%">
                        <tr>
                            <td class="TituloPrincipal">
                                &nbsp;
                            </td>
                            <td class="TituloPrincipalSeparadores">
                                Saldo 
                            </td>
                            <td class="TituloPrincipalSeparadores">
                                Días de Mora
                            </td>
                            <td class="TituloPrincipalSeparadores">
                                Rees
                            </td>
                            <td class="TituloPrincipalSeparadores">
                                Calif. Externa Modelo SFC
                            </td>
                            <td class="TituloPrincipalSeparadores">
                                Calif. Externa Actual
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfoSeparador">
                                Bancolombia
                            </td>
                            <td class="Separadores" align="right">
                                <asp:Label ID="lblSCBanc" runat="server" CssClass="lblNormalDerecha"></asp:Label>
                            </td>
                            <td class="Separadores" align="center">
                                &nbsp;<asp:Label ID="lblDMBanc" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                            </td>
                            <td class="Separadores" align="center">
                                <asp:Label ID="lblRBanc" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                            </td>
                            <td align="center" class="Separadores">
                                <asp:Label ID="lblCEMBanco" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                            </td>
                            <td class="Separadores" align="center">
                                <asp:Label ID="lblCEBanc" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfoSeparador">
                                Factoring
                            </td>
                            <td class="Separadores" align="right">
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
                                Sufi
                            </td>
                            <td class="Separadores" align="right">
                                <asp:Label ID="lblSCSuf" runat="server" CssClass="lblNormal"></asp:Label>
                            </td>
                            <td class="Separadores" align="center">
                                <asp:Label ID="lblDMSuf" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                            </td>
                            <td class="Separadores" align="center">
                                <asp:Label ID="lblRSuf" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                            </td>
                            <td align="center" class="Separadores">
                                <asp:Label ID="lblCEMSufi" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                            </td>
                            <td class="Separadores" align="center">
                                <asp:Label ID="lblCESuf" runat="server" CssClass="lblNormalCentrado"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfoSeparador">
                                Leasing
                            </td>
                            <td class="Separadores" align="right">
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
                    <table class="lblNormal" width="100%">
                        <tr>
                            <td class="lblLinea">
                                EXTERIOR
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table class="Separadores" width="100%">
                        <tr>
                            <td class="TituloPrincipal" width="15%">
                                &nbsp;
                            </td>
                            <td class="TituloPrincipalSeparadores" width="20%">
                                Saldo K
                            </td>
                            <td class="TituloPrincipalSeparadores" width="20%">
                                Saldo I
                            </td>
                            <td class="TituloPrincipalSeparadores">
                                Días de Mora
                            </td>
                            <td class="TituloPrincipalSeparadores">
                                Reestructurado
                            </td>
                            <td class="TituloPrincipalSeparadores">
                                Calif. Externa Actual
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
                            <td align="center" class="Separadores">
                                <asp:Label ID="lblDiasMoraPan" runat="server" CssClass="lblNormal"></asp:Label>
                            </td>
                            <td align="center" class="Separadores">
                                <asp:Label ID="lblReestructPan" runat="server" CssClass="lblNormal"></asp:Label>
                            </td>
                            <td align="center" class="Separadores">
                                <asp:Label ID="lblCalExtPan" runat="server" CssClass="lblNormal"></asp:Label>
                            </td>
                        </tr>
                        <%-- REQ002: Se excluye la línea miami de la visualización en pantalla (RF017)
                        <tr>
                            <td class="lblSubTitInfoSeparador">
                                Miami
                            </td>
                            <td class="Separadores" align="right">
                                <asp:Label ID="lblSKMia" runat="server" CssClass="lblNormal"></asp:Label>
                            </td>
                            <td class="Separadores" align="right">
                                <asp:Label ID="lblSIMia" runat="server" CssClass="lblNormal"></asp:Label>
                            </td>
                            <td align="right" class="Separadores">
                                &nbsp;</td>
                            <td align="right" class="Separadores">
                                &nbsp;</td>
                            <td align="right" class="Separadores">
                                &nbsp;</td>
                        </tr>
                        --%>
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
                            <td align="center" class="Separadores">
                                <asp:Label ID="lblDiasMoraPR" runat="server" CssClass="lblNormal"></asp:Label>
                            </td>
                            <td align="center" class="Separadores">
                                <asp:Label ID="lblReestructPR" runat="server" CssClass="lblNormal"></asp:Label>
                            </td>
                            <td align="center" class="Separadores">
                                <asp:Label ID="lblCalExtPR" runat="server" CssClass="lblNormal"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    &nbsp;<br />
                    <asp:Label ID="lblCifras" runat="server" CssClass="lblNormalBold" Text="Nota: Cifras en Pesos Colombianos"></asp:Label>
                </asp:Panel>
                <asp:Panel ID="pnlInformacionFinancieraNo" runat="server">
                    <table class="lblNormal" border="0" width="100%">
                        <tr>
                            <td class="lblLinea">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblLinea">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td class="lblLinea">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblLinea">
                                INFORMACIÓN FINANCIERA:
                            </td>
                        </tr>
                        <tr>
                            <td class="lblLinea">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblInformacionFinanciera" runat="server" Text="Información Financiera aún no disponible en el sistema"
                                    CssClass="lblErrorBold"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal">
                                <hr />
                            </td>
                        </tr>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlInformacionFinanciera" runat="server">
                    <table class="lblNormal" border="0" width="100%">
                        <tr>
                            <td colspan="2" class="lblLinea">
                                &nbsp;
                            </td>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblLinea" colspan="6">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td class="lblLinea" colspan="2">
                                &nbsp;
                            </td>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblLinea" colspan="3">
                                <asp:Label ID="Label1" runat="server" Text="INFORMACIÓN FINANCIERA: "></asp:Label>
                                <asp:Label ID="lblFechaInfoFinanciera" runat="server" CssClass="lblNormalBold2"></asp:Label>
                            </td>
                            <td class="lblLinea" colspan="3">
                                <asp:Label ID="Label41" runat="server" Text="INFORMACIÓN FINANCIERA SOLICITADA:"></asp:Label>
                                <asp:Label ID="lblCorteEstFros" runat="server" CssClass="lblNormalBold2"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfoBolderCentrado" colspan="2">
                                PyG
                            </td>
                            <td class="lblSubTitInfoBolderCentrado" colspan="2">
                                Balance
                            </td>
                            <td class="lblSubTitInfoBolderCentrado" colspan="2">
                                Indicadores
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
                            <td class="lblSubTitInfo">
                                Ventas:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblVentas" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfoPadding">
                                Pasivo Financiero CP:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblEndCP" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfoPadding">
                                %Var. Ventas:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblVarVentas" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Ebitda en pesos:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblEbidtaPesos" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfoPadding">
                                Pasivo Financiero LP:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblEndLP" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfoPadding">
                                Endeudamiento:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblEnd" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="lblNormal">
                            <td class="lblSubTitInfo">
                                Utilidad Operacional:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblUtilidadOp" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfoPadding">
                                Capital Social:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblCapitalSocial" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfoPadding">
                                Margen EBITDA:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblMargenEB" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Intereses Pagados:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblCostoFin" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfoPadding">
                                Total Activo:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblTotalActivo" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfoPadding">
                                Cobertura Ebitda:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblCobertEB" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Utilidad Neta:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblUtilidadNe" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfoPadding">
                                Total Pasivo:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblTotalPasivo" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfoPadding">
                                Margen Neto:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblMargNeto" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Utilidad Bruta:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblUtilidadBru" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfoPadding">
                                Superávit de Capital:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblSuperavit" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfoPadding">
                                Rotación de Cartera:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblRotacionCart" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblSubTitInfoPadding">
                                Reservas:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblReservas" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfoPadding">
                                Rotación de Inventarios:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblRotacionInv" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblSubTitInfoPadding">
                                Patrimonio:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblPatrimonio" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfoPadding">
                                Ciclo Financiero:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblCicloFinanciero" runat="server"></asp:Label>
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
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td class="lblSubTitInfoPadding">
                                Deuda/Ebitda:
                            </td>
                            <td class="lblNormalDerecha">
                                <asp:Label ID="lblDeudaEbitda" runat="server"></asp:Label>
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
                            <td class="lblSubTitInfo">
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
                            <td class="lblNormalBold">
                                <asp:Label ID="lblCausalDisolucion" runat="server" Text="Cumple Causal de disolución:"></asp:Label>
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblCausalDisulocionResp" runat="server"></asp:Label>
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
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal" colspan="6">
                                <asp:Label ID="lblCifras2" runat="server" CssClass="lblNormalBold" Text="Nota: Cifras en Pesos Colombianos"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal" colspan="6">
                                <hr />
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
                            <td class="lblNormal" colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlCalificacionComercial" runat="server">
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
                                <asp:Label ID="lblCalificacionINRatingCom" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Calificación Interna Ratificada:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCalNRRecom" runat="server" CssClass="listCampo">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Fecha Calificación Interna Actual:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblFechaCalifC" runat="server"></asp:Label>
                            </td>
                            <%-- 
                            <td>
                                <asp:Label ID="lblPerteneceIFRS" runat="server" CssClass="lblSubTitInfo" Text="Cliente pertenece a IFRS"> </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPerteneceIFRS" runat="server" CssClass="listCampo">
                                </asp:DropDownList>
                            </td>
                            --%>
                        </tr>
                        <tr class="lblNormal">
                            <td class="lblSubTitInfo">
                                Seguimiento de Covenants:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblCovenantC" runat="server"></asp:Label>
                            </td>
                            
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                El cliente se encuentra en lista de control:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblListasdeControlCom" runat="server">
                                </asp:Label>
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
                <asp:Panel ID="pnlCalificacionPIC" runat="server">
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
                                <asp:Label ID="lblCalificacionINRatingPIC" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Calificación Interna Ratificada:
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddlCalifInternaRNR"
                                    Operator="NotEqual" ValidationGroup="Guardar" ValueToCompare="0">*</asp:CompareValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCalifInternaRNR" runat="server" CssClass="listCampo">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Fecha Calificación Interna Actual:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblFechaCalifP" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Calificación Externa Ratificada:<asp:CompareValidator ID="cvCalExtRat" runat="server"
                                    ControlToValidate="ddlCalifExternaP" Operator="NotEqual" ValidationGroup="Guardar"
                                    ValueToCompare="0">*</asp:CompareValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCalifExternaP" runat="server" AutoPostBack="True" CssClass="listCampo" OnSelectedIndexChanged="ddlCalificacionExternaChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Seguimiento de Covenants:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblCovenantP" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Seguimiento en próximo Comité:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSeguimiento" runat="server" CssClass="listCampo">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="lblNormal">
                            <td class="lblSubTitInfo">
                                Calificación Modelo Rating:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblCalMAFNuevoRatingPic" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Recomendación AEC:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRecomendacion" runat="server" CssClass="listCampo">
                                </asp:DropDownList>
                            </td>
                          </tr>
                            <tr class="lblNormal">
                                <td class="lblSubTitInfo">
                                    Calificación Recomendada Alineación Grupo Bancolombia:
                                </td>
                                <td class="lblNormal">
                                    <asp:Label ID="lblCalifRecomenGrupo" runat="server"></asp:Label>
                                </td>
                                <td class="lblSubTitInfo">
                                    Razón Calificación:
                                </td>
                                <td class="lblNormal">
                                
                                    <div style="overflow-y: scroll; width: 300px; height: 150px">
                                        <asp:CheckBoxList ID="ddlRazon" runat="server">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                            </tr>
                        <%--</tr>--%>
                         <tr>
                            <td class="lblSubTitInfo">
                                Calificación Recomendada PEC (Sector Financiero)
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblCalifRecPEC" runat="server"></asp:Label>
                            </td>
                          </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                El cliente se encuentra en lista de control:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblListasdeControl" runat="server">
                                </asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Tipo Cliente:
                            </td>
                            <td class="lblNormal">
                                <asp:DropDownList ID="ddlTipoCliente" CssClass="listCampo" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Calificación Recomendada:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblCalNuevoRRecom" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Estado Calificación:
                                <asp:CompareValidator ID="cvEstadoCalificacion" runat="server" ControlToValidate="ddlEstadoCal"
                                    Operator="NotEqual" ValidationGroup="Guardar" ValueToCompare="0">*</asp:CompareValidator>
                            </td>
                            <td class="lblNormal">
                                <asp:DropDownList ID="ddlEstadoCal" runat="server" CssClass="listCampo">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                        <%-- PMO27494 
                            <td class="lblSubTitInfo">
                                Fecha MAF:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblFechaMaf" runat="server"></asp:Label>
                            </td>
                         --%>
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
                                <asp:DropDownList ID="ddlUtilizoEEFF" AutoPostBack="True" CssClass="listCampo" runat="server" OnSelectedIndexChanged="ddlUtilizoEEFFChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <%-- 
                        <tr>
                            <td class="lblSubTitInfo">
                                Puntaje MAF:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblPuntajeMaf" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Cliente Pertenece IFRS
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblClientePerteneceIFRS" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                &nbsp;
                            </td>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                        </tr>
                        --%>
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
                <asp:Panel ID="pnlCalificacionSuper" runat="server">
                    <table class="lblNormal" border="0">
                        <tr>
                            <td colspan="6" class="lblLinea">
                                CALIFICACIÓN
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
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Calificación Interna Ratificada:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblCalificacionIRNR" runat="server"></asp:Label>
                            </td>
                            <td class="lblLinea">
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
                            <td class="lblSubTitInfo">
                                Calificación Externa Ratificada:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblCalifExtRatif" runat="server"></asp:Label>
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
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Razón Calificación:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblRazonCalifInternaSuper" runat="server"></asp:Label>
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
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Calificación Modelo de Rating:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblCalMAFNuevoRatingSup" runat="server"></asp:Label>
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
                        </tr>
                        <tr>
                            <td class="lblNormal">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
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
                        </tr>
                    </table>
                </asp:Panel>
                <%-- PMO19939 - RF048: En el perfil “Consulta” y “Superfinanciera” para el comité presencial replicar la vista del Perfil “Riesgos” sin posibilidad de modificarla--%>
                <asp:Panel ID="pnlCalificacionConsulta" runat="server">
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
                                <asp:Label ID="lblCalificacionINRatingCon" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Calificación Interna Ratificada:
                             </td>
                            <td>
                                <asp:Label ID="lblCalifInternaRNRCon" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Fecha Calificación Interna Actual:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblFechaCalifCon" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Calificación Externa Ratificada:
                            </td>
                            <td>
                                <asp:Label ID="lblCalifExternaCon" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Seguimiento de Covenants:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblCovenantCon" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Seguimiento en próximo Comité:
                            </td>
                            <td>
                                <asp:Label ID="lblSeguimientoCon" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="lblNormal">
                            <td class="lblSubTitInfo">
                                Calificación Modelo de Rating:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblCalMAFNuevoRatingCon" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Recomendación AEC:
                            </td>
                            <td>
                                <asp:Label ID="lblRecomendacionCon" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Calificación Recomendada Alineación Grupo Bancolombia:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblCalifRecomenGrupoCons" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Calificación Recomendada PEC (Sector financiero)
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblCalifRecPECCon" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Razón Calificación:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblRazCalifExtCon" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        <td class="lblSubTitInfo">
                                El cliente se encuentra en lista de control: 
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblListasdeControlCons" runat="server">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                Calificación Recomendada:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblCalNuevoRRecomCon" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Tipo Cliente:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblTipoClienteCon" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        <%-- PMO27494
                            <td class="lblSubTitInfo">
                                Fecha MAF:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblFechaMafCon" runat="server"></asp:Label>
                            </td>
                            <td class="lblSubTitInfo">
                                Estado Calificación:
                             </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblEstadoCalCon" runat="server"></asp:Label>
                            </td>
                        --%>
                        </tr>
                        <tr>
                        <%-- PMO27494
                            <td class="lblSubTitInfo">
                                Puntaje MAF:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblPuntajeMafCon" runat="server"></asp:Label>
                            </td>
                        --%>
                            <td class="lblSubTitInfo">
                                Utilizó EEFF Cargados en la Herramienta:
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblUtilizoEEFF" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        <%-- PMO27494
                            <td class="lblSubTitInfo">
                                Cliente Pertenece IFRS
                            </td>
                            <td class="lblNormal">
                                <asp:Label ID="lblClientePerteneceIFRSCon" runat="server"></asp:Label>
                            </td>
                        --%>
                            <td class="lblSubTitInfo">
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
                            <td class="lblSubTitInfo">
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
                <%--end PMO19939-RF048: --%>
                <asp:Panel runat="server" ID="pnlInfoAdicional">
                    <table border="0" width="100%">
                        <tr>
                            <td style="text-align: center">
                                <asp:Button ID="btnComiteAnterior" runat="server" CssClass="boton" Text="Comité Anterior"
                                    Width="200px" OnClick="btnComiteAnterior_Click" />
                                <asp:Button ID="btnPEC" runat="server" CssClass="boton" Text="Central Externa" Width="200"
                                    OnClick="btnPEC_Click" />
                                <asp:Button ID="btnCovenants" runat="server" CssClass="boton" Text="Covenants" Width="200"
                                    OnClick="btnCovenants_Click" />
                                <asp:Button ID="btnProrrogas" runat="server" CssClass="boton" Text="Prórrogas" Width="200"
                                    OnClick="btnProrrogas_Click" />
                                <asp:Button ID="btnDummyComite" runat="server" Style="display: none" />
                                <!--Boton para nuevo comite PMO27494 !-->
                                <asp:Button ID="btnDummyNuevoComite" runat="server" Style="display: none" />
                                <asp:Button ID="btnDummyPopUps" runat="server" Style="display: none" />
                                <asp:Button ID="btnIndicadores" runat="server" CssClass="boton" Text="Indicadores"
                                    Width="200" OnClick="btnIndicadores_Click" />
                                <asp:Button ID="btnDummyIndicadoresConstructor" runat="server" Style="display: none" />
                                <asp:Button ID="btnDummyIndicadoresFinanciero" runat="server" Style="display: none" />
                                <asp:Button ID="btnDummyIndicadoresGobierno" runat="server" Style="display: none" />
                                <asp:Button ID="btnValidacionGuardar" runat="server" Style="display: none" />
                                <asp:Button ID="btnCorteFteEEFF" runat="server" Style="display: none"/>
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
                </asp:Panel>
                <asp:Panel ID="pnlComentarios" runat="server">
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
                                <asp:TextBox ID="txtSustentacionCalRec" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                    onkeyup="Count(this,3000)" MaxLength="3000" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
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
                            <asp:Label ID="lblPregunta1" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                            </td>
                            <td class="lblNormal">
                                <asp:DropDownList ID="ddlPregunta1" runat="server"  CssClass="listCampoPreguntas">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblDescripcion" colspan="2">
                            <asp:Label ID="lblComplementoPregunta1" runat="server"> </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal" colspan="2">
                                <asp:TextBox ID="txtRespuestaPregunta1" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                onkeyup="Count(this,400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td class="lblSubTitInfo">
                                <asp:Label ID="lblPregunta2" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                            </td>
                            <td class="lblNormal">
                                <asp:DropDownList ID="ddlPregunta2" runat="server" CssClass="listCampoPreguntas">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblDescripcion" colspan="2">
                            <asp:Label ID="lblComplementoPregunta2" runat="server"> </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal" colspan="2">
                                <asp:TextBox ID="txtRespuestaPregunta2" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                onkeyup="Count(this,400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td class="lblSubTitInfo">
                                <asp:Label ID="lblPregunta3" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                            </td>
                            <td class="lblNormal">
                                <asp:DropDownList ID="ddlPregunta3" runat="server" CssClass="listCampoPreguntas">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblDescripcion" colspan="2">
                            <asp:Label ID="lblComplementoPregunta3" runat="server"> </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal" colspan="2">
                                <asp:TextBox ID="txtRespuestaPregunta3" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                    onkeyup="Count(this,400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td class="lblSubTitInfo">
                                <asp:Label ID="lblPregunta4" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                            </td>
                            <td class="lblNormal">
                                <asp:DropDownList ID="ddlPregunta4" runat="server" CssClass="listCampoPreguntas">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblDescripcion" colspan="2">
                            <asp:Label ID="lblComplementoPregunta4" runat="server"> </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal" colspan="2">
                                <asp:TextBox ID="txtRespuestaPregunta4" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                onkeyup="Count(this,400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td class="lblSubTitInfo">
                                <asp:Label ID="lblPregunta5" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                            </td>
                            <td class="lblNormal">
                                <asp:DropDownList ID="ddlPregunta5" runat="server" CssClass="listCampoPreguntas">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblDescripcion" colspan="2">
                            <asp:Label ID="lblComplementoPregunta5" runat="server"> </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal" colspan="2">
                                <asp:TextBox ID="txtRespuestaPregunta5" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                onkeyup="Count(this,400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td class="lblSubTitInfo">
                                <asp:Label ID="lblPregunta6" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                            </td>
                            <td class="lblNormal">
                                <asp:DropDownList ID="ddlPregunta6" runat="server" CssClass="listCampoPreguntas">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblDescripcion" colspan="2">
                            <asp:Label ID="lblComplementoPregunta6" runat="server"> </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal" colspan="2">
                                <asp:TextBox ID="txtRespuestaPregunta6" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                    onkeyup="Count(this,400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td class="lblSubTitInfo">
                                <asp:Label ID="lblPregunta7" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                            </td>
                            <td class="lblNormal">
                                <asp:DropDownList ID="ddlPregunta7" runat="server" CssClass="listCampoPreguntas">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblDescripcion" colspan="2">
                            <asp:Label ID="lblComplementoPregunta7" runat="server"> </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal" colspan="2">
                                <asp:TextBox ID="txtRespuestaPregunta7" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                onkeyup="Count(this,400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td class="lblSubTitInfo">
                                <asp:Label ID="lblPregunta8" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                            </td>
                            <td class="lblNormal">
                                <asp:DropDownList ID="ddlPregunta8" runat="server" CssClass="listCampoPreguntas">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblDescripcion" colspan="2">
                            <asp:Label ID="lblComplementoPregunta8" runat="server"> </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal" colspan="2">
                                <asp:TextBox ID="txtRespuestaPregunta8" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                    onkeyup="Count(this,400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblSubTitInfo">
                                <asp:Label ID="lblPregunta9" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                            </td>
                            <td class="lblNormal">
                                <asp:DropDownList ID="ddlPregunta9" runat="server" CssClass="listCampoPreguntas">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblDescripcion" colspan="2">
                            <asp:Label ID="lblComplementoPregunta9" runat="server"> </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblNormal" colspan="2">
                                <asp:TextBox ID="txtRespuestaPregunta9" runat="server" Height="50px" 
                                    Width="96%" CssClass="txtCampo117" 
                                onkeyup="Count(this,500)" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
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
                                        <asp:Label ID="lblPregunta10" runat="server" CssClass="lblSubTitInfo">
                                        </asp:Label>
                                    </td>
                                    <td class="lblNormal">
                                        <asp:DropDownList ID="ddlPregunta10" runat="server" 
                                            CssClass="listCampoPreguntas">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblDescripcion" colspan="2">
                                        <asp:Label ID="lblComplementoPregunta10" runat="server"> </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblNormal" colspan="2">
                                        <asp:TextBox ID="txtRespuestaPregunta10" runat="server" CssClass="txtCampo117" 
                                            Height="50px" MaxLength="500" onkeyup="Count(this,500)" TextMode="MultiLine" 
                                            Width="96%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblSubTitInfo">
                                        <asp:Label ID="lblPregunta11" runat="server" CssClass="lblSubTitInfo">
                                        </asp:Label>
                                    </td>
                                    <td class="lblNormal">
                                        <asp:DropDownList ID="ddlPregunta11" runat="server" 
                                            CssClass="listCampoPreguntas">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblDescripcion" colspan="2">
                                        <asp:Label ID="lblComplementoPregunta11" runat="server"> </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblNormal" colspan="2">
                                        <asp:TextBox ID="txtRespuestaPregunta11" runat="server" CssClass="txtCampo117" 
                                            Height="50px" MaxLength="500" onkeyup="Count(this,500)" TextMode="MultiLine" 
                                            Width="96%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblSubTitInfo">
                                        <asp:Label ID="lblPregunta12" runat="server" CssClass="lblSubTitInfo">
                                        </asp:Label>
                                    </td>
                                    <td class="lblNormal">
                                        <asp:DropDownList ID="ddlPregunta12" runat="server" 
                                            CssClass="listCampoPreguntas">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblDescripcion" colspan="2">
                                        <asp:Label ID="lblComplementoPregunta12" runat="server"> </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblNormal" colspan="2">
                                        <asp:TextBox ID="txtRespuestaPregunta12" runat="server" CssClass="txtCampo117" 
                                            Height="50px" MaxLength="500" onkeyup="Count(this,500)" TextMode="MultiLine" 
                                            Width="96%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblSubTitInfo">
                                        <asp:Label ID="lblPregunta13" runat="server" CssClass="lblSubTitInfo">
                                        </asp:Label>
                                    </td>
                                    <td class="lblNormal">
                                        <asp:DropDownList ID="ddlPregunta13" runat="server" 
                                            CssClass="listCampoPreguntas">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblDescripcion" colspan="2">
                                        <asp:Label ID="lblComplementoPregunta13" runat="server"> </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblNormal" colspan="2">
                                        <asp:TextBox ID="txtRespuestaPregunta13" runat="server" CssClass="txtCampo117" 
                                            Height="50px" MaxLength="500" onkeyup="Count(this,500)" TextMode="MultiLine" 
                                            Width="96%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblSubTitInfo">
                                        <asp:Label ID="lblPregunta14" runat="server" CssClass="lblSubTitInfo">
                                        </asp:Label>
                                    </td>
                                    <td class="lblNormal">
                                        <asp:DropDownList ID="ddlPregunta14" runat="server" 
                                            CssClass="listCampoPreguntas">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblDescripcion" colspan="2">
                                        <asp:Label ID="lblComplementoPregunta14" runat="server"> </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblNormal" colspan="2">
                                        <asp:TextBox ID="txtRespuestaPregunta14" runat="server" CssClass="txtCampo117" 
                                            Height="50px" MaxLength="500" onkeyup="Count(this,500)" TextMode="MultiLine" 
                                            Width="96%"></asp:TextBox>
                                    </td>
                                </tr>                      
                              
                                <tr>
                                    <td class="lblNormal" colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tr>
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
                                    Calificación para Fondos Inmobiliaros
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
                                        <asp:Label ID="lblPregunta15" runat="server" CssClass="lblSubTitInfo">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblDescripcion" colspan="2">
                                        <asp:Label ID="lblComplementoPregunta15" runat="server"> </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblNormal" colspan="2">
                                        <asp:TextBox ID="txtRespuestaPregunta15" runat="server" CssClass="txtCampo117" 
                                            Height="50px" MaxLength="500" onkeyup="Count(this,500)" TextMode="MultiLine" 
                                            Width="96%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblSubTitInfo">
                                        <asp:Label ID="lblPregunta16" runat="server" CssClass="lblSubTitInfo">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblDescripcion" colspan="2">
                                        <asp:Label ID="lblComplementoPregunta16" runat="server"> </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblNormal" colspan="2">
                                        <asp:TextBox ID="txtRespuestaPregunta16" runat="server" CssClass="txtCampo117" 
                                            Height="50px" MaxLength="500" onkeyup="Count(this,500)" TextMode="MultiLine" 
                                            Width="96%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblSubTitInfo">
                                        <asp:Label ID="lblPregunta17" runat="server" CssClass="lblSubTitInfo">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblDescripcion" colspan="2">
                                        <asp:Label ID="lblComplementoPregunta17" runat="server"> </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblNormal" colspan="2">
                                        <asp:TextBox ID="txtRespuestaPregunta17" runat="server" CssClass="txtCampo117" 
                                            Height="50px" MaxLength="500" onkeyup="Count(this,500)" TextMode="MultiLine" 
                                            Width="96%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblSubTitInfo">
                                        <asp:Label ID="lblPregunta18" runat="server" CssClass="lblSubTitInfo">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblDescripcion" colspan="2">
                                        <asp:Label ID="lblComplementoPregunta18" runat="server"> </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblNormal" colspan="2">
                                        <asp:TextBox ID="txtRespuestaPregunta18" runat="server" CssClass="txtCampo117" 
                                            Height="50px" MaxLength="500" onkeyup="Count(this,500)" TextMode="MultiLine" 
                                            Width="96%"></asp:TextBox>
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td align="center" class="lblSubTitInfo" colspan="2">
                                        <asp:Label ID="lblComentariosRiesgos" runat="server" CssClass="lblSubTitInfo" 
                                            Text="COMENTARIOS RIEGOS - SUSTENTACIÓN CALIFICACIÓN"> </asp:Label>
                                        <asp:CustomValidator runat="server" 
                                            ClientValidationFunction="validarComentarios" 
                                            ControlToValidate="txtComentariosRiesgos" 
                                            ErrorMessage="Cuando un cliente es marcado para seguimiento AEC o seguimiento en próximo comité, es obligatorio indicar que variables se deben revisar" 
                                            ValidateEmptyText="true" ValidationGroup="Guardar">*</asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblNormal" colspan="2">
                                        <asp:TextBox ID="txtComentariosRiesgos" runat="server" CssClass="txtCampo117" 
                                            Height="100px" MaxLength="3000" onkeyup="Count(this,3000)" TextMode="MultiLine" 
                                            Width="96%"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                            FilterMode="InvalidChars" InvalidChars="'" 
                                            TargetControlID="txtComentariosRiesgos">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblNormal" colspan="2">
                                        <asp:ValidationSummary ID="vsErrores" runat="server" CssClass="lblError" 
                                            DisplayMode="List" HeaderText="" Height="54px" ShowMessageBox="True" 
                                            ShowSummary="False" ValidationGroup="Guardar" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblNormal" colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tr>
                        </tr>


                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlBotones" runat="server">
                    <table>
                        <tr>
                            <td colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnGuardar" runat="server" CssClass="boton" Text="Guardar" OnClick="btnGuardar_Click"
                                    ValidationGroup="Guardar" />
                            </td>
                            <td>
                                <asp:Button ID="btnBuscarNuevo" runat="server" CssClass="boton" OnClick="btnBuscarNuevo_Click"
                                    Text="Buscar Nuevo Cliente" Width="149px" />
                            </td>
                            <td>
                                <asp:Button ID="btnPdf" CssClass="boton" runat="server" OnClick="btnPdf_Click" Text="Generar PDF" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            
            <asp:Panel ID="plnCorteFteEEFF" runat="server" Width="25%" CssClass="popUpStyle" ScrollBars="Auto"
                Style="display: none; height: 180px; overflow-x: hidden" HorizontalAlign="Center">
                <table width="100%">
                    <tr>
                        <td>
                            <h1 style="height: 10%; padding-top: 5px; font-size: 10pt; background-color: #0066ae;
                                width: 100%; color: #FFFFFF;">
                                Corte y Fuente EEFF</h1>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%">
                                    <tr>
                                        <td bgcolor="0066ae" style="text-align: left; color: #FFFFFF; width:30%;">
                                            <asp:Label ID="lblCarteEEFF" runat="server" Text="Corte EEFF" />
                                        </td>
                                        <td style="text-align: left; color: #FFFFFF;">
                                            <asp:TextBox ID="txtEEFFUti" runat="server" MaxLength="7" Width="100%"/>
                                        </td>
                                        <td>
                                            <asp:CustomValidator ID="cvTxtEEFFUti" runat="server" ControlToValidate="txtEEFFUti" ValidateEmptyText="true"
                                                ClientValidationFunction="validarFechaCorte" Enabled="false">*</asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td bgcolor="0066ae" style="text-align: left; color: #FFFFFF; width:30%;">
                                            <asp:Label ID="lblFteEEFF" runat="server" Text="Fuente EEFF" />
                                        </td>
                                        <td style="text-align: left; color: #FFFFFF;">
                                            <asp:TextBox ID="txtFteEEFF" runat="server" MaxLength="30" Width="100%"/>
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
                                        <td colspan="2" style="text-align: center; color: #FFFFFF;">
                                            <asp:Button ID="btnCerrarPlnCorteFteEEFF" runat="server" CssClass="boton"
                                                    Text="Cerrar" OnClientClick="return validarCorteEEFF()" OnClick="btnCerrarPlnCorteFteEEFF_Click"/>
                                        </td>
                                    </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <!-- Comite anterior PMO27494!-->
            <asp:Panel ID="pnlNuevoComite" runat="server" Width="725px" CssClass="popUpStyle" Style="display: none;
                height: 80%; overflow-y: scroll; overflow-x: hidden" HorizontalAlign="Center" ScrollBars="Auto">
                <table>
                    <tr>
                        <td>
                            <asp:Panel runat="server" ID="pnlInfoNuevoComite" Width="700px">
                                <table width="100%">
                                    <tr>
                                        <td class="TituloPrincipal" colspan="4">
                                            Información Comité Anterior
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Nit
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCANNit" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Nombre
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCANNombre" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td class="lblSubTitInfo">
                                            Comité
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCANComite" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Segmento
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCANSegmento" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Oficina
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCANOficina" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Cod. Gerente
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCANGerente" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Región
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCANRegion" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Zona
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCANZona" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Calificación Modelo de Rating
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCANCalificacionMAF" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Calificación Externa Ratificada
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCANCalificacionExterna" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Calificación Interna Ratificada 
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCANCalificacionInterna" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Razón Calificación
                                        </td>
                                        <td class="lblNormal">
                                            <asp:TextBox ID="txtCANRazonCalificacion" runat="server" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
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
                                        <td class="lblLineaCentrado" colspan="4">
                                            RECOMENDACIÓN
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" align="center">
                                            SUSTENTACIÓN DE CALIFICACIÓN RECOMENDADA
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblDescripcion" align="center" colspan="4">
                                            (Describir actividad del cliente, hechos relevantes que generan variaciones en las cifras y argumentos que sustentan la calificación recomendada)
                                        </td>
                                    </tr>
                                     <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCANSustentacionCalRec" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                                ReadOnly="true" onkeyup="Count(this,2000)" MaxLength="2000" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
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
                                        <td class="lblSubTitInfo" colspan="4">
                                        <asp:Label ID="lblCANPregunta1" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                        <asp:Label ID="lblJustPregunta1" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblDescripcion" colspan="4">
                                        <asp:Label ID="lblCANComplementoPregunta1" runat="server"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCANRespuestaPregunta1" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                            ReadOnly="true" onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                    
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                        <asp:Label ID="lblCANPregunta2" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                        <asp:Label ID="lblJustPregunta2" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblDescripcion" colspan="4">
                                        <asp:Label ID="lblCANComplementoPregunta2" runat="server"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCANRespuestaPregunta2" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                            ReadOnly="true" onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                        <asp:Label ID="lblCANPregunta3" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                        <asp:Label ID="lblJustPregunta3" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblDescripcion" colspan="4">
                                        <asp:Label ID="lblCANComplementoPregunta3" runat="server"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCANRespuestaPregunta3" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                            ReadOnly="true" onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo"colspan="4">
                                        <asp:Label ID="lblCANPregunta4" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                        <asp:Label ID="lblJustPregunta4" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblDescripcion" colspan="4">
                                        <asp:Label ID="lblCANComplementoPregunta4" runat="server"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCANRespuestaPregunta4" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                            ReadOnly="true" onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                        <asp:Label ID="lblCANPregunta5" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                        <asp:Label ID="lblJustPregunta5" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblDescripcion" colspan="4">
                                        <asp:Label ID="lblCANComplementoPregunta5" runat="server"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCANRespuestaPregunta5" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                            ReadOnly="true" onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                        <asp:Label ID="lblCANPregunta6" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                        <asp:Label ID="lblJustPregunta6" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblDescripcion" colspan="4">
                                        <asp:Label ID="lblCANComplementoPregunta6" runat="server"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCANRespuestaPregunta6" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                            ReadOnly="true" onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                        <asp:Label ID="lblCANPregunta7" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                        <asp:Label ID="lblJustPregunta7" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblDescripcion" colspan="4">
                                        <asp:Label ID="lblCANComplementoPregunta7" runat="server"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCANRespuestaPregunta7" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                            ReadOnly="true" onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                        <asp:Label ID="lblCANPregunta8" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                        <asp:Label ID="lblJustPregunta8" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblDescripcion" colspan="4">
                                        <asp:Label ID="lblCANComplementoPregunta8" runat="server"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCANRespuestaPregunta8" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                            ReadOnly="true" onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                        <asp:Label ID="lblCANPregunta9" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                        <asp:Label ID="lblJustPregunta9" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblDescripcion" colspan="4">
                                        <asp:Label ID="lblCANComplementoPregunta9" runat="server"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCANRespuestaPregunta9" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                            ReadOnly="true" onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                        <asp:Label ID="lblCANPregunta10" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                        <asp:Label ID="lblJustPregunta10" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblDescripcion" colspan="4">
                                        <asp:Label ID="lblCANComplementoPregunta10" runat="server"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCANRespuestaPregunta10" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                            ReadOnly="true" onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                        <asp:Label ID="lblCANPregunta11" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                        <asp:Label ID="lblJustPregunta11" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblDescripcion" colspan="4">
                                        <asp:Label ID="lblCANComplementoPregunta11" runat="server"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCANRespuestaPregunta11" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                            ReadOnly="true" onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                        <asp:Label ID="lblCANPregunta12" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                        <asp:Label ID="lblJustPregunta12" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblDescripcion" colspan="4">
                                        <asp:Label ID="lblCANComplementoPregunta12" runat="server"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCANRespuestaPregunta12" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                            ReadOnly="true" onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                        <asp:Label ID="lblCANPregunta13" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                        <asp:Label ID="lblJustPregunta13" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblDescripcion" colspan="4">
                                        <asp:Label ID="lblCANComplementoPregunta13" runat="server"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCANRespuestaPregunta13" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                            ReadOnly="true" onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                        <asp:Label ID="lblCANPregunta14" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                        <asp:Label ID="lblJustPregunta14" runat="server" CssClass="lblSubTitInfo"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblDescripcion" colspan="4">
                                        <asp:Label ID="lblCANComplementoPregunta14" runat="server"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCANRespuestaPregunta14" runat="server" Height="50px" Width="96%" CssClass="txtCampo117" 
                                            ReadOnly="true" onkeyup="Count(this,1400)" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                                                        <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                        Comentarios Riesgos - Sustentación Calificación
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="TxtNuevoComentarioRiesgos" runat="server" CssClass="txtCampo117" Height="50px"
                                                ReadOnly="true" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
 

                                    
                                    
                                    
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnOKnc" runat="server" CssClass="boton" Text="Cerrar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            
            <asp:Panel ID="pnlComite" runat="server" Width="725px" CssClass="popUpStyle" Style="display: none;
                height: 80%; overflow-y: scroll; overflow-x: hidden" HorizontalAlign="Center" ScrollBars="Auto">
                <table>
                    <tr>
                        <td>
                            <asp:Panel runat="server" ID="pnlInfoComite" Width="700px">
                                <table width="100%">
                                    <tr>
                                        <td class="TituloPrincipal" colspan="4">
                                            Información Comité Anterior
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Nit
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCANit" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Nombre
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCANombre" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Comité
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCAComite" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Segmento
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCASegmento" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Oficina
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCAOficina" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Cod. Gerente
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCAGerente" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Región
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCARegion" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Zona
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCAZona" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Fecha MAF
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCAFechaMAF" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Puntaje MAF
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCAPuntajeMAF" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Calificación MAF NR
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCACalificacionMAF" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Calificación Externa Ratificada
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCACalificacionExterna" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Calificación Interna Ratificada NR
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCACalificacionInterna" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Razón Calificación
                                        </td>
                                        <td class="lblNormal">
                                            <%--<asp:Label ID="lblCARazonCalInterna" runat="server"></asp:Label>--%>
                                            <asp:TextBox ID="txtRazonCalificacion" runat="server" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Cliente Pertenece IFRS
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblCAClientePerteneceIFRS" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            &nbsp;
                                        </td>
                                        <td class="lblNormal">
                                            &nbsp;</asp:Label>
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
                                        <td class="lblLineaCentrado" colspan="4">
                                            RECOMENDACIÓN
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                            Actividad del cliente
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblDescripcion" colspan="2">
                                            (Describir brevemente la actividad que realiza el cliente)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCAActividadCliente" runat="server" CssClass="txtCampo117" Height="50px"
                                                ReadOnly="true" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                            Análisis de cifras financieras
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <span class="lblNormal">Descripción de los hechos relevantes que ocasionan variaciones
                                                en las cifras del PyG </span><span class="lblDescripcion">(No es transcripción de cifras
                                                    financieras)</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCAAnalisisPyG" runat="server" CssClass="txtCampo117" Height="50px"
                                                ReadOnly="true" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <span class="lblNormal">Descripción de los hechos relevantes que ocasionan variaciones
                                                en las cifras del Balance </span><span class="lblDescripcion">(No es transcripción de
                                                    cifras financieras)</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCAAnalisisBalance" runat="server" CssClass="txtCampo117" Height="50px"
                                                ReadOnly="true" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                            Hechos significativos que aporten a la calificación recomendada
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCAComAdicionales" runat="server" CssClass="txtCampo117" Height="50px"
                                                ReadOnly="true" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" colspan="4">
                                        Comentarios Riesgos - Sustentación Calificación
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            <asp:TextBox ID="txtCAComentariosRiesgos" runat="server" CssClass="txtCampo117" Height="50px"
                                                ReadOnly="true" TextMode="MultiLine" Width="96%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                            <asp:Button ID="btnOk" runat="server" CssClass="boton" Text="Cerrar" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlPopPups" runat="server" Width="800px" CssClass="popUpStyle" HorizontalAlign="Center"
                Style="display: none; height: 650px; overflow-y: auto; overflow-x: auto" ScrollBars="Auto">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Panel runat="server" ID="pnlPEC" Width="725px">
                                <table class="tableAlign" width="100%" style="border-spacing: 0px;">
                                    <tr>
                                        <td class="TituloPrincipal" colspan="5">
                                            Información Central Externa PEC
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="SubtituloPrincipalSeparador">
                                            Nit
                                        </td>
                                        <td class="LabelPrincipalSeparador" style="width: 200px;position: fixed;">
                                            <asp:Label ID="lblNitPEC" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="SubtituloPrincipalSeparador">
                                            Nombre
                                        </td>
                                        <td class="LabelPrincipalSeparador" style="width: 200px;position: fixed;">
                                            <asp:Label ID="lblNombrePEC" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="SubtituloPrincipalSeparador">
                                            Segmento
                                        </td>
                                        <td class="LabelPrincipalSeparador" style="width: 200px;position: fixed;">
                                            <asp:Label ID="lblSegmentoPEC" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TituloPrincipalSeparadores" colspan="2" style="background-color:#5CA8DF">
                                            DETERIORO PEC
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" style="border: solid 1px;font-weight:bold" nowrap>
                                            Calificación Sugerida PEC (MRC)
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblCalSug" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" style="border: solid 1px;font-weight:bold">
                                            Razón Deterioro PEC
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblRazon" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4" colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="SubtituloPrincipalSeparador" rowspan="2" style="text-align: center;">
                                            Causales
                                        </td>
                                        <td class="SubtituloPrincipalSeparador" colspan="2" style="text-align: center;">
                                            Resultado Sector
                                        </td>
                                        <td class="SubtituloPrincipalSeparador" colspan="2" style="text-align: center;">
                                            Principales Entidades
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TituloPrincipalSeparadores" style="color: black; font-weight: normal;
                                            background-color: #C0C3C2;">
                                            Calificación Sector
                                        </td>
                                        <td class="TituloPrincipalSeparadores" style="color: black; font-weight: normal;
                                            background-color: #C0C3C2;">
                                            Porcentaje Saldo
                                        </td>
                                        <td class="TituloPrincipalSeparadores" style="color: black; font-weight: normal;
                                            background-color: #C0C3C2;">
                                            1
                                        </td>
                                        <td class="TituloPrincipalSeparadores" style="color: black; font-weight: normal;
                                            background-color: #C0C3C2;">
                                            2
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            Arrastre Comercial
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px; text-align: center;">
                                            <asp:Label ID="lblArrComCalSec" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px; text-align: right;">
                                            <asp:Label ID="lblArrComPorSal" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px; text-align: center;" nowrap>
                                            <asp:Label ID="lblArrComEnt1" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px; text-align: center;" nowrap>
                                            <asp:Label ID="lblArrComEnt2" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            Reestructurados Comercial
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px; text-align: center;">
                                            <asp:Label ID="lblReeComCalSec" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px; text-align: right;">
                                            <asp:Label ID="lblReeComPorSal" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px; text-align: center;" nowrap>
                                            <asp:Label ID="lblReeComEnt1" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px; text-align: center;" nowrap>
                                            <asp:Label ID="lblReeComEnt2" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            Castigos Comercial
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px; text-align: center;">
                                            <asp:Label ID="lblCasComCalSec" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px; text-align: right;">
                                            <asp:Label ID="lblCasComPorSal" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px; text-align: center;" nowrap>
                                            <asp:Label ID="lblCasComEnt1" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px; text-align: center;" nowrap>
                                            <asp:Label ID="lblCasComEnt2" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TituloPrincipalSeparadores" colspan="2" style="background-color:#5CA8DF">
                                            ALINEACION GRUPO
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" style="border: solid 1px;font-weight:bold">
                                            Calificación Asignada (MRC)
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblCalAsig" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" style="border: solid 1px;font-weight:bold">
                                            Corte Información
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblCorInfo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="5" style="border: solid 1px; text-align: center; font-weight: bold;">
                                            DATOS INSUMO ALINEACION
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="SubtituloPrincipalSeparador" style="text-align: center;">
                                            Entidad
                                        </td>
                                        <td class="SubtituloPrincipalSeparador" style="text-align: center;">
                                            Calificación (MRC)
                                        </td>
                                        <td class="SubtituloPrincipalSeparador" style="text-align: center;">
                                            Saldo Capital
                                        </td>
                                        <td class="SubtituloPrincipalSeparador" colspan="2" style="text-align: center;">
                                            Corte Información
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            ENTIDAD 1
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblEntCal1" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblEntSal1" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;" colspan="2" rowspan="8">
                                            <asp:Label ID="lblObservacion" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            ENTIDAD 2
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblEntCal2" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblEntSal2" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            ENTIDAD 3
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblEntCal3" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblEntSal3" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            ENTIDAD 4
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblEntCal4" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblEntSal4" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            ENTIDAD 5
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblEntCal5" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblEntSal5" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            ENTIDAD 6
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblEntCal6" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblEntSal6" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            ENTIDAD 7
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblEntCal7" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblEntSal7" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            ENTIDAD 8
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblEntCal8" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblNormal" style="border: solid 1px;">
                                            <asp:Label ID="lblEntSal8" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblNormal" colspan="5">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="5">
                                            <asp:Button ID="btnOK1" runat="server" CssClass="boton" Text="Cerrar" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="pnlCovenants" Width="100%">
                                <table width="100%">
                                    <tr>
                                        <td class="TituloPrincipal" colspan="4">
                                            Información Covenants
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo" style="width:100px">
                                            Nit
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblNitCov" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Nombre
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblNombreCov" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Gte Cuenta
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblGerenteCov" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Frecuencia de Revisión
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblFrecRevCov" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Oficina
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblOficinaCov" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Segmento
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblSegCov" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Garantía
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblGarantiaCov" runat="server"></asp:Label>
                                        </td>
                                        <td class="lblSubTitInfo">
                                            Fecha última gestión
                                        </td>
                                        <td class="lblNormal">
                                            <asp:Label ID="lblFecUltGesCov" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Covenant(s)
                                        </td>
                                        <td class="lblNormal" colspan="3">
                                            <asp:TextBox ID="txtCovenantsCov" runat="server" ReadOnly="true" TextMode="MultiLine"
                                                Width="95%" Height="120px" CssClass="txtCampo117"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblSubTitInfo">
                                            Respuesta Seguimiento
                                        </td>
                                        <td class="lblNormal" colspan="3">
                                            <asp:TextBox ID="txtRespSegCov" runat="server" ReadOnly="true" TextMode="MultiLine"
                                                Width="95%" Height="120px" CssClass="txtCampo117"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                            <asp:Button ID="btnOK2" runat="server" CssClass="boton" Text="Cerrar" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="pnlProrrogas" Width="100%">
                                <table width="100%">
                                    <tr>
                                        <td class="TituloPrincipal">
                                            MANTENIMIENTOS REALIZADOS
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                
                                <table border="solid" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Table runat="server" ID="tblProrrogas" Style="width: 100%; border-collapse: collapse;" border="0">
                                                <asp:TableHeaderRow>
                                                    <asp:TableHeaderCell class="TituloPrincipal">ID</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell class="TituloPrincipal">Tipología Mantenimiento</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell class="TituloPrincipal">Obligación</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell class="TituloPrincipal">Número Mantenimiento</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell class="TituloPrincipal">Descripción</asp:TableHeaderCell>
                                                </asp:TableHeaderRow>
                                            </asp:Table>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            &nbsp;
                                            <asp:Button ID="btnOK3" runat="server" CssClass="boton" Text="Cerrar" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlIndicadoresConstructor" runat="server" Width="900px" CssClass="popUpStyle" ScrollBars="Auto"
                Style="display: none; height: 425px; overflow-y: auto; overflow-x: hidden" HorizontalAlign="Center">
                <table>
                    <tr>
                        <td>
                            <h1 style="height: 20px; padding-top: 5px; font-size: 10pt; background-color: #0066ae;
                                width: 100%; color: #FFFFFF;">
                                INDICADORES CALIFICACIÓN DE CARTERA SEGMENTOS ESPECIALES</h1>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divIndicadoresConstructor" runat="server" style="overflow-y: auto;overflow-x: auto; width: 900px;">
                                <asp:Panel runat="server" ID="pnlIndicadoresConstructorInner">
                                    <table>
                                        <tr>
                                            <td align="center">
                                                <asp:GridView ID="gvIndConstructor" runat="server" AllowSorting="true" AutoGenerateColumns="false"
                                                    RowStyle-Height="25" Width="100%">
                                                    <PagerSettings PageButtonCount="3" Position="Bottom" />
                                                    <HeaderStyle Font-Underline="false" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-BackColor="#0066ae" HeaderStyle-ForeColor="#FFFFFF"
                                                            HeaderText="ID" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label2" runat="server" Font-Bold="false" Style="font-weight: normal"
                                                                    Text='<% # Eval("INDNITCLI")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#0066ae" HeaderStyle-ForeColor="#FFFFFF"
                                                            HeaderText="TIPO ID" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFechaVencimiento" runat="server" Style="font-weight: normal" Text='<% # Eval("INDTNITCLI")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#0066ae" HeaderStyle-ForeColor="#FFFFFF"
                                                            HeaderText="TIPO" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label4" runat="server" Style="font-weight: normal" Text='<% #Eval("INDTIPO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#0066ae" HeaderStyle-ForeColor="#FFFFFF"
                                                            HeaderText="RADICADO" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label3" runat="server" Style="font-weight: normal" Text='<% #Eval("INDRADICA") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#EEEEEE" HeaderText="PROYECTO" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label5" runat="server" Style="font-weight: normal" Text='<% #Eval("INDPROYECT") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#EEEEEE" HeaderText="AVANCE DE OBRA %"
                                                            ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label6" runat="server" Style="font-weight: normal" Text='<% # Convert.ToDouble(Eval("INDPAOBRA"), System.Globalization.CultureInfo.InvariantCulture).ToString("#0.##%") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#EEEEEE" HeaderText="VENTAS %" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label7" runat="server" Style="font-weight: normal" Text='<% # Convert.ToDouble(Eval("INDPVENTAS"), System.Globalization.CultureInfo.InvariantCulture).ToString("#0.##%") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#EEEEEE" HeaderText="FECHA CULMINACIÓN OBRA"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label8" runat="server" Text='<% #Eval("INDFCOBRA") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#EEEEEE" HeaderText="VENCIMIENTO CREDITO"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label9" runat="server" Text='<% #Eval("INDFVCRED") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#EEEEEE" HeaderText="# PRORROGAS" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label10" runat="server" Text='<% #Eval("INDNPRORRO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#EEEEEE" HeaderText="SALDO PROYECTO" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label11" runat="server" Text='<% #Eval("INDSPROYEC") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#EEEEEE" HeaderText="CALIFICACION PROYECTO" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCalifProyecto" runat="server" Text='<% #Eval("INDCALPROY") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#EEEEEE" HeaderText="NIT AVALISTA 1" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNitValista1" runat="server" Text='<% #Eval("INDNITAVA1") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#EEEEEE" HeaderText="Nombre AVALISTA 1"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNomAvalista1" runat="server" Text='<% #Eval("INDNOMAVA1") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#EEEEEE" HeaderText="NIT AVALISTA 2" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNitValista2" runat="server" Text='<% #Eval("INDNITAVA2") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#EEEEEE" HeaderText="Nombre AVALISTA 2"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNomAvalista2" runat="server" Text='<% #Eval("INDNOMAVA2") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#EEEEEE" HeaderText="NIT AVALISTA 3" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNitValista3" runat="server" Text='<% #Eval("INDNITAVA3") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-BackColor="#EEEEEE" HeaderText="Nombre AVALISTA 3"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNomAvalista3" runat="server" Text='<% #Eval("INDNOMAVA3") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle BackColor="White" Font-Bold="false" Height="25" />
                                                </asp:GridView>
                                                <br />
                                                <asp:Button ID="btnCerrarIndicadoresConstructor" runat="server" CssClass="boton"
                                                    Text="Cerrar" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlIndicadoresFinanciero" Width="900px" CssClass="popUpStyle" ScrollBars="Auto"
                Style="display: none; height: 275px; overflow-y: auto; overflow-x: hidden" HorizontalAlign="Center">
                <table>
                    <tr>
                        <td>
                            <h1 style="height: 20px; padding-top: 5px; font-size: 10pt; background-color: #0066ae;
                                width: 880px; color: #FFFFFF;">
                                INDICADORES CALIFICACIÓN DE CARTERA SEGMENTOS ESPECIALES</h1>
                        </td>
                    </tr>
                    <tr>
                        <td>
                                <table class="tblIndicadores" width="150px">
                                    <tr>
                                        <td bgcolor="0066ae" style="text-align: left; color: #FFFFFF;">
                                            <asp:Label ID="lblIdFinanciero" runat="server" Text="ID" />
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblIdFinancieroValue" runat="server" Text="" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td bgcolor="0066ae" style="text-align: left; color: #FFFFFF;">
                                            <asp:Label ID="lblTipoIdFinanciero" runat="server" Text="TIPO ID" />
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblTipoIdFinancieroValue" runat="server" Text="" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td bgcolor="0066ae" style="text-align: left; color: #FFFFFF;">
                                            <asp:Label ID="lblTipoFinanciero" runat="server" Text="TIPO" />
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblTipoFinancieroValue" runat="server" Text="" />
                                        </td>
                                    </tr>
                                </table>
                        </td>
                    </tr>
                    <table class="tblIndicadores" width="880px">
                        <tr>
                            <th bgcolor="#666666" style="color: #FFFFFF">
                                INDICADOR
                            </th>
                            <th bgcolor="#666666" style="color: #FFFFFF">
                                VALOR
                            </th>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label29" runat="server" Text="CALIDAD CARTERA POR VENCIMIENTO (ICV)" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblCalidCar" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label32" runat="server" Text="COBERTURA CARTERA VENCIDA" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblCobCar" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label34" runat="server" Text="RELACIÓN DE SOLVENCIA" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblRelSol" runat="server" Text="ID" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label36" runat="server" Text="EFICIENCIA FINANCIERA" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblEfiFin" runat="server" Text="ID" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label38" runat="server" Text="UTILIDAD NETA" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblUtiNet" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label40" runat="server" Text="ROE" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblROE" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                </table>
                <tr>
                    <td align="center">
                        <br />
                        <asp:Button ID="btnCerrarIndicadoresFinanciero" runat="server" CssClass="boton" Text="Cerrar"
                            Visible="false" />
                    </td>
                </tr>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlIndicadoresGobierno" Width="900px" CssClass="popUpStyle" ScrollBars="Auto"
                Style="display: none; height: 380px; overflow-y: auto; overflow-x: hidden" HorizontalAlign="Center">
                <table>
                    <tr>
                        <td>
                            <h1 style="height: 20px; padding-top: 5px; font-size: 10pt; background-color: #0066ae;
                                width: 880px; color: #FFFFFF;">
                                INDICADORES CALIFICACIÓN DE CARTERA SEGMENTOS ESPECIALES</h1>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%">
                                <table class="tblIndicadores" width="150px">
                                    <tr>
                                        <td bgcolor="0066ae" style="text-align: left; color: #FFFFFF;">
                                            <asp:Label ID="lblIDGobierno" runat="server" Text="ID" />
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblIDGobiernoValue" runat="server" Text="" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td bgcolor="0066ae" style="text-align: left; color: #FFFFFF;">
                                            <asp:Label ID="lblTipoIdGobierno" runat="server" Text="TIPO ID" />
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblTipoIdGobiernoValue" runat="server" Text="" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td bgcolor="0066ae" style="text-align: left; color: #FFFFFF;">
                                            <asp:Label ID="lblTipoGobierno" runat="server" Text="TIPO" />
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblTipoGobiernoValue" runat="server" Text="" />
                                        </td>
                                    </tr>
                                </table>
                            </table>
                        </td>
                    </tr>
                    <table class="tblIndicadores" width="880px">
                        <tr>
                            <th bgcolor="#666666" style="color: #FFFFFF">
                                INDICADOR
                            </th>
                            <th bgcolor="#666666" style="color: #FFFFFF">
                                VALOR
                            </th>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label12" runat="server" Text="INDICADOR ADMINISTRACIÓN CENTRAL" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblIndicadorAdminCentral" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label13" runat="server" Text="LIMITE DE LEY INDICADOR ADMINISTRACIÓN CENTRAL" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblLimiteAdminCentral" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label15" runat="server" Text="INDICADOR ASAMBLEA ESTUDIO" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblIndAsam" runat="server" Text="ID" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label14" runat="server" Text="LIMITE DE LEY INDICADOR ASAMBLEA ESTUDIO" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblLimIndAsam" runat="server" Text="ID" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label16" runat="server" Text="INDICADOR CONTRALORÍA ESTUDIO" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblIndContraloria" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label18" runat="server" Text="LIMITE DE LEY INDICADOR CONTRALORÍA ESTUDIO" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblLimIndContraloria" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label20" runat="server" Text="SOLVENCIA" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblSolvencia" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label22" runat="server" Text="SOSTENIBILIDAD" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblSostenibilidad" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label24" runat="server" Text="COBERTURA" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblCobertura" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label26" runat="server" Text="INDICADOR CONCEJO ESTUDIO" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblIndConsejo" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label28" runat="server" Text="LIMITE DE LEY INDICADOR CONCEJO ESTUDIO" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblLimIndConsejo" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label30" runat="server" Text="INDICADOR PERSONERÍA ESTUDIO" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblIndPer" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="Label17" runat="server" Text="LIMITE DE LEY INDICADOR PERSONERÍA ESTUDIO" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblLimIndPer" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                    <tr>
                        <td align="center">
                            <br />
                            <asp:Button ID="btnCerrarIndicadoresGobierno" runat="server" CssClass="boton" Text="Cerrar" />
                        </td>
                    </tr>
            </asp:Panel>
            
            
            
            <asp:Panel ID="pnlValidacionCal" runat="server" Width="725px" CssClass="popUpStyle" ScrollBars="Auto"
                Style="display: none; height: 12%; overflow-x: hidden" HorizontalAlign="Center">
                 <table>
                    <tr>
                        <td>
                            <h1 style="height: 15%; padding-top: 5px; font-size: 10pt;
                                width: 100%;">
                                La Calificación Interna Ratificada Nuevo Rating y la Calificacion Externa Ratificada, no guardan coherencia. ¿Desea continuar con la grabación?</h1>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%">
                                    <tr>
                                        <asp:Button ID="cmdOk" runat="server" Text="Si" Width="60px" CssClass="boton" OnClick="cmdOk_Click" />
                                        <asp:Button ID="cmdCancel" runat="server" Text="No" Width="60px" CssClass="boton" />
                                    </tr>
                            </table>
                        </td>
                    </tr>
                </table>

            </asp:Panel>
            
            <asp:ModalPopupExtender ID="mpeCorteFteEEFF" runat="server" PopupControlID="plnCorteFteEEFF" 
                 TargetControlID="btnCorteFteEEFF" DropShadow="false" Drag="true" BackgroundCssClass="modalBackground"/>
                 
            <asp:ModalPopupExtender ID="mpeValidacionCal" runat="server" PopupControlID="pnlValidacionCal" 
                 TargetControlID="btnValidacionGuardar" DropShadow="false" Drag="true" BackgroundCssClass="modalBackground"/>
                
            <asp:ModalPopupExtender ID="mpeComiteAnterior" runat="server" PopupControlID="pnlComite"
                TargetControlID="btnDummyComite" DropShadow="false" Drag="true" BackgroundCssClass="modalBackground">
            </asp:ModalPopupExtender>
            <!-- Modal para consultar nuevo comite anterior PMO27494!-->
            <asp:ModalPopupExtender ID="mpeNuevoComiteAnterior" runat="server" PopupControlID="pnlNuevoComite"
                TargetControlID="btnDummyNuevoComite" DropShadow="false" Drag="true" BackgroundCssClass="modalBackground">
            </asp:ModalPopupExtender>

            <asp:ModalPopupExtender ID="mpePopPups" runat="server" PopupControlID="pnlPopPups"
                TargetControlID="btnDummyPopUps" DropShadow="false" Drag="true" BackgroundCssClass="modalBackground">
            </asp:ModalPopupExtender>
            <asp:ModalPopupExtender ID="mpeIndicadoresConstructor" runat="server" PopupControlID="pnlIndicadoresConstructor"
                TargetControlID="btnDummyIndicadoresConstructor" DropShadow="false" Drag="true"
                BackgroundCssClass="modalBackground">
            </asp:ModalPopupExtender>
            <asp:ModalPopupExtender ID="mpeIndicadoresFinanciero" runat="server" PopupControlID="pnlIndicadoresFinanciero"
                TargetControlID="btnDummyIndicadoresFinanciero" DropShadow="false" Drag="true"
                BackgroundCssClass="modalBackground">
            </asp:ModalPopupExtender>
            <asp:ModalPopupExtender ID="mpeIndicadoresGobierno" runat="server" PopupControlID="pnlIndicadoresGobierno"
                TargetControlID="btnDummyIndicadoresGobierno" DropShadow="false" Drag="true"
                BackgroundCssClass="modalBackground">
            </asp:ModalPopupExtender>
            
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

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(RunThisAfterEachAsyncPostback);
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

        function validarFechaCorte() {
            txtEEFFUti = $("#ctl00_ContentPlaceHolder1_txtEEFFUti").val();

            fechaSel = new Date(txtEEFFUti.substr(0, 4), txtEEFFUti.substr(4, 6) - 1, "01"); // Note: months are 0-based

            if (isNaN(fechaSel.getTime())) {
                alert("El formato de fecha debe ser AAAAMM");
                return false;
            }
            
            fechaFin = new Date();
            fechaIni = new Date();
            fechaIni.setFullYear(fechaFin.getFullYear() - 3);
            
            strFechaIni = fechaIni.getFullYear() + "/" + (fechaIni.getMonth()+1);
            strFechaFin = fechaFin.getFullYear() + "/" + (fechaFin.getMonth()+1);

            if (fechaSel <= fechaIni || fechaSel >= fechaFin) {
                alert("La fecha está fuera del rango, debe estar entre " + strFechaIni + " y " + strFechaFin);
                return false;
            }
            return true;
        }
        
        function validarCorteEEFF() {
            txtEEFFUti = $("#ctl00_ContentPlaceHolder1_txtEEFFUti").val();
            txtFteEEFF = $("#ctl00_ContentPlaceHolder1_txtFteEEFF").val();
            msjError = $("#ctl00_ContentPlaceHolder1_regExpTxtEEFFUti").css( "visibility" ) == "visible";
            if (txtEEFFUti == "" || txtFteEEFF == "" || msjError) {
                alert("La información debe ser diligenciada obligatoriamente");
                return false;
            }
            return true;
        }

        function RunThisAfterEachAsyncPostback() {
            numRazon = $('#ctl00_ContentPlaceHolder1_ddlRazon :checked').length;
            $('#ctl00_ContentPlaceHolder1_ddlRazon').on('click', ':checkbox', function() {
                if ($(this).is(':checked')) {
                    numRazon++;
                    if (numRazon > 3) {
                        $(this).attr('checked', false);
                        numRazon--;
                        alert("Solo puede seleccionar hasta 3 razones");
                    }
                } else {
                    numRazon--;
                }
            });
        }

        RunThisAfterEachAsyncPostback();
    </script>

</asp:Content>
