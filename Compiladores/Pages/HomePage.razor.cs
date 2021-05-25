using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Toolbelt.Blazor.SpeechRecognition;

namespace Compiladores.Pages
{
    class ResultadoCompilador
    {
        public bool FueExitoso { get; set; }
        public string Salida { get; set; }
    }

    public partial class HomePage : IDisposable
    {
        [Inject]
        public SpeechRecognition SpeechRecognition { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        public string EntradaDeUsuario { get; set; }

        public string Salida { get; set; }

        public string Estado { get; set; }

        public bool ButtonDisabled => string.IsNullOrWhiteSpace(EntradaDeUsuario);

        SpeechRecognitionResult[] Results = new SpeechRecognitionResult[0];
        bool IsListening = false;

        protected override void OnInitialized()
        {
            SpeechRecognition.Lang = "es-MX";
            SpeechRecognition.InterimResults = true;
            SpeechRecognition.Continuous = true;
            SpeechRecognition.Result += OnSpeechRecognized;
            SpeechRecognition.End += OnSpeechEnded;
        }

        // Empieza a grabar 
        private async Task OnClickStart()
        {
            if (!IsListening)
            {
                Clear();
                IsListening = true;
                await SpeechRecognition.StartAsync();
            }
        }

        private void Clear()
        {
            Salida = Estado = EntradaDeUsuario = string.Empty;
        }

        // Termina de grabar
        private async Task OnClickStop()
        {
            if (IsListening)
            {
                IsListening = false;
                await SpeechRecognition.StopAsync();
            }
        }

        // Destruye el objeto
        public void Dispose()
        {
            SpeechRecognition.Result -= OnSpeechRecognized;
            SpeechRecognition.End -= OnSpeechEnded;
        }

        // Se termina de escuchar (?)
        private void OnSpeechEnded(object sender, EventArgs args)
        {
            if (IsListening)
            {
                IsListening = false;
                StateHasChanged();
            }
        }

        // Almacena el contenido del contenido escuchado. 
        private void OnSpeechRecognized(object sender, SpeechRecognitionEventArgs args)
        {
            foreach (var i in args.Results.Skip(args.ResultIndex))
            {
                if (i.IsFinal)
                {
                    var transcript = i.Items.First().Transcript;
                    EntradaDeUsuario += string.IsNullOrEmpty(EntradaDeUsuario)
                        ? transcript
                        : $"\n{transcript}";
                }
            }
            StateHasChanged();
        }

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
