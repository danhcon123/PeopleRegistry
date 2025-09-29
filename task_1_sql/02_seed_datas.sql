USE PeopleDb_Task1;
GO

-- Lösch alle Datensätze in allen Tabellen
TRUNCATE TABLE dbo.Telefonverbindung;
TRUNCATE TABLE dbo.Anschrift;
TRUNCATE TABLE dbo.Person;
GO

DECLARE @p1 UNIQUEIDENTIFIER = NEWID();
DECLARE @p2 UNIQUEIDENTIFIER = NEWID();
DECLARE @p3 UNIQUEIDENTIFIER = NEWID();

INSERT INTO dbo.Person (Id, Vorname, Nachname, Geburtsdatum)
VALUES
    (@p1, N'Anna', N'Müller', '1997-04-12'),
    (@p2, N'Peter', N'Schmidt', '1999-09-30'),
    (@p3, N'Lena', N'Keller', '1998-01-05');

INSERT INTO dbo.Anschrift (PersonId, Postleitzahl, Ort, Strasse, Hausnummer)
VALUES
    (@p1, N'01067', N'Dresden', N'Altmarkt', N'1'),
    (@p2, N'10115', N'Berlin', N'Charlotte', N'29'),
    (@p1, N'01067', N'Dresden', N'Neustädte Markt', N'13'),
    (@p1, N'80331', N'München', N'Schiller', N'15');

INSERT INTO dbo.Telefonverbindung (PersonId, Telefonnummer)
VALUES
    (@p1, N'+49 176 43771350'),
    (@p1, N'+49 176 43771351'),
    (@p2, N'+49 30 445566'),
    (@p3, N'089 777666');
