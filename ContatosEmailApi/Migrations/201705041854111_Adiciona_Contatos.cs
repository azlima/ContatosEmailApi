namespace ContatosEmailApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adiciona_Contatos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contatos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Para = c.String(maxLength: 250),
                        Copia = c.String(maxLength: 250),
                        CopiaOculta = c.String(maxLength: 250),
                        Assunto = c.String(maxLength: 250),
                        Mensagem = c.String(maxLength: 750),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Contatos");
        }
    }
}
