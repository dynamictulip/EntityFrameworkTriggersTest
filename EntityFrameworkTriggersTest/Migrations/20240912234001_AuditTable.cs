using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkTriggersTest.Migrations
{
    /// <inheritdoc />
    public partial class AuditTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryKeyColumnName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryKeyType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryKeyValueAsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityOperationType = table.Column<int>(type: "int", nullable: false),
                    AlteredColumnName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlteredColumnType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEvents", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditEvents");
        }
    }
}
