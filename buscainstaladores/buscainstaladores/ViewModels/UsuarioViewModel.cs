using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using buscainstaladores.Database;
using buscainstaladores.Models;
using buscainstaladores.Views;
using buscainstaladores.Services;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace buscainstaladores.ViewModels
{
    public class UsuarioViewModel : INotifyPropertyChanged
    {        
        public Usuario UsuarioAtual { get; set; }
        public Command CommandEntrar { get; set; }
        public Command CommandLogout { get; set; }
        public Command CommandSalvaEdicao { get; set; }
        public Command CommandCarregaCidade { get; set; }
        public Command CommandInserirUsuario { get; set; }
        public Command CommandInserirInstalador { get; set; }
        public Command CommandCancelarAssinatura { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string MensagemErro { get; set; }
        public ObservableCollection<Estado> EstadoAgenda { get; set; }
        public ObservableCollection<Cidade> CidadeAgenda { get; set; }
        public Estado EstadoSelecionado { get; set; }
        public Cidade CidadeSelecionada { get; set; }
        public Cidade Cidade_1_Selecionada { get; set; }
        public Cidade Cidade_2_Selecionada { get; set; }
        public Cidade Cidade_3_Selecionada { get; set; }
        public Cidade Cidade_4_Selecionada { get; set; }
        INavigation Navigation;
        public int SelectedIndexEstado { get; set; }
        public int SelectedIndexCidade { get; set; }
        public int SelectedIndexCidade1 { get; set; }
        public int SelectedIndexCidade2 { get; set; }
        public int SelectedIndexCidade3 { get; set; }
        public int SelectedIndexCidade4 { get; set; }      
        public string Origem;
        public UsuarioViewModel(INavigation MainPageNav,string OrigemForm = "")
        {            
            if (App.UsuarioLogado == null)
            {
                UsuarioAtual = new Usuario();
            }
            else
            {
                UsuarioAtual = App.UsuarioLogado;
            }

            Origem = OrigemForm;

            CommandEntrar = new Command(Entrar);
            CommandLogout = new Command(Logout);
            CommandSalvaEdicao = new Command(Editar);
            CommandCarregaCidade = new Command(RecarregarCidades);
            CommandInserirUsuario = new Command(InserirUsuario);
            CommandInserirInstalador = new Command(InserirInstalador);
            CommandCancelarAssinatura = new Command(PageCancelar);


            Login = "alexjr.rambo@gmail.com";
            Senha = "001993";
            Navigation = MainPageNav;

            if (Origem == "cadastro")
            {
                EstadoAgenda = new ObservableCollection<Estado>(CarregarEstados());
                CidadeAgenda = new ObservableCollection<Cidade>();
            }

            if (Origem == "editar-informacoes")
            {
                EstadoAgenda = new ObservableCollection<Estado>(CarregarEstados());
                CidadeAgenda = new ObservableCollection<Cidade>(CarregarCidades());

                //inicializa estado
                for (int i = 0; i < EstadoAgenda.Count; i++)
                {
                    //case insensitive search
                    if (String.Equals(EstadoAgenda[i].Cod_estado.ToString(), UsuarioAtual.Id_estado.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        SelectedIndexEstado = i;
                        break;
                    }
                }
                //inicializa cidade
                for (int i = 0; i < CidadeAgenda.Count; i++)
                {
                    //case insensitive search
                    if (String.Equals(CidadeAgenda[i].Cod_cidade.ToString(), UsuarioAtual.Id_cidade.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        SelectedIndexCidade = i;
                        break;
                    }
                }
            }

            if (Origem == "area-premium")
            {
                CidadeAgenda = new ObservableCollection<Cidade>(CarregarCidades());

                SelectedIndexCidade1 = -1;
                SelectedIndexCidade2 = -1;
                SelectedIndexCidade3 = -1;
                SelectedIndexCidade4 = -1;

                Cidade_1_Selecionada = new Cidade();
                Cidade_2_Selecionada = new Cidade();
                Cidade_3_Selecionada = new Cidade();
                Cidade_4_Selecionada = new Cidade();

                //inicializa cidade1
                if (UsuarioAtual.Cidade_1 != 0)
                {                    
                    for (int i = 0; i < CidadeAgenda.Count; i++)
                    {
                        //case insensitive search
                        if (String.Equals(CidadeAgenda[i].Cod_cidade.ToString(), UsuarioAtual.Cidade_1.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            SelectedIndexCidade1 = i;
                            break;
                        }
                    }
                }

                //inicializa cidade2
                if (UsuarioAtual.Cidade_2 != 0)
                {
                    for (int i = 0; i < CidadeAgenda.Count; i++)
                    {
                        //case insensitive search
                        if (String.Equals(CidadeAgenda[i].Cod_cidade.ToString(), UsuarioAtual.Cidade_2.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            SelectedIndexCidade2 = i;
                            break;
                        }
                    }
                }

                //inicializa cidade3
                if (UsuarioAtual.Cidade_3 != 0)
                {
                    for (int i = 0; i < CidadeAgenda.Count; i++)
                    {
                        //case insensitive search
                        if (String.Equals(CidadeAgenda[i].Cod_cidade.ToString(), UsuarioAtual.Cidade_3.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            SelectedIndexCidade3 = i;
                            break;
                        }
                    }
                }

                //inicializa cidade4
                if (UsuarioAtual.Cidade_4 != 0)
                {
                    for (int i = 0; i < CidadeAgenda.Count; i++)
                    {
                        //case insensitive search
                        if (String.Equals(CidadeAgenda[i].Cod_cidade.ToString(), UsuarioAtual.Cidade_4.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            SelectedIndexCidade4 = i;
                            break;
                        }
                    }
                }
            }
        }

        private void InserirUsuario()
        {
            var values = new Dictionary<string, string>
            {
                { "primeiro_nome", App.UsuarioLogado.Primeiro_nome },
                { "ultimo_nome", App.UsuarioLogado.Ultimo_nome },
                { "celular", App.UsuarioLogado.Celular },
                { "data_nascimento", App.UsuarioLogado.Data_nascimento.ToString("dd/MM/yyyy") },
                { "email", App.UsuarioLogado.Email },
                { "senha", App.UsuarioLogado.Senha },
            };
            string response = Utils.MetodoPost(values, "InserirUsuario");

            dynamic objeto = JsonConvert.DeserializeObject<dynamic>(response);

            int resultado = objeto.resultado;

            if (resultado==1)
            {
                int id_usuario = ValidarEntrada(App.UsuarioLogado.Email,App.UsuarioLogado.Senha);

                CarregarUsuario(id_usuario);

                if (Device.RuntimePlatform == Device.iOS)
                    Application.Current.MainPage = new PrincipalPage();
                else
                    Application.Current.MainPage = new NavigationPage(new PrincipalPage());
            }
        }

        private void InserirInstalador()
        {
            var values = new Dictionary<string, string>
            {
                { "primeiro_nome", App.UsuarioLogado.Primeiro_nome },
                { "ultimo_nome", App.UsuarioLogado.Ultimo_nome },
                { "celular", App.UsuarioLogado.Celular },
                { "nascimento", App.UsuarioLogado.Data_nascimento.ToString("dd/MM/yyyy") },
                { "email", App.UsuarioLogado.Email },
                { "senha", App.UsuarioLogado.Senha },
                { "nome_fantasia", App.UsuarioLogado.Nome_fantasia },
                { "razao_social", App.UsuarioLogado.Razao_social },
                { "cnpj", App.UsuarioLogado.Cnpj },
                { "cpf", App.UsuarioLogado.Cpf },
                { "telefone", App.UsuarioLogado.Telefone },
                { "categoria", "1" },
                { "id_estado", App.UsuarioLogado.Id_estado.ToString() },
                { "id_cidade", App.UsuarioLogado.Id_cidade.ToString() },
                { "endereco", App.UsuarioLogado.Endereco },
                { "numero", App.UsuarioLogado.Numero },
                { "bairro", App.UsuarioLogado.Bairro },
                { "cep", App.UsuarioLogado.Cep },
                { "nom_cidade", App.UsuarioLogado.Nom_cidade },
                { "sgl_estado", App.UsuarioLogado.Sgl_estado },

            };


            /*var values = new Dictionary<string, string>
            {
                { "primeiro_nome", "lalala" },
                { "ultimo_nome", "lsadjsadjh" },
                { "celular", "(21) 8888-99999" },
                { "nascimento", "02/07/1993" },
                { "email", "lexxx@gmail.com" },
                { "senha", "001993" },
                { "nome_fantasia", "lepo lepo" },
                { "razao_social", "leipppp" },
                { "cnpj", "74.006.108/0001-28" },
                { "cpf", "010.079.010-04" },
                { "telefone", "(51) 8888-6666" },
                { "categoria", "1" },
                { "id_estado", "21" },
                { "id_cidade", "3941" },
                { "endereco", "Wowooww" },
                { "numero", "100" },
                { "bairro", "asdasd" },
                { "cep", "93950-000" },
                { "nom_cidade", "Dois Irmãos" },
                { "sgl_estado", "RS" },

            };*/

            string response = Utils.MetodoPost(values, "InserirInstalador");

            dynamic objeto = JsonConvert.DeserializeObject<dynamic>(response);

            int resultado = objeto.resultado;

            if (resultado == 1)
            {
                int id_usuario = ValidarEntrada(App.UsuarioLogado.Email, App.UsuarioLogado.Senha);

                CarregarUsuario(id_usuario);

                if (Device.RuntimePlatform == Device.iOS)
                    Application.Current.MainPage = new PrincipalPage();
                else
                    Application.Current.MainPage = new NavigationPage(new PrincipalPage());
            }
        }

        private void Entrar()
        {            
            if (App.UsuarioLogado != null)
                new UsuarioDataAccess().ExcluirUsuario(App.UsuarioLogado);

            int id_usuario = ValidarEntrada(Login, Senha);

            if (id_usuario!=0)
            {
                //se login for sucesso
                App.UsuarioLogado = new Usuario();
                App.UsuarioLogado.Email = Login;
                App.UsuarioLogado.Primeiro_nome = "Alex Junior Rambo";
                //new UsuarioDataAccess().InserirUsuario(App.UsuarioLogado);

                CarregarUsuario(id_usuario);

                UsuarioAtual = App.UsuarioLogado;

                if (Device.RuntimePlatform == Device.iOS)
                    Application.Current.MainPage = new PrincipalPage();
                else
                    Application.Current.MainPage = new NavigationPage(new PrincipalPage());

            }
            else
            {
                MensagemErro = "E-mail ou senha incorretos.";
                OnPropertyChanged("MensagemErro");
            }         
        }

        private void Logout()
        {
            
            new UsuarioDataAccess().ExcluirUsuario(App.UsuarioLogado);

            App.UsuarioLogado = null;
            UsuarioAtual = null;

            Application.Current.MainPage = new LoginPage();
        }

        private void Editar()
        {            
            var values = new Dictionary<string, string>{};
            if (Origem == "editar-informacoes")
            {                
                values = new Dictionary<string, string>
                {
                    { "id", App.UsuarioLogado.Id.ToString() },
                    { "primeiro_nome", UsuarioAtual.Primeiro_nome },
                    { "ultimo_nome", UsuarioAtual.Ultimo_nome },
                    { "nome_fantasia", UsuarioAtual.Nome_fantasia },
                    { "razao_social", UsuarioAtual.Razao_social },
                    { "cnpj", UsuarioAtual.Cnpj },
                    { "cpf", UsuarioAtual.Cpf },
                    { "data_nascimento", UsuarioAtual.Data_nascimento.ToString("dd/MM/yyyy") },
                    { "telefone", UsuarioAtual.Telefone },
                    { "celular", UsuarioAtual.Celular },
                    { "categoria", UsuarioAtual.Categoria },
                    { "id_estado", EstadoSelecionado.Cod_estado.ToString() },
                    { "id_cidade", CidadeSelecionada.Cod_cidade.ToString() },
                    { "descricao", UsuarioAtual.Descricao },
                    { "endereco", UsuarioAtual.Endereco },
                    { "numero", UsuarioAtual.Numero },
                    { "bairro", UsuarioAtual.Bairro },
                    { "complemento", UsuarioAtual.Complemento },
                    { "cep", UsuarioAtual.Cep },
                    { "origem", Origem }

                };

                UsuarioAtual.Id_estado = EstadoSelecionado.Cod_estado;
                UsuarioAtual.Id_cidade = CidadeSelecionada.Cod_cidade;
            }

            if(Origem == "seguranca")
            {
                values = new Dictionary<string, string>
                {
                    { "id", App.UsuarioLogado.Id.ToString() },
                    { "email", UsuarioAtual.Email },
                    { "senha", UsuarioAtual.Senha },
                    { "origem", Origem }

                };
            }
          
            if(Origem == "area-premium")
            {                                    
                values = new Dictionary<string, string>
                {
                    { "id", App.UsuarioLogado.Id.ToString() },
                    { "cidade_1", Cidade_1_Selecionada.Cod_cidade.ToString() },
                    { "cidade_2", Cidade_2_Selecionada.Cod_cidade.ToString() },
                    { "cidade_3", Cidade_3_Selecionada.Cod_cidade.ToString() },
                    { "cidade_4", Cidade_4_Selecionada.Cod_cidade.ToString() },
                    { "facebook", UsuarioAtual.Facebook },
                    { "site", UsuarioAtual.Site },
                    { "origem", Origem }
                };
                UsuarioAtual.Cidade_1 = Cidade_1_Selecionada.Cod_cidade;
                UsuarioAtual.Cidade_2 = Cidade_2_Selecionada.Cod_cidade;
                UsuarioAtual.Cidade_3 = Cidade_3_Selecionada.Cod_cidade;
                UsuarioAtual.Cidade_4 = Cidade_4_Selecionada.Cod_cidade;
            }


            string response = Utils.MetodoPost(values, "EditarUsuario");

            dynamic objeto = JsonConvert.DeserializeObject<dynamic>(response);

            int Resultado = objeto.resultado;

            if (Resultado == 1)
            {
                CarregarUsuario(App.UsuarioLogado.Id);
                Navigation.PopAsync();
            }
        }

        public static int ValidarEntrada(string email, string senha)
        {
            var values = new Dictionary<string, string>
            {
                { "email", email },
                { "senha", senha }
            };
            string response = Utils.MetodoPost(values, "ValidarEntrada");

            dynamic objeto = JsonConvert.DeserializeObject<dynamic>(response);

            int Resultado = objeto.resultado;

            return Resultado;
        }

        public void CarregarUsuario(int id_usuario)
        {
            var values = new Dictionary<string, string>
            {
                { "id_usuario", id_usuario.ToString() }
            };

            string response = Utils.MetodoPost(values, "CarregarUsuario");

            var objetosLista = JsonConvert.DeserializeObject<List<Usuario>>(response);
            var usuario = objetosLista[0];

            App.UsuarioLogado = usuario;
        }


        public void RecarregarUsuario()
        {
            var values = new Dictionary<string, string>
            {
                { "id_usuario", App.UsuarioLogado.Id.ToString() }
            };

            string response = Utils.MetodoPost(values, "CarregarUsuario");

            var objetosLista = JsonConvert.DeserializeObject<List<Usuario>>(response);
            var usuario = objetosLista[0];

            UsuarioAtual = usuario;
        }

        public static ObservableCollection<Estado> CarregarEstados()
        {
            string response = Utils.MetodoPost(null, "CarregarEstados");

            var objetosLista = JsonConvert.DeserializeObject<ObservableCollection<Estado>>(response);

            return objetosLista;
        }

        public static ObservableCollection<Cidade> CarregarCidades()
        {
            var values = new Dictionary<string, string>
            {
                { "id_estado", App.UsuarioLogado.Id_estado.ToString() }
            };

            string response = Utils.MetodoPost(values, "CarregarCidades");

            var objetosLista = JsonConvert.DeserializeObject<ObservableCollection<Cidade>>(response);

            return objetosLista;
        }


        private void RecarregarCidades()
        {
            var values = new Dictionary<string, string>
            {
                { "id_estado", EstadoSelecionado.Cod_estado.ToString() }
            };

            string response = Utils.MetodoPost(values, "CarregarCidades");

            var objetosLista = JsonConvert.DeserializeObject<ObservableCollection<Cidade>>(response);

            CidadeAgenda = objetosLista;

            OnPropertyChanged("CidadeAgenda");
        }

        private void CancelarAssinatura(TextoGenericoPage page)
        {
            var values = new Dictionary<string, string>
            {
                { "id_usuario", App.UsuarioLogado.Id.ToString() },   
                { "motivo", page.GetText() }
            };

            string response = Utils.MetodoPost(values, "CancelarAssinatura");

            page.SetVisibleForm(false);
            page.SetVisibleFormMensagem(true);
        }

        private void PageCancelar()
        {
            TextoGenericoPage form = new TextoGenericoPage();

            form.SetLabelText("Motivo do cancelamento");
            form.SetButton1Text("Efetuar cancelamento");
            form.SetTextLabelMensagem("Cancelamento solicitado com sucesso.");
            form.SetButton2Visible(false);
            form.SetVisibleFormMensagem(false);
            form.SetCommandConfirmar(new Command<TextoGenericoPage>(CancelarAssinatura));

            Navigation.PushAsync(form);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string e = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e));
        }
    }
}
