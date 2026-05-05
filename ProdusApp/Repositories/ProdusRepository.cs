using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ProdusApp.Models;

namespace ProdusApp.Repositories
{
    public class ProdusRepository
    {
        private readonly string _connectionString;

        public ProdusRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // CREATE - Adaugă un produs nou
        public int Adauga(Produs produs)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Produs (Nume, Descriere, Pret, Stoc, CategorieId, DataCreare)
                    VALUES (@Nume, @Descriere, @Pret, @Stoc, @CategorieId, @DataCreare);
                    SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nume", produs.Nume);
                    command.Parameters.AddWithValue("@Descriere", (object)produs.Descriere ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Pret", produs.Pret);
                    command.Parameters.AddWithValue("@Stoc", produs.Stoc);
                    command.Parameters.AddWithValue("@CategorieId", produs.CategorieId);
                    command.Parameters.AddWithValue("@DataCreare", produs.DataCreare);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        // READ - Obține toate produsele cu categoria asociată
        public List<Produs> ObtineToate()
        {
            var produse = new List<Produs>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT p.Id, p.Nume, p.Descriere, p.Pret, p.Stoc, p.CategorieId, p.DataCreare,
                           c.Id as CatId, c.Nume as CatNume, c.Descriere as CatDescriere, c.DataCreare as CatDataCreare
                    FROM Produs p
                    INNER JOIN Categorie c ON p.CategorieId = c.Id
                    ORDER BY p.Id";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var produs = new Produs
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nume = reader["Nume"].ToString(),
                            Descriere = reader["Descriere"] != DBNull.Value ? reader["Descriere"].ToString() : null,
                            Pret = Convert.ToDecimal(reader["Pret"]),
                            Stoc = Convert.ToInt32(reader["Stoc"]),
                            CategorieId = Convert.ToInt32(reader["CategorieId"]),
                            DataCreare = Convert.ToDateTime(reader["DataCreare"]),
                            Categorie = new Categorie
                            {
                                Id = Convert.ToInt32(reader["CatId"]),
                                Nume = reader["CatNume"].ToString(),
                                Descriere = reader["CatDescriere"] != DBNull.Value ? reader["CatDescriere"].ToString() : null,
                                DataCreare = Convert.ToDateTime(reader["CatDataCreare"])
                            }
                        };
                        produse.Add(produs);
                    }
                }
            }

            return produse;
        }

        // READ - Obține un produs după ID cu categoria asociată
        public Produs ObtineDupaId(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT p.Id, p.Nume, p.Descriere, p.Pret, p.Stoc, p.CategorieId, p.DataCreare,
                           c.Id as CatId, c.Nume as CatNume, c.Descriere as CatDescriere, c.DataCreare as CatDataCreare
                    FROM Produs p
                    INNER JOIN Categorie c ON p.CategorieId = c.Id
                    WHERE p.Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Produs
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nume = reader["Nume"].ToString(),
                                Descriere = reader["Descriere"] != DBNull.Value ? reader["Descriere"].ToString() : null,
                                Pret = Convert.ToDecimal(reader["Pret"]),
                                Stoc = Convert.ToInt32(reader["Stoc"]),
                                CategorieId = Convert.ToInt32(reader["CategorieId"]),
                                DataCreare = Convert.ToDateTime(reader["DataCreare"]),
                                Categorie = new Categorie
                                {
                                    Id = Convert.ToInt32(reader["CatId"]),
                                    Nume = reader["CatNume"].ToString(),
                                    Descriere = reader["CatDescriere"] != DBNull.Value ? reader["CatDescriere"].ToString() : null,
                                    DataCreare = Convert.ToDateTime(reader["CatDataCreare"])
                                }
                            };
                        }
                    }
                }
            }

            return null;
        }

        // UPDATE - Actualizează un produs
        public bool Actualizeaza(Produs produs)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    UPDATE Produs
                    SET Nume = @Nume, Descriere = @Descriere, Pret = @Pret, Stoc = @Stoc, CategorieId = @CategorieId
                    WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", produs.Id);
                    command.Parameters.AddWithValue("@Nume", produs.Nume);
                    command.Parameters.AddWithValue("@Descriere", (object)produs.Descriere ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Pret", produs.Pret);
                    command.Parameters.AddWithValue("@Stoc", produs.Stoc);
                    command.Parameters.AddWithValue("@CategorieId", produs.CategorieId);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        // DELETE - Șterge un produs
        public bool Sterge(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Produs WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        // Verifică dacă există produse pentru o categorie
        public bool ExistaProdusePentruCategorie(int categorieId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Produs WHERE CategorieId = @CategorieId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategorieId", categorieId);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }
    }
}
