using System;
using System.Collections.Generic;
using ProdusApp.Models;
using ProdusApp.Repositories;

namespace ProdusApp.Services
{
    public class ProdusService
    {
        private readonly CategorieRepository _categorieRepository;
        private readonly ProdusRepository _produsRepository;

        public ProdusService(string connectionString)
        {
            _categorieRepository = new CategorieRepository(connectionString);
            _produsRepository = new ProdusRepository(connectionString);
        }

        // Operații Categorie
        public int AdaugaCategorie(Categorie categorie)
        {
            return _categorieRepository.Adauga(categorie);
        }

        public List<Categorie> ObtineToateCategoriile()
        {
            return _categorieRepository.ObtineToate();
        }

        public Categorie ObtineCategorieDupaId(int id)
        {
            return _categorieRepository.ObtineDupaId(id);
        }

        public bool ActualizeazaCategorie(Categorie categorie)
        {
            return _categorieRepository.Actualizeaza(categorie);
        }

        public bool StergeCategorie(int id)
        {
            if (_produsRepository.ExistaProdusePentruCategorie(id))
            {
                throw new Exception("Nu se poate șterge categoria deoarece există produse asociate.");
            }
            return _categorieRepository.Sterge(id);
        }

        // Operații Produs
        public int AdaugaProdus(Produs produs)
        {
            // Verificăm dacă categoria există
            var categorie = _categorieRepository.ObtineDupaId(produs.CategorieId);
            if (categorie == null)
            {
                throw new Exception($"Categoria cu ID-ul {produs.CategorieId} nu există.");
            }

            return _produsRepository.Adauga(produs);
        }

        public List<Produs> ObtineToateProdusele()
        {
            return _produsRepository.ObtineToate();
        }

        public Produs ObtineProdusDupaId(int id)
        {
            return _produsRepository.ObtineDupaId(id);
        }

        public bool ActualizeazaProdus(Produs produs)
        {
            // Verificăm dacă categoria există
            var categorie = _categorieRepository.ObtineDupaId(produs.CategorieId);
            if (categorie == null)
            {
                throw new Exception($"Categoria cu ID-ul {produs.CategorieId} nu există.");
            }

            return _produsRepository.Actualizeaza(produs);
        }

        public bool StergeProdus(int id)
        {
            return _produsRepository.Sterge(id);
        }

        // Populare date de test
        public void PopuleazaDateTest()
        {
            // Adăugăm categorii doar dacă nu există deja
            var categoriiExistente = _categorieRepository.ObtineToate();
            
            if (categoriiExistente.Count == 0)
            {
                // Adăugăm 3 categorii de test
                var categorie1 = new Categorie { Nume = "Electronice", Descriere = "Dispozitive electronice și gadget-uri" };
                var categorie2 = new Categorie { Nume = "Îmbrăcăminte", Descriere = "Haine și accesorii vestimentare" };
                var categorie3 = new Categorie { Nume = "Alimentare", Descriere = "Produse alimentare și băuturi" };

                AdaugaCategorie(categorie1);
                AdaugaCategorie(categorie2);
                AdaugaCategorie(categorie3);

                Console.WriteLine("Au fost adăugate 3 categorii de test.");
            }

            // Adăugăm produse doar dacă nu există deja
            var produseExistente = _produsRepository.ObtineToate();

            if (produseExistente.Count == 0)
            {
                // Obținem categoriile pentru a le asocia produselor
                var categorii = _categorieRepository.ObtineToate();
                
                // Adăugăm 3 produse de test
                var produs1 = new Produs
                {
                    Nume = "Smartphone Samsung Galaxy",
                    Descriere = "Smartphone cu ecran AMOLED 6.5 inch, 128GB stocare",
                    Pret = 1999.99m,
                    Stoc = 50,
                    CategorieId = 1 // Electronice
                };

                var produs2 = new Produs
                {
                    Nume = "Tricou Bumbac Premium",
                    Descriere = "Tricou din bumbac 100%, disponibil în mai multe culori",
                    Pret = 79.99m,
                    Stoc = 200,
                    CategorieId = 2 // Îmbrăcăminte
                };

                var produs3 = new Produs
                {
                    Nume = "Ciocolată cu Lapte 100g",
                    Descriere = "Ciocolată fină cu lapte, producție locală",
                    Pret = 12.50m,
                    Stoc = 500,
                    CategorieId = 3 // Alimentare
                };

                AdaugaProdus(produs1);
                AdaugaProdus(produs2);
                AdaugaProdus(produs3);

                Console.WriteLine("Au fost adăugate 3 produse de test.");
            }
        }
    }
}
