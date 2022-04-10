namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ACCControllerPacket : ReactiveObject, IControllerPacket<double>
    {
        private double input;
        private double output;
        private double target;
        private double error;
        private double lastError;
        private double accumulator;
        private double derivative;
        private double maxInput;
        private double gain;
        private double gain_i;
        private double gain_d;
        private double timeConstant;
        private double counter;

        public double Input { get => this.input; set => this.RaiseAndSetIfChanged(ref this.input, value); }
        public double Output { get => this.output; set => this.RaiseAndSetIfChanged(ref this.output, value); }
        public double Target { get => this.target; set => this.RaiseAndSetIfChanged(ref this.target, value); }
        public double Error { get => this.Target - this.Input; }
        public double LastError { get => this.lastError; set => this.RaiseAndSetIfChanged(ref this.lastError, value); }
        public double Accumulator { get => this.accumulator; set => this.RaiseAndSetIfChanged(ref this.accumulator, value); }
        public double Derivative { get => this.derivative; set => this.RaiseAndSetIfChanged(ref this.derivative, value); }
        public double MaxInput { get => this.maxInput; set => this.RaiseAndSetIfChanged(ref this.maxInput, value); }
        public double Gain { get => this.gain; set => this.RaiseAndSetIfChanged(ref this.gain, value); }
        public double Gain_i { get => this.gain_i; set => this.RaiseAndSetIfChanged(ref this.gain_i, value); }
        public double Gain_d { get => this.gain_d; set => this.RaiseAndSetIfChanged(ref this.gain_d, value); }
        public double TimeConstant { get => this.timeConstant; set => this.RaiseAndSetIfChanged(ref this.timeConstant, value); }
        public double Counter { get => this.counter; set => this.RaiseAndSetIfChanged(ref this.counter, value); }

        /// <summary>
        /// The transfer function.
        /// </summary>
        /// <param name="x">Input is velocity.</param>
        /// <returns>Output is pedal level.</returns>
        public virtual double TransferG(double x)
        {
            double L = 160;
            double k = 0.25F;
            double x0 = 0;

            switch (x)
            {
                case >= 0:
                    x0 = 0;
                    break;
                case >= -3:
                    k = 0;
                    break;
                default:
                    x0 = -3;
                    break;
            }

            double output = (L / (1 + Math.Pow(Math.E, -k * (x - x0)))) - 80;

            return output;
        }

        public virtual double Transfer(double x)
        {
            double L = 160;
            double k = 0.15F;
            double x0 = 0;

            double output = (L / (1 + Math.Pow(Math.E, -k * (x - x0)))) - 80;

            return output;
        }

        public double CalculateProportionalTerm()
        {
            return 1 * this.Error;
        }

        public double CalculateIntegralTerm()
        {
            this.Accumulator += 1 / this.TimeConstant * this.Error;
            if (this.Accumulator < -20)
            {
                this.Accumulator = -20;
            }
            else if (this.Accumulator > 20)
            {
                this.Accumulator = 20;
            }

            return this.Accumulator;
        }

        public double CalculateDerivativeTerm()
        {
            if (this.Counter++ == 179)
            {
                this.Derivative = 1 * (this.Error - this.LastError);
                this.LastError = this.Error;
                this.Counter = 0;
            }

            return this.Derivative;
        }

        public double CalculateOutput()
        {
            return
                (this.Gain * this.Transfer(this.CalculateProportionalTerm())) +
                (this.Gain_i * this.Transfer(this.CalculateIntegralTerm())) +
                (this.Gain_d * this.Transfer(this.CalculateDerivativeTerm()));
        }
    }
}
