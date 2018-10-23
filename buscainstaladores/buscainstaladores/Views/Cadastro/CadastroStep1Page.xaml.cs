using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace buscainstaladores.Views.Cadastro
{
    public partial class CadastroStep1Page : ContentPage
    {
        public CarouselPage Page { get; set; }
        public CadastroStep1Page()
        {
            InitializeComponent();
        }

        void ConsumidorClicked(object sender, System.EventArgs e)
        {
            App.UsuarioLogado.Tipo_usuario = "U";
            NextStep();
        }

        void InstaladorClicked(object sender, System.EventArgs e)
        {
            App.UsuarioLogado.Tipo_usuario = "I";
            NextStep();
        }

        private void NextStep()
        {
            var step2 = new CadastroStep2Page();
            step2.Page = Page;
            Page.Children.Add(step2);
            Page.CurrentPage = step2;
        }
    }
}
