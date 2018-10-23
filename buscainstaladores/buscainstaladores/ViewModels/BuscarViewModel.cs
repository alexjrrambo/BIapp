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
    public class BuscarViewModel : INotifyPropertyChanged
    { 

        public Command CommandCarregaCidade { get; set; }
        public Command CommandPesquisar { get; set; }
        public Command CommandCarregarPreco { get; set; }
        public ObservableCollection<Usuario> ListaInstaladores { get; set; }
        public ObservableCollection<Estado> EstadoLista { get; set; }
        public ObservableCollection<Cidade> CidadeLista { get; set; }
        public Estado EstadoSelecionado { get; set; }
        public Cidade CidadeSelecionada { get; set; }
        public INavigation Navegacao;
        public Usuario InstaladorAtual { get; set; }
        public ObservableCollection<TabelaPreco> ListaPrecosHiwall { get; set; }
        public ObservableCollection<TabelaPreco> ListaPrecosCassete { get; set; }
        public ObservableCollection<TabelaPreco> ListaPrecosPiso { get; set; }
        public string Preco_orcado { get; set; }
        public string MensagemErro { get; set; }
        public BuscarViewModel(INavigation MainPageNav, Usuario instalador = null)
        {
            Navegacao = MainPageNav;

            CommandCarregaCidade = new Command(CarregarCidades);
            CommandPesquisar = new Command(Pesquisar);

            EstadoLista = new ObservableCollection<Estado>(CarregarEstados());
            CidadeLista = new ObservableCollection<Cidade>();
            ListaInstaladores = new ObservableCollection<Usuario>();

            if (instalador!=null)
            {
                InstaladorAtual = instalador;
                ListaPrecosHiwall = new ObservableCollection<TabelaPreco>(CarregarPrecos(1));
                ListaPrecosCassete = new ObservableCollection<TabelaPreco>(CarregarPrecos(2));
                ListaPrecosPiso = new ObservableCollection<TabelaPreco>(CarregarPrecos(3));

                Preco_orcado = "A negociar";
            }
                
        }

        public void CarregarPrecoPadrao(string btu, string modelo, string servico)
        {
            var values = new Dictionary<string, string>
            {
                { "id_instalador", InstaladorAtual.Id.ToString() },
                { "btu", btu },
                { "tipo_servico", servico },
                { "modelo", modelo },
            };

            string response = Utils.MetodoPost(values, "BuscaPrecoPadrao");

            var objeto = JsonConvert.DeserializeObject<dynamic>(response);

            string preco = objeto.resultado;

            if(preco=="0.00" || preco == "0,00" || preco == "" || preco == "0")
                Preco_orcado = "A negociar";
            else
                Preco_orcado = preco.Replace(".",",");      

            OnPropertyChanged("Preco_orcado");
        }

        public ObservableCollection<TabelaPreco> CarregarPrecos(int modelo)
        {
            
            var values = new Dictionary<string, string>
            {
                { "id_usuario", InstaladorAtual.Id.ToString() },
                { "modelo", modelo.ToString() }
            };

            string response = Utils.MetodoPost(values, "CarregarPrecos");

            var objetosLista = JsonConvert.DeserializeObject<ObservableCollection<TabelaPreco>>(response);

            return objetosLista;
        }

        private void Pesquisar()
        {            
            if(EstadoSelecionado == null)
            {
                MensagemErro = "Por favor, informe um estado.";
                OnPropertyChanged("MensagemErro");         
                return;
            }

            if (CidadeSelecionada == null)
            {
                MensagemErro = "Por favor, informe uma cidade.";
                OnPropertyChanged("MensagemErro");
                return;
            }

            var values = new Dictionary<string, string>
            {
                { "codigo_estado", EstadoSelecionado.Codigo.ToString() },
                { "codigo_cidade", CidadeSelecionada.Codigo.ToString() },
            };

            string response = Utils.MetodoPost(values, "CarregarInstaladores");

            var objetosLista = JsonConvert.DeserializeObject<ObservableCollection<Usuario>>(response);

            ListaInstaladores = objetosLista;

            Navegacao.PushAsync(new InstaladoresPage(this));
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

            CidadeLista = objetosLista;

            OnPropertyChanged("CidadeLista");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string e = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e));
        }
    }
}
