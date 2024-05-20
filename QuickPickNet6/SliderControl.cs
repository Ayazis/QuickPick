using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuickPick
{
    public class SliderControl
    {
        readonly IInputElement _inputElement;
        bool _buttonDown;
        ProgressBar _progressBar;
        Point _previousPosition;
        private double _percentage;

        public SliderControl(IInputElement inputElement, ProgressBar progressBar)
        {
            _inputElement = inputElement;
            _progressBar = progressBar;
        }

        public void AttachToButtonEvents()
        {
            this._inputElement.PreviewMouseLeftButtonDown += Button_MouseDown;
            this._inputElement.PreviewMouseLeftButtonUp += Button_MouseUp;
            this._inputElement.PreviewMouseMove += Button_MouseMove;
        }
        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _buttonDown = true;
            _progressBar.Visibility = Visibility.Visible;
            _previousPosition = e.GetPosition(_inputElement);
            // Capture the mouse
            ((dynamic)sender).CaptureMouse();
        }

        private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _progressBar.Visibility = Visibility.Collapsed;
            _buttonDown = false;
            // Release the mouse
            ((dynamic)sender).ReleaseMouseCapture();
        }
        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            if (_buttonDown)
            {
                // get current mousePosition
                Point position = e.GetPosition(_inputElement);
                // compare with previousposition, calculate vertical distance:
                var pointDifference = -(position.Y - _previousPosition.Y);
                _percentage += pointDifference;

                if (_percentage > 100)
                    _percentage = 100;
                if (_percentage < 50)
                    _percentage = 50;

                _progressBar.Value = _percentage;
                ValueChanged?.Invoke(_progressBar.Value);
                _previousPosition = position;
            }
        }

        public delegate void IntValueChangedEventHandler(double value);
        public event IntValueChangedEventHandler ValueChanged;
    }
}
