using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using buscainstaladores.Models;
using buscainstaladores.Services;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace buscainstaladores.ViewModels
{
    public class TabelaPrecoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TabelaPreco> ListaPrecos { get; set; }
        public Command CommandRecarregarPrecos { get; set; }
        public Command CommandSalvaPrecos { get; set; }
        public string FiltroSelecionado { get; set; }
        INavigation Navigation;
        public TabelaPrecoViewModel(INavigation MainPageNav)
        {
            Navigation = MainPageNav;
            FiltroSelecionado = "Hi-wall";
            ListaPrecos = new ObservableCollection<TabelaPreco>(CarregarPrecos(1));

            CommandRecarregarPrecos = new Command(RecarregarPrecos);
            CommandSalvaPrecos = new Command(SalvarPrecos);


        }

        private void SalvarPrecos()
        {
            int modelo = 0;
            if (FiltroSelecionado == "Hi-wall")
                modelo = 1;                            
            if (FiltroSelecionado == "Cassete")
                modelo = 2;
            if (FiltroSelecionado == "Piso-teto")
                modelo = 3;


            for (int i = 0; i < ListaPrecos.Count; i++)
            {
                string preco = ListaPrecos[i].Preco;
                preco = preco.Replace(",", ".");

                string preco_h = ListaPrecos[i].Preco_h;
                preco_h = preco_h.Replace(",", ".");

                var values = new Dictionary<string, string>
                {
                    { "id_usuario", App.UsuarioLogado.Id.ToString() },
                    { "modelo", modelo.ToString() },
                    { "btu", ListaPrecos[i].Btu },
                    { "preco", preco },
                    { "preco_h", preco_h }
                };      
                string response = Utils.MetodoPost(values, "SalvarPrecos");
            }

            Navigation.PopAsync();  
        }

        public static ObservableCollection<TabelaPreco> CarregarPrecos(int modelo)
        {
            var values = new Dictionary<string, string>
            {
                { "id_usuario", App.UsuarioLogado.Id.ToString() },
                { "modelo", modelo.ToString() }
            };

            string response = Utils.MetodoPost(values, "CarregarPrecos");

            var objetosLista = JsonConvert.DeserializeObject<ObservableCollection<TabelaPreco>>(response);

            return objetosLista;
        }

        private void RecarregarPrecos()
        {
            int modelo = 0;
            if (FiltroSelecionado == "Hi-wall")
                modelo = 1;
            if (FiltroSelecionado == "Cassete")
                modelo = 2;
            if (FiltroSelecionado == "Piso-teto")
                modelo = 3;

            ListaPrecos = new ObservableCollection<TabelaPreco>(CarregarPrecos(modelo));
            OnPropertyChanged("ListaPrecos");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string e = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e));
        }

    }
}

