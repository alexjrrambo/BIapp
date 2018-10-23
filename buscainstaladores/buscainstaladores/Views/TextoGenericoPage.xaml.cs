using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class TextoGenericoPage : ContentPage
    {
        public string DialogResult { get; set; }
        public TextoGenericoPage()
        {
            InitializeComponent();

            this.Title = "Cancelar";
        }

        public string GetText()
        {
            return textoEditor.Text;
        }

        public void SetText(string texto)
        {
            textoEditor.Text = texto;
        }

        public void SetTextLabelMensagem(string texto)
        {
            labelMensagem.Text = texto;
        }

        public void SetLabelText(string texto)
        {
            labelTitulo.Text = texto;
        }

        public void SetButton1Text(string texto)
        {
            button1.Text = texto;
        }

        public void SetButton2Text(string texto)
        {
            button2.Text = texto;
        }

        public void SetButton1Visible(bool visivel)
        {
            button1.IsVisible = visivel;
        }

        public void SetButton2Visible(bool visivel)
        {
            button2.IsVisible = visivel;
        }

        public void SetVisibleForm(bool visible)
        {
            form.IsVisible = visible;
        }

        public void SetVisibleFormMensagem(bool visible)
        {            
            formMensagem.IsVisible = visible;
        }

        public void SetCommandConfirmar(Command command)
        {
            button1.Command = command;
            button1.CommandParameter = this;
        }

        void Button2Clicked(object sender, System.EventArgs e)
        {
            DialogResult = "button2";
            Navigation.PopAsync();
        }

        void ButtonConfirmarClicked(object sender, System.EventArgs e)
        {
            DialogResult = "buttonConfirmar";
            Navigation.PopAsync();
        }

    }
}
