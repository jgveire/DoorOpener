namespace DoorService
{
    using System;
    using System.Threading.Tasks;
    using Windows.Devices.Gpio;

    internal class PressButtonCommand
    {
        private const int RELAY_PIN1 = 2;
        private const int RELAY_PIN2 = 3;
        private const int RELAY_PIN3 = 4;
        private const int RELAY_PIN4 = 5;
        private readonly object _lockObject = new object();
        private bool _executing;
        private GpioPin _pin1;
        private GpioPin _pin2;
        private GpioPin _pin3;
        private GpioPin _pin4;

        public PressButtonCommand()
        {
            InitGpio();
        }

        public void Execute()
        {
            lock (_lockObject)
            {
                if (_executing)
                {
                    return;
                }

                _executing = true;
            }

            InternalExecute();

            lock (_lockObject)
            {
                _executing = false;
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

            _pin1 = gpio.OpenPin(RELAY_PIN1);
            _pin1.SetDriveMode(GpioPinDriveMode.Output);

            _pin2 = gpio.OpenPin(RELAY_PIN2);
            _pin2.SetDriveMode(GpioPinDriveMode.Output);

            _pin3 = gpio.OpenPin(RELAY_PIN3);
            _pin3.SetDriveMode(GpioPinDriveMode.Output);

            _pin4 = gpio.OpenPin(RELAY_PIN4);
            _pin4.SetDriveMode(GpioPinDriveMode.Output);
        }

        private void InternalExecute()
        {
            _pin1.Write(GpioPinValue.Low);
            _pin2.Write(GpioPinValue.Low);
            Task.Delay(TimeSpan.FromMilliseconds(1000)).Wait();

            _pin1.Write(GpioPinValue.High);
            _pin2.Write(GpioPinValue.High);
            Task.Delay(TimeSpan.FromMilliseconds(5000)).Wait();

            _pin3.Write(GpioPinValue.Low);
            _pin4.Write(GpioPinValue.Low);
            Task.Delay(TimeSpan.FromMilliseconds(900)).Wait();

            _pin3.Write(GpioPinValue.High);
            _pin4.Write(GpioPinValue.High);
            Task.Delay(TimeSpan.FromMilliseconds(100)).Wait();
        }
    }
}
