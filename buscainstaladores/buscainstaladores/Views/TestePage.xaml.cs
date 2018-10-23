using System;
using System.Collections.Generic;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class TestePage : ContentPage
    {
        MensagemViewModel vm;
        public TestePage()
        {
            InitializeComponent();

            BindingContext = vm = new MensagemViewModel(Navigation);


 
        }
    }    
}
