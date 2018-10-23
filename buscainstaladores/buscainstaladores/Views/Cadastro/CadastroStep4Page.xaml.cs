using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using buscainstaladores.Models;
using buscainstaladores.Services;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views.Cadastro
{
    public partial class CadastroStep4Page : ContentPage
    {
        public CarouselPage Page { get; set; }
        public CadastroStep4Page()
        {
            InitializeComponent();

            BindingContext = new UsuarioViewModel(Navigation,"cadastro");
        }

        void SelectedIndexChangedEstado(object sender, System.EventArgs e)
        { 
            ((UsuarioViewModel)BindingContext).CommandCarregaCidade.Execute(true);
        }

        void ContinuarClicked(object sender, System.EventArgs e)
        {
            App.UsuarioLogado.Cep = cepEntry.Text;
            App.UsuarioLogado.Endereco = ruaEntry.Text;
            App.UsuarioLogado.Numero = numeroEntry.Text;
            App.UsuarioLogado.Bairro = bairroEntry.Text;
            Estado estado = (Estado)estadoPicker.SelectedItem;
            Cidade cidade = (Cidade)cidadePicker.SelectedItem;
            App.UsuarioLogado.Id_estado = estado.Cod_estado;
            App.UsuarioLogado.Id_cidade = cidade.Cod_cidade;
            App.UsuarioLogado.Sgl_estado = estado.Sgl_estado;
            App.UsuarioLogado.Nom_cidade = cidade.Nom_cidade;


            var ListaRes = new List<ValidationResult>();
            var Contexto = new ValidationContext(App.UsuarioLogado);
            var IsValid = Validator.TryValidateObject(App.UsuarioLogado, Contexto, ListaRes);

            string ErroMensagem = string.Empty;

            ErroMensagem = Utils.ValildarObjeto(App.UsuarioLogado, "Cep,Endereco,Numero,Bairro,Id_estado,Id_cidade");

            if (string.IsNullOrEmpty(ErroMensagem))
            {
                var nextStep = new CadastroFinalStep();
                nextStep.Page = Page;
                Page.Children.Add(nextStep);
                Page.CurrentPage = nextStep;
            }
            else
            {
                labelMensagem.Text = ErroMensagem;
            }
            
        }

        void CepTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            cepEntry.Text = Utils.AplicaMascaraCep(cepEntry.Text);
        }

        void NumeroTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            numeroEntry.Text = Utils.RemoveNaoNumericos(numeroEntry.Text);
        }
    }
}
