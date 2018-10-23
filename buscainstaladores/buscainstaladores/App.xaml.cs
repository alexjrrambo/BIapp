using buscainstaladores.Models;
using buscainstaladores.Services;
using buscainstaladores.Views;
using PushNotification.Plugin;
using Xamarin.Forms;

namespace buscainstaladores
{
    public partial class App : Application
    {
        public static Usuario UsuarioLogado;
        public static string Url;
        public static string UrlWebService;
        public App()
        {

#if DEBUG
            Url = "http://192.168.0.102:8080/";
            UrlWebService = "http://192.168.0.102:8080/ws/";
#endif
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new PrincipalPage();
                //MainPage = new TestePage();
            else
                MainPage = new NavigationPage(new PrincipalPage());
        }

        protected override void OnStart()
        {
            CrossPushNotification.Current.Register();

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
