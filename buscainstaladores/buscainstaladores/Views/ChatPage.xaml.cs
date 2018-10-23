using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using buscainstaladores.Models;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class ChatPage : ContentPage
    {
        public Mensagem Mensagens { get; set; }
        public Task ThreadChat { get; set; }
        public ChatPage(Mensagem MensagemParam)
        {            
           InitializeComponent();

            //NavigationPage.SetBackButtonTitle(this, "");

            if (App.UsuarioLogado != null)
            {
                BindingContext = new MensagemViewModel(Navigation, MensagemParam);

                Mensagens = MensagemParam;

                this.Title = MensagemParam.Destinatario;

                if(App.UsuarioLogado.Tipo_usuario!="I")
                {
                    reservarLayout.IsVisible = false;
                }
                ((INotifyCollectionChanged)ChatList.ItemsSource).CollectionChanged += ScrollToEndAnimated;   
                ScrollToEnd(null, null); 
            }
              
        }



        protected override async void OnAppearing()
        {
            ((MensagemViewModel)BindingContext).CommandRecarregarMensagem.Execute(Mensagens);

            ScrollToEnd(null, null);                  

            base.OnAppearing();
            if (App.UsuarioLogado != null)
            {                
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
                    ((MensagemViewModel)BindingContext).CommandThreadChat.Execute(true);
                }
            }
            catch (Exception ex)
               {
                   tcs.SetException(ex);
               }         
            return tcs.Task; ;
        }

        void EnviarClicked(object sender, System.EventArgs e)
        {
            ((MensagemViewModel)BindingContext).CommandEnviarMensagem.Execute(true);
            ScrollToEndAnimated(null,null);
        }

        private void ScrollToEnd(object sender, NotifyCollectionChangedEventArgs e)
        {
            List<Mensagem> someVar = ((IEnumerable<Mensagem>)this.ChatList.ItemsSource).ToList();
            if(someVar.Any())
                ChatList.ScrollTo(someVar[someVar.Count - 1], ScrollToPosition.End, false);
        }

        private void ScrollToEndAnimated(object sender, NotifyCollectionChangedEventArgs e)
        {
            List<Mensagem> someVar = ((IEnumerable<Mensagem>)this.ChatList.ItemsSource).ToList();
            if (someVar.Any())
                ChatList.ScrollTo(someVar[someVar.Count - 1], ScrollToPosition.End, true);
        }
    }
}
