

namespace Bancolombia.Riesgo.MAFWeb.Entidades.CalificacionCartera
{
    public class ErroresEntity
    {
        #region "Atributos"
        private string url = "";
        private string error = "";
        private string log = "";
        #endregion


        #region "Propiedades"
        public string Url
        {
            get { return url; }
            set { url = value; }

        }

        public string Error
        {
            get { return error; }
            set { error = value; }
        }

        public string Log
        {
            get { return log; }
            set { log = value; }
        }
        #endregion

        #region "Constructor"



        #endregion
    }
}
