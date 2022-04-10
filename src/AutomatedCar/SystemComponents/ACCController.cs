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
            this.ControllerPacket.Gain = 2F;
            this.ControllerPacket.Gain_i = 0.1F;
            this.ControllerPacket.Gain_d = 2F;
            this.ControllerPacket.TimeConstant = 6;
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
                    $"Recommended pedal level: {this.ControllerPacket.CalculateOutput()}" +
                    $"\tP: {this.ControllerPacket.CalculateProportionalTerm()}" +
                    $"\tI: {this.ControllerPacket.CalculateIntegralTerm()}" +
                    $"\tD: {this.ControllerPacket.CalculateDerivativeTerm()}" +
                    $"\tE: {this.ControllerPacket.Error}" +
                    $"\tE: { this.ControllerPacket.LastError}");
                int output = this.ControllerPacket.CalculateOutput();
                if (output > 0)
                {
                    //this.car.Pedal.PedalPacket.BreakPedalLevel = 0;
                    this.car.Pedal.PedalPacket.GasPedalLevel = output;
                }
                else if (output < -40)
                {
                    //this.car.Pedal.PedalPacket.GasPedalLevel = 0;
                    this.car.Pedal.PedalPacket.BreakPedalLevel = output;
                }
            }
        }
    }
}
