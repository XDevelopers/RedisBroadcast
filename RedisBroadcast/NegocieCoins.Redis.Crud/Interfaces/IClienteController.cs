using System.Collections.Generic;

namespace NegocieCoins.Redis.Crud.Interfaces
{
    public interface IClienteController
    {
        void InserirCliente(Cliente cliente);

        Cliente SearchCliente(string email);

        void DeleteCliente(string email);

        IEnumerable<Cliente> GetAllClientes();
    }
}