using Microsoft.EntityFrameworkCore;
using Social_network.DAL;
using Xunit;

namespace Social_network.DAL.Tests
{
    public class RealDatabaseTestsConnect
    {
        // Используем реальную строку подключения
        private const string ConnectionString =
            "Server=ZOOM712\\SQLEXPRESS01;Database=Social_Network;Trusted_Connection=True;TrustServerCertificate=True";
        [Fact]
        public async Task Can_Connect_To_Real_Database()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ContextSocial_Network_Context>()
                .UseSqlServer(ConnectionString) 
                .Options;

            // Act
            using var context = new ContextSocial_Network_Context(options);
            var canConnect = await context.Database.CanConnectAsync();

            // Assert
            Assert.True(canConnect, "Не могу подключиться к базе данных!");
        }
    }
}