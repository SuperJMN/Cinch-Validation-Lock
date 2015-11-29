namespace ComplexValidation.Tests
{
    using Configuration.Model;
    using Configuration.Model.RealPersistence;
    using Xunit;

    public class DefaultFieldsRepositoryTests
    {
        [Fact]
        public void TestGetAll()
        {
            var sut = new DefaultFieldsForNewConfigsRepository(AppConfigConnectionFactory.CreateSicConnection());
            var allFields = sut.GetAll();
            Assert.NotEmpty(allFields);
        }

        [Fact]
        public void TestAdd()
        {
            var dbConnection = AppConfigConnectionFactory.CreateSicConnection();

            using (dbConnection.BeginTransaction())
            {
                var sut = new DefaultFieldsForNewConfigsRepository(dbConnection);
                var expected = new Field { Name = "MyField", Description = "Description"};
                var id = sut.Add(expected);
                expected.Id = id;
                var actual = sut.Get(id);

                Assert.Equal(expected, actual);
            }
        }       
    }
}