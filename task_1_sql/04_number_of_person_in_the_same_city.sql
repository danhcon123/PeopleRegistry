USE PeopleDb_Task1;
GO

-- Anzahl der Personen pro Ort
SELECT a.Ort, COUNT (DISTINCT a.PersonId) AS Personen
FROM dbo.Anschrift
GROUP BY a.Ort
ORDER BY Personen DESC, a.Ort;
GO