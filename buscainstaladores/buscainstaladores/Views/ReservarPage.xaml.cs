using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using buscainstaladores.Models;
using buscainstaladores.Services;
using buscainstaladores.ViewModels;
using Xamarin.Forms;

namespace buscainstaladores.Views
{
    public partial class ReservarPage : ContentPage
    {        
        public ImageSource SourceModelo { get; set; }
        public String TextModelo { get; set; }
        public List<string> BtuLista { get; set; }
        public ReservarPage(BuscarViewModel contexto)
        {
            InitializeComponent();

            BindingContext = contexto;
        }

        public void Initialize()
        {
            modeloImage.Source = SourceModelo;
            modeloText.Text = TextModelo;

            CarregaListaBtu();
        }

        async void ReservaClicked(object sender, System.EventArgs e)
        {
            carregandoIndicator.IsRunning = true;
            carregandoContent.IsVisible = true;
            await Task.Delay(TimeSpan.FromSeconds(1));
            EfetuaReserva();
            carregandoIndicator.IsRunning = false;
            carregandoContent.IsVisible = false;
        }

        private void EfetuaReserva()
        {
            Estado estado = (Estado)estadoPicker.SelectedItem;
            Cidade cidade = (Cidade)cidadePicker.SelectedItem;
            Agenda agenda = new Agenda();

            //campos obrigatorios
            agenda.Dtevento = DataVisivel.Date.ToString("dd/MM/yyyy");
            agenda.Hora = HoraVisivel.Time.ToString();
            agenda.Cep = cepEntry.Text;
            agenda.Endereco = ruaEntry.Text;
            agenda.Numero = numeroEntry.Text;
            agenda.Bairro = bairroEntry.Text;

            if (estado != null)
                agenda.Id_estado = estado.Cod_estado;

            if (cidade != null)
            agenda.Id_cidade = cidade.Cod_cidade;

            string erroMensagem = string.Empty;

            if (DataVisivel.Date <= DateTime.Now)
            {
                erroMensagem += "Não é possível agendar o serviço na data informada. \n";
            }

            if (Capacidade.SelectedItem == null)
            {
                erroMensagem += "BTU deve ser informado.\n";
            }

            if (Servico.SelectedItem == null)
            {
                erroMensagem += "Serviço deve ser informado.\n";
            }

            if (Tipo.SelectedItem == null)
            {
                erroMensagem += "Tipo de residência deve ser informado.\n"; 
            }                   

            erroMensagem += Utils.ValildarObjeto(agenda, "Dtevento,Hora,Cep,Id_estado,Id_cidade,Endereco,Numero,Bairro,Cep");

            if (string.IsNullOrEmpty(erroMensagem))
            {
                string btu = Capacidade.SelectedItem.ToString();
                btu = btu.Substring(0, btu.IndexOf("B") - 1);
                agenda.Titulo = Servico.SelectedItem.ToString() + " ar condicionado split " + TextModelo + " " + btu + " BTUs";
                agenda.Tipo_residencia = Tipo.SelectedItem.ToString();
                agenda.Id_cliente = App.UsuarioLogado.Id;
                agenda.Id_instalador = ((BuscarViewModel)BindingContext).InstaladorAtual.Id;
                // 7 = "Aguardando confirmação;
                agenda.Situacao = "5";
                agenda.Valor = precoOrcado.Text;

                int id_agenda = AgendaViewModel.EfetuaReserva(agenda, estado, cidade);


                Mensagem msg = new Mensagem();
                if (agenda.Valor == null || agenda.Valor == "0,00" || agenda.Valor == "0.00" || agenda.Valor == "A negociar" || agenda.Valor == "0")
                {
                    msg.Tipo = "O";
                }
                else
                {
                    msg.Tipo = "R";
                }
                msg.Id_destinatario = ((BuscarViewModel)BindingContext).InstaladorAtual.Id;

                msg.Id_agenda = id_agenda;
                msg.Texto = "Olá, desejo agendar uma " + Servico.SelectedItem.ToString() + ".<br>" +
                    "Equipamento: Ar condicionado " + TextModelo + " " + btu + " BTUs <br> " +
                    "<input type=\"hidden\" id=\"a_descricao\" value=\"" + Servico.SelectedItem.ToString() + " ar condicionado split " + btu + " BTUs\" >" +
                    "Data desejada: " + agenda.Dtevento + " às " + agenda.Hora + " <br> " +
                    "<input type=\"hidden\" id=\"a_dtevento\" value=\"" + agenda.Dtevento + "\" >" +
                    "<input type=\"hidden\" id=\"a_hora\" value=\"" + agenda.Hora + "\" >" +
                    "Endereço: " + agenda.Endereco + ", " + agenda.Numero + ", " + agenda.Bairro + ", " + agenda.Cep + " <br> " + cidade.Nom_cidade + " - " + estado.Sgl_estado + " <br> " +
                    "<input type=\"hidden\" id=\"a_endereco\" value=\"" + agenda.Endereco + "\" >" +
                    "<input type=\"hidden\" id=\"a_estado\" value=\"" + estado.Cod_estado + "\" >" +
                    "<input type=\"hidden\" id=\"a_cidade\" value=\"" + cidade.Cod_cidade + "\" >" +
                    "<input type=\"hidden\" id=\"a_numero\" value=\"" + agenda.Numero + "\" >" +
                    "<input type=\"hidden\" id=\"a_bairro\" value=\"" + agenda.Bairro + "\" >" +
                    "<input type=\"hidden\" id=\"a_cep\" value=\"" + agenda.Cep + "\" >" +
                    "Tipo de residência: " + agenda.Tipo_residencia + " <br> " +
                    "<input type=\"hidden\" id=\"a_tipo_residencia\" value=\"" + agenda.Tipo_residencia + "\" >" +
                    "Valor orçado: " + agenda.Valor + "<br>" +
                    "<input type=\"hidden\" id=\"a_valor\" value=\"" + agenda.Valor + "\" >";

                MensagemViewModel.EnviarMensagem(msg);

                DisplayAlert("Aviso", "Reserva efetuada com sucesso :)\n"+"Você receberá um e-mail quando o instalador confirmar o serviço.", "OK");

                Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1]);

