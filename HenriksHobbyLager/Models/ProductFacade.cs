using HenriksHobbyLager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HenriksHobbyLager.Models
{
    public class ProductFacade : IProductFacade
    {
        private readonly IRepository<Product> _repository;

        public ProductFacade(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public void CreateProduct(Product product) => _repository.Add(product);

        public void DeleteProduct(int id) => _repository.Delete(id);

        public IEnumerable<Product> GetAllProducts() => _repository.GetAll();

        public Product GetProduct(int id) => _repository.GetById(id);

        public IEnumerable<Product> SearchProducts(string searchTerm) => _repository.Search(p => p.Name.Contains(searchTerm));

        public void UpdateProduct(Product product) => _repository.Update(product);
    }
}
