using System;
using System.Collections.Generic;
using buscainstaladores.Services;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class WebServicePage : ContentPage
    {
        public WebServicePage()
        {
            InitializeComponent();
        }

        private async void Handle_Clicked(object sender, System.EventArgs e)
        {
            string resultado = await ServicoTeste.BuscaCEP(txtCEP.Text);

            lblResultado.Text = resultado;
        }
    }
}
