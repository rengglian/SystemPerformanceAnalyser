using Prism.Mvvm;

namespace SystemPerformanceAnalyser.Model
{
    public class CheckableItem<T> : BindableBase
    {
        private T _value;
        private bool _isCheckedLeftAxis;
        private bool _isCheckedRightAxis;
        private bool _enableLeftAxis = true;
        private bool _enableRightAxis = true;

        public CheckableItem(T value)
        {
            _value = value;
        }

        public T Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public bool IsCheckedLeftAxis
        {
            get => _isCheckedLeftAxis;
            set
            {
                SetProperty(ref _isCheckedLeftAxis, value);
                EnableRightAxis = !_isCheckedLeftAxis;
            }
        }

        public bool IsCheckedRightAxis
        {
            get => _isCheckedRightAxis;
            set
            {
                SetProperty(ref _isCheckedRightAxis, value);
                EnableLeftAxis = !_isCheckedRightAxis;
            }
        }

        public bool EnableLeftAxis
        {
            get { return _enableLeftAxis; }
            set { SetProperty(ref _enableLeftAxis, value); }
        }

        public bool EnableRightAxis
        {
            get { return _enableRightAxis; }
            set { SetProperty(ref _enableRightAxis, value); }
        }
    }
}
