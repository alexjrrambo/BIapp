using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace buscainstaladores.Models
{
    public class Agenda
    {
        public int? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Título deve ser informado.")]
        public string Titulo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Data deve ser informada.")]
        public string Dtevento { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Hora deve ser informada.")]
        public string Hora { get; set; }

        public string Descricao { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Rua deve ser informada.")]
        public string Endereco { get; set; }
        public int? Id_instalador { get; set; }
        public int? Id_cliente { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Valor deve ser informado.")]
        public string Valor { get; set; }
        public string Situacao { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Estado deve ser informado.")]
        public int? Id_estado { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Cidade deve ser informada.")]
        public int? Id_cidade { get; set; }
        public string Tipo_residencia { get; set; }
        public string Taxa { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Motivo deve ser informado.")]
        public string Motivo { get; set; }
        public string Id_pag_moip { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Número deve ser informado.")]
        public string Numero { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Bairro deve ser informado.")]
        public string Bairro { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Cep deve ser informado.")]
        public string Cep { get; set; }
        public string TituloView => string.Format("#{0} - {1}", Id, Titulo);
        public string EnderecoView => string.Format("{0}, {1}, {2}, {3}", Endereco, Bairro, Numero, Cep);
        public string Nom_cidade { get; set; }
        public string Nom_estado { get; set; }
        public string Sgl_estado { get; set; }
        public string Primeiro_nome { get; set; }
        public string Ultimo_nome { get; set; }
        public string Nome_fantasia { get; set; }
        public string LabelNomeAgenda => GetLabelNome();
        public string NomeAgenda => GetNomeAgenda();

        /// <summary>
        /// Defini se o botao ficara visivel
        /// </summary>
        public bool VisibleEditar
        {

            get
            {
                if (App.UsuarioLogado.Tipo_usuario == "I")
                {
                    DateTime DataServico = DateTime.ParseExact(Dtevento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime DataAtual = DateTime.Now;
                    if (DataServico <= DataAtual && (Situacao == "1" || Situacao == "2"))
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Defini se o botao ficara visivel
        /// </summary>
        public bool VisibleCancelar
        {

            get
            {
                if (App.UsuarioLogado.Tipo_usuario == "I")
                {
                    if (Situacao == "1" || Situacao == "5")
                        return true;
                }

                if (App.UsuarioLogado.Tipo_usuario == "U")
                {
                    if (Situacao == "1" || Situacao == "5" || Situacao == "2")
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Defini se o botao ficara visivel
        /// </summary>
        public bool VisiblePagamento
        {
            get
            {
                DateTime DataServico  = DateTime.ParseExact(Dtevento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime DataAtual = DateTime.Now;
                if (App.UsuarioLogado.Tipo_usuario == "U" && DataServico <= DataAtual)
                {
                    if (Situacao == "1")
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Defini se o botao ficara visivel
        /// </summary>
        public bool VisibleAvaliar 
        {
            get
            {
                if (App.UsuarioLogado.Tipo_usuario == "U")
                {
                    if (Situacao == "2")
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Defini se o botao ficara visivel
        /// </summary>
        public bool VisibleConfirmar
        {
            get
            {
                if (App.UsuarioLogado.Tipo_usuario == "I")
                {
                    DateTime DataServico = DateTime.ParseExact(Dtevento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime DataAtual = DateTime.Now;
                    if (Situacao == "5" && DataServico <= DataAtual)
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Busca qual nome será exibido na agenda.
        /// </summary>
        private string GetNomeAgenda()
        {
            if (App.UsuarioLogado.Tipo_usuario != "I")
            {
                return Nome_fantasia;
            }
            else
            {
                return Primeiro_nome;
            }
        }

        private string GetLabelNome()
        {
            if(App.UsuarioLogado.Tipo_usuario!="I")
            {
                return "Instalador: ";
            }
            else
            {
                return "Cliente: ";
            }
        }
    }
}
