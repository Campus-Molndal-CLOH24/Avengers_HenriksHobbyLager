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
using HenriksHobbyLager;
using HenriksHobbyLager.Database;
using HenriksHobbyLager.Interfaces;
using HenriksHobbyLager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RefactoringExercise
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Data Source = HenriksLager.db";
            var repository = new SqliteRepository(connectionString);
            var facade = new ProductFacade(repository);
            var ui = new ConsoleUI(facade);
            ui.Start();
        }
    }
}