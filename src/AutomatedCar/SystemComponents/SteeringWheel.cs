namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;

    public class SteeringWheel : SystemComponent
    {
        private SteeringWheelPacket steeringWheelPacket;
        private AutomatedCar automatedCar;

        public SteeringWheel(VirtualFunctionBus virtualFunctionBus, AutomatedCar automatedCar)
            : base(virtualFunctionBus)
        {
            this.steeringWheelPacket = new SteeringWheelPacket();
            this.steeringWheelPacket.NextPositionX = automatedCar.X;
            this.steeringWheelPacket.NextPositionY = automatedCar.Y;
            this.virtualFunctionBus.SteeringWheelPacket = this.steeringWheelPacket;
            this.automatedCar = automatedCar;
        }

        public override void Process()
        {
            if (this.automatedCar.VirtualFunctionBus.PowerTrainPacket.Speed > 0)
            {

                switch (this.steeringWheelPacket.IsBeingRotated)
                {
                    case false: this.SteeringWheelReset(); break;
                    case true: this.SteeringWheelReset(); break;
                }

                this.Steering();
            }
        }

        private void SteeringWheelReset()
        {
            switch (this.virtualFunctionBus.SteeringWheelPacket.WheelRotation)
            {
                case int n when (n < 0): this.RotateWheelByInputRotation(5); break;
                case int n when (n > 0): this.RotateWheelByInputRotation(-5); break;
            }
        }

        public void RotateWheelByInputRotation(int rotationSize)
        {
            int actualWheelRotation = this.steeringWheelPacket.WheelRotation;

            int newWheelRotation = actualWheelRotation + rotationSize;

            if (newWheelRotation <= 60 && newWheelRotation >= -60)
            {
                this.steeringWheelPacket.WheelRotation = newWheelRotation;
            }
            else if (newWheelRotation > 60)
            {
                this.steeringWheelPacket.WheelRotation = 60;
            }
            else if (newWheelRotation < -60)
            {
                this.steeringWheelPacket.WheelRotation = -60;
            }
        }

        private void Steering()
        {
            int steerAngle = this.steeringWheelPacket.WheelRotation;
            int wheelBase = 130;
            double dt = 1;
            int carLocationX = this.automatedCar.X;
            int carLocationY = this.automatedCar.Y;
            double carHeading = this.automatedCar.Rotation;
            int carSpeed = this.automatedCar.VirtualFunctionBus.PowerTrainPacket.Speed;

            if (this.automatedCar.VirtualFunctionBus.GearShiftPacket.CurrentGear == Helpers.Gear.Reverse)
            {
                carSpeed = carSpeed * (-1);
                steerAngle = steerAngle * (-1);
            }

            double frontWheelX = carLocationX + (wheelBase / 2 * Math.Sin((carHeading * Math.PI) / 180));
            double frontWheelY = carLocationY - (wheelBase / 2 * Math.Cos((carHeading * Math.PI) / 180));

            double backWheelX = carLocationX - (wheelBase / 2 * Math.Sin((carHeading * Math.PI) / 180));
            double backWheelY = carLocationY + (wheelBase / 2 * Math.Cos((carHeading * Math.PI) / 180));

            backWheelX += carSpeed * dt * Math.Sin((carHeading * Math.PI) / 180);
            backWheelY -= carSpeed * dt * Math.Cos((carHeading * Math.PI) / 180);

            frontWheelX += carSpeed * dt * Math.Sin((carHeading + steerAngle) * Math.PI / 180);
            frontWheelY -= carSpeed * dt * Math.Cos((carHeading + steerAngle) * Math.PI / 180);

            carLocationX = (int)Math.Round((frontWheelX + backWheelX) / 2);
            carLocationY = (int)Math.Round((frontWheelY + backWheelY) / 2);

            this.steeringWheelPacket.NextPositionX = carLocationX;
            this.steeringWheelPacket.NextPositionY = carLocationY;

            this.automatedCar.Rotation = carHeading + (steerAngle / 20);
        }
    }
}
