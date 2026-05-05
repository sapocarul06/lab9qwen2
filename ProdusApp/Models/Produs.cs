using System;

namespace ProdusApp.Models
{
    public class Produs
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Descriere { get; set; }
        public decimal Pret { get; set; }
        public int Stoc { get; set; }
        public int CategorieId { get; set; }
        public DateTime DataCreare { get; set; }

        // Proprietate de navigare
        public Categorie Categorie { get; set; }

        public Produs()
        {
            DataCreare = DateTime.Now;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Nume: {Nume}, Pret: {Pret:F2} RON, Stoc: {Stoc}, Categorie: {(Categorie != null ? Categorie.Nume : "N/A")}";
        }
    }
}
