using System;
using System.Collections.Generic;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class AreaPremiumPage : ContentPage
    {
        public AreaPremiumPage()
        {
            InitializeComponent();

            this.Title = "Premium";

            BindingContext = new UsuarioViewModel(Navigation, "area-premium");
        }

        async void CancelarClicked(object sender, System.EventArgs e)
        {
            var resposta = await DisplayAlert("Atenção", "Você realmente deseja cancelar sua assinatura?", "Sim", "Não");

            if(resposta)
            {
                ((UsuarioViewModel)BindingContext).CommandCancelarAssinatura.Execute(true);    
            }
        }
    }
}
