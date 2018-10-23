using System;
using System.Collections.Generic;
using buscainstaladores.Database;
using buscainstaladores.Models;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class PrincipalPage : TabbedPage
    {        
        public PrincipalPage()
        {                              

            List<Usuario> usuario = new UsuarioDataAccess().GetUsuarioList();

            if (usuario.Count != 0)
            {                
                App.UsuarioLogado = new Usuario();
                App.UsuarioLogado = usuario[0];
            }


            if (App.UsuarioLogado == null)
            {                
                Navigation.PushModalAsync(new LoginPage());
            }

            Page mensagemPage, perfilPage, servicoPage, buscaPage = null;                        

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    mensagemPage = new NavigationPage(new MensagemPage())
                    {
                        Title = "Mensagens"
                    };

                    servicoPage = new NavigationPage(new ServicoPage())
                    {
                        Title = "Serviços"
                    };

                    perfilPage = new NavigationPage(new PerfilPage())
                    {
                        Title = "Perfil"
                    };

                    buscaPage = new NavigationPage(new BuscarPage())
                    {
                        Title = "Buscar"
                    };
                    mensagemPage.Icon = "conversation.png";
                    perfilPage.Icon = "profile.png";
                    servicoPage.Icon = "services.png";
                    buscaPage.Icon = "search.png";
                    break;
                default:         
                    var c = Color.FromHex("#2e3842");
                    this.BarBackgroundColor = c;
                    mensagemPage = new MensagemPage()
                   {
                       Title = "Chat"
                   };

                    servicoPage = new ServicoPage()
                    {
                        Title = "Serviços"
                    };

                    perfilPage = new PerfilPage()
                    {
                        Title = "Perfil"
                    };

                    buscaPage = new BuscarPage()
                    {
                        Title = "Buscar"
                    };
                    break;
            }



            Children.Add(buscaPage);
            Children.Add(mensagemPage);
            Children.Add(servicoPage);
            Children.Add(perfilPage);

            Title = Children[0].Title;

            BindingContext = this.CurrentPage;


            this.CurrentPageChanged += CurrentPageHasChanged;

        }


        protected void CurrentPageHasChanged(object sender, EventArgs e)         
        {
            if (Device.RuntimePlatform == Device.iOS)
                this.Title = this.CurrentPage.Title;
            else
                this.Title = "buscaInstaladores.com";            
        } 

    }
}