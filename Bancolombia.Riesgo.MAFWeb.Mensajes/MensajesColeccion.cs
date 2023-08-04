
// MensajesColeccion.cs
//
// Autor: Yenny Ocampo C. (yocampo@intergrupo.com),  Feb 10, 2008
//
// COPYRIGHT(C), 2008, Intergrupo S.A.
// Todos los derechos resevados.
//

using Bancolombia.Riesgo.MAF.Mensajes.Controladoras;

namespace Bancolombia.Riesgo.MAFWeb.Mensajes
{
    using System.Collections.Generic;

    /// <summary>
    /// Clase que contiene una colección de objetos Mensaje.  Esta clase debe
    /// heredar de BindingList<MensajesColeccion>.
    /// </summary>
    public class MensajesColeccion : List<Mensaje>
    {
    }//end MensajesColeccion
}//end Namespace
