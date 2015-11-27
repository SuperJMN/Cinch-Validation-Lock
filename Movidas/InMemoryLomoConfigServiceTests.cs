namespace ComplexValidation.Tests
{
    using System;
    using Configuration.Model;
    using Xunit;

    public class InMemoryLomoConfigServiceTests
    {
        [Fact]
        public void Add()
        {
            var sut = new InMemoryLomoConfigService();
            var expectedId = sut.Add(new LomoConfig());
            var entity = sut.Get(expectedId);

            Assert.Equal(expectedId, entity.Id);
        }

        [Fact]
        public void AddWithIdSetPreviously_Throws()
        {
            var sut = new InMemoryLomoConfigService();
            
            Assert.Throws<InvalidOperationException>(() => sut.Add(new LomoConfig() { Id = 10 }));
        }

        [Fact]
        public void Delete()
        {
            var sut = new InMemoryLomoConfigService();
            var idToDelete = sut.Add(new LomoConfig());
            sut.Delete(idToDelete);

            Assert.Throws<InvalidOperationException>(() => sut.Get(idToDelete));
        }

        [Fact]
        public void Update()
        {
            var sut = new InMemoryLomoConfigService();
            var id = sut.Add(new LomoConfig());

            var expected = new LomoConfig
            {
                Name = "New",
                Id = id
            };

            sut.Update(expected);
            var actual = sut.Get(id);

            Assert.Equal(expected, actual);
        }
    }
}