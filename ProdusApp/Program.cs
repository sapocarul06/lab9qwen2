using System;
using System.Configuration;
using ProdusApp.Data;
using ProdusApp.Models;
using ProdusApp.Services;

namespace ProdusApp
{
    class Program
    {
        private static ProdusService _produsService;
        private static string _connectionString;

        static void Main(string[] args)
        {
            try
            {
                // Inițializare conexiune și service
                _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                _produsService = new ProdusService(_connectionString);

                // Inițializare bază de date (creare tabele)
                DatabaseHelper.InitializeDatabase();

                Console.WriteLine("===========================================");
                Console.WriteLine("   Aplicație CRUD pentru Produse și Categorii");
                Console.WriteLine("   .NET Framework 4.7.2 - SQL Server");
                Console.WriteLine("===========================================\n");

                // Populare cu date de test
                _produsService.PopuleazaDateTest();
                Console.WriteLine();

                bool continueRunning = true;
                while (continueRunning)
                {
                    AfiseazaMeniuPrincipal();
                    string optiune = Console.ReadLine();

                    switch (optiune)
                    {
                        case "1":
                            GestionareProduse();
                            break;
                        case "2":
                            GestionareCategorii();
                            break;
                        case "3":
                            AfiseazaToateDatele();
                            break;
                        case "4":
                            continueRunning = false;
                            Console.WriteLine("\nLa revedere!");
                            break;
                        default:
                            Console.WriteLine("\nOpțiune invalidă! Încercați din nou.\n");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nEroare: {ex.Message}");
                Console.WriteLine($"Detalii: {ex.InnerException?.Message}");
                Console.WriteLine("\nAsigurați-vă că SQL Server este instalat și rulează, iar connection string-ul este corect configurat.");
            }
        }

        static void AfiseazaMeniuPrincipal()
        {
            Console.WriteLine("\n--- MENIU PRINCIPAL ---");
            Console.WriteLine("1. Gestionare Produse (CRUD)");
            Console.WriteLine("2. Gestionare Categorii (CRUD)");
            Console.WriteLine("3. Afișare toate datele");
            Console.WriteLine("4. Ieșire");
            Console.Write("\nAlegeți o opțiune: ");
        }

        static void GestionareProduse()
        {
            bool continueRunning = true;
            while (continueRunning)
            {
                Console.WriteLine("\n--- GESTIONARE PRODUSE ---");
                Console.WriteLine("1. Adaugă produs nou");
                Console.WriteLine("2. Listează toate produsele");
                Console.WriteLine("3. Caută produs după ID");
                Console.WriteLine("4. Actualizează produs");
                Console.WriteLine("5. Șterge produs");
                Console.WriteLine("0. Înapoi la meniul principal");
                Console.Write("\nAlegeți o opțiune: ");

                string optiune = Console.ReadLine();

                switch (optiune)
                {
                    case "1":
                        AdaugaProdusNou();
                        break;
                    case "2":
                        ListeazaProduse();
                        break;
                    case "3":
                        CautaProdusDupaId();
                        break;
                    case "4":
                        ActualizeazaProdus();
                        break;
                    case "5":
                        StergeProdus();
                        break;
                    case "0":
                        continueRunning = false;
                        break;
                    default:
                        Console.WriteLine("\nOpțiune invalidă!");
                        break;
                }
            }
        }

        static void GestionareCategorii()
        {
            bool continueRunning = true;
            while (continueRunning)
            {
                Console.WriteLine("\n--- GESTIONARE CATEGORII ---");
                Console.WriteLine("1. Adaugă categorie nouă");
                Console.WriteLine("2. Listează toate categoriile");
                Console.WriteLine("3. Caută categorie după ID");
                Console.WriteLine("4. Actualizează categorie");
                Console.WriteLine("5. Șterge categorie");
                Console.WriteLine("0. Înapoi la meniul principal");
                Console.Write("\nAlegeți o opțiune: ");

                string optiune = Console.ReadLine();

                switch (optiune)
                {
                    case "1":
                        AdaugaCategorieNoua();
                        break;
                    case "2":
                        ListeazaCategorii();
                        break;
                    case "3":
                        CautaCategorieDupaId();
                        break;
                    case "4":
                        ActualizeazaCategorie();
                        break;
                    case "5":
                        StergeCategorie();
                        break;
                    case "0":
                        continueRunning = false;
                        break;
                    default:
                        Console.WriteLine("\nOpțiune invalidă!");
                        break;
                }
            }
        }

        #region Operații Produse

        static void AdaugaProdusNou()
        {
            try
            {
                Console.WriteLine("\n--- Adaugă Produs Nou ---");
                
                // Afișăm categoriile disponibile
                var categorii = _produsService.ObtineToateCategoriile();
                if (categorii.Count == 0)
                {
                    Console.WriteLine("Nu există categorii definite. Adăugați mai întâi o categorie.");
                    return;
                }

                Console.WriteLine("Categorii disponibile:");
                foreach (var cat in categorii)
                {
                    Console.WriteLine($"  {cat.Id}. {cat.Nume}");
                }

                Console.Write("\nNume produs: ");
                string nume = Console.ReadLine();

                Console.Write("Descriere: ");
                string descriere = Console.ReadLine();

                Console.Write("Preț (RON): ");
                decimal pret = Convert.ToDecimal(Console.ReadLine());

                Console.Write("Stoc: ");
                int stoc = Convert.ToInt32(Console.ReadLine());

                Console.Write("ID Categorie: ");
                int categorieId = Convert.ToInt32(Console.ReadLine());

                var produs = new Produs
                {
                    Nume = nume,
                    Descriere = descriere,
                    Pret = pret,
                    Stoc = stoc,
                    CategorieId = categorieId
                };

                int id = _produsService.AdaugaProdus(produs);
                Console.WriteLine($"\nProdusul a fost adăugat cu succes! ID: {id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nEroare la adăugarea produsului: {ex.Message}");
            }
        }

        static void ListeazaProduse()
        {
            Console.WriteLine("\n--- Lista Produse ---");
            var produse = _produsService.ObtineToateProdusele();

            if (produse.Count == 0)
            {
                Console.WriteLine("Nu există produse în baza de date.");
                return;
            }

            foreach (var produs in produse)
            {
                Console.WriteLine(produs.ToString());
            }
        }

        static void CautaProdusDupaId()
        {
            try
            {
                Console.Write("\nIntroduceți ID-ul produsului: ");
                int id = Convert.ToInt32(Console.ReadLine());

                var produs = _produsService.ObtineProdusDupaId(id);
                if (produs != null)
                {
                    Console.WriteLine("\n" + produs.ToString());
                }
                else
                {
                    Console.WriteLine("\nProdusul nu a fost găsit.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nEroare: {ex.Message}");
            }
        }

        static void ActualizeazaProdus()
        {
            try
            {
                Console.Write("\nIntroduceți ID-ul produsului de actualizat: ");
                int id = Convert.ToInt32(Console.ReadLine());

                var produs = _produsService.ObtineProdusDupaId(id);
                if (produs == null)
                {
                    Console.WriteLine("Produsul nu a fost găsit.");
                    return;
                }

                Console.WriteLine($"\nProdus curent: {produs.Nume}");
                Console.Write("Nume nou (Enter pentru a păstra): ");
                string numeInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(numeInput))
                    produs.Nume = numeInput;

                Console.Write("Descriere nouă (Enter pentru a păstra): ");
                string descriereInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(descriereInput))
                    produs.Descriere = descriereInput;

                Console.Write($"Preț nou ({produs.Pret}) (Enter pentru a păstra): ");
                string pretInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(pretInput))
                    produs.Pret = Convert.ToDecimal(pretInput);

                Console.Write($"Stoc nou ({produs.Stoc}) (Enter pentru a păstra): ");
                string stocInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(stocInput))
                    produs.Stoc = Convert.ToInt32(stocInput);

                // Afișăm categoriile
                var categorii = _produsService.ObtineToateCategoriile();
                Console.WriteLine("Categorii disponibile:");
                foreach (var cat in categorii)
                {
                    Console.WriteLine($"  {cat.Id}. {cat.Nume}");
                }
                Console.Write($"ID Categorie nou ({produs.CategorieId}) (Enter pentru a păstra): ");
                string catInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(catInput))
                    produs.CategorieId = Convert.ToInt32(catInput);

