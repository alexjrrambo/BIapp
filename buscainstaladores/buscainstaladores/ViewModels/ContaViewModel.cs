using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;
using buscainstaladores.Models;
using buscainstaladores.Services;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace buscainstaladores.ViewModels
{
    public class ContaViewModel : INotifyPropertyChanged
    {
        public ContaFinanceira ContaAtual { get; set; }
        public Command CommandSalvaConta { get; set; }
        INavigation Navigation;
        public ContaViewModel(INavigation MainPageNav)
        {
            Navigation = MainPageNav;

            CarregarConta();
            CommandSalvaConta = new Command(SalvarConta);
        }

        private void SalvarConta()
        {

            string banco = ContaAtual.Banco.Substring(0, 3);
            string pessoa = ContaAtual.Pessoa.Substring(7, 1);

            string conta = ContaAtual.Conta_visivel;
            string digitoConta = ContaAtual.Conta_visivel;

            if(conta.Contains("-"))
            {
                conta = conta.Substring(0, conta.IndexOf("-"));
            }

            if (digitoConta.Contains("-"))
            {
                digitoConta = digitoConta.Substring(digitoConta.IndexOf("-") + 1);
            }

            string agencia = ContaAtual.Agencia_visivel;
            string digitoAgencia = ContaAtual.Agencia_visivel;

            if (agencia.Contains("-"))
            {
                agencia = agencia.Substring(0, agencia.IndexOf("-"));
            }

            if (digitoAgencia.Contains("-"))
            {
                digitoAgencia = digitoAgencia.Substring(digitoAgencia.IndexOf("-") + 1);
            }

            var values = new Dictionary<string, string>
            {
                { "id_usuario", App.UsuarioLogado.Id.ToString() },
                { "banco", banco },
                { "agencia", agencia },
                { "favorecido", ContaAtual.Favorecido },
                { "digito_conta", digitoConta },
                { "digito_agencia", digitoAgencia },
                { "pessoa", pessoa},
                { "conta", conta},
            };
            string response = Utils.MetodoPost(values, "SalvarConta");        

            Navigation.PopAsync();
        }

        private void CarregarConta()
        {
            var values = new Dictionary<string, string>
            {
                { "id_usuario", App.UsuarioLogado.Id.ToString() }
            };

            string response = Utils.MetodoPost(values, "CarregarConta");


            var objetosLista = JsonConvert.DeserializeObject<List<ContaFinanceira>>(response);
            var conta = objetosLista[0];

            ContaAtual = conta;

            //tratamento especial para os pickers
            if (ContaAtual.Pessoa == "F")
                ContaAtual.Pessoa = "Pessoa Física";
            else
                ContaAtual.Pessoa = "Pessoa Jurídica";

            //alimenta objeto conforme dados do picker
            switch (ContaAtual.Banco)
            {
                case "001":
                    ContaAtual.Banco = "001 - BANCO DO BRASIL S.A.";
                    break;
                case "237":
                    ContaAtual.Banco = "237 - BANCO BRADESCO S.A.";
                    break;
                case "341":
                    ContaAtual.Banco = "341 - BANCO ITAÚ S.A.";
                    break;
                case "104":
                    ContaAtual.Banco = "104 - CAIXA ECONOMICA FEDERAL";
                    break;
                case "033":
                    ContaAtual.Banco = "033 - BANCO SANTANDER BANESPA S.A.";
                    break;
                case "399":
                    ContaAtual.Banco = "399 - HSBC BANK BRASIL S.A. - BANCO MULTIPLO";
                    break;
                case "745":
                    ContaAtual.Banco = "745 - BANCO CITIBANK S.A.";
                    break;
                case "041":
                    ContaAtual.Banco = "041 - BANCO DO ESTADO DO RIO GRANDE DO SUL S.A.";
                    break;
                default:
                    ContaAtual.Banco = "Selecione seu banco";
                    break;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string e = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e));
        }

    }
}

