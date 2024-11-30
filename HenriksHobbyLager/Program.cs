/*
testdata:
var anka = new Product
{
    Name = "Gummianka",
    Price = 10,
    Stock = 10,
    Category = "programmering"
};
*/

/*
    HENRIKS HOBBYLAGER™ 1.0
    Skapat av: Henrik Hobbykodare
    Datum: En sen kväll i oktober efter fyra Red Bull
    Version: 1.0 (eller kanske 1.1, jag har ändrat lite sen första versionen)

    TODO-lista:
    * Kolla vad interfaces egentligen gör
    * Fixa så att datan inte försvinner när datorn stängs av
    * Lägga till stöd för bilder på produkterna (kanske)
    * Göra backups (förlorade nästan allt förra veckan när skärmsläckaren startade)
    * Kolla upp det där med "molnet" som alla pratar om
    * Snygga till koden (när jag har tid)
    * Lägg till ljudeffekter när man lägger till produkter???
    * Fixa så att programmet startar automatiskt när datorn startar om
    * Be någon förklara vad "dependency injection" betyder
    * Köpa en UPS (strömavbrott är INTE kul!)
    * Lära mig vad XML är (folk säger att det är viktigt)
    * Göra en logga till programmet i Paint
    
    VIKTIGT: Stäng inte av datorn! All data ligger i minnet!
    
    PS. Om någon hittar det här i framtiden: Jag vet att koden kunde varit snyggare, 
    men den fungerar! Och det är huvudsaken... right?
*/
using HenriksHobbyLager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RefactoringExercise
{
    class Program
    {
        // Min fantastiska databas! Fungerar perfekt så länge datorn är igång
        private static List<Product> _products = new List<Product>();
        
        // Räknare för ID. Börjar på 1 för att 0 känns så negativt
        private static int _nextId = 1;

        static void Main(string[] args)
        {
            // Huvudloopen - Stäng inte av programmet, då försvinner allt!
            while (true)
            {
                Console.Clear();  // Rensar skärmen så det ser proffsigt ut
                Console.WriteLine("=== Henriks HobbyLager™ 1.0 ===");
                Console.WriteLine("1. Visa alla produkter");
                Console.WriteLine("2. Lägg till produkt");
                Console.WriteLine("3. Uppdatera produkt");
                Console.WriteLine("4. Ta bort produkt");
                Console.WriteLine("5. Sök produkter");
                Console.WriteLine("6. Avsluta");  // Använd inte denna om du vill behålla datan!
                
                var choice = Console.ReadLine();

                // Switch är tydligen bättre än if-else enligt Google
                switch (choice)
                {
                    case "1":
                        ShowAllProducts();
                        break;
                    case "2":
                        AddProduct();
                        break;
                    case "3":
                        UpdateProduct();
                        break;
                    case "4":
                        DeleteProduct();
                        break;
                    case "5":
                        SearchProducts();
                        break;
                    case "6":
                        return;  // OBS! All data försvinner om du väljer denna!
                    default:
                        Console.WriteLine("Ogiltigt val! Är du säker på att du tryckte på rätt knapp?");
                        break;
                }

                Console.WriteLine("\nTryck på valfri tangent för att fortsätta... (helst inte ESC)");
                Console.ReadKey();
            }
        }

        // Visar alla produkter som finns i "databasen"
        private static void ShowAllProducts()
        {
            // Kollar om det finns några produkter alls
            // !_products.Any() låter mer proffsigt än _products.Count == 0
            if (!_products.Any())
            {
                Console.WriteLine("Inga produkter finns i lagret. Dags att shoppa grossist!");
                return;
            }

            foreach (var product in _products)
            {
                DisplayProduct(product);
            }
        }

        // Lägger till en ny produkt i systemet
        private static void AddProduct()
        {
            Console.WriteLine("=== Lägg till ny produkt ===");
            
            Console.Write("Namn: ");
            var name = Console.ReadLine();
            
            Console.Write("Pris: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Ogiltigt pris! Använd punkt istället för komma (lärde mig den hårda vägen)");
                return;
            }
            
            Console.Write("Antal i lager: ");
            if (!int.TryParse(Console.ReadLine(), out int stock))
            {
                Console.WriteLine("Ogiltig lagermängd! Hela tal endast (kan inte sälja halva helikoptrar)");
                return;
            }
            
            Console.Write("Kategori: ");
            var category = Console.ReadLine();

            // Skapar produkten - Id räknas upp automatiskt så vi slipper hålla reda på det
            var product = new Product
            {
                Id = _nextId++,  // Plussar på direkt, smart va?
                Name = name,
                Price = price,
                Stock = stock,
                Category = category,
                Created = DateTime.Now  // Automatiskt datum, smooth!
            };

            _products.Add(product);
            Console.WriteLine("Produkt tillagd! Glöm inte att hålla datorn igång!");
        }

        // Uppdaterar en befintlig produkt
        private static void UpdateProduct()
        {
            Console.Write("Ange produkt-ID att uppdatera (finns i listan ovan): ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ogiltigt ID! Bara siffror tack!");
                return;
            }

            // LINQ - Google säger att det är snabbt
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                Console.WriteLine("Produkt hittades inte! Är du säker på att du skrev rätt?");
                return;
            }

            // Uppdatera bara det som användaren faktiskt skriver in
            Console.Write("Nytt namn (tryck bara enter om du vill behålla det gamla): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                product.Name = name;

            Console.Write("Nytt pris (tryck bara enter om du vill behålla det gamla): ");
            var priceInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(priceInput) && decimal.TryParse(priceInput, out decimal price))
                product.Price = price;

            Console.Write("Ny lagermängd (tryck bara enter om du vill behålla den gamla): ");
            var stockInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(stockInput) && int.TryParse(stockInput, out int stock))
                product.Stock = stock;

            Console.Write("Ny kategori (tryck bara enter om du vill behålla den gamla): ");
            var category = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(category))
                product.Category = category;

            product.LastUpdated = DateTime.Now;  // Håller koll på när saker ändras
            Console.WriteLine("Produkt uppdaterad! Stäng fortfarande inte av datorn!");
        }

        // Ta bort en produkt (använd med försiktighet!)
        private static void DeleteProduct()
        {
            Console.Write("Ange produkt-ID att ta bort (dubbel-check att det är rätt, går inte att ångra!): ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ogiltigt ID! Bara siffror är tillåtna här.");
                return;
            }

            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                Console.WriteLine("Produkt hittades inte! Puh, inget blev raderat av misstag!");
                return;
            }

            _products.Remove(product);
            Console.WriteLine("Produkt borttagen! (Hoppas det var meningen)");
        }

        // Sökfunktion - Min stolthet! Söker i både namn och kategori
        private static void SearchProducts()
        {
            Console.Write("Sök (namn eller kategori - versaler spelar ingen roll!): ");
            var searchTerm = Console.ReadLine().ToLower();

            // LINQ igen! Kollar både namn och kategori
            var results = _products.Where(p => 
                p.Name.ToLower().Contains(searchTerm) || 
                p.Category.ToLower().Contains(searchTerm)
            ).ToList();

            if (!results.Any())
            {
                Console.WriteLine("Inga produkter matchade sökningen. Prova med något annat!");
                return;
            }

            foreach (var product in results)
            {
                DisplayProduct(product);
            }
        }

        // Visar en enskild produkt snyggt formaterat
        private static void DisplayProduct(Product product)
        {
            // Snygga streck som separerar produkterna
            Console.WriteLine($"\nID: {product.Id}");
            Console.WriteLine($"Namn: {product.Name}");
            Console.WriteLine($"Pris: {product.Price:C}");  // :C gör att det blir kronor automatiskt!
            Console.WriteLine($"Lager: {product.Stock}");
            Console.WriteLine($"Kategori: {product.Category}");
            Console.WriteLine($"Skapad: {product.Created}");
            if (product.LastUpdated.HasValue)  // Kollar om produkten har uppdaterats någon gång
                Console.WriteLine($"Senast uppdaterad: {product.LastUpdated}");
            Console.WriteLine(new string('-', 40));  // Snyggt streck mellan produkterna
        }
    }
}