                bool rezultat = _produsService.ActualizeazaProdus(produs);
                if (rezultat)
                {
                    Console.WriteLine("\nProdusul a fost actualizat cu succes!");
                }
                else
                {
                    Console.WriteLine("\nActualizarea a eșuat.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nEroare la actualizare: {ex.Message}");
            }
        }

        static void StergeProdus()
        {
            try
            {
                Console.Write("\nIntroduceți ID-ul produsului de șters: ");
                int id = Convert.ToInt32(Console.ReadLine());

                var produs = _produsService.ObtineProdusDupaId(id);
                if (produs == null)
                {
                    Console.WriteLine("Produsul nu a fost găsit.");
                    return;
                }

                Console.Write($"Sigur doriți să ștergeți produsul '{produs.Nume}'? (y/n): ");
                string confirmare = Console.ReadLine();

                if (confirmare.ToLower() == "y")
                {
                    bool rezultat = _produsService.StergeProdus(id);
                    if (rezultat)
                    {
                        Console.WriteLine("\nProdusul a fost șters cu succes!");
                    }
                    else
                    {
                        Console.WriteLine("\nȘtergerea a eșuat.");
                    }
                }
                else
                {
                    Console.WriteLine("\nOperația a fost anulată.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nEroare la ștergere: {ex.Message}");
            }
        }

        #endregion

        #region Operații Categorii

        static void AdaugaCategorieNoua()
        {
            try
            {
                Console.WriteLine("\n--- Adaugă Categorie Nouă ---");

                Console.Write("Nume categorie: ");
                string nume = Console.ReadLine();

                Console.Write("Descriere: ");
                string descriere = Console.ReadLine();

                var categorie = new Categorie
                {
                    Nume = nume,
                    Descriere = descriere
                };

                int id = _produsService.AdaugaCategorie(categorie);
                Console.WriteLine($"\nCategoria a fost adăugată cu succes! ID: {id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nEroare la adăugarea categoriei: {ex.Message}");
            }
        }

        static void ListeazaCategorii()
        {
            Console.WriteLine("\n--- Lista Categorii ---");
            var categorii = _produsService.ObtineToateCategoriile();

            if (categorii.Count == 0)
            {
                Console.WriteLine("Nu există categorii în baza de date.");
                return;
            }

            foreach (var categorie in categorii)
            {
                Console.WriteLine(categorie.ToString());
            }
        }

        static void CautaCategorieDupaId()
        {
            try
            {
                Console.Write("\nIntroduceți ID-ul categoriei: ");
                int id = Convert.ToInt32(Console.ReadLine());

                var categorie = _produsService.ObtineCategorieDupaId(id);
                if (categorie != null)
                {
                    Console.WriteLine("\n" + categorie.ToString());
                }
                else
                {
                    Console.WriteLine("\nCategoria nu a fost găsită.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nEroare: {ex.Message}");
            }
        }

        static void ActualizeazaCategorie()
        {
            try
            {
                Console.Write("\nIntroduceți ID-ul categoriei de actualizat: ");
                int id = Convert.ToInt32(Console.ReadLine());

                var categorie = _produsService.ObtineCategorieDupaId(id);
                if (categorie == null)
                {
                    Console.WriteLine("Categoria nu a fost găsită.");
                    return;
                }

                Console.WriteLine($"\nCategoria curentă: {categorie.Nume}");
                Console.Write("Nume nou (Enter pentru a păstra): ");
                string numeInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(numeInput))
                    categorie.Nume = numeInput;

                Console.Write("Descriere nouă (Enter pentru a păstra): ");
                string descriereInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(descriereInput))
                    categorie.Descriere = descriereInput;

                bool rezultat = _produsService.ActualizeazaCategorie(categorie);
                if (rezultat)
                {
                    Console.WriteLine("\nCategoria a fost actualizată cu succes!");
                }
                else
                {
                    Console.WriteLine("\nActualizarea a eșuat.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nEroare la actualizare: {ex.Message}");
            }
        }

        static void StergeCategorie()
        {
            try
            {
                Console.Write("\nIntroduceți ID-ul categoriei de șters: ");
                int id = Convert.ToInt32(Console.ReadLine());

                var categorie = _produsService.ObtineCategorieDupaId(id);
                if (categorie == null)
                {
                    Console.WriteLine("Categoria nu a fost găsită.");
                    return;
                }

                Console.Write($"Sigur doriți să ștergeți categoria '{categorie.Nume}'? (y/n): ");
                string confirmare = Console.ReadLine();

                if (confirmare.ToLower() == "y")
                {
                    bool rezultat = _produsService.StergeCategorie(id);
                    if (rezultat)
                    {
                        Console.WriteLine("\nCategoria a fost ștearsă cu succes!");
                    }
                    else
                    {
                        Console.WriteLine("\nȘtergerea a eșuat.");
                    }
                }
                else
                {
                    Console.WriteLine("\nOperația a fost anulată.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nEroare la ștergere: {ex.Message}");
            }
        }

        #endregion

        static void AfiseazaToateDatele()
        {
            Console.WriteLine("\n===========================================");
            Console.WriteLine("   TOATE DATELE DIN BAZA DE DATE");
            Console.WriteLine("===========================================\n");

            // Afișăm categoriile
            Console.WriteLine("--- CATEGORII ---");
            var categorii = _produsService.ObtineToateCategoriile();
            if (categorii.Count > 0)
            {
                foreach (var cat in categorii)
                {
                    Console.WriteLine(cat.ToString());
                }
            }
            else
            {
                Console.WriteLine("Nu există categorii.");
            }

            Console.WriteLine("\n--- PRODUSE ---");
            var produse = _produsService.ObtineToateProdusele();
            if (produse.Count > 0)
            {
                foreach (var produs in produse)
                {
                    Console.WriteLine(produs.ToString());
                }
            }
            else
            {
                Console.WriteLine("Nu există produse.");
            }

            Console.WriteLine("\n===========================================");
        }
    }
}
