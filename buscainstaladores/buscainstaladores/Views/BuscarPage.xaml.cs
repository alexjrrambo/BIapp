using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class BuscarPage : ContentPage
    {
        public BuscarPage()
        {
            InitializeComponent();

            NavigationPage.SetBackButtonTitle(this, "");

            BindingContext = new BuscarViewModel(Navigation);
        }

        void SelectedIndexChangedEstado(object sender, System.EventArgs e)
        {            
            ((BuscarViewModel)BindingContext).CommandCarregaCidade.Execute(true);
        }

        async void PesquisarClicked(object sender, System.EventArgs e)
        {            
            carregandoIndicator.IsRunning = true;
            carregandoContent.IsVisible = true;
            await Task.Delay(TimeSpan.FromSeconds(1));
            ((BuscarViewModel)BindingContext).CommandPesquisar.Execute(true);
            carregandoContent.IsVisible = false;
            carregandoIndicator.IsRunning = false;
        }
    }
}
