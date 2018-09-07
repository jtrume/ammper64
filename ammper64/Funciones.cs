using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ammper64
{
    class Funciones
    {
        private static Boolean _globalactivado = false;
        private static string _globalhorario = "";
        private static int _globalbuscapor = 0;
        private static string _globaldiasemana = "";
        private static string _globaldiames = "";
        private static string _globalhora = "";

        public static Boolean activado
        {
            get { return _globalactivado; }
            set { _globalactivado = value; }
        }

        public static string horario
        {
            get { return _globalhorario; }
            set { _globalhorario = value; }
        }

        public static int buscapor
        {
            get { return _globalbuscapor; }
            set { _globalbuscapor = value; }
        }

        public static string diasemana
        {
            get { return _globaldiasemana; }
            set { _globaldiasemana = value; }
        }

        public static string diames
        {
            get { return _globaldiames; }
            set { _globaldiames = value; }
        }

        public static string hora
        {
            get { return _globalhora; }
            set { _globalhora = value; }
        }
        //public Boolean activado = false;
        //public string horario = string.Empty;
        //public int buscapor = 0;
        //public string _diasemana = "";
    }
}
