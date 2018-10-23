using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using buscainstaladores.Services;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views.Cadastro
{
    public partial class CadastroFinalStep : ContentPage
    {
        public CarouselPage Page { get; set; }
        public CadastroFinalStep()
        {
            InitializeComponent();

            BindingContext = new UsuarioViewModel(Navigation);
        }

        void ContinuarClicked(object sender, System.EventArgs e)
        {
            App.UsuarioLogado.Email = emailEntry.Text;
            App.UsuarioLogado.Senha = senhaEntry.Text;

            var ListaRes = new List<ValidationResult>();
            var Contexto = new ValidationContext(App.UsuarioLogado);
            var IsValid = Validator.TryValidateObject(App.UsuarioLogado, Contexto, ListaRes,true);

            string ErroMensagem = string.Empty;

            ErroMensagem = Utils.ValildarObjeto(App.UsuarioLogado, "Email,Senha");

            if (string.IsNullOrEmpty(ErroMensagem))
            {
                if (App.UsuarioLogado.Tipo_usuario == "U")
                    ((UsuarioViewModel)BindingContext).CommandInserirUsuario.Execute(true);
                else
                    ((UsuarioViewModel)BindingContext).CommandInserirInstalador.Execute(true);
            }
            else
            {
                labelMensagem.Text = ErroMensagem;
            }
        }
    }
}
