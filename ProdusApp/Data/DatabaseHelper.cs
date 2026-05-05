using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ProdusApp.Data
{
    public class DatabaseHelper
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static void InitializeDatabase()
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                // Creare tabel Categorie
                string createCategorieTable = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Categorie' AND xtype='U')
                    BEGIN
                        CREATE TABLE Categorie (
                            Id INT PRIMARY KEY IDENTITY(1,1),
                            Nume NVARCHAR(100) NOT NULL,
                            Descriere NVARCHAR(500),
                            DataCreare DATETIME NOT NULL DEFAULT GETDATE()
                        )
                    END";

                using (var command = new SqlCommand(createCategorieTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Creare tabel Produs
                string createProdusTable = @"
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
                    END";

                using (var command = new SqlCommand(createProdusTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
