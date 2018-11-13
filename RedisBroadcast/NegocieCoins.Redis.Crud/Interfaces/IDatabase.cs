using System.Collections.Generic;

namespace NegocieCoins.Redis.Crud.Data
{
    public interface IDatabase
    {
        void Insert(Cliente cliente);

        Cliente Search(string email);

        void Delete(string email);

        IEnumerable<Cliente> GetAll();
    }
}