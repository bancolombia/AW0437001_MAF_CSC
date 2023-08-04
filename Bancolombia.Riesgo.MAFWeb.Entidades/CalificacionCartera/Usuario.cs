using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bancolombia.Riesgo.MAFWeb.Entidades.CalificacionCartera
{
    public class Usuario
    {
        private string _rol;
        private string _usuario;
        private string _nombre;
        private string _apellido;
        private string _filial;

        #region "Propiedades"

        public string rol
        {
            set { _rol = value; }
            get { return _rol; }
        }
        public string usuario
        {
            set { _usuario = value; }
            get { return _usuario; }
        }
        public string nombre
        {
            set { _nombre = value; }
            get { return _nombre; }
        }
        public string apellido
        {
            set { _apellido = value; }
            get { return _apellido; }
        }
        public string filial
        {
            set { _filial = value; }
            get { return _filial; }
        }


        #endregion
    }
}
