namespace ComplexValidation.Configuration.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Cinch;
    using Model;
    using Supporters;

    public class FieldViewModel : FullyFledgedViewModel
    {
        private readonly string name;
        private static readonly PropertyChangedEventArgs NamePropertyChangeArgs = new PropertyChangedEventArgs("Name");
        private static readonly PropertyChangedEventArgs DescriptionPropertyChangeArgs = new PropertyChangedEventArgs("Description");

        private const string Required = "Requerido";
        private const string InvalidRange = "Rango no válido";


        public FieldViewModel(string name)
        {
            this.name = name;
            if (name == null)
            {
                throw new ArgumentNullException(name);
            }

            DisplayName = "el Campo";

            SetupDataWrappers();
            SetupFixedData();
        }

        private void SetupFixedData()
        {
            Angles = FixedData.Angles;
            FieldTypes = FixedData.FieldTypes;
        }

        private void SetupDataWrappers()
        {
            Name = new DataWrapper<string>(this, NamePropertyChangeArgs) { DataValue = name };
            Name.AddRule(DataWrapperRules.NotNullOrEmtpyRule(Required));

            Description = new DataWrapper<string>(this, DescriptionPropertyChangeArgs) { DataValue = string.Empty };

            IsActive = new DataWrapper<bool>(this, new PropertyChangedEventArgs("IsActive"));
            IsRequired = new DataWrapper<bool>(this, new PropertyChangedEventArgs("IsRequired"));

            SelectedFieldType = new DataWrapper<FieldType>(this, new PropertyChangedEventArgs("SelectedFieldType"));
            SelectedFieldType.PropertyChanged += (sender, args) => ResetMinAndMax();


            Mask = new DataWrapper<string>(this, new PropertyChangedEventArgs("Mask"));
            Min = new DataWrapper<int?>(this, new PropertyChangedEventArgs("Min"));
            Max = new DataWrapper<int?>(this, new PropertyChangedEventArgs("Max"));

            Min.AddRule(DataWrapperRules.MinMax(Min, Max, InvalidRange));
            Max.AddRule(DataWrapperRules.MinMax(Min, Max, InvalidRange));

            Max.PropertyChanged += UpdateMinMaxState;
            Min.PropertyChanged += UpdateMinMaxState;


            FixedValue = new DataWrapper<string>(this, new PropertyChangedEventArgs("FixedValue"));

            ValidChars = new DataWrapper<string>(this, new PropertyChangedEventArgs("ValidChars"));
            SelectedAngle = new DataWrapper<int>(this, new PropertyChangedEventArgs("SelectedAngle")) { DataValue = 0 };

            SubscribeToChangesInAllDataWrappers();
        }

        private void ResetMinAndMax()
        {
            Min.DataValue = null;
            Max.DataValue = null;
        }

        private void UpdateMinMaxState(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Max");
            NotifyPropertyChanged("Min");
        }

        public DataWrapper<string> Name { get; set; }

        public DataWrapper<string> Description { get; set; }

        public DataWrapper<bool> IsActive { get; set; }

        public DataWrapper<bool> IsRequired { get; set; }

        public IEnumerable<FieldType> FieldTypes { get; set; }

        public DataWrapper<FieldType> SelectedFieldType { get; set; }

        public DataWrapper<string> Mask { get; set; }

        public DataWrapper<int?> Min { get; set; }

        public DataWrapper<int?> Max { get; set; }

        public DataWrapper<string> FixedValue { get; set; }

        public DataWrapper<string> ValidChars { get; set; }

        public IEnumerable<int> Angles { get; set; }

        public DataWrapper<int> SelectedAngle { get; set; }
    }
}