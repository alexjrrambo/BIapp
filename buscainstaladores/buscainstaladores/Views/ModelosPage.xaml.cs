using System;
using System.Collections.Generic;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class ModelosPage : ContentPage
    {
        public ModelosPage(BuscarViewModel contexto)
        {
            InitializeComponent();

            this.Title = "Modelo";

            BindingContext = contexto;

            //precos
            hiwallImage.GestureRecognizers.Add(new TapGestureRecognizer(ImageTappedReservar));
            casseteImage.GestureRecognizers.Add(new TapGestureRecognizer(ImageTappedReservar));
            pisoImage.GestureRecognizers.Add(new TapGestureRecognizer(ImageTappedReservar));
        }

        void ImageTappedReservar(View arg1, object arg2)
        {
            Image origem = (Image)arg1;
            string textoModelo = "";
            if (origem.Source.ToString().Contains("hiwall"))
                textoModelo = "Hi-Wall";    
            if (origem.Source.ToString().Contains("cassete"))
                textoModelo = "Cassete";  
            if (origem.Source.ToString().Contains("piso"))
                textoModelo = "Piso Teto";  
    

            BuscarViewModel contexo = (BuscarViewModel)this.BindingContext;
            ReservarPage page = new ReservarPage(contexo);
            page.SourceModelo = origem.Source;
            page.TextModelo = textoModelo;
            page.Initialize();

            Navigation.PushAsync(page);
        }
    }
}