                Navigation.PopAsync();
            }
            else
            {
                DisplayAlert("Aviso", erroMensagem, "OK");
            }
        }

        void BtuSelectedIndexChanged(object sender, System.EventArgs e)
        {
            CarregaPrecoPardao();
        }

        void ServicoSelectedIndexChanged(object sender, System.EventArgs e)
        {
            CarregaPrecoPardao();
        }

        void SelectedIndexChangedEstado(object sender, System.EventArgs e)
        {
            ((BuscarViewModel)BindingContext).CommandCarregaCidade.Execute(true);
        }

        private void CarregaPrecoPardao()
        {
            int modelo=0;

            if (Capacidade.SelectedItem != null && Servico.SelectedItem != null)
            {
                string btu = Capacidade.SelectedItem.ToString();
                string servico = Servico.SelectedItem.ToString().Substring(0, 1);

                if (TextModelo == "Hi-Wall")
                    modelo = 1;

                if (TextModelo == "Cassete")
                    modelo = 2;

                if (TextModelo == "Piso")
                    modelo = 3;

                btu = btu.Substring(0, btu.IndexOf("B") - 1);

                ((BuscarViewModel)BindingContext).CarregarPrecoPadrao(btu, modelo.ToString(), servico);
            }
        }

        private void CarregaListaBtu()
        {
            BtuLista = new List<string>();

            BtuLista.Add("7000 BTU (5000 até 8000)");
            BtuLista.Add("9000 BTU (8500 até 10500)");
            BtuLista.Add("12000 BTU (11000 até 15000)");
            BtuLista.Add("18000 BTU (16000 até 21000)");
            BtuLista.Add("24000 BTU (22000 até 24000)");
            BtuLista.Add("30000 BTU (26000 até 30000)");
            BtuLista.Add("36000 BTU (35000 até 36000)");
            BtuLista.Add("48000 BTU (41000 até 51000)");
            BtuLista.Add("60000 BTU (55000 até 80000)");

            if (TextModelo == "Cassete")
            {
                BtuLista.Remove("7000 BTU (5000 até 8000)"); 
                BtuLista.Remove("9000 BTU (8500 até 10500)");
            }

            if (TextModelo == "Piso Teto")
            {
                BtuLista.Remove("7000 BTU (5000 até 8000)");
                BtuLista.Remove("9000 BTU (8500 até 10500)");
                BtuLista.Remove("12000 BTU (11000 até 15000)");
            }

            Capacidade.ItemsSource = BtuLista;
                
        }

        void CepTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            cepEntry.Text = Utils.AplicaMascaraCep(cepEntry.Text);
        }

        void NumeroTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            numeroEntry.Text = Utils.RemoveNaoNumericos(numeroEntry.Text);
        }
    }
}
