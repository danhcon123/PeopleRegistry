USE PeopleDb_Task1;
GO

-- 1) Wie viele Personendatensätze sind vorhanden?
SELECT COUNT (*) AS AnzahlPerson FROM dbo.Person;
GO

-- 2) Wie viele Person wohnen in Dresden?
SELECT COUNT (DISTINCT a.PersonId) AS AnzahlPersonInDresden
    FROM dbo.Anschrift a
    WHERE a.Ort = N'Dresden';
GO
    
-- 3) Wie viele Person hat mehr als ein Telefonnummer?
SELECT COUNT (*) AS PersonMitMehrAlsEinerNummer
FROM (
    SELECT PersonId
    FROM dbo.Telefonverbindung
    GROUP BY PersonId
    HAVING COUNT (*) > 1
) x;
GO

