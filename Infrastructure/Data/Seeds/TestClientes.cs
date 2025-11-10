using Core.Entities.Clientes;
using Infrastructure.Data.DbContext;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace Infrastructure.Data.Seeds
{
    public static class TestClientes
    {

        //---------------------------------------------------------------------------------------------------


        public static void SeedClientes(LVLgroupDbContext db, IWebHostEnvironment environment)
        {
            //Seed Clientes

            if (db.Clientes.Any()) return;

            var cli1 = new Cliente
            {
                Nome = "Diogo Costa",
                Email = "diogo.costa@gmail.com",
                NIF = "111222333",
                Telefone = "123456789",
                IBAN = null,
                Morada = "Porto"
            };
            DbClienteSeed(cli1, db);

            var cli2 = new Cliente
            {
                Nome = "José Sá",
                Email = "jose.sa@gmail.com",
                NIF = "222333444",
                Telefone = "234567890",
                IBAN = null,
                Morada = "Londres"
            };
            DbClienteSeed(cli2, db);

            var cli3 = new Cliente
            {
                Nome = "Rui Patrício",
                Email = "rui.patricio@gmail.com",
                NIF = "333444555",
                Telefone = "345678901",
                IBAN = null,
                Morada = "Roma"
            };
            DbClienteSeed(cli3, db);

            var cli4 = new Cliente
            {
                Nome = "Diogo Dalot",
                Email = "diogo.dalot@gmail.com",
                NIF = "444555666",
                Telefone = "456789012",
                IBAN = null,
                Morada = "Manchester"
            };
            DbClienteSeed(cli4, db);

            var cli5 = new Cliente
            {
                Nome = "João Cancelo",
                Email = "joao.cancelo@gmail.com",
                NIF = "555666777",
                Telefone = "567890123",
                IBAN = null,
                Morada = "Manchester"
            };
            DbClienteSeed(cli5, db);

            var cli6 = new Cliente
            {
                Nome = "Rúben Dias",
                Email = "ruben.dias@gmail.com",
                NIF = "666777888",
                Telefone = "678901234",
                IBAN = "000777666555444333222111000",
                Morada = "Manchester"
            };
            DbClienteSeed(cli6, db);

            var cli7 = new Cliente
            {
                Nome = "Danilo Pereira",
                Email = "danilo.pereira@gmail.com",
                NIF = "777888999",
                Telefone = "789012345",
                IBAN = null,
                Morada = "Paris"
            };
            DbClienteSeed(cli7, db);

            var cli8 = new Cliente
            {
                Nome = "António Silva",
                Email = "antonio.silva@gmail.com",
                NIF = "888999000",
                Telefone = "890123456",
                IBAN = "098098098098098098098098",
                Morada = "Lisboa"
            };
            DbClienteSeed(cli8, db);

            var cli9 = new Cliente
            {
                Nome = "Nuno Mendes",
                Email = "nuno.mendes@gmail.com",
                NIF = "999000111",
                Telefone = "901234567",
                IBAN = "11113333555577779999",
                Morada = "Paris"
            };
            DbClienteSeed(cli9, db);

            var cli10 = new Cliente
            {
                Nome = "Raphaël Guerreiro",
                Email = "rafael.guerreiro@gmail.com",
                NIF = "000111222",
                Telefone = "012345678",
                IBAN = "22224444666688880000",
                Morada = "Dortmund"
            };
            DbClienteSeed(cli10, db);
        }


        //---------------------------------------------------------------------------------------------------


        private static int DbClienteSeed(Cliente cliente, LVLgroupDbContext db)
        {
            db.Clientes.Add(cliente);
            db.SaveChanges();
            return cliente.Id;
        }



        //---------------------------------------------------------------------------------------------------

    }
}
