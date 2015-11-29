namespace ComplexValidation.Tests
{
    using System.Collections.ObjectModel;
    using Configuration.Model;
    using Configuration.Model.RealPersistence;
    using Xunit;

    public class LomoConfigRepositoryTests
    {
        [Fact]
        public void TestGetAll()
        {
            var sut = new ConfigRepository(AppConfigConnectionFactory.CreateSicConnection());
            var allFields = sut.GetAll();
            Assert.NotEmpty(allFields);
        }

        [Fact]
        public void TestAdd()
        {
            var dbConnection = AppConfigConnectionFactory.CreateSicConnection();

            //using (dbConnection.BeginTransaction())
            {
                var sut = new ConfigRepository(dbConnection);
                var expected = new LomoConfig
                {
                    Name = "MyConfig",
                    Description = "Description",
                    Customer = new Customer("John", 0),
                    Fields = new Collection<Field>
                    {
                        new Field {Name = "Test1", Description = "Description1"},
                        new Field {Name = "Test2", Description = "Description2"},
                    }
                };

                var id = sut.Create(expected);
                expected.Id = id;

                var actual = sut.Get(id);

                Assert.Equal(expected, actual);
            }
        }
    }
}