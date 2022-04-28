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
            this.ControllerPacket.Gain_proportional = .6F;
            this.ControllerPacket.Gain_integral = .4F;
            this.ControllerPacket.Gain_derivative = 0.4F;

            this.timer.Start();
        }

        public override void Process()
        {
            if (this.virtualFunctionBus.PowerTrainPacket.Speed > 0)
            {
                this.car.isTracked = true;
            }
            else
            {
                this.car.isTracked = false;
            }

            //Change this to set target speed.
            //ControllerPacket.Target = 10;

            //Change this to turn on ACC
            if (ControllerPacket.Enabled)//this.car.track)
            {
                this.ControllerPacket.Input = this.virtualFunctionBus.PowerTrainPacket.Speed * 7;
                int output = (int)this.ControllerPacket.Output;

                if (this.car.isTracked)
                {
                    Debug.WriteLine(
                        $"Target speed: {this.ControllerPacket.Target,-5}" +
                        $"Actual speed: {this.virtualFunctionBus.PowerTrainPacket.Speed,-5}" +
                        $"Recommended pedal level: {output,-5}" +
                        $"\t\tP: {this.ControllerPacket.CalculateProportionalTerm(),-6:0.00}" +
                        $"\t\tI: {this.ControllerPacket.CalculateIntegralTerm(),-6:0.00}" +
                        $"\t\tD: {this.ControllerPacket.CalculateDerivativeTerm(),-6:0.00}"/* +
                        $"\tEa: {this.ControllerPacket.Error}" +
                        $"\tEl: {this.ControllerPacket.LastError}"*/);
                }

                if (output >= 0)
                {
                    this.car.Pedal.PedalPacket.BreakPedalLevel = 0;
                    this.car.Pedal.PedalPacket.GasPedalLevel = output;
                }
                else if (output >= -30)
                {
                    this.car.Pedal.PedalPacket.GasPedalLevel = 0;
                    this.car.Pedal.PedalPacket.BreakPedalLevel = 0;
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
