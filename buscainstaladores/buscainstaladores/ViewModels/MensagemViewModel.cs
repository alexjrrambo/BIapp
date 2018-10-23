using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using buscainstaladores.Models;
using buscainstaladores.Services;
using buscainstaladores.Views;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace buscainstaladores.ViewModels
{
    public class MensagemViewModel : INotifyPropertyChanged
    {
        public Mensagem MensagemAtual { get; set; }
        public static int? IdDestinatarioChat { get; set; }
        public ObservableCollection<Mensagem> ListaMensagens { get; set; }
        public ObservableCollection<Mensagem> ChatMensagens { get; set; }
        public Command CommandEnviarMensagem { get; set; }
        public static Command CommandEnviarMensagemPublic { get; set; }
        public Command CommandAgendar { get; set; }
        public Command CommandConfirmarAgenda { get; set; }
        public Command CommandThreadChat { get; set; }
        public Command CommandListaThreadChat { get; set; }
        public Command CommandRecarregarMensagem { get; set; }
        INavigation Navigation;

        public MensagemViewModel(INavigation MainPageNav, Mensagem MensagemParam = null)
        {
            MensagemAtual = new Mensagem();
            ListaMensagens = new ObservableCollection<Mensagem>(CarregarUltimasMensagens(App.UsuarioLogado.Id));
            if (MensagemParam != null)
                ChatMensagens = new ObservableCollection<Mensagem>(CarregarMensagens(MensagemParam));

            CommandEnviarMensagem = new Command(EnviarMensagem);
            CommandEnviarMensagemPublic = new Command<Mensagem>(EnviarMensagemPublic);
            CommandAgendar = new Command(Agendar);
            CommandConfirmarAgenda = new Command<Mensagem>(ConfirmarAgenda);
            CommandThreadChat = new Command(MensagemThread);
            CommandListaThreadChat = new Command(ListaMensagemThread);
            CommandRecarregarMensagem = new Command<Mensagem>(RecarregarMensagens);            
            Navigation = MainPageNav;
        }

        private void ListaMensagemThread()
        {
            ListaMensagens = new ObservableCollection<Mensagem>(CarregarUltimasMensagens(App.UsuarioLogado.Id));
            OnPropertyChanged("ListaMensagens");
        }

        private void MensagemThread()
        {
            var lista = CarregarMsgNaoLidas();
            if (lista != null)
            {
                ObservableCollection<Mensagem> msgLista = new ObservableCollection<Mensagem>(lista);

                if (msgLista != null)
                {
                    foreach (Mensagem item in msgLista)
                    {
                        ChatMensagens.Add(item);
                        Task.Delay(4000);
                        //ChatMensagens = new ObservableCollection<Mensagem>(CarregarMensagens(item));
                        //OnPropertyChanged("MensagemAtual");
                    }
                }
            }
        }

        private ObservableCollection<Mensagem> CarregarMsgNaoLidas()
        {
            var values = new Dictionary<string, string>
            {
                { "id_usuario", App.UsuarioLogado.Id.ToString() },
                { "id_destinatario", IdDestinatarioChat.ToString() },
            };

            string response = Utils.MetodoPost(values, "CarregarMensagemNaoLida");

            var objetosLista = JsonConvert.DeserializeObject<ObservableCollection<Mensagem>>(response);

            if (response != "")
            {
                foreach (Mensagem item in objetosLista)
                {
                        if (item.Data_emissao != null)
                    {
                        item.Data_visivel = Utils.RetornaDiaSemana(item.Data_emissao);
                    }
                }
            }

            return objetosLista;
        }

        private void ConfirmarAgenda(Mensagem msg)
        {

            Agenda agendamento = AgendaViewModel.BuscaAgendaById((int)msg.Id_agenda);

            Navigation.PushAsync(new AgendarPage(agendamento, msg));
        }

        public static void ConfirmarAgendaMensagem(Mensagem msg)
        {
            AgendaViewModel.EfetuaConfirmacao((int)msg.Id_agenda);

            Agenda agenda = AgendaViewModel.BuscaAgendaById((int)msg.Id_agenda);

            Mensagem msgConfirmar = new Mensagem();
            msgConfirmar.Id_remetente = App.UsuarioLogado.Id;
            msgConfirmar.Id_destinatario = msg.Id_remetente;
            msgConfirmar.Texto = "Serviço #" + msg.Id_agenda + " - " + agenda.Titulo + ", no valor de " + agenda.Valor + " foi aceito pelo instalador. Realize o pagamento em até 2 dias úteis para confirmar seu serviço.";
            EnviarMensagem(msgConfirmar);

            /*ChatMensagens.Add(msgConfirmar);

            //limpa o campo texto
            MensagemAtual = new Mensagem();
            ChatMensagens = new ObservableCollection<Mensagem>(CarregarMensagens(msgConfirmar));
            OnPropertyChanged("MensagemAtual");*/
        }

        private void Agendar()
        {
            Agenda AgendaTemp = new Agenda();
            int? IdCliente = 0;
            if (App.UsuarioLogado.Id == MensagemAtual.Id_remetente)
                IdCliente = MensagemAtual.Id_destinatario;
            else
                IdCliente = MensagemAtual.Id_remetente;
            
            AgendaTemp.Id_cliente = IdDestinatarioChat;
            Navigation.PushAsync(new AgendarPage(AgendaTemp));
        }

        private void EnviarMensagemPublic(Mensagem mensagem)
        {
            MensagemAtual.Texto = mensagem.Texto;

            EnviarMensagem();
        }

        private void EnviarMensagem()
        {
            MensagemAtual.Data_visivel = Utils.RetornaDiaSemana(DateTime.Now);
            MensagemAtual.Remetente = "Eu";
            MensagemAtual.Id_destinatario = IdDestinatarioChat;
            MensagemAtual.Tipo = "";
            EnviarMensagem(MensagemAtual);

            MensagemAtual.Texto = Utils.FormataMensagem(MensagemAtual.Texto);

            ChatMensagens.Add(MensagemAtual);

            //atualiza lista de mensagem capa
            /*var item = ListaMensagens.FirstOrDefault(i => i.Id_destinatario == IdDestinatarioChat);
            var item2 = ListaMensagens.FirstOrDefault(i => i.Id_remetente == IdDestinatarioChat);
            if (item != null || item2 != null)
            {
                item.Id_remetente = App.UsuarioLogado.Id;
                item.Id_destinatario = IdDestinatarioChat;
                item.Texto = MensagemAtual.Texto;
            }*/

            //limpa o campo texto
            MensagemAtual = new Mensagem();
            OnPropertyChanged("MensagemAtual");

        }

        public static void EnviarMensagem(Mensagem msg)
        {
            if (msg.Id_agenda == null)
                msg.Id_agenda = 0;
            
            var values = new Dictionary<string, string>
                {
                    { "id_usuario", App.UsuarioLogado.Id.ToString() },
                    { "id_destinatario", msg.Id_destinatario.ToString() },
                    { "texto", msg.Texto },
                    { "tipo", msg.Tipo },
                    { "id_agenda", msg.Id_agenda.ToString() },
                };

            string response = Utils.MetodoPost(values, "EnviarMensagem");
        }

        private void RecarregarMensagens(Mensagem mensagem)
        {
            ChatMensagens = new ObservableCollection<Mensagem>(CarregarMensagens(mensagem));
            OnPropertyChanged("ChatMensagens");
        }

        public static ObservableCollection<Mensagem> CarregarMensagens(Mensagem mensagem)
        {
            IdDestinatarioChat = 0;
            if (mensagem != null ){                
                if (App.UsuarioLogado.Id == mensagem.Id_remetente)
                    IdDestinatarioChat = mensagem.Id_destinatario;
                else
                    IdDestinatarioChat = mensagem.Id_remetente;

                var values = new Dictionary<string, string>
                {
                    { "id_usuario", App.UsuarioLogado.Id.ToString() },
                    { "id_destinatario", IdDestinatarioChat.ToString() }
                };

                string response = Utils.MetodoPost(values, "CarregarMensagens");

                var objetosLista = JsonConvert.DeserializeObject<ObservableCollection<Mensagem>>(response);

                foreach (Mensagem item in objetosLista)
                {
                    item.Texto = Utils.FormataMensagem(item.Texto);

                    if (App.UsuarioLogado.Tipo_usuario == "I")
                    {
                        if (item.Id_remetente == App.UsuarioLogado.Id)
                            item.Remetente = "Eu";                        
                            
                    }
                    else
                    {
                        if (item.Id_remetente == App.UsuarioLogado.Id)
                            item.Remetente = "Eu";
                        else
                            item.Remetente = item.Fantasia_remetente;

                    }

                    if (item.Data_emissao != null)
                    {
                        item.Data_visivel = Utils.RetornaDiaSemana(item.Data_emissao);
                    }

                    if (item.Data_visivel == null || item.Data_visivel=="")
                    {
                        item.Data_visivel = Utils.RetornaDiaSemana(item.Data_emissao);
                    }
                }

                return objetosLista;
            }
            return null;
        }

        public static ObservableCollection<Mensagem> CarregarUltimasMensagens(int id_usuario)
        {
            var values = new Dictionary<string, string>
            {
                { "id_usuario", id_usuario.ToString() }
            };

            string response = Utils.MetodoPost(values, "CarregarListaMensagens");

            var objetosLista = JsonConvert.DeserializeObject<ObservableCollection<Mensagem>>(response);

            foreach (Mensagem item in objetosLista)
            {
                item.Texto = Utils.FormataMensagem(item.Texto);

                if (App.UsuarioLogado.Tipo_usuario=="I"){                    
                    item.Destinatario = item.Remetente;
                }
                else
                {
                    if (item.Fantasia_remetente != null)
                        item.Destinatario = item.Fantasia_remetente;                    
                    else                    
                        item.Destinatario = item.Fantasia_destinatario;
                }

                if(item.Data_emissao != null)
                {
                    item.Data_visivel = Utils.RetornaDiaSemana(item.Data_emissao);
                }
            }

            return objetosLista;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string e = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e));
        }

    }
}
