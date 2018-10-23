using System;
using System.Collections.Generic;
using buscainstaladores.Models;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class PagamentoPage : ContentPage
    {
        public PagamentoPage(Agenda agenda)
        {
            InitializeComponent();

            this.Title = "Pagamento";

            pagamentoWebView.Source = App.UrlWebService + "pagamentoview/" + agenda.Id.ToString();
        }
    }
}
