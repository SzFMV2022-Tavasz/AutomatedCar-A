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
            this.WheelRotation = 0;
        }

        public int WheelRotation { get; set; }

        public override void Process()
        {
            this.RotateWheelByInputRotation();
        }

        public void RotateWheelByInputRotation()
        {
            // FOR FUTURE DEVELOPMENT. --> THE WHEEL SHOULD SLOWLS GET BACK TO ITS ORIGINAL POSITION IF NOT STEERED.
            //int newRotation = this.steeringWheelPacket.WheelRotation;
            //newRotation += this.WheelRotation;

            int newRotation = this.WheelRotation;

            if (newRotation > 60)
            {
                this.steeringWheelPacket.WheelRotation = 60;
            }
            else if (newRotation < -60)
            {
                this.steeringWheelPacket.WheelRotation = -60;
            }
            else
            {
                this.steeringWheelPacket.WheelRotation = newRotation;
            }
        }
    }
}
