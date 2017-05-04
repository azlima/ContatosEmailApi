using System.Data.Entity;

namespace ContatosEmailApi.Models
{
    public class ContatosContext : DbContext
    {
        public DbSet<Contato> Contatos { get; set; }
    }
}