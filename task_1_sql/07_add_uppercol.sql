USE PeopleDb_Task1;
GO

--Füge eine großgeschriebene Name-Spalter hinzu
IF COL_LENGTH('dbo.Person', 'NameUpper') IS NULL
BEGIN
    ALTER TABLE dbo.Person 
    ADD NameUpper NVARCHAR(201) NULL;
    UPDATE dbo.Person
    SET NameUpper = UPPER(CONCAT(Vorname, N' ', Nachname));
END
ELSE
BEGIN
    PRINT 'Column NameUpper already exists.';
END

