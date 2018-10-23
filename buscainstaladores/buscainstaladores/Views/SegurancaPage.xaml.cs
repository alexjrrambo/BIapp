using System;
using System.Collections.Generic;
using buscainstaladores.Services;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class SegurancaPage : ContentPage
    {
        public SegurancaPage()
        {
            InitializeComponent();

            BindingContext = new UsuarioViewModel(Navigation, "seguranca");
        }

        void SalvarClicked(object sender, System.EventArgs e)
        {
            string erroMensagem = string.Empty;

            if (string.IsNullOrEmpty(emailEntry.Text))
                erroMensagem += "E-mail deve ser informado.\n";

            if (!string.IsNullOrEmpty(senhaEntry.Text) && string.IsNullOrEmpty(senhaInvEntry.Text))
                erroMensagem += "Repita sua nova senha.\n";

            if (senhaEntry.Text != repSenhaEntry.Text)
                erroMensagem += "Senhas não conferem.\n";


            if (string.IsNullOrEmpty(erroMensagem))
            {
                senhaInvEntry.Text = senhaEntry.Text;
                ((UsuarioViewModel)BindingContext).CommandSalvaEdicao.Execute(true);
            }
            else
            {
                DisplayAlert("Aviso", erroMensagem, "OK");
            }
        }
    }
}
