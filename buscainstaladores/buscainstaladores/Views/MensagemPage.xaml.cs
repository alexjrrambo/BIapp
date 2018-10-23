using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using buscainstaladores.Models;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class MensagemPage : ContentPage
    {
        public MensagemPage()
        {
            InitializeComponent();

            NavigationPage.SetBackButtonTitle(this, "");

            if (App.UsuarioLogado != null)
            {
                BindingContext = new MensagemViewModel(Navigation);
                ExisteRegistros();
            }                
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (App.UsuarioLogado != null)
            {                
                BindingContext = new MensagemViewModel(Navigation);
                Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await CarregaMensagens();
                    });
                    return true;
                });
            }

        }

        public Task CarregaMensagens()
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            try
            {
                if (App.UsuarioLogado != null)
                {
                    ((MensagemViewModel)BindingContext).CommandListaThreadChat.Execute(true);
                }
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return tcs.Task;
        }

        async void ConversaTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var selectedItem = e.Item as Mensagem;

            if (Device.RuntimePlatform == Device.iOS)
            {
                await Navigation.PushAsync(new ChatPage(selectedItem));
            }
            else
            {                
                //await Navigation.PushModalAsync(new NavigationPage(new ChatPage(selectedItem)));   
                await Navigation.PushAsync(new ChatPage(selectedItem));
            }                

        }

        private void ExisteRegistros()
        {
            ObservableCollection<Mensagem> listaMensagens = ((MensagemViewModel)BindingContext).ListaMensagens;
            if (listaMensagens.Count != 0)
            {
                semRegistroLayout.IsVisible = false;
                MensagensList.IsVisible = true;
            }
            else
            {
                semRegistroLayout.IsVisible = true;
                MensagensList.IsVisible = false;
            }
        }
    }
}
