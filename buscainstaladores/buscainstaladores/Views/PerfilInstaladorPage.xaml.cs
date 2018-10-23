using System;
using System.Collections.Generic;
using buscainstaladores.Models;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class PerfilInstaladorPage : ContentPage
    {
        public Command CommandTapped { get; set; }
        public string Destinatario { get; set; }
        public PerfilInstaladorPage(Usuario usuarioParam)
        {
            InitializeComponent();

            NavigationPage.SetBackButtonTitle(this, "");
            this.Title = usuarioParam.Nome_fantasia;


            if(usuarioParam.Premium!="1")
            {
                colunaCentro.Width = 0;
                fotosImage.IsVisible = false;
                perfilTable.Root.Remove(descricaoSection);
            }
                

            Destinatario = usuarioParam.Nome_fantasia;

            BindingContext = new BuscarViewModel(Navigation,usuarioParam);

            //acoes
            reservarImage.GestureRecognizers.Add(new TapGestureRecognizer(ImageTappedReservar));
            fotosImage.GestureRecognizers.Add(new TapGestureRecognizer(ImageTappedFotos));
            mensagemImage.GestureRecognizers.Add(new TapGestureRecognizer(ImageTappedMensagem));
            imageAvaliacao.GestureRecognizers.Add(new TapGestureRecognizer(ImageTappedAvaliacao));


            //precos
            hiwallImage.GestureRecognizers.Add(new TapGestureRecognizer(ImageTappedPreco));
            casseteImage.GestureRecognizers.Add(new TapGestureRecognizer(ImageTappedPreco));
            pisoImage.GestureRecognizers.Add(new TapGestureRecognizer(ImageTappedPreco));
        }

        void ImageTappedAvaliacao(View arg1, object arg2)
        { 
            AvaliacoesView page = new AvaliacoesView();

            Navigation.PushAsync(page);
        }

        void ImageTappedMensagem(View arg1, object arg2)
        {
            if(App.UsuarioLogado.Tipo_usuario == "I")
            {
                DisplayAlert("Aviso", "Apenas clientes podem conversar com instaladores", "OK");
            }
            else
            {
                Mensagem msg = new Mensagem();
                msg.Id_remetente = App.UsuarioLogado.Id;
                msg.Id_destinatario = int.Parse(idInstaladorLabel.Text);
                msg.Destinatario = Destinatario;
                ChatPage page = new ChatPage(msg);

                Navigation.PushAsync(page);   
            }
        }

        void ImageTappedFotos(View arg1, object arg2)
        {                        
            AlbumFotosPage page = new AlbumFotosPage();

            Navigation.PushAsync(page);
        }

        void ImageTappedReservar(View arg1, object arg2)
        {
            if (App.UsuarioLogado.Tipo_usuario == "I")
            {
                DisplayAlert("Aviso", "Apenas clientes podem reservar serviços", "OK");
            }
            else
            {
                BuscarViewModel contexo = (BuscarViewModel)this.BindingContext;

                Navigation.PushAsync(new ModelosPage(contexo));
            }
        }

        void ImageTappedPreco(View arg1, object arg2)
        {            
            PrecoHiwall.IsVisible = false;            
            PrecoCassete.IsVisible = false;            
            PrecoPiso.IsVisible = false;  

            hiwallImage.BackgroundColor = Color.White;            
            casseteImage.BackgroundColor = Color.White;            
            pisoImage.BackgroundColor = Color.White;            

            Image origem = (Image)arg1;

            if(origem.Source == hiwallImage.Source)
            {
                hiwallImage.BackgroundColor = Color.FromHex("#ECECEC");
                PrecoHiwall.IsVisible = true;
            }
                            
            if (origem.Source == casseteImage.Source)
            {
                casseteImage.BackgroundColor = Color.FromHex("#ECECEC");
                PrecoCassete.IsVisible = true;
            }                
            
            if (origem.Source == pisoImage.Source)
            {
                pisoImage.BackgroundColor = Color.FromHex("#ECECEC");
                PrecoPiso.IsVisible = true;      
            }
        }
    }
}
