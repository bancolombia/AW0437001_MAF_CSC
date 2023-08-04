// Mensajes.cs
//
// Autor: Yenny Ocampo C. (yocampo@intergrupo.com), Feb 10, 2008
//
// COPYRIGHT(C), 2008, Intergrupo S.A.
// Todos los derechos resevados.
//

using System;
using System.Text;
using System.Web.UI;
using System.Windows.Forms;
using Bancolombia.Riesgo.MAF.Mensajes;
using Bancolombia.Riesgo.MAF.Mensajes.Controladoras;
using Bancolombia.Riesgo.MAFWeb.Mensajes.Interfaces;


namespace Bancolombia.Riesgo.MAFWeb.Mensajes
{


    /// <summary>
    /// Clase para la manipulación de los mensajes.
    /// </summary>
    public static class Mensajes
    {

        /// <summary>
        /// Obtiene el mensaje que se le deben de mostrar al usuario.
        /// </summary>
        /// <param name="nombreAplicacion">Nombre de la aplicación a la que pertenece el mensaje.</param>
        /// <param name="identificador">Contiene el código del mensaje a mostrar</param>
        public static string ObtenerTextoMensaje(string nombreAplicacion, string identificador)
        {
            return Bancolombia.Riesgo.MAF.Mensajes.Fachadas.Mensajes.ObtenerMensaje(nombreAplicacion, identificador).Texto;
        }


        /// <summary>
        /// Obtiene el mensaje que se le deben de mostrar al usuario.
        /// </summary>
        /// <param name="nombreAplicacion">Nombre de la aplicación a la que pertenece el mensaje.</param>
        /// <param name="identificador">Contiene el código del mensaje a mostrar</param>
        public static Mensaje ObtenerMensaje(string nombreAplicacion, string identificador)
        {
            return Bancolombia.Riesgo.MAF.Mensajes.Fachadas.Mensajes.ObtenerMensaje(nombreAplicacion, identificador);
        }



        /// <summary>
        /// Muestra el mensaje en pantalla, para eso se basa en la existencia del método MostrarMensaje de la página master y en la interface IMensajeError.
        /// </summary>
        /// <seealso cref="IMensajeInformacion"/>
        /// <param name="strMensaje">Mensaje a mostrar</param>
        /// <param name="pMostrarError">Página master a utilizar para mostrar el mensaje, la página master o 
        /// alguna de sus padres debe implementar la interface IMensajeError, de lo contrario se presenta el mensaje de forma de un MessageBox</param>
        //public static void MostrarMensaje(string strMensaje, Page pMostrarError)
        //{
        //   MensajesColeccion mensajeValidacionColeccion = new MensajesColeccion();
        //   mensajeValidacionColeccion.Add(
        //      new Mensaje(string.Empty, strMensaje, string.Empty, string.Empty));

        //   MostrarMensajes(mensajeValidacionColeccion, pMostrarError);
        //}


