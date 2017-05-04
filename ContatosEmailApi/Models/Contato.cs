using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContatosEmailApi.Models
{
    [Table("Contatos")]
    public class Contato
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(250)]
        public string Para { get; set; }

        [MaxLength(250)]
        public string Copia { get; set; }

        [MaxLength(250)]
        public string CopiaOculta { get; set; }

        [MaxLength(250)]
        public string Assunto { get; set; }

        [MaxLength(750)]
        public string Mensagem { get; set; }
    }
}