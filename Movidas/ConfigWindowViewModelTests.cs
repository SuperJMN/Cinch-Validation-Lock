namespace ComplexValidation.Tests
{
    using System.Linq;
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
}