using System;
using System.Collections.Generic;
using System.Globalization;
using buscainstaladores.Services;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{    
    public partial class EditarPerfilPage : ContentPage
    {
        public EditarPerfilPage()
        {
            InitializeComponent();

            BindingContext = new UsuarioViewModel(Navigation,"editar-informacoes");

            DataVisivel.Date = DateTime.Parse(DataInvisivel.Text);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((UsuarioViewModel)BindingContext).RecarregarUsuario();
        }

        void SelectedIndexChangedEstado(object sender, System.EventArgs e)
        {
            ((UsuarioViewModel)BindingContext).CommandCarregaCidade.Execute(true);
        }

        void DateSelectedData(object sender, Xamarin.Forms.DateChangedEventArgs e)
        {
            DataInvisivel.Text = DataVisivel.Date.ToString("dd/MM/yyyy");
        }

        void CpfTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            cpfEntry.Text = Utils.AplicaMascaraCpf(cpfEntry.Text);
        }

        void CelularTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            celularEntry.Text = Utils.AplicaMascaraCelular(celularEntry.Text);
        }

        void TelefoneTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            telefoneEntry.Text = Utils.AplicaMascaraTelefone(telefoneEntry.Text);
        }

        void CnpjTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            cnpjEntry.Text = Utils.AplicaMascaraCnpj(cnpjEntry.Text);
        }

        void CepTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            cepEntry.Text = Utils.AplicaMascaraCep(cepEntry.Text);
        }

        void NumeroTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            numeroEntry.Text = Utils.RemoveNaoNumericos(numeroEntry.Text);
        }

        void SalvarClicked(object sender, System.EventArgs e)
        {
            string erroMensagem = string.Empty;


            if (App.UsuarioLogado.Tipo_usuario == "I")
            {
                erroMensagem = Utils.ValildarObjeto(((UsuarioViewModel)BindingContext).UsuarioAtual, "Primeiro_nome,Ultimo_nome,Celular,Cpf,Nome_fantasia,Razao_social,Cnpj,Celular,Id_estado,Id_cidade,Endereco,Numero,Bairro,Cep");
                if (!string.IsNullOrEmpty(cnpjEntry.Text))
                {
                    if (!Utils.ValidaCnpj(cnpjEntry.Text))
                    {
                        erroMensagem += "CNPJ inválido. \n";
                    }
                }

            }
            if (App.UsuarioLogado.Tipo_usuario == "U")
            {
                erroMensagem = Utils.ValildarObjeto(((UsuarioViewModel)BindingContext).UsuarioAtual, "Primeiro_nome,Ultimo_nome,Celular");
            }

            if (DataVisivel.Date >= DateTime.Now)
            {
                erroMensagem += "Data de nascimento inválida. \n";
            }

            if (!string.IsNullOrEmpty(cpfEntry.Text))
            {
                if (!Utils.ValidaCPF(cpfEntry.Text))
                {
                    erroMensagem += "CPF inválido. \n";
                }
            }

            if(string.IsNullOrEmpty(erroMensagem))
            {
                ((UsuarioViewModel)BindingContext).CommandSalvaEdicao.Execute(true);
            }
            else
            {             
                DisplayAlert("Aviso", erroMensagem, "OK");
            }
        }
    }
}
