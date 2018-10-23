using System;
using buscainstaladores.Services;
using SQLite;
using System.ComponentModel.DataAnnotations;

namespace buscainstaladores.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "E-mail deve ser informado.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Primeiro nome deve ser informado.")]
        public string Primeiro_nome { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Último nome deve ser informado.")]
        public string Ultimo_nome { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Razão social deve ser informada.")]
        public string Razao_social { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nome fantasia deve ser informado.")]
        public string Nome_fantasia { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "CNPJ deve ser informado.")]
        public string Cnpj { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Senha deve ser informada.")]
        public string Senha { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Telefone deve ser informado.")]
        public string Telefone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Celular deve ser informado.")]
        public string Celular { get; set; }
        public string Categoria { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Estado deve ser informado.")]
        public int? Id_estado { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Cidade deve ser informado.")]
        public int? Id_cidade { get; set; }

        public string Descricao { get; set; }
        public int? Cidade_1 { get; set; }
        public int? Cidade_2 { get; set; }
        public int? Cidade_3 { get; set; }
        public int? Cidade_4 { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Rua deve ser informada.")]
        public string Endereco { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Número deve ser informado.")]
        public string Numero { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Bairro deve ser informado.")]
        public string Bairro { get; set; }
        public string Complemento { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Cep deve ser informado.")]
        public string Cep { get; set; }
        public string Site { get; set; }
        public string Facebook { get; set; }
        public string Tipo_usuario { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "CPF deve ser informado.")]
        public string Cpf { get; set; }
        public string Acess_token { get; set; }
        public string Id_moip { get; set; }
        public DateTime Data_nascimento { get; set; }
        public string FotoPerfil => BuscaFoto();

        //instaladores
        public string Premium { get; set; }
        public string Nom_estado { get; set; }
        public string Nom_cidade { get; set; }
        public string Sgl_estado { get; set; }
        public string Preco_maior { get; set; }
        public string Preco_menor { get; set; }
        public string Qtd_avaliacoes { get; set; } 
        public float? Media_avaliacoes { get; set; }
        public string RatingImage => BuscaRatingImage();
        public string StringPrecos => MontaStringPrecos();
        public string LocalString => MontaStringLocal();
        public string AvaliacaoString => MontaStringAvaliacao();

        /// <summary>
        /// Se for premium retorna a imagem premium.
        /// </summary>
        public string ImagemPremium 
        { 
            get
            {
                if (Premium == "1")
                    return "https://buscainstaladores.com/images/premium.png";   
                return "";
            }
            
        }

        public string BuscaFoto()
        {
            if (!Utils.isConnectadURL(string.Format("https://s3-sa-east-1.amazonaws.com/buscainstaladores/{0}/perfil/{0}-perfil.png", Id)))
                    return "http://localhost:8080/images/instaladores/default-perfil.png";
                else
                    return string.Format("https://s3-sa-east-1.amazonaws.com/buscainstaladores/{0}/perfil/{0}-perfil.png", Id);
        }   

        public string BuscaRatingImage()
        {
            if (Media_avaliacoes <= 1.5 || Media_avaliacoes == null)
                return "stars1.png";
            if (Media_avaliacoes > 1.5 && Media_avaliacoes <= 2.5)
                return "stars2.png";
            if (Media_avaliacoes > 2.5 && Media_avaliacoes <= 3.5)
                return "stars3.png";
            if (Media_avaliacoes > 3.5 && Media_avaliacoes <= 4.5)
                return "stars4.png";
            if (Media_avaliacoes > 4.5)
                return "stars5.png";

            return "stars-3.png";
        }  

        public string MontaStringPrecos()
        {
            if (Preco_menor == null || Preco_menor == "0.00" || Preco_maior == null || Preco_maior == "0.00")
                return "Preços por mensagem";
            else
                return "Preços de " + Preco_menor.Replace(".", ",") + " até " + Preco_maior.Replace(".", ",");
        }

        public string MontaStringLocal()
        {
            return Nom_cidade+" - "+Sgl_estado;
        }

        public string MontaStringAvaliacao()
        {
            return string.Format("{0} Avaliações", Qtd_avaliacoes);
        }
    }
}
