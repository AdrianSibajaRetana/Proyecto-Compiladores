using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Compiladores.Pages
{
    class ResultadoCompilador
    {
        public bool FueExitoso { get; set; }
        public string Salida { get; set; }
    }

    public partial class HomePage
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        public string EntradaDeUsuario { get; set; }

        public string Salida { get; set; }

        public string Estado { get; set; }

        public bool ButtonDisabled => string.IsNullOrWhiteSpace(EntradaDeUsuario);

        private async Task CompileEntry()
        {
            try
            {
                var resultado = await JSRuntime.InvokeAsync<ResultadoCompilador>(
                    "compilerParse",
                    EntradaDeUsuario);

                Estado = resultado.FueExitoso ? "Compilado" : "Error";
                Salida = resultado.Salida;
            }
            catch (Exception)
            {
                Estado = "Error";
                Salida = "Algo salió mal :(";
            }
        }
    }
}
