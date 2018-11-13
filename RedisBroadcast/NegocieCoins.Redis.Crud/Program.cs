using MarkdownLog;
using NegocieCoins.Redis.Crud.Controllers;
using NegocieCoins.Redis.Crud.Data;
using NegocieCoins.Redis.Crud.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegocieCoins.Redis.Crud
{
    public class Program
    {
        private static IClienteController controller = new ClienteController(new Database());

        static void Main(string[] args)
        {
            Console.WriteLine("[POC] Negocie-Coins - Redis Broadcast");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------");
            PrintMenu();

            Console.WriteLine("[INFO] Programa finalizado, tecle ENTER para sair");
            Console.ReadKey();
        }

        public static void PrintMenu()
        {
            Console.WriteLine(" ");
            Console.WriteLine("Selecione uma opção abaixo:");
            Console.WriteLine("1 - Inserir novo cliente");
            //Console.WriteLine("2 - Atualizar cliente");
            Console.WriteLine("3 - Deletar cliente");
            Console.WriteLine("4 - Exibir todos os clientes");
            Console.WriteLine("5 - Fechar Programa");

            try
            {
                var opcao = Console.ReadLine();
                switch (opcao)
                {
                    case "1":
                        var cliente = new Cliente();
                        Console.WriteLine("---------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("Informe Nome do cliente:");
                        cliente.Nome = Console.ReadLine();

                        Console.WriteLine("Informe Email do cliente:");
                        cliente.Email = Console.ReadLine();

                        controller.InserirCliente(cliente);
                        PrintReport(controller.GetAllClientes());
                        PrintMenu();
                        break;
                    case "2":
                        Console.WriteLine("---------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("Informe Email do cliente:");
                        var email = Console.ReadLine();
                        var cliente1 = controller.SearchCliente(email);
                        PrintReport(new List<Cliente> { cliente1 });
                        PrintMenu();
                        break;
                    case "3":
                        Console.WriteLine("---------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("Informe Email do cliente:");
                        var email1 = Console.ReadLine();
                        controller.DeleteCliente(email1);
                        PrintReport(controller.GetAllClientes());
                        PrintMenu();
                        break;
                    case "4":
                        Console.WriteLine("---------------------------------------------------------------------------------------------------------");
                        PrintReport(controller.GetAllClientes());
                        PrintMenu();
                        break;
                    case "5":
                        break;
                    default:
                        Console.WriteLine("ERRO: Opção inválida");
                        PrintMenu();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERRO: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
                PrintMenu();
            }
        }

        static void PrintReport(IEnumerable<Cliente> list)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" ");
            Console.WriteLine
            (

                 list.Select(s => new
                 {
                     Nome = s.Nome,
                     Email = s.Email
                 })
                 .ToMarkdownTable()
            );
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}