namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
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
            //this.RotateWheelByInputRotation();
            Steering();
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

        private void Steering()
        {
            Vector2 carLocation;
            int steerAngle = 0;
            int wheelBase = 130;
            double dt = 1;
            carLocation.X = World.Instance.ControlledCar.X;
            carLocation.Y = World.Instance.ControlledCar.Y;
            double carHeading = World.Instance.ControlledCar.Rotation;
            int carSpeed = 1;

            double fele = (wheelBase / 2 * Math.Cos(carHeading));
            double fele2 = (wheelBase / 2 * Math.Sin(carHeading));

            double frontWheelX = (carLocation.X + fele2);
            double frontWheelY = (carLocation.Y + fele);

            double backWheelX = (carLocation.X - wheelBase / 2 * Math.Sin(carHeading));
            double backWheelY = (carLocation.Y - wheelBase / 2 * Math.Cos(carHeading));

            ///////////////////////////////////////////////////

            backWheelX += (carSpeed * dt * Math.Sin(carHeading));
            backWheelY -= (carSpeed * dt * Math.Cos(carHeading));

            frontWheelX += (carSpeed * dt * Math.Sin(carHeading + steerAngle));
            frontWheelY -= (carSpeed * dt * Math.Cos(carHeading + steerAngle));

            carLocation.X = (float)((frontWheelX + backWheelX) / 2);
            carLocation.Y = (float)((frontWheelY + backWheelY) / 2);

            World.Instance.ControlledCar.X = (int)carLocation.X;
            World.Instance.ControlledCar.Y = (int)carLocation.Y;

            carHeading = (int)Math.Atan2(frontWheelY - backWheelY, frontWheelX - backWheelX);
            //World.Instance.ControlledCar.Rotation += 5;

        }
    }
}
