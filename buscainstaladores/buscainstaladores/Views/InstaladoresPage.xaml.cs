using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using buscainstaladores.Models;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class InstaladoresPage : ContentPage
    {
        public InstaladoresPage(BuscarViewModel contexto)
        {
            InitializeComponent();

            NavigationPage.SetBackButtonTitle(this, "");
            this.Title = "Resultados";

            BindingContext = contexto;

            ExisteRegistros();
        }

        void InstaladorTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var selectedItem = e.Item as Usuario;

            Navigation.PushAsync(new PerfilInstaladorPage(selectedItem));
        }

        private void ExisteRegistros()
        {
            ObservableCollection<Usuario> listaInstaladores = ((BuscarViewModel)BindingContext).ListaInstaladores;
            if (listaInstaladores.Count != 0)
            {
                semRegistroLayout.IsVisible = false;
                InstaladoresList.IsVisible = true;
            }
            else
            {
                semRegistroLayout.IsVisible = true;
                InstaladoresList.IsVisible = false;
            }
        }
    }
}
