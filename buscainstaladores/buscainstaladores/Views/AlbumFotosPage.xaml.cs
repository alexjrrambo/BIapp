using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class AlbumFotosPage : ContentPage
    {
        public ImageSource ImageZoom { get; set; }
        public AlbumFotosPage()
        {
            InitializeComponent();
            this.Title = "Fotos";
            Initialize();
        }

        public void Initialize()
        {            

            if (ImageZoom != null)
            {                
                imageZoom.Source = ImageZoom;
                gridFotosZoom.IsVisible = true;
                gridFotos.IsVisible = false;
            }

            if (ImageZoom == null)
            {
                gridFotosZoom.IsVisible = false;
                gridFotos.IsVisible = true;
                string url = App.UrlWebService + "imagens";
                string result = "";
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = client.GetAsync(url).Result)
                    {
                        using (HttpContent content = response.Content)
                        {
                            result = content.ReadAsStringAsync().Result;
                            string[] imagens = result.Split(';');

                            int linha = 0;
                            int coluna = 0;
                            for (int i = 0; i < imagens.Length; i++)
                            {
                                RowDefinition row = new RowDefinition();
                                row.Height = GridLength.Auto;
                                gridFotos.RowDefinitions.Add(row);

                                Image img = new Image();
                                img.Source = imagens[i];
                                img.HeightRequest = 150;
                                img.Aspect = Aspect.AspectFill;
                                img.GestureRecognizers.Add(new TapGestureRecognizer(ImageTapped));

                                if (i % 2 == 0)
                                {
                                    linha = i;
                                    coluna = 0;
                                }

                                gridFotos.Children.Add(img, coluna, linha);
                                coluna++;
                            }
                        }
                    }
                }
            }
        }

        void ImageTapped(View arg1, object arg2)
        {
            Image img = (Image) arg1;

            AlbumFotosPage page = new AlbumFotosPage();
            page.ImageZoom = img.Source;
            page.Initialize();

            Navigation.PushAsync(page);
        }
    }
}
