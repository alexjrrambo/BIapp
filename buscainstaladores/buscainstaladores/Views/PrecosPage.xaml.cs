using System;
using System.Collections.Generic;
using buscainstaladores.Services;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class PrecosPage : ContentPage
    {
        public PrecosPage()
        {
            InitializeComponent();

            BindingContext = new TabelaPrecoViewModel(Navigation);
        }

        void FiltroModeloSelected(object sender, System.EventArgs e)
        {
            ((TabelaPrecoViewModel)BindingContext).CommandRecarregarPrecos.Execute(true);
        }

        void InstalacaoTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            Entry valorEntry = (Entry)sender;

            string valor = valorEntry.Text;
            valor = Utils.RemoveNaoNumericos(valor);
            valor = Utils.AplicaMascaraDinheiro(valor);

            valorEntry.Text = valor;
        }

        void HigienizacaoTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            Entry valorEntry = (Entry)sender;

            string valor = valorEntry.Text;
            valor = Utils.RemoveNaoNumericos(valor);
            valor = Utils.AplicaMascaraDinheiro(valor);

            valorEntry.Text = valor;
        }
    }
}
