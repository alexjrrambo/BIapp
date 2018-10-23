using System;
using System.Collections.Generic;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class PerfilPage : ContentPage
    {
        public PerfilPage()
        {
            InitializeComponent();

            NavigationPage.SetBackButtonTitle(this, "");

            BindingContext = new UsuarioViewModel(Navigation);

            if (App.UsuarioLogado != null)
            {
                if (App.UsuarioLogado.Tipo_usuario!="I")
                {
                    usuarioTableSection.Remove(fotosViewcell);
                    usuarioTableSection.Remove(transferirViewcell);
                    usuarioTableSection.Remove(premiumViewcell);
                }
            }
                
        }

        void LogoutTapped(object sender, System.EventArgs e)
        {
            ((UsuarioViewModel)BindingContext).CommandLogout.Execute(true);
        }

        void EditarPerfilTapped(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new OpcoesEditarPage());
        }

        void PremiumTapped(object sender, System.EventArgs e)
        {
            if (App.UsuarioLogado.Tipo_usuario == "I" && App.UsuarioLogado.Premium != "1")
            {
                //Device.OpenUri(new Uri("https://buscainstaladores.com/assinar"));
                Navigation.PushAsync(new AssinarPage());   
            }
            else
            {
                Navigation.PushAsync(new AreaPremiumPage());   
            }
        }

        void FotosTapped(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new FotosPage());
        }

        void TransferirTapped(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ExtratoPage());
        }
    }
}
