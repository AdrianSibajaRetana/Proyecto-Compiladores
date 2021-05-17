using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compiladores.Pages
{
    public partial class HomePage
    {
        public string EntradaDeUsuario { get; set; }

        public string Salida { get; set; }

        public string Estado { get; set; }

        private void CompileEntry()
        {
            if (EntradaDeUsuario == "Entrada")
            {
                Salida = "Salida";
                Estado = "Compilado";
            }
            else 
            {
                Salida = "";
                Estado = "Error";
            }
        }
    }
}
