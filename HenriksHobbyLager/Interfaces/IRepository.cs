using HenriksHobbyLager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HenriksHobbyLager.Interfaces
{
    public interface IRepository<Product>
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        void Add(Product entity);
        void Update(Product entity);
        void Delete(int id);
        IEnumerable<Product> Search(Func<Product, bool> predicate);
    }
}
