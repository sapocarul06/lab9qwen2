-- Script SQL pentru crearea bazei de date ProdusDB
-- Rulați acest script în SQL Server Management Studio

USE master;
GO

-- Creare bază de date
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ProdusDB')
BEGIN
    CREATE DATABASE ProdusDB;
END
GO

USE ProdusDB;
GO

-- Creare tabel Categorie
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Categorie' AND xtype='U')
BEGIN
    CREATE TABLE Categorie (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nume NVARCHAR(100) NOT NULL,
        Descriere NVARCHAR(500),
        DataCreare DATETIME NOT NULL DEFAULT GETDATE()
    )
END
GO

-- Creare tabel Produs
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Produs' AND xtype='U')
BEGIN
    CREATE TABLE Produs (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nume NVARCHAR(200) NOT NULL,
        Descriere NVARCHAR(1000),
        Pret DECIMAL(18,2) NOT NULL,
        Stoc INT NOT NULL DEFAULT 0,
        CategorieId INT NOT NULL,
        DataCreare DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Produs_Categorie FOREIGN KEY (CategorieId) REFERENCES Categorie(Id)
    )
END
GO

-- Inserare date de test în Categorie (3 înregistrări)
IF NOT EXISTS (SELECT 1 FROM Categorie)
BEGIN
    INSERT INTO Categorie (Nume, Descriere) VALUES 
    ('Electronice', 'Dispozitive electronice și gadget-uri'),
    ('Îmbrăcăminte', 'Haine și accesorii vestimentare'),
    ('Alimentare', 'Produse alimentare și băuturi');
END
GO

-- Inserare date de test în Produs (3 înregistrări)
IF NOT EXISTS (SELECT 1 FROM Produs)
BEGIN
    INSERT INTO Produs (Nume, Descriere, Pret, Stoc, CategorieId) VALUES 
    ('Smartphone Samsung Galaxy', 'Smartphone cu ecran AMOLED 6.5 inch, 128GB stocare', 1999.99, 50, 1),
    ('Tricou Bumbac Premium', 'Tricou din bumbac 100%, disponibil în mai multe culori', 79.99, 200, 2),
    ('Ciocolată cu Lapte 100g', 'Ciocolată fină cu lapte, producție locală', 12.50, 500, 3);
END
GO

-- Verificare date
SELECT * FROM Categorie;
SELECT * FROM Produs;
GO
