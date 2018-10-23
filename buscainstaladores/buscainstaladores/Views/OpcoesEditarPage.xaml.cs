using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class OpcoesEditarPage : ContentPage
    {
        public OpcoesEditarPage()
        {
            InitializeComponent();

            this.Title = "Opções";

            if (App.UsuarioLogado != null)
            {
                if (App.UsuarioLogado.Tipo_usuario != "I")
                {
                    usuarioTableSection.Remove(financViewCell);
                    usuarioTableSection.Remove(precoViewCell);
                }
            }
        }

        void InformacoesTapped(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new EditarPerfilPage());
        }

        void SegurancaTapped(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new SegurancaPage());
        }

        void PrecoTapped(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new PrecosPage());
        }

        void FinanceiraTapped(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ContaFinanceiraPage());
        }
    }
}
