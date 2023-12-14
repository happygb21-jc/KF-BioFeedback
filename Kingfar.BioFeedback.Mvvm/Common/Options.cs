using System;

namespace Kingfar.BioFeedback.Mvvm
{
    [Serializable]
    public class TrainTypeOption : ObservableObject
    {
        public string Name { get; set; } = string.Empty;
        public TrainType Type { get; set; }
        private bool _isSelected = false;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public TrainTypeOption(string name, TrainType type)
        {
            Name = name;
            Type = type;
        }
    }

    [Serializable]
    public class TrainApplicationTypeOption : ObservableObject
    {
        public string Name { get; set; } = string.Empty;
        public TrainApplicationType Type { get; set; }
        private bool _isSelected = false;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public TrainApplicationTypeOption(string name, TrainApplicationType type)
        {
            Name = name;
            Type = type;
        }
    }
}