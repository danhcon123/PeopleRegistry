using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeopleRegistry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Seed_Init_People : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Geburtsdatum",
                schema: "dbo",
                table: "person",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            // --- Personen ----------------------------------------------------
            // (Id, Vorname, Nachname, Geburtsdatum)
            var persons = new (Guid Id, string Vorname, string Nachname, DateTime? Geburtsdatum)[]
            {
                (new Guid("11111111-1111-1111-1111-111111111111"), "Anna",       "Müller",    new DateTime(1990,  3, 12)),
                (new Guid("11111111-1111-1111-1111-111111111112"), "Ben",        "Schmidt",   new DateTime(1988, 11,  5)),
                (new Guid("11111111-1111-1111-1111-111111111113"), "Clara",      "Schneider", new DateTime(1995,  6, 21)),
                (new Guid("11111111-1111-1111-1111-111111111114"), "David",      "Fischer",   new DateTime(1982,  1, 30)),
                (new Guid("11111111-1111-1111-1111-111111111115"), "Emma",       "Weber",     new DateTime(1999,  9,  2)),
                (new Guid("11111111-1111-1111-1111-111111111116"), "Felix",      "Wagner",    new DateTime(1993,  4, 18)),
                (new Guid("11111111-1111-1111-1111-111111111117"), "Greta",      "Becker",    new DateTime(1987, 12, 10)),
                (new Guid("11111111-1111-1111-1111-111111111118"), "Henry",      "Hoffmann",  new DateTime(1991,  7,  7)),
                (new Guid("11111111-1111-1111-1111-111111111119"), "Isabel",     "Schäfer",   new DateTime(1985,  5, 14)),
                (new Guid("11111111-1111-1111-1111-11111111111A"), "Jonas",      "Koch",      new DateTime(2000, 10, 25)),
                (new Guid("11111111-1111-1111-1111-11111111111B"), "Kira",       "Bauer",     new DateTime(1996,  3,  3)),
                (new Guid("11111111-1111-1111-1111-11111111111C"), "Lukas",      "Richter",   new DateTime(1983,  8, 20)),
                (new Guid("11111111-1111-1111-1111-11111111111D"), "Mara",       "Klein",     new DateTime(1998,  2, 11)),
                (new Guid("11111111-1111-1111-1111-11111111111E"), "Noah",       "Wolf",      new DateTime(1994,  6,  6)),
                (new Guid("11111111-1111-1111-1111-11111111111F"), "Olivia",     "Neumann",   new DateTime(1989,  1,  9)),
                (new Guid("11111111-1111-1111-1111-111111111120"), "Paul",       "Schwarz",   new DateTime(1981, 11, 27)),
                (new Guid("11111111-1111-1111-1111-111111111121"), "Queena",     "Zimmer",    new DateTime(1997, 12, 15)),
                (new Guid("11111111-1111-1111-1111-111111111122"), "Rafael",     "Krüger",    new DateTime(1992,  9,  1)),
                (new Guid("11111111-1111-1111-1111-111111111123"), "Sophia",     "Hartmann",  new DateTime(1990,  4,  4)),
                (new Guid("11111111-1111-1111-1111-111111111124"), "Tom",        "Werner",    new DateTime(1986,  7, 19)),
            };

            foreach (var p in persons)
            {
                migrationBuilder.InsertData(
                    schema: "dbo",
                    table: "person",
                    columns: new[] { "Id", "Vorname", "Nachname", "Geburtsdatum" },
                    values: new object?[] { p.Id, p.Vorname, p.Nachname, p.Geburtsdatum }
                );
            }

            // --- Anschriften -------------------------------------------------
            // Helper to shorten id creation
            Guid A(string hex) => new Guid(hex);

            var addresses = new (Guid Id, Guid PersonId, string PLZ, string Ort, string Strasse, string Hausnummer)[]
            {
                (A("22222222-2222-2222-2222-000000000001"), persons[0].Id,  "10115", "Berlin",     "Invalidenstraße", "12A"),
                (A("22222222-2222-2222-2222-000000000002"), persons[0].Id,  "10117", "Berlin",     "Friedrichstraße", "44"),
                (A("22222222-2222-2222-2222-000000000003"), persons[1].Id,  "20095", "Hamburg",    "Spitalerstraße",  "7"),
                (A("22222222-2222-2222-2222-000000000004"), persons[2].Id,  "50667", "Köln",       "Hohe Straße",     "101"),
                (A("22222222-2222-2222-2222-000000000005"), persons[3].Id,  "80331", "München",    "Neuhauser Str.",  "3"),
                (A("22222222-2222-2222-2222-000000000006"), persons[3].Id,  "80333", "München",    "Leopoldstraße",   "88"),
                (A("22222222-2222-2222-2222-000000000007"), persons[4].Id,  "01067", "Dresden",    "Prager Straße",   "25"),
                (A("22222222-2222-2222-2222-000000000008"), persons[5].Id,  "04109", "Leipzig",    "Grimmaische Str.","9"),
                (A("22222222-2222-2222-2222-000000000009"), persons[6].Id,  "60311", "Frankfurt",  "Zeil",            "110"),
                (A("22222222-2222-2222-2222-00000000000A"), persons[7].Id,  "70173", "Stuttgart",  "Königstraße",     "5"),
                (A("22222222-2222-2222-2222-00000000000B"), persons[7].Id,  "70563", "Stuttgart",  "Hauptstraße",     "17"),
                (A("22222222-2222-2222-2222-00000000000C"), persons[8].Id,  "28195", "Bremen",     "Sögestraße",      "2"),
                (A("22222222-2222-2222-2222-00000000000D"), persons[9].Id,  "90402", "Nürnberg",   "Königstorgraben", "28"),
                (A("22222222-2222-2222-2222-00000000000E"), persons[10].Id, "34117", "Kassel",     "Obere Königsstr.","13"),
                (A("22222222-2222-2222-2222-00000000000F"), persons[11].Id, "76133", "Karlsruhe",  "Kaiserstraße",    "66"),
                (A("22222222-2222-2222-2222-000000000010"), persons[12].Id, "49074", "Osnabrück",  "Große Straße",    "41"),
                (A("22222222-2222-2222-2222-000000000011"), persons[13].Id, "93047", "Regensburg", "Ludwigstraße",    "8"),
                (A("22222222-2222-2222-2222-000000000012"), persons[14].Id, "44135", "Dortmund",   "Westenhellweg",   "70"),
                (A("22222222-2222-2222-2222-000000000013"), persons[14].Id, "44787", "Bochum",     "Kortumstraße",    "3"),
                (A("22222222-2222-2222-2222-000000000014"), persons[15].Id, "99084", "Erfurt",     "Anger",           "19"),
                (A("22222222-2222-2222-2222-000000000015"), persons[16].Id, "07743", "Jena",       "Johannisstraße",  "22"),
                (A("22222222-2222-2222-2222-000000000016"), persons[17].Id, "81379", "München",    "Flößergasse",     "4"),
                (A("22222222-2222-2222-2222-000000000017"), persons[17].Id, "80336", "München",    "Goethestraße",    "99"),
                (A("22222222-2222-2222-2222-000000000018"), persons[18].Id, "68161", "Mannheim",   "Planken",         "55"),
                (A("22222222-2222-2222-2222-000000000019"), persons[18].Id, "68159", "Mannheim",   "Fressgasse",      "10"),
                (A("22222222-2222-2222-2222-00000000001A"), persons[19].Id, "01069", "Dresden",    "Wiener Straße",   "2C"),
                (A("22222222-2222-2222-2222-00000000001B"), persons[5].Id,  "04229", "Leipzig",    "Karl-Heine-Str.", "103"),
                (A("22222222-2222-2222-2222-00000000001C"), persons[0].Id,  "10405", "Berlin",     "Prenzlauer Allee","240")
            };

            foreach (var a in addresses)
            {
                migrationBuilder.InsertData(
                    schema: "dbo",
                    table: "anschrift",
                    columns: new[] { "Id", "PersonId", "Postleitzahl", "Ort", "Strasse", "Hausnummer" },
                    values: new object[] { a.Id, a.PersonId, a.PLZ, a.Ort, a.Strasse, a.Hausnummer }
                );
            }

            // --- Telefonverbindungen ----------------------------------------
            var phones = new (Guid Id, Guid PersonId, string Nummer)[]
            {
                (A("33333333-3333-3333-3333-000000000001"), persons[0].Id,  "+49 30 1234567"),
                (A("33333333-3333-3333-3333-000000000002"), persons[0].Id,  "+49 151 1111111"),
                (A("33333333-3333-3333-3333-000000000003"), persons[1].Id,  "+49 40 7654321"),
                (A("33333333-3333-3333-3333-000000000004"), persons[2].Id,  "+49 221 5555555"),
                (A("33333333-3333-3333-3333-000000000005"), persons[3].Id,  "+49 89 2222222"),
                (A("33333333-3333-3333-3333-000000000006"), persons[3].Id,  "+49 176 3333333"),
                (A("33333333-3333-3333-3333-000000000007"), persons[4].Id,  "+49 351 4444444"),
                (A("33333333-3333-3333-3333-000000000008"), persons[6].Id,  "+49 69 1231230"),
                (A("33333333-3333-3333-3333-000000000009"), persons[7].Id,  "+49 711 9090900"),
                (A("33333333-3333-3333-3333-00000000000A"), persons[8].Id,  "+49 421 8080808"),
                (A("33333333-3333-3333-3333-00000000000B"), persons[10].Id, "+49 561 6060606"),
                (A("33333333-3333-3333-3333-00000000000C"), persons[11].Id, "+49 721 5050505"),
                (A("33333333-3333-3333-3333-00000000000D"), persons[12].Id, "+49 541 4040404"),
                (A("33333333-3333-3333-3333-00000000000E"), persons[13].Id, "+49 941 3030303"),
                (A("33333333-3333-3333-3333-00000000000F"), persons[14].Id, "+49 231 2020202"),
                (A("33333333-3333-3333-3333-000000000010"), persons[15].Id, "+49 361 1010101"),
                (A("33333333-3333-3333-3333-000000000011"), persons[16].Id, "+49 3641 909090"),
                (A("33333333-3333-3333-3333-000000000012"), persons[17].Id, "+49 176 2222444"),
                (A("33333333-3333-3333-3333-000000000013"), persons[17].Id, "+49 89 7777777"),
                (A("33333333-3333-3333-3333-000000000014"), persons[18].Id, "+49 621 8888888"),
                (A("33333333-3333-3333-3333-000000000015"), persons[18].Id, "+49 621 1111222"),
                (A("33333333-3333-3333-3333-000000000016"), persons[19].Id, "+49 351 9999999"),
                (A("33333333-3333-3333-3333-000000000017"), persons[5].Id,  "+49 341 5555123"),
                (A("33333333-3333-3333-3333-000000000018"), persons[9].Id,  "+49 911 2222333")
            };

            foreach (var t in phones)
            {
                migrationBuilder.InsertData(
                    schema: "dbo",
                    table: "telefonverbindung",
                    columns: new[] { "Id", "PersonId", "Telefonnummer" },
                    values: new object[] { t.Id, t.PersonId, t.Nummer }
                );
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Geburtsdatum",
                schema: "dbo",
                table: "person",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);
            
            // Delete child rows first (phones, addresses), then persons.
            var phoneIds = new Guid[]
            {
                new("33333333-3333-3333-3333-000000000001"),
                new("33333333-3333-3333-3333-000000000002"),
                new("33333333-3333-3333-3333-000000000003"),
                new("33333333-3333-3333-3333-000000000004"),
                new("33333333-3333-3333-3333-000000000005"),
                new("33333333-3333-3333-3333-000000000006"),
                new("33333333-3333-3333-3333-000000000007"),
                new("33333333-3333-3333-3333-000000000008"),
                new("33333333-3333-3333-3333-000000000009"),
                new("33333333-3333-3333-3333-00000000000A"),
                new("33333333-3333-3333-3333-00000000000B"),
                new("33333333-3333-3333-3333-00000000000C"),
                new("33333333-3333-3333-3333-00000000000D"),
                new("33333333-3333-3333-3333-00000000000E"),
                new("33333333-3333-3333-3333-00000000000F"),
                new("33333333-3333-3333-3333-000000000010"),
                new("33333333-3333-3333-3333-000000000011"),
                new("33333333-3333-3333-3333-000000000012"),
                new("33333333-3333-3333-3333-000000000013"),
                new("33333333-3333-3333-3333-000000000014"),
                new("33333333-3333-3333-3333-000000000015"),
                new("33333333-3333-3333-3333-000000000016"),
                new("33333333-3333-3333-3333-000000000017"),
                new("33333333-3333-3333-3333-000000000018"),
            };

            foreach (var id in phoneIds)
            {
                migrationBuilder.DeleteData(schema: "dbo", table: "telefonverbindung", keyColumn: "Id", keyValue: id);
            }

            var addressIds = new Guid[]
            {
                new("22222222-2222-2222-2222-000000000001"),
                new("22222222-2222-2222-2222-000000000002"),
                new("22222222-2222-2222-2222-000000000003"),
                new("22222222-2222-2222-2222-000000000004"),
                new("22222222-2222-2222-2222-000000000005"),
                new("22222222-2222-2222-2222-000000000006"),
                new("22222222-2222-2222-2222-000000000007"),
                new("22222222-2222-2222-2222-000000000008"),
                new("22222222-2222-2222-2222-000000000009"),
                new("22222222-2222-2222-2222-00000000000A"),
                new("22222222-2222-2222-2222-00000000000B"),
                new("22222222-2222-2222-2222-00000000000C"),
                new("22222222-2222-2222-2222-00000000000D"),
                new("22222222-2222-2222-2222-00000000000E"),
                new("22222222-2222-2222-2222-00000000000F"),
                new("22222222-2222-2222-2222-000000000010"),
                new("22222222-2222-2222-2222-000000000011"),
                new("22222222-2222-2222-2222-000000000012"),
                new("22222222-2222-2222-2222-000000000013"),
                new("22222222-2222-2222-2222-000000000014"),
                new("22222222-2222-2222-2222-000000000015"),
                new("22222222-2222-2222-2222-000000000016"),
                new("22222222-2222-2222-2222-000000000017"),
                new("22222222-2222-2222-2222-000000000018"),
                new("22222222-2222-2222-2222-000000000019"),
                new("22222222-2222-2222-2222-00000000001A"),
                new("22222222-2222-2222-2222-00000000001B"),
                new("22222222-2222-2222-2222-00000000001C"),
            };

            foreach (var id in addressIds)
            {
                migrationBuilder.DeleteData(schema: "dbo", table: "anschrift", keyColumn: "Id", keyValue: id);
            }

            var personIds = new Guid[]
            {
                new("11111111-1111-1111-1111-111111111111"),
                new("11111111-1111-1111-1111-111111111112"),
                new("11111111-1111-1111-1111-111111111113"),
                new("11111111-1111-1111-1111-111111111114"),
                new("11111111-1111-1111-1111-111111111115"),
                new("11111111-1111-1111-1111-111111111116"),
                new("11111111-1111-1111-1111-111111111117"),
                new("11111111-1111-1111-1111-111111111118"),
                new("11111111-1111-1111-1111-111111111119"),
                new("11111111-1111-1111-1111-11111111111A"),
                new("11111111-1111-1111-1111-11111111111B"),
                new("11111111-1111-1111-1111-11111111111C"),
                new("11111111-1111-1111-1111-11111111111D"),
                new("11111111-1111-1111-1111-11111111111E"),
                new("11111111-1111-1111-1111-11111111111F"),
                new("11111111-1111-1111-1111-111111111120"),
                new("11111111-1111-1111-1111-111111111121"),
                new("11111111-1111-1111-1111-111111111122"),
                new("11111111-1111-1111-1111-111111111123"),
                new("11111111-1111-1111-1111-111111111124"),
            };

            foreach (var id in personIds)
            {
                migrationBuilder.DeleteData(schema: "dbo", table: "person", keyColumn: "Id", keyValue: id);
            }
        }
    }
}
