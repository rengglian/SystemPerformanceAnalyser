using Prism.Mvvm;

namespace SystemPerformanceAnalyser.Model
{
    public class CheckableItem<T> : BindableBase
    {
        private T _value;
        private bool _enableLeftAxis;
        private bool _enableRightAxis;
        public CheckableItem(T value)
        {
            _value = value;
        }

        public T Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public bool EnableLeftAxis
        {
            get => _enableLeftAxis;
            set => SetProperty(ref _enableLeftAxis, value);
        }
        public bool EnableRightAxis
        {
            get => _enableRightAxis;
            set => SetProperty(ref _enableRightAxis, value);
        }
    }
}
