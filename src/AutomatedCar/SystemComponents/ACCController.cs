namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.SystemComponents.Packets;
    using AutomatedCar.Models;
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ACCController : SystemComponent
    {
        public ACCControllerPacket ControllerPacket;
        private AutomatedCar car;
        private Stopwatch timer = new Stopwatch();

        public ACCController(VirtualFunctionBus virtualFunctionBus, AutomatedCar car)
            : base(virtualFunctionBus)
        {
            this.car = car;
            this.ControllerPacket = new ACCControllerPacket();
            this.ControllerPacket.Gain_proportional = .34F;
            this.ControllerPacket.Gain_integral = .26F;
            this.ControllerPacket.Gain_derivative = .4F;
            this.ControllerPacket.TimeConstant = 12;
            this.ControllerPacket.MaxInput = 20;
            this.virtualFunctionBus.ControllerPacket = this.ControllerPacket;
            timer.Start();
        }

        public override void Process()
        {
            if (this.car.track)
            {
                this.ControllerPacket.Input = this.virtualFunctionBus.PowerTrainPacket.Speed;
                Debug.WriteLine(
                    $"Recommended pedal level: {(int)this.ControllerPacket.CalculateOutput()}" +
                    $"\tP: {this.ControllerPacket.CalculateProportionalTerm():0.00}" +
                    $"\tI: {this.ControllerPacket.CalculateIntegralTerm():0.00}" +
                    $"\tD: {this.ControllerPacket.CalculateDerivativeTerm():0.00}" +
                    $"\tE: {this.ControllerPacket.Error}" +
                    $"\tE: {this.ControllerPacket.LastError}");
                int output = (int)this.ControllerPacket.CalculateOutput();
                if (Math.Abs(output) > 80)
                {
                   output -= output % 80;
                }

                if (output >= 0)
                {
                    this.car.Pedal.PedalPacket.BreakPedalLevel = 0;
                    this.car.Pedal.PedalPacket.GasPedalLevel = output;
                }
                else
                {
                    this.car.Pedal.PedalPacket.GasPedalLevel = 0;
                    this.car.Pedal.PedalPacket.BreakPedalLevel = -output;
                }
            }
        }
    }
}
