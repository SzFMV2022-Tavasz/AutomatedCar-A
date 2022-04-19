namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Diagnostics;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;

    /// <summary>
    /// Adaptive Cruise Control.
    /// </summary>
    public class ACCController : SystemComponent
    {
        private AutomatedCar car;
        private Stopwatch timer;

        public ACCControllerPacket ControllerPacket { get; private set; }

        public ACCController(VirtualFunctionBus virtualFunctionBus, AutomatedCar car)
            : base(virtualFunctionBus)
        {
            this.car = car;
            this.timer = new Stopwatch();
            this.ControllerPacket = new ACCControllerPacket();
            this.virtualFunctionBus.ControllerPacket = this.ControllerPacket;

            this.ControllerPacket.MaxInput = 20;
            this.ControllerPacket.TimeConstant = 12;
            this.ControllerPacket.Gain_proportional = .34F;
            this.ControllerPacket.Gain_integral = .26F;
            this.ControllerPacket.Gain_derivative = .4F;

            this.timer.Start();
        }

        public override void Process()
        {
            //Change this to set target speed.
            ControllerPacket.Target = 10;

            //Change this to turn on ACC
            if (false)//this.car.track)
            {
                this.ControllerPacket.Input = this.virtualFunctionBus.PowerTrainPacket.Speed;
                int output = (int)this.ControllerPacket.Output;

                Debug.WriteLine(
                    $"Recommended pedal level: {output}" +
                    $"\tP: {this.ControllerPacket.CalculateProportionalTerm():0.00}" +
                    $"\tI: {this.ControllerPacket.CalculateIntegralTerm():0.00}" +
                    $"\tD: {this.ControllerPacket.CalculateDerivativeTerm():0.00}" +
                    $"\tE: {this.ControllerPacket.Error}" +
                    $"\tE: {this.ControllerPacket.LastError}");

                if (output >= 0)
                {
                    this.car.Pedal.PedalPacket.BreakPedalLevel = 0;
                    this.car.Pedal.PedalPacket.GasPedalLevel = output;
                }
                else
                {
                    this.car.Pedal.PedalPacket.GasPedalLevel = 0;
                    this.car.Pedal.PedalPacket.BreakPedalLevel = -output / 3;
                }
            }
        }
    }
}
