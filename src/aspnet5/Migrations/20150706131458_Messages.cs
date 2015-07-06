using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace aspnet5.Migrations
{
    public partial class Messages : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    Author = table.Column(type: "nvarchar(max)", nullable: true),
                    Date = table.Column(type: "nvarchar(max)", nullable: true),
                    Text = table.Column(type: "nvarchar(max)", nullable: true),
                    Time = table.Column(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                });
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropTable("Message");
        }
    }
}
