using System;
using System.Collections.Generic;
using System.Net;
using buscainstaladores.Services;
using buscainstaladores.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class ContaFinanceiraPage : ContentPage
    {
        public ContaFinanceiraPage()
        {
            InitializeComponent();

            BindingContext = new ContaViewModel(Navigation);
        }

        void AgenciaTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            agenciaEntry.Text = Utils.AplicaMascaraAgencia(agenciaEntry.Text,(string)BancoPicker.SelectedItem);
        }

        void ContaTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            contaEntry.Text = Utils.AplicaMascaraConta(contaEntry.Text,(string)BancoPicker.SelectedItem);
        }

        void SalvarClicked(object sender, System.EventArgs e)
        {
            string erroMensagem = string.Empty;
                     
            erroMensagem = Utils.ValildarObjeto(((ContaViewModel)BindingContext).ContaAtual, "Banco,Pessoa,Agencia,Favorecido,Conta");

            if (string.IsNullOrEmpty(erroMensagem))
            {
                ((ContaViewModel)BindingContext).CommandSalvaConta.Execute(true);
            }
            else
            {
                DisplayAlert("Aviso", erroMensagem, "OK");
            }
        }
    }
}
