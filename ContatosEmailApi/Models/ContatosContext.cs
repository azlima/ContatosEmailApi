using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ContatosEmailApi.Models
{
    public class ContatosContext : DbContext
    {
        public DbSet<Contato> Contatos { get; set; }
    }
}