namespace ComplexValidation.Tests
{
    using System.Linq;
    using Configuration.ViewModel;
    using Xunit;

    public class ConfigWindowViewModelTests
    {
        [Fact]
        public void WithNameOnly_IsInvalid()
        {
            var sut = new ConfigWindowViewModel(new LomoConfigServiceMock(SampleData.WithNameAndNoSelectedCustomer), null, null, null);
            Assert.False(sut.IsValid);
        }

        [Fact]
        public void WithNameAndCustomer_IsValid()
        {
            var sut = new ConfigWindowViewModel(new LomoConfigServiceMock(SampleData.UniqueConfigThatIsValid), null, null, null);
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
            var sut = new ConfigWindowViewModel(new LomoConfigServiceMock(SampleData.UnsavedUniqueConfigThatIsValid), null, null, null);

            sut.DiscardCommand.Execute(null);

            Assert.Empty(sut.Configs);
        }

        [Fact]
        public void AfterAddingNewInvalidConfig_SaveIsDisabled()
        {
            var sut = new ConfigWindowViewModel(new LomoConfigServiceMock(), null, null, null);
            sut.AddCommand.Execute(null);

            Assert.False(sut.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void AfterAddingValidConfig_SaveIsEnabled()
        {
            var sut = new ConfigWindowViewModel(new LomoConfigServiceMock(), null, null, null);
            sut.AddCommand.Execute(null);
            var createdConfig = sut.Configs.First();
            FillWithValidData(createdConfig);

            Assert.True(sut.SaveCommand.CanExecute(null));
        }

        private void FillWithValidData(LomoConfigViewModel createdConfig)
        {
            createdConfig.BoxCount.DataValue = 1;
            createdConfig.ImagePath.DataValue = "SomePath";
            createdConfig.Name.DataValue = "SomeName";
            createdConfig.SelectedCustomer.DataValue = new CustomerViewModel { Id = 1, Name = "SomeCustomer" };
        }

        private static ConfigWindowViewModel ModifiedSut()
        {
            var sut = new ConfigWindowViewModel(new LomoConfigServiceMock(SampleData.UniqueConfigThatIsValid), null, null, null);
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