namespace DoorService
{
    using System;
    using Windows.Devices.Gpio;

    internal class PressButtonEventHandler
    {
        public delegate void ButtonPressedEventHandler(object sender, EventArgs e);

        private const int BUTTON_PIN = 16;
        private GpioPin _pin;

        public PressButtonEventHandler()
        {
            InitGpio();

        }

        public event ButtonPressedEventHandler ButtonPressed;

        protected void RaiseButtonPressed()
        {
            if (ButtonPressed != null)
            {
                ButtonPressed(this, new EventArgs());
            }
        }

        private void InitGpio()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                throw new InvalidOperationException("There is no GPIO controller on this device.");
            }

            _pin = gpio.OpenPin(BUTTON_PIN);
            _pin.SetDriveMode(GpioPinDriveMode.Input);
            _pin.ValueChanged += PinValueChanged;
        }

        private void PinValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge == GpioPinEdge.RisingEdge)
            {
                RaiseButtonPressed();
            }
        }
    }
}
