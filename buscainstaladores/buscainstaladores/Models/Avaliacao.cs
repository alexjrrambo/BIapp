using System;
using System.ComponentModel.DataAnnotations;

namespace buscainstaladores.Models
{
    public class Avaliacao
    {
        public int? Id { get; set; }
        public int? Id_instalador { get; set; }
        public int? Id_cliente { get; set; }
        public int? Id_agendamento { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Descreva sua avaliação.")]
        public string Texto { get; set; }
        public string Rating { get; set; }
    }
}
