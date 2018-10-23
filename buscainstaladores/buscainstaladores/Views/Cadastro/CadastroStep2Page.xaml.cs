using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using buscainstaladores.Services;
using Xamarin.Forms;

namespace buscainstaladores.Views.Cadastro
{
    public partial class CadastroStep2Page : ContentPage
    {
        public CarouselPage Page { get; set; }
        public CadastroStep2Page()
        {
            InitializeComponent();

            if(App.UsuarioLogado.Tipo_usuario == "I")
            {
                telefoneEntry.IsVisible = true;
                cpfEntry.IsVisible = true;
            }

        }

        void ContinuarClicked(object sender, System.EventArgs e)
        {
            labelMensagem.Text = string.Empty;

            App.UsuarioLogado.Primeiro_nome = nomeEntry.Text;
            App.UsuarioLogado.Ultimo_nome = sobrenomeEntry.Text;
            App.UsuarioLogado.Celular = celularEntry.Text;
            App.UsuarioLogado.Data_nascimento = DataVisivel.Date;
            App.UsuarioLogado.Telefone = telefoneEntry.Text;
            App.UsuarioLogado.Cpf = cpfEntry.Text;

            var ListaRes = new List<ValidationResult>();
            var Contexto = new ValidationContext(App.UsuarioLogado);
            var IsValid = Validator.TryValidateObject(App.UsuarioLogado, Contexto, ListaRes);

            string ErroMensagem = string.Empty;

            if (App.UsuarioLogado.Tipo_usuario == "I")
            {
                
                if (!string.IsNullOrEmpty(cpfEntry.Text))
                {
                    if (!Utils.ValidaCPF(cpfEntry.Text))
                    {
                        ErroMensagem = "CPF inválido. \n";
                    }
                }

                ErroMensagem += Utils.ValildarObjeto(App.UsuarioLogado, "Primeiro_nome,Ultimo_nome,Celular,Cpf");

                if(string.IsNullOrEmpty(ErroMensagem))
                {
                    var nextStep = new CadastroStep3Page();
                    nextStep.Page = Page;
                    Page.Children.Add(nextStep);
                    Page.CurrentPage = nextStep;   
                }
                else
                {
                    labelMensagem.Text = ErroMensagem;
                }
            }

            if (App.UsuarioLogado.Tipo_usuario == "U")
            {           
                ErroMensagem = Utils.ValildarObjeto(App.UsuarioLogado, "Primeiro_nome,Ultimo_nome,Celular");

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
    }
}
