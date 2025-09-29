USE PeopleDb_Task1;
GO

-- Erstell View, welche alle Personen mit deren Anschriften und Telefonnummer als eine
-- Ergebnistabelle ausgibt.
IF OBJECT_ID('dbo.vw_Person_Anschrift_Telefon', 'V') IS NOT NULL
    DROP VIEW dbo.vw_Person_Anschrift_Telefon;
GO

CREATE VIEW dbo.vw_Person_Anschrift_Telefon
AS
    SELECT
        p.Id               AS PersonId,
        p.Vorname,
        p.Nachname,
        p.Geburtsdatum,
        a.Postleitzahl,
        a.Ort,
        a.Strasse,
        a.Hausnummer,
        t.Telefonnummer
    FROM dbo.Person p
    LEFT JOIN dbo.Anschrift a
        ON a.PersonId = p.Id
    LEFT JOIN dbo.Telefonverbindung t
        ON t.PersonId = p.Id;
GO

-- View Datensätze anzeigen
SELECT * FROM dbo.vw_Person_Anschrift_Telefon; 
GO
