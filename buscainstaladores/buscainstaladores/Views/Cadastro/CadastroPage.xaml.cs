using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace buscainstaladores.Views.Cadastro
{
    public partial class CadastroPage : CarouselPage
    {
        public CadastroPage()
        {
            InitializeComponent();

            //instancia novo usuario
            App.UsuarioLogado = new Models.Usuario();

            var step1 = new CadastroStep1Page();
            step1.Page = this;

            BindingContext = this.CurrentPage;

            this.Children.Add(step1);

            //Tratamento especial para tratar navegacao entre as paginas CarouselPage
            this.CurrentPageChanged +=  (s, e) =>
            {
                var pageAtual = Children.IndexOf(CurrentPage);

                var step2 = Children.IndexOf(Children.ElementAtOrDefault(1));
                var step3 = Children.IndexOf(Children.ElementAtOrDefault(2));
                var step4 = Children.IndexOf(Children.ElementAtOrDefault(3));
                var step5 = Children.IndexOf(Children.ElementAtOrDefault(4));

                if (pageAtual < step5)
                {
                    Children.RemoveAt(4);
                }
                else if (pageAtual < step4)
                {                                        
                    Children.RemoveAt(3);
                }
                else if ( pageAtual < step3)
                {
                    Children.RemoveAt(2);
                }
                else if (pageAtual < step2)
                {
                    Children.RemoveAt(1);
                }
            };

        }
    }
}
