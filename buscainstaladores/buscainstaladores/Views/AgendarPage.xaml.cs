using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using buscainstaladores.Models;
using buscainstaladores.Services;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class AgendarPage : ContentPage
    {
        public string Origem { get; set; }
        public Mensagem MensagemConfimar { get; set; }
        public AgendarPage(Agenda AgendaParam, Mensagem msg =null)
        {
            InitializeComponent();

            if (App.UsuarioLogado != null)
                BindingContext = new AgendaViewModel(Navigation,AgendaParam,"R");

            if (AgendaParam.Id != null && AgendaParam.Id != 0 && msg==null)
            {
                Origem = "Editar";
                reservarToobarButton.Text = "Salvar";
                HoraVisivel.Time = TimeSpan.Parse(HoraInvisivel.Text);
                DataVisivel.Date = DateTime.ParseExact(DataInvisivel.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            if(msg!=null)
            {
                MensagemConfimar = msg;
                Origem = "Confirmar";
                reservarToobarButton.Text = "Confirmar";
                HoraVisivel.Time = TimeSpan.Parse(HoraInvisivel.Text);
                DataVisivel.Date = DateTime.ParseExact(DataInvisivel.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            DataInvisivel.Text = DataVisivel.Date.ToString("dd/MM/yyyy");
        }

        void SelectedIndexChangedEstado(object sender, System.EventArgs e)
        {                            
            ((AgendaViewModel)BindingContext).CommandCarregaCidade.Execute(true);
        }

        void DateSelectedData(object sender, Xamarin.Forms.DateChangedEventArgs e)
        {
            DataInvisivel.Text = DataVisivel.Date.ToString("dd/MM/yyyy");
        }

        void UnfocusedHora(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            HoraInvisivel.Text = HoraVisivel.Time.ToString();
        }

        async void SalvarClicked(object sender, System.EventArgs e)
        {
            IsLoading(true);
            await Task.Delay(TimeSpan.FromSeconds(1));
            string erroMensagem = string.Empty;

            if (((AgendaViewModel)BindingContext).AgendaAtual.Valor == null || 
                ((AgendaViewModel)BindingContext).AgendaAtual.Valor == "0,00" || 
                ((AgendaViewModel)BindingContext).AgendaAtual.Valor == "0.00" || 
                ((AgendaViewModel)BindingContext).AgendaAtual.Valor == "A negociar" || 
                ((AgendaViewModel)BindingContext).AgendaAtual.Valor == "0")
            {
                erroMensagem += "Deve ser informado um valor para o serviço. \n";
            }

            if(DataVisivel.Date<=DateTime.Now)            
            {
                erroMensagem += "Não é possível agendar o serviço na data informada. \n";
            }

            erroMensagem += Utils.ValildarObjeto(((AgendaViewModel)BindingContext).AgendaAtual, "Titulo,Dtevento,Hora,Valor,Cep,Id_estado,Id_cidade,Endereco,Numero,Bairro,Cep");            

            if (string.IsNullOrEmpty(erroMensagem))
            {
                if(Origem == "Confirmar")
                {
                    AgendaViewModel.EfetuaReserva(((AgendaViewModel)BindingContext).AgendaAtual,
                                                  ((AgendaViewModel)BindingContext).EstadoSelecionado,
                                                  ((AgendaViewModel)BindingContext).CidadeSelecionada);
                    MensagemViewModel.ConfirmarAgendaMensagem(MensagemConfimar);
                    Navigation.PopAsync();
                }
                else
                {
                    ((AgendaViewModel)BindingContext).CommandReservarAgenda.Execute(true);   
                }
            }
            else
            {
                DisplayAlert("Aviso", erroMensagem, "OK");
            }
            IsLoading(false);
        }

        void CepTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            cepEntry.Text = Utils.AplicaMascaraCep(cepEntry.Text);
        }

        void NumeroTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            numeroEntry.Text = Utils.RemoveNaoNumericos(numeroEntry.Text);
        }

        void ValorTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            string valor = Utils.RemoveNaoNumericos(valorEntry.Text);
            valorEntry.Text = Utils.AplicaMascaraDinheiro(valor);
        }

        private void IsLoading(bool carregando)
        {
            carregandoIndicator.IsRunning = carregando;
            carregandoContent.IsVisible = carregando;
        }
    }
}
