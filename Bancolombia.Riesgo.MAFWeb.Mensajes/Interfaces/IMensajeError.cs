// IMensajeError.cs
//
// Autor: Alberto Puyana M (apuyana@intergrupo.com), Mayo 22, 2008
//
// COPYRIGHT(C), 2008, Intergrupo S.A.
// Todos los derechos resevados.
//


namespace Bancolombia.Riesgo.MAFWeb.Mensajes
{

    /// <summary>
    /// Interface que debe implementar una página para utilizar el método MostrarMensaje.
    /// </summary>
    public interface IMensajeError
    {
        /// <summary>
        /// Permite visualizar una colección de mensajes
        /// </summary>
        /// <param name="mensajeValidacionColeccion">Contiene una colección de mensajes</param>
        void MostrarMensajesError(MensajesColeccion mensajeValidacionColeccion);
    }
}
