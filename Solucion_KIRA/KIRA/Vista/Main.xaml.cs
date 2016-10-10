using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using KIRA.Modelo;
using KIRA.Controlador;

namespace KIRA.Vista
{
    /// <summary>
    /// Lógica de interacción para Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        SpeechRecognitionEngine reconocedor = new SpeechRecognitionEngine(); // reconocedor de voz
        SpeechSynthesizer kira = new SpeechSynthesizer(); // kira, sintetizador

        Random random = new Random();

        List<Social> sociales = new List<Social>(); // comando sociales



        public Main()
        {
            InitializeComponent();

            kira.Speak("Iniciando el Sistema");
            if (Properties.Settings.Default.Saludo)
            {
                kira.SpeakAsync("Bienvenido " + Properties.Settings.Default.Usuario + ", Hoy estamos a " + DateTime.Now.ToLongDateString() + " y son las " + DateTime.Now.ToLongTimeString());
            }        
            sociales = SocialControlador.ListarComandos();
            kira.SpeakAsync("Sistema cargado correctamente");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                reconocedor.SetInputToDefaultAudioDevice(); // microfono por defecto

                // Gramaticas Default
                reconocedor.LoadGrammar(new Grammar(new GrammarBuilder(new Choices(new string[] { "descanza kira", "que día es", "que hora es", "kira", "gracias" }))));
                //Gramaticas Sociales
                reconocedor.LoadGrammar(new Grammar(new GrammarBuilder(new Choices(SocialControlador.RetornarComandoSociales()))));

                reconocedor.RecognizeAsync(RecognizeMode.Multiple);

                reconocedor.SpeechRecognized += Reconocedor_SpeechRecognized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // App.Current.Shutdown();
            }
        }

        private void Reconocedor_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence > 0.6)
            {
                if (e.Result.Text.Equals("que hora es"))
                {
                    KiraHabla("Son las " + DateTime.Now.ToLongTimeString().ToString());
                }
                if (e.Result.Text.Equals("que día es"))
                {
                    KiraHabla("Hoy es  " + DateTime.Now.ToLongDateString().ToString());
                }
                if (e.Result.Text.Equals("descanza kira"))
                {
                    KiraHabla("Hasta Pronto Jefe");
                    App.Current.Shutdown();
                }
                if (e.Result.Text.Equals("kira"))
                {
                    KiraHabla("diga señor.");
                }
                
                if (e.Result.Text.Equals("gracias"))
                {
                    KiraHabla("no hay de que señor.");
                }

                foreach (Social social in sociales)
                {
                    if (e.Result.Text.Equals(social.Comando))
                    {
                        KiraHabla(social.Respuestas[random.Next(0, social.Respuestas.Length)]);
                    }
                }
            }
        }

        private void KiraHabla(string respuesta)
        {
            reconocedor.RecognizeAsyncCancel();
            kira.Speak(respuesta);
            reconocedor.RecognizeAsync(RecognizeMode.Multiple);
        }
    }
}
