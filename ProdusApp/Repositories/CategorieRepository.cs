using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ProdusApp.Models;

namespace ProdusApp.Repositories
{
    public class CategorieRepository
    {
        private readonly string _connectionString;

        public CategorieRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // CREATE - Adaugă o categorie nouă
        public int Adauga(Categorie categorie)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Categorie (Nume, Descriere, DataCreare)
                    VALUES (@Nume, @Descriere, @DataCreare);
                    SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nume", categorie.Nume);
                    command.Parameters.AddWithValue("@Descriere", (object)categorie.Descriere ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DataCreare", categorie.DataCreare);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        // READ - Obține toate categoriile
        public List<Categorie> ObtineToate()
        {
            var categorii = new List<Categorie>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Nume, Descriere, DataCreare FROM Categorie ORDER BY Id";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categorii.Add(new Categorie
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nume = reader["Nume"].ToString(),
                            Descriere = reader["Descriere"] != DBNull.Value ? reader["Descriere"].ToString() : null,
                            DataCreare = Convert.ToDateTime(reader["DataCreare"])
                        });
                    }
                }
            }

            return categorii;
        }

        // READ - Obține o categorie după ID
        public Categorie ObtineDupaId(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Nume, Descriere, DataCreare FROM Categorie WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Categorie
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nume = reader["Nume"].ToString(),
                                Descriere = reader["Descriere"] != DBNull.Value ? reader["Descriere"].ToString() : null,
                                DataCreare = Convert.ToDateTime(reader["DataCreare"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        // UPDATE - Actualizează o categorie
        public bool Actualizeaza(Categorie categorie)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    UPDATE Categorie
                    SET Nume = @Nume, Descriere = @Descriere
                    WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", categorie.Id);
                    command.Parameters.AddWithValue("@Nume", categorie.Nume);
                    command.Parameters.AddWithValue("@Descriere", (object)categorie.Descriere ?? DBNull.Value);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        // DELETE - Șterge o categorie
        public bool Sterge(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Categorie WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
