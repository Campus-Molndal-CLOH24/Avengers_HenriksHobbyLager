using HenriksHobbyLager.Models;
using RefactoringExercise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HenriksHobbyLager.Interfaces
{
    public interface IProductFacade
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProduct(int id);
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        IEnumerable<Product> SearchProducts(string searchTerm);
    }
}
