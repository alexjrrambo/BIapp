using System;
using System.Collections.Generic;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class AssinarPage : ContentPage
    {
        public AssinarPage()
        {
            InitializeComponent();

            this.Title = "Assinar";

            BindingContext = new UsuarioViewModel(Navigation, "assinar");

            pagamentoWebView.Source = App.UrlWebService + "pagamentoassinarview/" + App.UsuarioLogado.Id.ToString();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ((UsuarioViewModel)BindingContext).CarregarUsuario(App.UsuarioLogado.Id);
        }
    }
}
