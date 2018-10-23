using System;
using System.Collections.Generic;
using buscainstaladores.Models;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class DetalheServicoPage : ContentPage
    {
        public DetalheServicoPage(Agenda AgendaParam)
        {
            InitializeComponent();

            NavigationPage.SetBackButtonTitle(this, "");

            if (App.UsuarioLogado != null)
            {
                BindingContext = new AgendaViewModel(Navigation, AgendaParam);

                //caso for usuário
                if(App.UsuarioLogado.Tipo_usuario == "U")
                {
                    taxaLabel.Text = "10,00";   
                }                    
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (App.UsuarioLogado != null)
                ((AgendaViewModel)BindingContext).CommandRecarregarAgendaAtual.Execute(true);
        }

        void CancelarClicked(object sender, System.EventArgs e)
        {
            ((AgendaViewModel)BindingContext).CommandPageCancelar.Execute(true);            
        }

        void EditarClicked(object sender, System.EventArgs e)
        {            
            ((AgendaViewModel)BindingContext).CommandEditarAgenda.Execute(true);
        }

        void PagamentoClicked(object sender, System.EventArgs e)
        {
            ((AgendaViewModel)BindingContext).CommandPagamento.Execute(true);
        }
    }
}
