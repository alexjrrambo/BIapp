using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class AvaliacoesView : ContentPage
    {
        public AvaliacoesView()
        {
            InitializeComponent();
                
            avaliacaoWebView.Source = App.UrlWebService + "avaliacaoview/" + App.UsuarioLogado.Id.ToString();

            this.Title = "Avaliações";
        }
    }
}
