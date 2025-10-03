using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeopleRegistry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "person",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Vorname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nachname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Geburtsdatum = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "anschrift",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Hausnummer = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Strasse = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Postleitzahl = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Ort = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anschrift", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anschrift_Person",
                        column: x => x.PersonId,
                        principalSchema: "dbo",
                        principalTable: "person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "telefonverbindung",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Telefonnummer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefonverbindung", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Telefon_Person",
                        column: x => x.PersonId,
                        principalSchema: "dbo",
                        principalTable: "person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_anschrift_PersonId",
                schema: "dbo",
                table: "anschrift",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_telefonverbindung_PersonId",
                schema: "dbo",
                table: "telefonverbindung",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "anschrift",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "telefonverbindung",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "person",
                schema: "dbo");
        }
    }
}
