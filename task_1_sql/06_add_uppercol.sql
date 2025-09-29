USE PeopleDb_Task1;
GO

--Füge eine großgeschriebene Name-Spalter hinzu
ALTER TABLE dbo.Person ADD NameUpper NVARCHAR(201) NULL;
GO
UPDATE dbo.Person
SET NameUpper = UPPER(CONCAT(Vorname, N' ', Nachname));
GO
