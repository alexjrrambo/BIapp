using System;
using System.Collections.Generic;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class AvaliacaoPage : ContentPage
    {
        public AvaliacaoPage(AgendaViewModel contexto)
        {
            InitializeComponent();

            this.Title = "Avaliar";

            BindingContext = contexto;
        }

        void SubRatingClicked(object sender, System.EventArgs e)
        {
            CarregaRating("-");
        }

        void AddRatingClicked(object sender, System.EventArgs e)
        {
            CarregaRating("+");
        }

        private void CarregaRating(string acao)
        {
            int rating = int.Parse(ratingLabel.Text);

            if (acao == "+" && rating < 5)
                rating = rating + 1;

            if (acao == "-" && rating > 1)
                rating = rating - 1;

            imageAvaliacao.Source = "stars" + rating.ToString() + ".png";
            ratingLabel.Text = rating.ToString();
        }
    }
}
