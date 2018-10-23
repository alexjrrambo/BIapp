using System;
using buscainstaladores.Models;
using Xamarin.Forms;

namespace buscainstaladores.Custom
{
    public class SelectorDataTemplate : DataTemplateSelector
    {
        private readonly DataTemplate textInDataTemplate;
        private readonly DataTemplate textOutDataTemplate;

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as Mensagem;
            if (messageVm == null)
                return null;
            
            return messageVm.Id_remetente==App.UsuarioLogado.Id ? this.textInDataTemplate : this.textOutDataTemplate;
        }


        public SelectorDataTemplate()
        {
            this.textInDataTemplate = new DataTemplate(typeof(TextInViewCell));
            this.textOutDataTemplate = new DataTemplate(typeof(TextOutViewCell));
        }

    }
}
