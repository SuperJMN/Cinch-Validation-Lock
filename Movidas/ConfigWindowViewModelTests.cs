namespace Movidas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ComplexValidation.Configuration.Model;
    using ComplexValidation.Configuration.ViewModel;
    using Xunit;

    public class ConfigWindowViewModelTests
    {
        [Fact]
        public void WithNameOnly_IsInvalid()
        {
            var sut = new ConfigWindowViewModel(new LomoConfigServiceMock(SampleData.WithNameAndNoSelectedCustomer), null, null);
            Assert.False(sut.IsValid);
        }

        [Fact]
        public void WithNameAndCustomer_IsValid()
        {
            var sut = new ConfigWindowViewModel(new LomoConfigServiceMock(SampleData.UniqueConfigThatIsValid), null, null);
            Assert.True(sut.IsValid);
        }

        [Fact]
        public void ModifiedConfig_CanSave()
        {
            var sut = ModifiedSut();

            Assert.True(sut.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ModifiedConfig_CanDiscardChanges()
        {
            var sut = ModifiedSut();

            Assert.True(sut.DiscardCommand.CanExecute(null));
        }

        [Fact]
        public void CannotDiscardTwice()
        {
            var sut = ModifiedSut();

            sut.DiscardCommand.Execute(null);

            Assert.False(sut.DiscardCommand.CanExecute(null));
        }

        [Fact]
        public void CannotSaveTwice()
        {
            var sut = ModifiedSut();

            sut.SaveCommand.Execute(null);

            Assert.False(sut.DiscardCommand.CanExecute(null));
        }

        [Fact]
        public void AfterDiscard_CannotSave()
        {
            var sut = ModifiedSut();

            sut.DiscardCommand.Execute(null);

            Assert.False(sut.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void GivenUnsavedChanges_DiscardingChanges_ConfigsAreEmpty()
        {
            var sut = new ConfigWindowViewModel(new LomoConfigServiceMock(SampleData.UnsavedUniqueConfigThatIsValid), null, null);

            sut.DiscardCommand.Execute(null);

            Assert.Empty(sut.Configs);
        }

        private static ConfigWindowViewModel ModifiedSut()
        {
            var sut = new ConfigWindowViewModel(new LomoConfigServiceMock(SampleData.UniqueConfigThatIsValid), null, null);
            ModifySomething(sut);
            return sut;
        }

        private static void ModifySomething(ConfigWindowViewModel v)
        {
            var config = v.Configs.First();
            config.Name.DataValue = "Changed";
        }
    }


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

    public static class SampleData
    {
        public static IEnumerable<LomoConfig> UniqueConfigThatIsValid
        {
            get
            {
                return new List<LomoConfig>
                {
                    new LomoConfig
                    {
                        Name = "Hola",
                        Id = 1,
                        Customer = new Customer("CustomerName", 1),
                        ImagePath = "C:\\",
                        BoxCount = 1,
                    }
                };
            }
        }

        public static IEnumerable<LomoConfig> UnsavedUniqueConfigThatIsValid
        {
            get
            {
                return new List<LomoConfig>
                {
                    new LomoConfig
                    {
                        Name = "Hola",
                        Id = null,
                        Customer = new Customer("CustomerName", 1),
                        ImagePath = "C:\\",
                        BoxCount = 1,
                    }
                };
            }
        }

        public static IEnumerable<LomoConfig> WithNameAndNoSelectedCustomer
        {
            get
            {
                return new List<LomoConfig>() { new LomoConfig()
                {
                    Name = "Hola",
                    Id = 1,
                } };
            }
        }
    }
}