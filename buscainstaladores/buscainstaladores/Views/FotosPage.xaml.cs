using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class FotosPage : ContentPage
    {
        public FotosPage()
        {
            InitializeComponent();

            fotosWebView.Source = App.UrlWebService + "albumfotosview/" + App.UsuarioLogado.Id.ToString();
              
            this.Title = "Fotos";
        }
    }
}
