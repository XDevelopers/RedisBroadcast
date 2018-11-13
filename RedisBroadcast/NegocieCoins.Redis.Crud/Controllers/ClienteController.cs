using NegocieCoins.Redis.Crud.Data;
using NegocieCoins.Redis.Crud.Interfaces;
using System;
using System.Collections.Generic;

namespace NegocieCoins.Redis.Crud.Controllers
{
    public class ClienteController : IClienteController
    {
        public ClienteController(IDatabase database)
        {
            if (database == null)
                throw new Exception("Banco não informado");

            Database = database;
        }

        public IDatabase Database { get; set; }

        public void InserirCliente(Cliente cliente)
        {
            if (cliente == null)
                throw new Exception("Cliente não informado.");

            if (string.IsNullOrEmpty(cliente.Email))
                throw new Exception("Email não informado");

            if (IsValidEmail(cliente.Email) == false)
                throw new Exception("Email inválido");

            if (cliente.Nome.Length > 50)
                throw new Exception("Nome inválido");

            Database.Insert(cliente);
        }

        public Cliente SearchCliente(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new Exception("Email não informado");

            return Database.Search(email);
        }

        public IEnumerable<Cliente> GetAllClientes()
        {
            return Database.GetAll();
        }

        public void DeleteCliente(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new Exception("Email não informado");

            var cliente = Database.Search(email);
            if (cliente == null)
                throw new Exception("Cliente não encontrado");

            Database.Delete(email);
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}