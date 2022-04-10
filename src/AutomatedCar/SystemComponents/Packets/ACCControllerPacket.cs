namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ACCControllerPacket : ReactiveObject, IControllerPacket<int>
    {
        private int input;
        private int output;
        private int target;
        private int error;
        private int lastError;
        private float accumulator;
        private int derivative;
        private int maxInput;
        private float gain;
        private float gain_i;
        private float gain_d;
        private int timeConstant;
        private int counter;

        public int Input { get => this.input; set => this.RaiseAndSetIfChanged(ref this.input, value); }

        public int Output { get => this.output; set => this.RaiseAndSetIfChanged(ref this.output, value); }

        public int Target { get => this.target; set => this.RaiseAndSetIfChanged(ref this.target, value); }

        public int Error { get => this.Target - this.Input; }

        public int LastError { get => this.lastError; set => this.RaiseAndSetIfChanged(ref this.lastError, value); }

        public float Accumulator { get => this.accumulator; set => this.RaiseAndSetIfChanged(ref this.accumulator, value); }
        public int Derivative { get => this.derivative; set => this.RaiseAndSetIfChanged(ref this.derivative, value); }

        public int MaxInput { get => this.maxInput; set => this.RaiseAndSetIfChanged(ref this.maxInput, value); }

        public float Gain { get => this.gain; set => this.RaiseAndSetIfChanged(ref this.gain, value); }
        public float Gain_i { get => this.gain_i; set => this.RaiseAndSetIfChanged(ref this.gain_i, value); }
        public float Gain_d { get => this.gain_d; set => this.RaiseAndSetIfChanged(ref this.gain_d, value); }

        public int TimeConstant { get => this.timeConstant; set => this.RaiseAndSetIfChanged(ref this.timeConstant, value); }
        public int Counter { get => this.counter; set => this.RaiseAndSetIfChanged(ref this.counter, value); }

        /// <summary>
        /// The transfer function.
        /// </summary>
        /// <param name="input">Input is velocity.</param>
        /// <returns>Output is pedal level.</returns>
        public virtual int Transfer(int input)
        {
            int output = (int)Math.Round((float)input / this.maxInput * 80);
            if (output > 80)
            {
                output = 80;
            }
            else if (output < 0)
            {
                output = 0;
            }

            return output;
        }

        public int CalculateProportionalTerm()
        {
            return (int)Math.Round(this.Gain * this.Error);
        }

        public int CalculateIntegralTerm()
        {
            this.Accumulator += this.Gain_i / this.TimeConstant * this.Error;
            if (this.Accumulator >= 5)
            {
                this.Accumulator = 5;
            }
            else if (this.Accumulator <= -5)
            {
                this.Accumulator = -5;
            }

            return (int)Math.Round(this.Accumulator);
        }

        public int CalculateDerivativeTerm()
        {
            if (this.Counter++ == 59)
            {
                this.Derivative = (int)Math.Round(this.Gain_d * (this.Error - this.LastError));
                this.LastError = this.Error;
                this.Counter = 0;
            }

            return this.Derivative;
        }

        public int CalculateOutput()
        {
            return this.Transfer(CalculateProportionalTerm() + CalculateIntegralTerm() + CalculateDerivativeTerm());
        }
    }
}
