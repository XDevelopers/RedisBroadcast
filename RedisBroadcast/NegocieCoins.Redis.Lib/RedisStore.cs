using StackExchange.Redis;
using System;
using System.Configuration;

namespace NegocieCoins.Redis.Lib
{
    public class RedisStore
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

        static RedisStore()
        {
            // Pega algumas configurações do arquivo - App.Config
            var host = ConfigurationManager.AppSettings["host"].ToString();
            var port = ConfigurationManager.AppSettings["port"].ToString();
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { $"{host}:{port}" },
                AllowAdmin = true
            };

            var password = ConfigurationManager.AppSettings["auth"].ToString();
            if (!string.IsNullOrWhiteSpace(password))
            {
                configurationOptions.Password = password;
            }

            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
        }

        public static ConnectionMultiplexer Connection => LazyConnection.Value;

        public static IDatabase RedisCache => Connection.GetDatabase();
    }
}