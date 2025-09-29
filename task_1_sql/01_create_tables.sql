-- Erstell Databases
IF DB_ID('PeopleDb_Task1') IS NULL
BEGIN
    CREATE DATABASE PeopleDb_Task1;
END;
GO

IF DB_ID('PeopleDb_Task2') IS NULL
BEGIN
    CREATE DATABASE PeopleDb_Task2;
END;
GO

-- Erstell Tabellen für Task 1
USE PeopleDb_Task1;
GO

-- Person
IF OBJECT_ID('dbo.Person', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Person (
        Id            UNIQUEIDENTIFIER NOT NULL 
                    CONSTRAINT PK_Person PRIMARY KEY 
                    DEFAULT NEWID(),
        Nachname      NVARCHAR(100)    NOT NULL,
        Vorname       NVARCHAR(100)    NOT NULL,
        Geburtsdatum  DATE             NULL
    );
END
GO

-- Anschrift (many-to-one Person)
IF OBJECT_ID('dbo.Anschrift', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Anschrift (
        Id            UNIQUEIDENTIFIER NOT NULL 
                    CONSTRAINT PK_Anschrift PRIMARY KEY 
                    DEFAULT NEWID(),
        PersonId      UNIQUEIDENTIFIER NOT NULL,
        Postleitzahl  NVARCHAR(10)     NOT NULL,
        Ort           NVARCHAR(100)    NOT NULL,
        Strasse       NVARCHAR(100)    NOT NULL,
        Hausnummer    NVARCHAR(10)     NOT NULL,
        CONSTRAINT FK_Anschrift_Person
            FOREIGN KEY (PersonId) REFERENCES dbo.Person(Id)
            ON DELETE NO ACTION     
    );
END
GO

-- Telefonverbindung (many-to-one Person)
IF OBJECT_ID('dbo.Telefonverbindung', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Telefonverbindung (
        Id             UNIQUEIDENTIFIER NOT NULL 
                    CONSTRAINT PK_Telefonverbindung PRIMARY KEY 
                    DEFAULT NEWID(),
        PersonId       UNIQUEIDENTIFIER NOT NULL,
        Telefonnummer  NVARCHAR(20)     NOT NULL,
        CONSTRAINT FK_Telefon_Person
            FOREIGN KEY (PersonId) REFERENCES dbo.Person(Id)
            ON DELETE NO ACTION
    );
END
GO