        /// <summary>
        /// Muestra el mensaje en pantalla, para eso se basa en la existencia del método MostrarMensaje de la página master y en la interface IMensajeError.
        /// </summary>
        /// <seealso cref="IMensajeInformacion"/>
        /// <param name="codMensaje">Código del mensaje a mostrar</param>
        /// <param name="pMostrarError">Página master a utilizar para mostrar el mensaje, la página master o 
        /// alguna de sus padres debe implementar la interface IMensajeError, de lo contrario se presenta el mensaje de forma de un MessageBox</param>		
        public static void MostrarMensaje(string nombreAplicacion, string identificador, Page pMostrarError)
        {
            MensajesColeccion mensajeValidacionColeccion = new MensajesColeccion();
            mensajeValidacionColeccion.Add(ObtenerMensaje(nombreAplicacion, identificador));

            MostrarMensajes(mensajeValidacionColeccion, pMostrarError);
        }
        /// <summary>
        /// Muestra el mensaje en pantalla, para eso se basa en la existencia del método MostrarMensaje de la página master y en la interface IMensajeError.
        /// </summary>
        /// <seealso cref="IMensajeInformacion"/>
        /// <param name="mensaje">Mensaje a mostrar</param>
        /// <param name="pMostrarError">Página master a utilizar para mostrar el mensaje, la página master o 
        /// alguna de sus padres debe implementar la interface IMensajeError, de lo contrario se presenta el mensaje de forma de un MessageBox</param>		
        public static void MostrarMensaje(Mensaje mensaje, Page pMostrarError)
        {
            MensajesColeccion mensajeValidacionColeccion = new MensajesColeccion();
            mensajeValidacionColeccion.Add(mensaje);

            MostrarMensajes(mensajeValidacionColeccion, pMostrarError);
        }
        /// <summary>
        /// Muestra los mensajes en pantalla, para eso se basa en la existencia del método
        /// MostrarMensaje de la página master y en la interface IMensajeError.
        /// </summary>
        /// <param name="mensajeValidacionColeccion">Entidad que contiene una colección de
        /// mensajes</param>
        /// <param name="pMostrarError"></param>
        public static void MostrarMensajes(MensajesColeccion mensajeValidacionColeccion, Page pMostrarError)
        {

            bool mensajeMostrado = false;
            MasterPage mpTmpError = null;
            IMensajeInformacion paginaMensaje = pMostrarError as IMensajeInformacion;

            if (paginaMensaje != null)
            {
                paginaMensaje.MostrarMensajesInformacion(mensajeValidacionColeccion);
                mensajeMostrado = true;
            }
            else
            {
                mpTmpError = pMostrarError.Master;
            }

            while (mpTmpError != null)
            {
                paginaMensaje = mpTmpError as IMensajeInformacion;
                if (paginaMensaje != null)
                {
                    paginaMensaje.MostrarMensajesInformacion(mensajeValidacionColeccion);
                    mpTmpError = null;
                    mensajeMostrado = true;
                }
                else
                {
                    mpTmpError = mpTmpError.Master;
                }
            }

            if (!mensajeMostrado)
            {
                string strTitulo = string.Empty;
                if ((pMostrarError != null) && (pMostrarError.Title != null))
                {
                    strTitulo = pMostrarError.Title;
                }
                StringBuilder sbMensajes = new StringBuilder();
                foreach (Mensaje mensaje in mensajeValidacionColeccion)
                {
                    sbMensajes.AppendLine(mensaje.Texto);
                }
                MessageBox.Show(sbMensajes.ToString(), strTitulo, MessageBoxButtons.OK,
                   MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }

        }


        /// <summary>
        /// Muestra los mensajes en pantalla, para eso se basa en la existencia del método
        /// MostrarMensaje de la página master y en la interface IMensajeError.
        /// </summary>
        /// <param name="mensajeValidacionColeccion">Entidad que contiene una colección de
        /// mensajes</param>
        /// <param name="pMostrarError"></param>
        public static void MostrarMensajesError(MensajesColeccion mensajeValidacionColeccion, Page pMostrarError)
        {
            bool mensajeMostrado = false;
            MasterPage mpTmpError = null;
            IMensajeError paginaMensaje = pMostrarError as IMensajeError;

            if (paginaMensaje != null)
            {
                paginaMensaje.MostrarMensajesError(mensajeValidacionColeccion);
                mensajeMostrado = true;
            }
            else
            {
                mpTmpError = pMostrarError.Master;
            }

            while (mpTmpError != null)
            {
                paginaMensaje = mpTmpError as IMensajeError;
                if (paginaMensaje != null)
                {
                    paginaMensaje.MostrarMensajesError(mensajeValidacionColeccion);
                    mpTmpError = null;
                    mensajeMostrado = true;
                }
                else
                {
                    mpTmpError = mpTmpError.Master;
                }
            }

            if (!mensajeMostrado)
            {
                string strTitulo = string.Empty;
                if ((pMostrarError != null) && (pMostrarError.Title != null))
                {
                    strTitulo = pMostrarError.Title;
                }
                StringBuilder sbMensajes = new StringBuilder();
                foreach (Mensaje mensaje in mensajeValidacionColeccion)
                {
                    sbMensajes.AppendLine(mensaje.Texto);
                }
                MessageBox.Show(sbMensajes.ToString(), strTitulo, MessageBoxButtons.OK,
                   MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }
        /// <summary>
        /// Muestra el mensaje en pantalla, para eso se basa en la existencia del método MostrarMensaje de la página master y en la interface IMensajeError.
        /// </summary>
        /// <seealso cref="IMensajeError"/>
        /// <param name="strMensaje">Mensaje a mostrar</param>
        /// <param name="pMostrarError">Página master a utilizar para mostrar el mensaje, la página master o 
        /// alguna de sus padres debe implementar la interface IMensajeError, de lo contrario se presenta el mensaje de forma de un MessageBox</param>
        //public static void MostrarMensajeError(string strMensaje, Page pMostrarError)
        //{
        //   MensajesColeccion mensajeValidacionColeccion = new MensajesColeccion();
        //   mensajeValidacionColeccion.Add(
        //      new Mensaje(string.Empty, strMensaje, string.Empty, string.Empty));

        //   MostrarMensajesError(mensajeValidacionColeccion, pMostrarError);
        //}

        /// <summary>
        /// Muestra el mensaje en pantalla, para eso se basa en la existencia del método MostrarMensaje de la página master y en la interface IMensajeError.
        /// </summary>
        /// <seealso cref="IMensajeError"/>
        /// <param name="codMensaje">Código del mensaje a mostrar</param>
        /// <param name="pMostrarError">Página master a utilizar para mostrar el mensaje, la página master o 
        /// alguna de sus padres debe implementar la interface IMensajeError, de lo contrario se presenta el mensaje de forma de un MessageBox</param>		
        public static void MostrarMensajeError(string nombreAplicacion, string identificador, Page pMostrarError)
        {
            MensajesColeccion mensajeValidacionColeccion = new MensajesColeccion();
            mensajeValidacionColeccion.Add(ObtenerMensaje(nombreAplicacion, identificador));

            MostrarMensajesError(mensajeValidacionColeccion, pMostrarError);
        }
        /// <summary>
        /// Muestra el mensaje en pantalla, para eso se basa en la existencia del método MostrarMensaje de la página master y en la interface IMensajeError.
        /// </summary>
        /// <seealso cref="IMensajeError"/>
        /// <param name="mensaje">Mensaje a mostrar</param>
        /// <param name="pMostrarError">Página master a utilizar para mostrar el mensaje, la página master o 
        /// alguna de sus padres debe implementar la interface IMensajeError, de lo contrario se presenta el mensaje de forma de un MessageBox</param>		
        public static void MostrarMensajeError(Mensaje mensaje, Page pMostrarError)
        {
            MensajesColeccion mensajeValidacionColeccion = new MensajesColeccion();
            mensajeValidacionColeccion.Add(mensaje);

            MostrarMensajesError(mensajeValidacionColeccion, pMostrarError);
        }

    }
}
