using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using buscainstaladores.Models;
using buscainstaladores.Services;
using buscainstaladores.Views;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace buscainstaladores.ViewModels
{
    public class AgendaViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Agenda> ListaAgenda { get; set; }
        public INavigation Navegacao;
        public Command CommandAbrirDetalhes { get; set; }
        public Command CommandEditarAgenda { get; set; }
        public Command CommandCarregaCidade { get; set; }
        public Command CommandReservarAgenda { get; set; }
        public Command CommandRecarregarAgenda { get; set; }
        public Command CommandCancelarAgenda { get; set; }
        public Command CommandPageCancelar { get; set; }
        public Command CommandPagamento { get; set; }
        public Command CommandAvaliacao { get; set; }
        public Command CommandSalvarAvaliacao { get; set; }
        public Command CommandConfirmar { get; set; }
        public Command CommandRecarregarAgendaAtual { get; set; }
        public Agenda AgendaAtual { get; set; }
        public ObservableCollection<Estado> EstadoAgenda { get; set; }
        public ObservableCollection<Cidade> CidadeAgenda { get; set; }
        public Estado EstadoSelecionado { get; set; }
        public Cidade CidadeSelecionada { get; set; }
        public int IdCliente { get; set; }
        public DateTime DataFiltro { get; set; }
        public Avaliacao AvaliacaoAtual { get; set; }
        public AgendaViewModel(INavigation MainPageNav, Agenda AgendaParam = null, string Origem = "")
        {
            DataFiltro = DateTime.Now;
            ListaAgenda = new ObservableCollection<Agenda>(CarregarAgenda(App.UsuarioLogado.Id, DataFiltro));
            Navegacao = MainPageNav;

            CommandAbrirDetalhes = new Command<Agenda>(CarregarAgendaDetalhe);
            CommandCarregaCidade = new Command(CarregarCidades);
            CommandReservarAgenda = new Command(ReservarAgenda);
            CommandRecarregarAgenda = new Command(RecarregarAgenda);
            CommandCancelarAgenda = new Command<TextoGenericoPage>(CancelarAgenda);
            CommandPageCancelar = new Command(PageCancelar);
            CommandEditarAgenda = new Command(EditarAgenda);
            CommandPagamento = new Command(Pagamento);
            CommandAvaliacao = new Command(Avaliacao);
            CommandSalvarAvaliacao = new Command(SalvarAvaliacao);
            CommandConfirmar = new Command(Confirmar);
            CommandRecarregarAgendaAtual = new Command(RecarregarAgendaAtual);

            AgendaAtual = new Agenda();
            AvaliacaoAtual = new Avaliacao();
            if (AgendaParam != null)
                AgendaAtual = AgendaParam;

            if (Origem == "R")
            {
                EstadoAgenda = new ObservableCollection<Estado>(CarregarEstados());
                CidadeAgenda = new ObservableCollection<Cidade>();

                if (AgendaAtual.Id != null)
                {

                    //inicializa estado
                    for (int i = 0; i < EstadoAgenda.Count; i++)
                    {
                        //case insensitive search
                        if (String.Equals(EstadoAgenda[i].Cod_estado.ToString(), AgendaAtual.Id_estado.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            EstadoSelecionado = EstadoAgenda[i];
                            break;
                        }
                    }

                    CommandCarregaCidade.Execute(true);
                }
            }
        }

        private void Confirmar()
        {            
            AgendaViewModel.EfetuaConfirmacao((int)AgendaAtual.Id);

            Mensagem msgConfirmar = new Mensagem();
            msgConfirmar.Id_remetente = App.UsuarioLogado.Id;
            msgConfirmar.Id_destinatario = AgendaAtual.Id_cliente;
            msgConfirmar.Texto = "Serviço #" + AgendaAtual.Id.ToString() + " - " + AgendaAtual.Titulo + ", no valor de R$" + AgendaAtual.Valor.ToString() + " foi aceito pelo instalador. Realize o pagamento em até 2 dias úteis para confirmar seu serviço.";
            MensagemViewModel.EnviarMensagem(msgConfirmar);

            RecarregarAgendaAtual();
        }

        public static void EfetuaConfirmacao(int idAgenda)
        {
            var values = new Dictionary<string, string>
                {                    
                    { "id_agenda",idAgenda.ToString() },
                };

            string response = Utils.MetodoPost(values, "ConfirmarAgendaById");
        }

        private void SalvarAvaliacao()
        {
            var values = new Dictionary<string, string>
            {
                { "id_instalador", AvaliacaoAtual.Id_instalador.ToString() },
                { "id_cliente", AvaliacaoAtual.Id_cliente.ToString() },
                { "id_agendamento", AvaliacaoAtual.Id_agendamento.ToString() },
                { "texto", AvaliacaoAtual.Texto },
                { "rating", AvaliacaoAtual.Rating },
            };

            string response = Utils.MetodoPost(values, "CriaAvaliacao");

            Navegacao.PopAsync();
        }

        private void Avaliacao()
        {
            AvaliacaoAtual.Id_cliente = App.UsuarioLogado.Id;
            AvaliacaoAtual.Id_instalador = AgendaAtual.Id_instalador;
            AvaliacaoAtual.Id_agendamento = AgendaAtual.Id;
            AvaliacaoAtual.Rating = "3";

            Navegacao.PushAsync(new AvaliacaoPage(this));
        }

        private void Pagamento()
        {
            Navegacao.PushAsync(new PagamentoPage(AgendaAtual));
        }

        private void EditarAgenda()
        {
            Navegacao.PushAsync(new AgendarPage(AgendaAtual));
        }

        private void PageCancelar()
        {
            TextoGenericoPage form = new TextoGenericoPage();

            form.SetLabelText("Motivo do cancelamento");
            form.SetButton1Text("Efetuar cancelamento");
            form.SetTextLabelMensagem("Cancelamento solicitado com sucesso.");
            form.SetButton2Visible(false);
            form.SetVisibleFormMensagem(false);
            form.SetCommandConfirmar(new Command<TextoGenericoPage>(CancelarAgenda));

            Navegacao.PushAsync(form);
        }

        private void CancelarAgenda(TextoGenericoPage page)
        {
            AgendaAtual.Motivo = page.GetText();

            //id_usuario
            //id_cliente
            //id_agenda
            //motivo
            var values = new Dictionary<string, string>
            {
                { "id_usuario", AgendaAtual.Id_instalador.ToString() },
                { "id_cliente", AgendaAtual.Id_cliente.ToString() },
                { "id_agenda", AgendaAtual.Id.ToString() },
                { "motivo", AgendaAtual.Motivo },
            };

            string response = Utils.MetodoPost(values, "CancelarAgenda");

            page.SetVisibleForm(false);
            page.SetVisibleFormMensagem(true);
        }

        private void ReservarAgenda()
        {
            if (AgendaAtual.Id == null)
                AgendaAtual.Id = 0;

            //salva reserva no banco
            EfetuaReserva(AgendaAtual,EstadoSelecionado,CidadeSelecionada);

            //envia mensagem de criacao
            if (AgendaAtual.Id == null || AgendaAtual.Id == 0)
            {
                Mensagem msg = new Mensagem();

                msg.Id_destinatario = AgendaAtual.Id_cliente;

                msg.Texto = "<p>" + AgendaAtual.Titulo.ToString() + ".<br>Agendamento criado no dia " + AgendaAtual.Dtevento.ToString() + ".<br>O pagamento deve ser efetuado em 2 dias.<br>Para detalhes acesse suas instalações.</p>";

                MensagemViewModel.CommandEnviarMensagemPublic.Execute(msg);
            }

            OnPropertyChanged("AgendaAtual");

            Navegacao.PopToRootAsync();

        }

        public static int EfetuaReserva (Agenda agenda, Estado estado, Cidade cidade)
        {

            int id_instalador = App.UsuarioLogado.Id;
            int? id_cliente = agenda.Id_cliente;
            string situacaoInicial = "1";

            if (agenda.Id_instalador != 0 && agenda.Id_instalador != null)
                id_instalador = (int)agenda.Id_instalador;

            if (agenda.Id_cliente != 0 && agenda.Id_cliente != null)
                id_cliente = (int)agenda.Id_cliente;

            if (agenda.Situacao != "" && agenda.Situacao != null)
                situacaoInicial = agenda.Situacao;

            string valor = agenda.Valor.Replace(".", "");
            valor = valor.Replace(",", ".");
            var values = new Dictionary<string, string>
            {
                { "id", agenda.Id.ToString() },
                { "titulo", agenda.Titulo.ToString() },
                { "dtevento", agenda.Dtevento.ToString() },
                { "situacao", situacaoInicial },
                { "hora", agenda.Hora.ToString() },
                { "descricao", "Desc teste" },
                { "endereco", agenda.Endereco.ToString() },
                { "cep", agenda.Cep.ToString() },
                { "bairro", agenda.Bairro.ToString() },
                { "numero", agenda.Numero.ToString() },
                { "id_instalador", id_instalador.ToString() },
                { "id_cliente", id_cliente.ToString() },
                { "valor", valor },
                { "id_estado", estado.Cod_estado.ToString() },
                { "id_cidade", cidade.Cod_cidade.ToString() },
                { "tipo_residencia", agenda.Tipo_residencia.ToString() },
            };
            string response = "";

            if (agenda.Id == null || agenda.Id == 0)
                response = Utils.MetodoPost(values, "InserirAgenda");
            else
                response = Utils.MetodoPost(values, "EditarAgenda");

            dynamic objeto = JsonConvert.DeserializeObject<dynamic>(response);
            int id_agenda = objeto.resultado;

            return id_agenda;
        }


        public static ObservableCollection<Estado> CarregarEstados()
        {
            string response = Utils.MetodoPost(null, "CarregarEstados");

            var objetosLista = JsonConvert.DeserializeObject<ObservableCollection<Estado>>(response);

            return objetosLista;
        }


        private void CarregarCidades()
        {
            var values = new Dictionary<string, string>
            {
                { "id_estado", EstadoSelecionado.Cod_estado.ToString() }
            };

            string response = Utils.MetodoPost(values, "CarregarCidades");

            var objetosLista = JsonConvert.DeserializeObject<ObservableCollection<Cidade>>(response);

            CidadeAgenda = objetosLista;

            //tratamento para picker do editar agenda
            if (AgendaAtual.Id != null)
            {
                //inicializa picker cidade no editar agenda
                for (int i = 0; i < CidadeAgenda.Count; i++)
                {
                    //case insensitive search
                    if (String.Equals(CidadeAgenda[i].Cod_cidade.ToString(), AgendaAtual.Id_cidade.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        CidadeSelecionada = CidadeAgenda[i];
                        break;
                    }
                }
            }

            OnPropertyChanged("CidadeAgenda");
        }

        public static ObservableCollection<Agenda> CarregarAgenda(int id_usuario,DateTime data)
        {            
            var values = new Dictionary<string, string>
            {
                { "id_usuario", id_usuario.ToString() },
                { "data", data.ToString("dd/MM/yyyy") }
            };

            string response = "";
            if (App.UsuarioLogado.Tipo_usuario == "I")
                response = Utils.MetodoPost(values, "CarregarListaAgendaInstalador");
            else
                response = Utils.MetodoPost(values, "CarregarListaAgenda");

            var objetosLista = JsonConvert.DeserializeObject<ObservableCollection<Agenda>>(response);

            return objetosLista;
        }

        private void RecarregarAgendaAtual()
        {
            var values = new Dictionary<string, string>
            {
                { "id_agenda", AgendaAtual.Id.ToString() }
            };

            AgendaAtual = BuscaAgendaById((int)AgendaAtual.Id);

            OnPropertyChanged("AgendaAtual");
        }

        public static Agenda BuscaAgendaById(int id_agenda)
        {
            var values = new Dictionary<string, string>
            {
                { "id_agenda", id_agenda.ToString() }
            };

            string response = Utils.MetodoPost(values, "CarregarAgendaById");

            var objetosLista = JsonConvert.DeserializeObject<List<Agenda>>(response);
            var agenda = objetosLista[0];

            return agenda;

        }

        private void RecarregarAgenda()
        {
            ListaAgenda = new ObservableCollection<Agenda>(CarregarAgenda(App.UsuarioLogado.Id, DataFiltro));
            OnPropertyChanged("ListaAgenda");
        }

        private void CarregarAgendaDetalhe(Agenda agenda)
        {            
            AgendaAtual = new Agenda();
            AgendaAtual = agenda;

            //Navegacao.PushAsync(new NavigationPage(new DetalheServicoPage()));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string e = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e));
        }

    }
}
