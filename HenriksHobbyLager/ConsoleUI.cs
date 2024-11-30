using HenriksHobbyLager.Interfaces;
using HenriksHobbyLager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HenriksHobbyLager
{
    public class ConsoleUI
    {
        private readonly IProductFacade _productFacade;
        public ConsoleUI(IProductFacade productFacade)
        {
            _productFacade = productFacade;
        }

        public void Start()
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
                Console.WriteLine("6. Avsluta");// Använd inte denna om du vill behålla datan!

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

        private void SearchProducts()
        {
            Console.Write("Ange sökterm (namn eller kategori): ");
            var searchTerm = Console.ReadLine();

            var results = _productFacade.SearchProducts(searchTerm);

            if (!results.Any())
            {
                Console.WriteLine("Inga produkter matchade din sökning.");
                return;
            }

            foreach (var product in results)
            {
                DisplayProduct(product);
            }
        }

        private void DeleteProduct()
        {
            Console.Write("Ange produkt-ID att ta bort: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ogiltigt ID. Endast siffror är tillåtna.");
                return;
            }

            var product = _productFacade.GetProduct(id);
            if (product == null)
            {
                Console.WriteLine("Produkten hittades inte.");
                return;
            }

            Console.Write($"Är du säker på att du vill ta bort produkten \"{product.Name}\"? (y/n): ");
            var confirmation = Console.ReadLine();
            if (confirmation?.ToLower() == "y")
            {
                _productFacade.DeleteProduct(id);
                Console.WriteLine("Produkten har tagits bort.");
            }
            else
            {
                Console.WriteLine("Åtgärden avbröts.");
            }
        }

        private void UpdateProduct()
        {
            Console.Write("Ange produkt-ID att uppdatera: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ogiltigt ID. Endast siffror är tillåtna.");
                return;
            }

            var product = _productFacade.GetProduct(id);
            if (product == null)
            {
                Console.WriteLine("Produkten hittades inte.");
                return;
            }

            Console.WriteLine("Lämna fält tomt om du inte vill ändra värdet.");

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

            product.LastUpdated = DateTime.Now;
            _productFacade.UpdateProduct(product);

            Console.WriteLine("Produkten har uppdaterats!");
        }

        private void AddProduct()
        {
            Console.WriteLine("=== Lägg till ny produkt ===");

            Console.Write("Namn: ");
            var name = Console.ReadLine();

            Console.Write("Pris: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Ogiltigt pris! Använd punkt istället för komma.");
                return;
            }

            Console.Write("Antal i lager: ");
            if (!int.TryParse(Console.ReadLine(), out int stock))
            {
                Console.WriteLine("Ogiltig lagermängd! Endast heltal är tillåtna.");
                return;
            }

            Console.Write("Kategori: ");
            var category = Console.ReadLine();

            var product = new Product
            {
                Name = name,
                Price = price,
                Stock = stock,
                Category = category,
                Created = DateTime.Now
            };

            _productFacade.CreateProduct(product);
            Console.WriteLine("Produkten har lagts till!");
        }

        private void ShowAllProducts()
        {
            var products = _productFacade.GetAllProducts();
            if (!products.Any())
            {
                Console.WriteLine("Inga produkter finns i lagret. Dags att shoppa grossist!");
                return;
            }
            foreach (var product in products)
            {
                DisplayProduct(product);
            }
            }

        private void DisplayProduct(Product product)
        {
            Console.WriteLine($"Id: {product.Id}");
            Console.WriteLine($"Namn: {product.Name}");
            Console.WriteLine($"Pris: {product.Price:C}");
            Console.WriteLine($"Lager: {product.Stock} st");
            Console.WriteLine($"Kategori: {product.Category}");
            Console.WriteLine($"Skapad: {product.Created}");
            if(product.LastUpdated.HasValue)
                Console.WriteLine($"Senast uppdaterad: {product.LastUpdated}");
            Console.WriteLine();
        }
    }
}
