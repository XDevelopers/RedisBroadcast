using NegocieCoins.Redis.Lib;
using System;
using System.Configuration;

namespace RedisBroadcast.App
{
    class Program
    {
        static void Main(string[] args)
        {
            // defaultCollection == negocie-coins
            var ncCollection = ConfigurationManager.AppSettings["defaultCollection"].ToString();

            // Sample code related to PubSub using C#
            // Sample code from: http://taswar.zeytinsoft.com/redis-pub-sub/
            var redis = RedisStore.RedisCache;

            var sub = redis.Multiplexer.GetSubscriber();

            // M.L. Aqui é criado o Primeiro Subscriber que ficará aguardando mensagens do Redis, ou que atendam ao Canal solicitado
            sub.Subscribe(ncCollection, (channel, message) => {
                Console.WriteLine($"Ouvindo o Canal:{channel}, recebi:\n {message}\n");
            });

            // M.L. Criado aqui o Publisher, que poderá ser usado nesse contexto para Publicar mensagens...
            var pub = redis.Multiplexer.GetSubscriber();

            // M.L. Aqui o Publisher recém criado manda uma mensagem para o Subcriber também recém criado...
            var count = pub.Publish(ncCollection, $"Hello guys I'm a [ {ncCollection} ] message");

            // M.L. Exibe a quantidade de Listeners(ouvintes) para o Subscriber recém criado...
            Console.WriteLine($"Number of listeners for [ {ncCollection} ] - {count}");

            // M.L. - Exemplos abaixo apenas mostram apenas que senão atender ao demais detalhes do canal, os novos Subs e Pubs não ouvirão ao mesmo canal...
            #region [ Outros Exemplos ]

            //// pattern match with a message
            //sub.Subscribe(new RedisChannel("a*c", RedisChannel.PatternMode.Pattern), (channel, message) => {
            //    Console.WriteLine($"Got pattern a*c notification: {message}");
            //});


            //count = pub.Publish("a*c", "Hello there I am a a*c message");
            //Console.WriteLine($"Number of listeners for a*c {count}");

            //pub.Publish("abc", "Hello there I am a abc message");
            //pub.Publish("a1234567890c", "Hello there I am a a1234567890c message");
            //pub.Publish("ab", "Hello I am a lost message"); //this mesage is never printed


            ////Never a pattern match with a message
            //sub.Subscribe(new RedisChannel("*123", RedisChannel.PatternMode.Literal), (channel, message) => {
            //    Console.WriteLine($"Got Literal pattern *123 notification: {message}");
            //});

            //pub.Publish("*123", "Hello there I am a *123 message");
            //pub.Publish("a123", "Hello there I am a a123 message"); //message is never received due to literal pattern


            ////Auto pattern match with a message
            //sub.Subscribe(new RedisChannel("zyx*", RedisChannel.PatternMode.Auto), (channel, message) => {
            //    Console.WriteLine($"Got Literal pattern zyx* notification: {message}");
            //});


            //pub.Publish("zyxabc", "Hello there I am a zyxabc message");
            //pub.Publish("zyx1234", "Hello there I am a zyxabc message");

            #endregion [ Outros Exemplos ]

            // Nenhuma Mensagem sendo Publicada, portanto apenas aguardando!
            sub.Subscribe(ncCollection, (channel, message) => {
                Console.WriteLine($"Estou ouvindo o canal: {channel} e recebi a mensagem:\n {message}\n");
            });

            sub.Subscribe("message-room", (channel, message) => {
                Console.WriteLine($"Canal: {channel} - Mensagem: {message}\n");
            });

            sub.Subscribe("message", (channel, message) => {
                Console.WriteLine($"Canal: {channel} - Mensagem: {message}\n");
            });

            // Exemplo de como negar a um Ouvinte acesso (unsubscribe)
            //sub.Unsubscribe("a*c");
            //count = pub.Publish("abc", "Hello there I am a abc message"); //no one listening anymore
            //Console.WriteLine($"Number of listeners for a*c {count}");

            Console.ReadKey();
        }
    }
}