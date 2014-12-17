namespace EntityDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LogType = c.String(),
                        Source = c.String(),
                        Command = c.String(),
                        Inputs = c.String(),
                        LogText = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LogItems");
        }
    }
}
