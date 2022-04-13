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
        private double lastError;
        private double accumulator;
        private double derivative;
        private double maxInput;
        private double gain_proportional;
        private double gain_integral;
        private double gain_derivative;
        private double timeConstant;
        private byte counter;

        public double Input { get => this.input; set => this.RaiseAndSetIfChanged(ref this.input, value); }
        public double Output { get => this.output; set => this.RaiseAndSetIfChanged(ref this.output, value); }
        public double Target { get => this.target; set => this.RaiseAndSetIfChanged(ref this.target, value); }
        public double Error { get => this.Target - this.Input; }
        public double LastError { get => this.lastError; set => this.RaiseAndSetIfChanged(ref this.lastError, value); }
        public double Accumulator { get => this.accumulator; set => this.RaiseAndSetIfChanged(ref this.accumulator, value); }
        public double Derivative { get => this.derivative; set => this.RaiseAndSetIfChanged(ref this.derivative, value); }
        public double MaxInput { get => this.maxInput; set => this.RaiseAndSetIfChanged(ref this.maxInput, value); }
        public double Gain_proportional { get => this.gain_proportional; set => this.RaiseAndSetIfChanged(ref this.gain_proportional, value); }
        public double Gain_integral { get => this.gain_integral; set => this.RaiseAndSetIfChanged(ref this.gain_integral, value); }
        public double Gain_derivative { get => this.gain_derivative; set => this.RaiseAndSetIfChanged(ref this.gain_derivative, value); }
        public double TimeConstant { get => this.timeConstant; set => this.RaiseAndSetIfChanged(ref this.timeConstant, value); }
        public byte Counter { get => this.counter; set => this.RaiseAndSetIfChanged(ref this.counter, value); }

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

        /// <summary>
        /// Logistic function https://www.desmos.com/calculator/z2f4vox0fg.
        /// </summary>
        /// <param name="x">Velocity.</param>
        /// <returns>Pedal level ]-80, 80[.</returns>
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
            return this.Gain_proportional * this.Error;
        }

        public double CalculateIntegralTerm()
        {
            if (Math.Abs(this.Accumulator) > 20)
            {
                this.Accumulator -= this.Accumulator % 20;
            }

            this.Accumulator += 1 / this.TimeConstant * this.Error;

            return this.Gain_integral * this.Accumulator;
        }

        public double CalculateDerivativeTerm()
        {
            if (this.Counter++ == 0)
            {
                if (this.Error != 0)
                {
                    this.Derivative = this.Gain_derivative * (this.Error - this.LastError);
                }
                else
                {
                    this.Derivative = 0;
                }

                this.LastError = this.Error;
            }

            return this.Derivative;
        }

        public double CalculateOutput()
        {
            return this.Transfer(
                this.CalculateProportionalTerm() +
                this.CalculateIntegralTerm() +
                this.CalculateDerivativeTerm());
        }
    }
}
