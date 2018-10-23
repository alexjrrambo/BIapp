using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class ExtratoPage : ContentPage
    {
        public ExtratoPage()
        {
            InitializeComponent();

            extratoWebView.Source = App.UrlWebService+"extratoview/"+App.UsuarioLogado.Id.ToString();

            this.Title = "Receber dinheiro";
        }
    }
}
