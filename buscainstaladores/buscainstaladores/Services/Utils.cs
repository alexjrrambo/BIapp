using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace buscainstaladores.Services
{
    public class Utils
    {
        public Utils()
        {
        }

        public static string MetodoPost(Dictionary<string, string> parametros, string nomeMetodo)
        {
            //criando url para chamar o web service
            string URL = App.UrlWebService + nomeMetodo;

            HttpClient http = new HttpClient();

            if (parametros == null)
            {
                parametros = new Dictionary<string, string>
                            {
                                { "", "" }
                             };
            }

            var content = new FormUrlEncodedContent(parametros);

            var result = http.PostAsync(URL, content).Result;

            var finalResult = result.Content.ReadAsStringAsync().Result;

            return finalResult;
        }

        public static string RetornaDiaSemana(DateTime DataEmissao)
        {
            DateTime dt = DataEmissao;
            DateTime dtAtual = DateTime.Now;

            TimeSpan ts = dtAtual.Subtract(dt);
            DateTime periodo = new DateTime(ts.Ticks);


            if (dt.ToString("dd/MM/yyyy") == dtAtual.ToString("dd/MM/yyyy"))
                return dt.ToString("HH:mm");

            if (periodo.Day >= 1 && periodo.Day < 7 && periodo.Month == 0)
            {

                switch ((int)dt.DayOfWeek)
                {
                    case 1:
                        return "segunda-feira";
                    case 2:
                        return "terça-feira";
                    case 3:
                        return "quarta-feira";
                    case 4:
                        return "quinta-feira";
                    case 5:
                        return "sexta-feira";
                    case 6:
                        return "sábado";
                    case 7:
                        return "domingo";
                    default:
                        return "";
                }
            }

            if (periodo.Day >= 7 || periodo.Month >= 1)
                return dt.ToString("dd/MM/yyyy");

            return "";
        }

        public static string FormataMensagem(string Texto)
        {
            string textoFormatado = Texto;
            string textoAux = "";
            int startIndex = 0;
            int endIndex = 0;

            textoFormatado = textoFormatado.Replace("<br>", Environment.NewLine);

            while(textoFormatado.IndexOf("<")>0)
            {
                startIndex = textoFormatado.IndexOf("<");
                endIndex = textoFormatado.IndexOf(">", startIndex);

                textoAux = textoFormatado.Substring(startIndex, endIndex + 1 - startIndex);

                textoFormatado = textoFormatado.Replace(textoAux, "");
            }

            return textoFormatado;
        }


        public static bool isConnectadURL(string url)
        {
            bool fail;

            System.Uri Url = new System.Uri(url); //é sempre bom por um site que costuma estar sempre on, para n haver problemas

            WebRequest WebReq = null;
            Task<WebResponse> Resp;

            WebReq = WebRequest.Create(Url);

            try
            {
                Resp = WebReq.GetResponseAsync();
                bool completed = Resp.IsCompleted;
                var resultado = Resp.Result;
                WebReq = null;
                fail = true; //consegue conexão à internet                
            }

            catch
            {
                WebReq = null;
                fail = false; //falhou a conexão à internet                
            }

            return fail;
        }

        public static object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {   
            NumberFormatInfo nfi = new CultureInfo("pt-BR").NumberFormat;
            return Decimal.Parse(value.ToString()).ToString("C", nfi);
        }

        /// <summary>
        /// Remove caracteres não numéricos
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveNaoNumericos(string text)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[^0-9]");
            string ret = reg.Replace(text, string.Empty);
            return ret;
        }

        /// <summary>
        /// Valida se um cpf é válido
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static bool ValidaCPF(string cpf)
        {
            //Remove formatação do número, ex: "123.456.789-01" vira: "12345678901"
            cpf = Utils.RemoveNaoNumericos(cpf);

            if (cpf.Length > 11)
                return false;

            while (cpf.Length != 11)
                cpf = '0' + cpf;

            bool igual = true;
            for (int i = 1; i < 11 && igual; i++)
                if (cpf[i] != cpf[0])
                    igual = false;

            if (igual || cpf == "12345678909")
                return false;

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(cpf[i].ToString());

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else
                if (numeros[10] != 11 - resultado)
                return false;

            return true;
        }

        public static bool ValidaCnpj(string cnpjString)
        {
            var cnpj = Utils.RemoveNaoNumericos(cnpjString);

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma;

            int resto;

            string digito;

            string tempCnpj;

            cnpj = cnpj.Trim();

            if (cnpj.Length != 14)

                return false;

            tempCnpj = cnpj.Substring(0, 12);

            soma = 0;

            for (int i = 0; i < 12; i++)

                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito = resto.ToString();

            tempCnpj = tempCnpj + digito;

            soma = 0;

            for (int i = 0; i < 13; i++)

                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);

        }

        public static string AplicaMascaraCpf(string stringCpf)
        {
            var cpf = Utils.RemoveNaoNumericos(stringCpf);

            if (cpf.Length > 9)
            {
                cpf = cpf.Insert(3, ".");
                cpf = cpf.Insert(7, ".");
                cpf = cpf.Insert(11, "-");
            }

            if (cpf.Length >= 7 && cpf.Length <= 9)
            {
                cpf = cpf.Insert(3, ".");
                cpf = cpf.Insert(7, ".");
            }

            // 01007901004
            if (cpf.Length > 3 && cpf.Length < 7)
            {
                cpf = cpf.Insert(3, ".");
            }

            if (cpf.Length > 14)
            {
                cpf = cpf.Remove(14);
            }

            return cpf;
        }

        public static string AplicaMascaraCnpj(string stringCnpj)
        {
            var cnpj = Utils.RemoveNaoNumericos(stringCnpj);

            if (cnpj.Length > 12)
            {
                cnpj = cnpj.Insert(2, ".");
                cnpj = cnpj.Insert(6, ".");
                cnpj = cnpj.Insert(10, "/");
                cnpj = cnpj.Insert(15, "-");
            }

            if (cnpj.Length > 8 && cnpj.Length <= 12)
            {
                cnpj = cnpj.Insert(2, ".");
                cnpj = cnpj.Insert(6, ".");
                cnpj = cnpj.Insert(10, "/");
            }

            if (cnpj.Length >= 6 && cnpj.Length <= 8)
            {
                cnpj = cnpj.Insert(2, ".");
                cnpj = cnpj.Insert(6, ".");
            }

            // 01007901004
            if (cnpj.Length > 2 && cnpj.Length < 6)
            {
                cnpj = cnpj.Insert(2, ".");
            }

            if (cnpj.Length > 18)
            {
                cnpj = cnpj.Remove(18);
            }

            return cnpj;
        }

        public static string AplicaMascaraTelefone(string stringTelefone)
        {
            var telefone = Utils.RemoveNaoNumericos(stringTelefone);

            if (telefone.Length > 7)
            {                
                telefone = telefone.Insert(0, "(");
                telefone = telefone.Insert(3, ") ");
                telefone = telefone.Insert(9, "-");
            }

            if (telefone.Length > 2 && telefone.Length <=7)
            {
                telefone = telefone.Insert(0, "(");
                telefone = telefone.Insert(3, ") ");
            }

            if (telefone.Length > 14)
            {
                telefone = telefone.Remove(14);
            }

            return telefone;
        }

        public static string AplicaMascaraCelular(string stringTelefone)
        {
            var telefone = Utils.RemoveNaoNumericos(stringTelefone);

            if (telefone.Length > 7)
            {
                telefone = telefone.Insert(0, "(");
                telefone = telefone.Insert(3, ") ");
                telefone = telefone.Insert(9, "-");
            }

            if (telefone.Length > 2 && telefone.Length <= 7)
            {
                telefone = telefone.Insert(0, "(");
                telefone = telefone.Insert(3, ") ");
            }

            if (telefone.Length > 15)
            {
                telefone = telefone.Remove(15);
            }

            return telefone;
        }

        public static string AplicaMascaraCep(string stringCep)
        {
            var cep = Utils.RemoveNaoNumericos(stringCep);
            //93950-000
            if (cep.Length > 5)
            {
                cep = cep.Insert(5, "-");
            }

            if (cep.Length > 9)
            {
                cep = cep.Remove(9);
            }

            return cep;
        }

        public static string ValildarObjeto(object objeto, string campos)
        {
            var ListaRes = new List<ValidationResult>();
            var Contexto = new ValidationContext(objeto);
            var IsValid = Validator.TryValidateObject(objeto, Contexto, ListaRes);

            string ErroMensagem = string.Empty;

            if (ListaRes.Count > 0)
            {
                foreach (var x in ListaRes)
                {
                    List<string> listaCampos = x.MemberNames.ToList();
                    var campo = listaCampos[0];

                    if (campos.Contains(campo))
                    {
                        ErroMensagem += x.ErrorMessage + "\n";
                    }
                }
            }

            return ErroMensagem;
        }

        public static string AplicaMascaraConta(string stringConta, string banco)
        {
  
            var conta = Utils.RemoveNaoNumericos(stringConta);
            int contaLen = 0;
            int digitoLen = 0;


            switch (banco)
            {
                case "001 - BANCO DO BRASIL S.A.":
                    contaLen = 8;
                    digitoLen = 1;
                    break;
                case "237 - BANCO BRADESCO S.A.":
                    contaLen = 7;
                    digitoLen = 1;
                    break;
                case "341 - BANCO ITAÚ S.A.":
                    contaLen = 5;
                    digitoLen = 1;
                    break;
                case "104 - CAIXA ECONOMICA FEDERAL":
                    contaLen = 11;
                    digitoLen = 1;
                    break;
                case "033 - BANCO SANTANDER BANESPA S.A.":
                    contaLen = 8;
                    digitoLen = 1;
                    break;
                case "399 - HSBC BANK BRASIL S.A. - BANCO MULTIPLO":
                    contaLen = 6;
                    digitoLen = 1;
                    break;
                case "745 - BANCO CITIBANK S.A.":
                    contaLen = 7;
                    digitoLen = 1;
                    break;
                case "041 - BANCO DO ESTADO DO RIO GRANDE DO SUL S.A.":
                    contaLen = 9;
                    digitoLen = 1;
                    break;
                default:
                    contaLen = 0;
                    digitoLen = 0;
                    break;
            }
                
            if (conta.Length > contaLen)
            {
                conta = conta.Insert(contaLen, "-");
            }

            if (conta.Length > contaLen+digitoLen+1)
            {
                conta = conta.Remove(contaLen + digitoLen + 1);
            }

            return conta;
        }

        public static string AplicaMascaraAgencia(string stringAgencia, string banco)
        {

            var agencia = Utils.RemoveNaoNumericos(stringAgencia);
            int agenciaLen = 0;
            int digitoLen = 0;


            switch (banco)
            {
                case "001 - BANCO DO BRASIL S.A.":
                    agenciaLen = 4;
                    digitoLen = 1;
                    break;
                case "237 - BANCO BRADESCO S.A.":
                    agenciaLen = 4;
                    digitoLen = 1;
                    break;
                case "341 - BANCO ITAÚ S.A.":
                    agenciaLen = 5;
                    digitoLen = 1;
                    break;
                case "104 - CAIXA ECONOMICA FEDERAL":
                    agenciaLen = 4;
                    digitoLen = 0;
                    break;
                case "033 - BANCO SANTANDER BANESPA S.A.":
                    agenciaLen = 4;
                    digitoLen = 0;
                    break;
                case "399 - HSBC BANK BRASIL S.A. - BANCO MULTIPLO":
                    agenciaLen = 4;
                    digitoLen = 0;
                    break;
                case "745 - BANCO CITIBANK S.A.":
                    agenciaLen = 4;
                    digitoLen = 0;
                    break;
                case "041 - BANCO DO ESTADO DO RIO GRANDE DO SUL S.A.":
                    agenciaLen = 4;
                    digitoLen = 2;
                    break;
                default:
                    agenciaLen = 0;
                    digitoLen = 0;
                    break;
            }

            if (digitoLen == 0)
            {
                if (agencia.Length > agenciaLen)
                {
                    agencia = agencia.Remove(agenciaLen + digitoLen + 1);
                }
            }
            else
            {

                if (agencia.Length > agenciaLen)
                {
                    agencia = agencia.Insert(agenciaLen, "-");
                }

                if (agencia.Length > agenciaLen + digitoLen + 1)
                {
                    agencia = agencia.Remove(agenciaLen + digitoLen + 1);
                }
            }
            return agencia;
        }

        public static string AplicaMascaraDinheiro(string stringDinheiro)
        {
            string n = string.Empty;
            double v = 0;
            try
            {
                n = stringDinheiro.Replace(".", "").Replace(",", "");
                if (n.Equals(""))
                    n = "";
                n = n.PadLeft(3, '0');
                if (n.Length > 3 && n.Substring(0, 1) == "0")
                    n = n.Substring(1, n.Length - 1);
                v = Double.Parse(n) / 100;

                CultureInfo provider;
                provider = new CultureInfo("pt-BR");
                stringDinheiro = string.Format(provider,"{0:N}", v);

            }
            catch (Exception erro)
            {
                
            }
            return stringDinheiro;
        }

    }
}
