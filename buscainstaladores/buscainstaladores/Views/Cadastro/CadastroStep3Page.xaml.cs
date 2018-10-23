using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using buscainstaladores.Services;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views.Cadastro
{
    public partial class CadastroStep3Page : ContentPage
    {
        public CarouselPage Page { get; set; }
        public CadastroStep3Page()
        {
            InitializeComponent();
        }

        void ContinuarClicked(object sender, System.EventArgs e)
        {
            App.UsuarioLogado.Nome_fantasia = fantasiaEntry.Text;
            App.UsuarioLogado.Razao_social = razaoEntry.Text;
            App.UsuarioLogado.Cnpj = cnpjEntry.Text;


            var ListaRes = new List<ValidationResult>();
            var Contexto = new ValidationContext(App.UsuarioLogado);
            var IsValid = Validator.TryValidateObject(App.UsuarioLogado, Contexto, ListaRes);

            string ErroMensagem = string.Empty;

            if (App.UsuarioLogado.Tipo_usuario == "I")
            {
                if (!string.IsNullOrEmpty(cnpjEntry.Text))
                {
                    if (!Utils.ValidaCnpj(cnpjEntry.Text))
                    {
                        ErroMensagem = "CNPJ inválido. \n";
                    }
                }

                ErroMensagem += Utils.ValildarObjeto(App.UsuarioLogado, "Nome_fantasia,Razao_social,Cnpj");

                if (string.IsNullOrEmpty(ErroMensagem))
                {
                    var nextStep = new CadastroStep4Page();
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

        void CnpjTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            cnpjEntry.Text = Utils.AplicaMascaraCnpj(cnpjEntry.Text);
        }
    }
}
