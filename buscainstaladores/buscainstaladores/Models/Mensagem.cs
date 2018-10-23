using System;
namespace buscainstaladores.Models
{
    public class Mensagem
    {
        public int Id { get; set; }
        public int? Id_remetente { get; set; }
        public string Remetente { get; set; }
        public string Fantasia_remetente { get; set; }
        public int? Id_destinatario { get; set; }
        public string Destinatario { get; set; }
        public string Fantasia_destinatario { get; set; }
        public DateTime Data_emissao { get; set; }
        public string Texto { get; set; }
        public int? Lida { get; set; }
        public string Tipo { get; set; }
        public string Data_visivel { get; set; }
        public bool MensagemReserva => Tipo == "R" && App.UsuarioLogado.Tipo_usuario=="I";
        public bool MensagemOrcar => Tipo == "O" && App.UsuarioLogado.Tipo_usuario == "I";
        public bool SouDestinatario => Id_remetente == App.UsuarioLogado.Id;
        public bool SouRemetente => Id_destinatario == App.UsuarioLogado.Id;
        public int? Id_agenda { get; set; }
    }
}
