USE PeopleDb_Task1;
GO

-- Lösche die Telefonnummer, welche nicht mit "+" oder "0" starten
DELETE FROM dto.Telefonverbindung
WHERE Telefonummer NOT LIKE N'0%' AND Telefonnummer NOT LIKE N'+%';
