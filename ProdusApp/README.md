# Aplicație CRUD Produse și Categorii - .NET Framework 4.7.2

## Descriere
Aplicație consolă în C# care gestionează operațiile CRUD (Create, Read, Update, Delete) pentru o bază de date SQL Server cu tabelele `Produs` și `Categorie`.

## Structura Proiectului

```
ProdusApp/
├── Program.cs                    # Punctul de intrare și interfața consolei
├── App.config                    # Configurare connection string
├── ProdusApp.csproj              # Fișier proiect .NET Framework 4.7.2
├── ScriptBazaDeDate.sql          # Script SQL pentru creare tabele și date test
├── Models/
│   ├── Categorie.cs              # Model entitate Categorie
│   └── Produs.cs                 # Model entitate Produs
├── Data/
│   └── DatabaseHelper.cs         # Helper pentru conexiune și inițializare DB
├── Repositories/
│   ├── CategorieRepository.cs    # Operații CRUD pentru Categorie
│   └── ProdusRepository.cs       # Operații CRUD pentru Produs
└── Services/
    └── ProdusService.cs          # Logică de business și validări
```

## Cerințe

- .NET Framework 4.7.2 sau superior
- SQL Server (Express sau versiuni superioare)
- Visual Studio 2019 sau superior (opțional, pentru compilare)

## Configurare

### 1. Baza de date
Rulați scriptul `ScriptBazaDeDate.sql` în SQL Server Management Studio sau lăsați aplicația să creeze automat tabelele la prima rulare.

### 2. Connection String
În fișierul `App.config`, modificați connection string-ul dacă este necesar:

```xml
<connectionStrings>
    <add name="DefaultConnection" 
         connectionString="Server=localhost;Database=ProdusDB;Integrated Security=true;" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

## Funcționalități

### Gestionare Categorii (CRUD)
- Adăugare categorie nouă
- Listare toate categoriile
- Căutare categorie după ID
- Actualizare categorie
- Ștergere categorie (doar dacă nu are produse asociate)

### Gestionare Produse (CRUD)
- Adăugare produs nou (cu asociere la categorie)
- Listare toate produsele (cu informații despre categorie)
- Căutare produs după ID
- Actualizare produs
- Ștergere produs

### Date de Test
La prima rulare, aplicația populează automat baza de date cu:
- **3 categorii**: Electronice, Îmbrăcăminte, Alimentare
- **3 produse**: Smartphone Samsung Galaxy, Tricou Bumbac Premium, Ciocolată cu Lapte

## Utilizare

1. Compilați proiectul:
   ```bash
   msbuild ProdusApp.csproj
   ```

2. Rulați aplicația:
   ```bash
   ProdusApp.exe
   ```

3. Navigați prin meniul interactiv pentru a efectua operațiuni CRUD.

## Structura Bazei de Date

### Tabelul Categorie
| Coloană | Tip | Descriere |
|---------|-----|-----------|
| Id | INT (PK, Identity) | Identificator unic |
| Nume | NVARCHAR(100) | Numele categoriei |
| Descriere | NVARCHAR(500) | Descrierea categoriei |
| DataCreare | DATETIME | Data creării |

### Tabelul Produs
| Coloană | Tip | Descriere |
|---------|-----|-----------|
| Id | INT (PK, Identity) | Identificator unic |
| Nume | NVARCHAR(200) | Numele produsului |
| Descriere | NVARCHAR(1000) | Descrierea produsului |
| Pret | DECIMAL(18,2) | Prețul produsului |
| Stoc | INT | Cantitatea în stoc |
| CategorieId | INT (FK) | Referință către Categorie |
| DataCreare | DATETIME | Data creării |

## Autor
Generat pentru gestionarea CRUD a produselor și categoriilor în SQL Server.
