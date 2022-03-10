namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SteeringWheel : SystemComponent
    {
        private SteeringWheelPacket steeringWheelPacket;

        public SteeringWheel(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.steeringWheelPacket = new SteeringWheelPacket();
            this.virtualFunctionBus.SteeringWheelPacket = this.steeringWheelPacket;
        }

        public override void Process()
        {
            var a = 5 + 5;
        }
    }
}
