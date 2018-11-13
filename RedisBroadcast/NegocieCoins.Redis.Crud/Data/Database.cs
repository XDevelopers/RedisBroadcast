using NegocieCoins.Redis.Lib;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NegocieCoins.Redis.Crud.Data
{
    public class Database : IDatabase
    {
        private StackExchange.Redis.IDatabase Redis = RedisStore.RedisCache;

        //private ISubscriber Subscriber = RedisStore.RedisCache.Multiplexer.GetSubscriber();

        private ISubscriber Publisher = RedisStore.RedisCache.Multiplexer.GetSubscriber();

        private string DefaultChannel = ConfigurationManager.AppSettings["defaultCollection"].ToString();

        public void Delete(string email)
        {
            try
            {
                var clienteKey = $"cliente:{email}";

                Redis.SetRemove(clienteKey, Redis.StringGet(clienteKey));

                // O Publisher dispara uma mensagem via Redis avisando que ocorreu algo!
                Publisher.Publish(DefaultChannel, $"O cliente com email: {email} foi removido!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Cliente Search(string email)
        {
            try
            {
                var clienteKey = $"cliente:{email}";

                // O Publisher dispara uma mensagem via Redis avisando que ocorreu algo!
                Publisher.Publish(DefaultChannel, $"O cliente com email: {email} foi pesquisado!");

                return JsonConvert.DeserializeObject<Cliente>(Redis.StringGet(clienteKey));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Cliente> GetAll()
        {
            try
            {
                var clienteKey = $"cliente";
                var result = new List<Cliente>();

                var redisResult = Redis.StringGet(DefaultChannel);
                if (!redisResult.IsNullOrEmpty)
                {
                    result = JsonConvert.DeserializeObject<List<Cliente>>($"[{redisResult.ToString().Replace("}", "},")}]");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Insert(Cliente cliente)
        {
            try
            {
                var json = JsonConvert.SerializeObject(cliente);
                Redis.StringAppend(DefaultChannel, json);

                // O Publisher dispara uma mensagem via Redis avisando que ocorreu algo!
                Publisher.Publish(DefaultChannel, $"O cliente com email: {cliente.Email} foi inserido!");
                var payload = new {
                    datetime = DateTime.Now,
                    room = DefaultChannel,
                    message = "Data Inserted!",
                    body = json
                };
                Publisher.Publish(DefaultChannel, JsonConvert.SerializeObject(payload));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}