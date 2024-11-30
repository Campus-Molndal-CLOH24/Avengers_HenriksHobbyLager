using HenriksHobbyLager.Interfaces;
using HenriksHobbyLager.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HenriksHobbyLager.Database
{
    public class SqliteRepository : IRepository<Product>
    {
        private readonly string _connectionString;

        public SqliteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Product> GetAll()
        {
            var products = new List<Product>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Products";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var product = new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    Stock = reader.GetInt32(3),
                    Category = reader.GetString(4),
                    Created = reader.GetDateTime(5),
                    LastUpdated = reader.IsDBNull(6) ? null : reader.GetDateTime(6)
                };
                products.Add(product);
            }

            return products;

        }
        public Product GetById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Products WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    Stock = reader.GetInt32(3),
                    Category = reader.GetString(4),
                    Created = reader.GetDateTime(5),
                    LastUpdated = reader.IsDBNull(6) ? null : reader.GetDateTime(6)
                };
            }
            return null;
        }
        public void Add(Product entity)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "CREATE TABLE IF NOT EXISTS Products (Id INTEGER PRIMARY KEY, Name TEXT, Price DECIMAL, Stock INTEGER, Category TEXT, Created TEXT, LastUpdated TEXT)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO Products (Name, Price, Stock, Category, Created) VALUES (@name, @price, @stock, @category, @created)";
            command.Parameters.AddWithValue("@name", entity.Name);
            command.Parameters.AddWithValue("@price", entity.Price);
            command.Parameters.AddWithValue("@stock", entity.Stock);
            command.Parameters.AddWithValue("@category", entity.Category);
            command.Parameters.AddWithValue("@created", entity.Created);
            command.ExecuteNonQuery();
            connection.Close();

        }
        public void Update(Product entity)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "UPDATE Products SET Name = @name, Price = @price, Stock = @stock, Category = @category, LastUpdated = @lastUpdated WHERE Id = @id";
            command.Parameters.AddWithValue("@name", entity.Name);
            command.Parameters.AddWithValue("@price", entity.Price);
            command.Parameters.AddWithValue("@stock", entity.Stock);
            command.Parameters.AddWithValue("@category", entity.Category);
            command.Parameters.AddWithValue("@lastUpdated", entity.LastUpdated);
            command.Parameters.AddWithValue("@id", entity.Id);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void Delete(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Products WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public IEnumerable<Product> Search(Func<Product, bool> predicate)
        {
            return GetAll().Where(predicate);
        }
    }
}
