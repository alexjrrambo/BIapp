using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using buscainstaladores.Database;
using buscainstaladores.Models;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            BindingContext = new UsuarioViewModel(Navigation);
        }

        async void EntrarClicked(object sender, System.EventArgs e)
        {
            carregandoIndicator.IsRunning = true;
            carregandoContent.IsVisible = true;
            await Task.Delay(TimeSpan.FromSeconds(1));
            ((UsuarioViewModel)BindingContext).CommandEntrar.Execute(true);
            carregandoContent.IsVisible = false;
            carregandoIndicator.IsRunning = false;
        }

        void CirarContaClicked(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new Cadastro.CadastroPage());
        }
    }
}
