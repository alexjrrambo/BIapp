using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace buscainstaladores.Services
{
    public class ServicoTeste
    {
        private static string UrlBase = "https://viacep.com.br/ws/{0}/json/"; 

        public async static Task<string> BuscaCEP(string cep)
        {
            string URL = string.Format(UrlBase, cep);

            HttpClient http = new HttpClient();

            string json = await http.GetStringAsync(URL);

            dynamic objeto = JsonConvert.DeserializeObject<dynamic>(json);

            string Resultado = string.Format("CEP: {0}, Endereco: {1}, Cidade: {2}", objeto.cep, objeto.logradouro, objeto.localidade);

            return Resultado;
        }
    }
}
