using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using buscainstaladores.Models;
using buscainstaladores.ViewModels;
using Corcav.Behaviors;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class ServicoPage : ContentPage
    {
        public ServicoPage()
        {
            InitializeComponent();

            if (App.UsuarioLogado != null)
            {
                BindingContext = new AgendaViewModel(Navigation);

                if (App.UsuarioLogado.Tipo_usuario != "I")
                {
                    filtroLayout.IsVisible = false;
                }

                ExisteRegistros();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (App.UsuarioLogado != null)
                ((AgendaViewModel)BindingContext).CommandRecarregarAgenda.Execute(true);
            
        }

        async void AgendaTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var selectedItem = e.Item as Agenda;

            await Navigation.PushAsync(new DetalheServicoPage(selectedItem));
        }

        void FiltroDateSelected(object sender, Xamarin.Forms.DateChangedEventArgs e)
        {
            ((AgendaViewModel)BindingContext).CommandRecarregarAgenda.Execute(true);
            ExisteRegistros();
        }

        private void ExisteRegistros()
        {
            ObservableCollection<Agenda> listaAegnda = ((AgendaViewModel)BindingContext).ListaAgenda;
            if (listaAegnda.Count != 0)
            {
                semRegistroLayout.IsVisible = false;
                ServicosList.IsVisible = true;

            }
            else
            {
                if (App.UsuarioLogado.Tipo_usuario == "I")
                {
                    semRegLabel.Text = "Você ainda não possui serviços reservados nesta data.";
                }
                else
                {
                    semRegLabel.Text = "Você ainda não possui serviços reservados.";   
                }   

                semRegistroLayout.IsVisible = true;
                ServicosList.IsVisible = false;
            }
        }
    }
}
