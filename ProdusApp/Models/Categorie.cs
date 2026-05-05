using System;

namespace ProdusApp.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Descriere { get; set; }
        public DateTime DataCreare { get; set; }

        public Categorie()
        {
            DataCreare = DateTime.Now;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Nume: {Nume}, Descriere: {Descriere}, Data Creare: {DataCreare:dd.MM.yyyy HH:mm}";
        }
    }
}
