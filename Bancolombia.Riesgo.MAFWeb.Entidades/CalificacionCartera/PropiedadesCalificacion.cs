using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bancolombia.Riesgo.MAFWeb.Entidades.CalificacionCartera
{
    [Serializable()]
    public class PropiedadesCalificacion
    {
        #region"Atributos"

        private string _nombrePropiedad;
        private string _valorPropiedad;

        #endregion

        #region "Propiedades"

        public string nombrePripiedad
        {
            get { return _nombrePropiedad; }
            set { _nombrePropiedad = value; }
        }

        public string valorPropiedad
        {
            get { return _valorPropiedad; }
            set { _valorPropiedad = value; }
        }

        #endregion
    }
}
