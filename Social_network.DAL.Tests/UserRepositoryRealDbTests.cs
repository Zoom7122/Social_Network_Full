using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Social_network.DAL.Models;
using Social_network.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Social_network.DAL.Tests
{
    public class UserRepositoryRealDbTests
    {
        private const string REAL_DB_CONNECTION =
           "Server=ZOOM712\\SQLEXPRESS01;Database=Social_Network;Trusted_Connection=True;TrustServerCertificate=True";

        [Fact]
        public async Task RegisterUser_Should_Add_User_To_Real_Database()
        {
            Random rnd = new Random();

            var options = new DbContextOptionsBuilder<ContextSocial_Network_Context>().UseSqlServer(REAL_DB_CONNECTION).Options;

            using var context = new ContextSocial_Network_Context(options);
            var repo = new UserRepo(context);

            var user = new User
            {
                FirstName = "Тест",
                LastName = "Тестов",
                BirthDate = DateTime.Now,
                password = "12345678",
                Email =rnd.Next(1,1000).ToString() + "@email.ru"
            };

            await repo.RegisterUser(user);

            Console.WriteLine($"ID нового пользователя: {user.Id}");

            Assert.NotNull(user.Id);

        }

        [Theory]
        [InlineData("B9ED54FF-077E-4F2E-AC8D-47BFDC19A549")]
        [InlineData("4F3AECC2-94F4-4F67-AE28-BBB2B98513D8")]
        public async Task CanGetUserById(Guid id)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            var opption = new DbContextOptionsBuilder<ContextSocial_Network_Context>().UseSqlServer(REAL_DB_CONNECTION).Options;

            using var context = new ContextSocial_Network_Context(opption);
            var repo = new UserRepo(context);

            var user = await repo.GetUserById(id);
            Console.WriteLine(user.FirstName + " " + user.LastName);
            Assert.NotNull(user);
        }

        [Fact]
        public async Task CanGerAllUsers()
        {
            var opption = new DbContextOptionsBuilder<ContextSocial_Network_Context>().UseSqlServer(REAL_DB_CONNECTION).Options;

            using var context = new ContextSocial_Network_Context(opption);
            var repo = new UserRepo(context);

            var users = await repo.GetAllUsers();

            Assert.NotNull(users);
        }

        [Theory]
        [InlineData("5593B9A6-64D0-4635-8986-0047266A8BF3", "2D6357CF-295B-47C5-8579-5F92F71F58B5")]
        [InlineData("2D6357CF-295B-47C5-8579-5F92F71F58B5", "5593B9A6-64D0-4635-8986-0047266A8BF3")]
        public async Task CanAddToFriends(Guid main_ID, Guid toAdd_ID)
        {
            var opption = new DbContextOptionsBuilder<ContextSocial_Network_Context>().UseSqlServer(REAL_DB_CONNECTION).Options;

            using var context = new ContextSocial_Network_Context(opption);
            var repo1 = new UserRepo(context);
            var repo2 = new PersonalFunction(repo1,context);

            var answer = await repo2.AddToFriends(main_ID, toAdd_ID);

            Assert.True(answer);
        }
    }
}
