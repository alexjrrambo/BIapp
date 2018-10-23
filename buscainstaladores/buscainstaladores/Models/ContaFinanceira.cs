using System;
using System.ComponentModel.DataAnnotations;

namespace buscainstaladores.Models
{
    public class ContaFinanceira
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Banco deve ser informado.")]
        public string Banco { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Conta deve ser informado.")]
        public string Conta { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Agência deve ser informada.")]
        public string Agencia { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Favorecido deve ser informado.")]
        public string Favorecido { get; set; }
        public int Id_usuario { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Dígito da conta deve ser informado.")]
        public string Digito_conta { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Dígito da agência deve ser informado.")]
        public string Digito_agencia { get; set; }

        public string Conta_visivel 
        {
            get
            {
                if (Digito_conta != "" && !Conta.Contains("-"))
                    return Conta + "-" + Digito_conta;
                else
                    return Conta;
            }
            set => Conta = value;
        }

        public string Agencia_visivel
        {
            get
            {
                if (Digito_agencia != "" && !Agencia.Contains("-"))
                    return Agencia + "-" + Digito_agencia;
                else
                    return Agencia;
            } 
            set => Agencia = value;
        }

        public string Pessoa { get; set; }
        public string Id_bancario_moip { get; set; }
    }
}
