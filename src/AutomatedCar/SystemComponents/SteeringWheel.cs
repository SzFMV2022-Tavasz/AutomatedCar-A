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
        private AutomatedCar automatedCar;

        public SteeringWheel(VirtualFunctionBus virtualFunctionBus, AutomatedCar automatedCar)
            : base(virtualFunctionBus)
        {
            this.steeringWheelPacket = new SteeringWheelPacket();
            this.virtualFunctionBus.SteeringWheelPacket = this.steeringWheelPacket;
            //this.WheelRotation = 0;
            this.automatedCar = automatedCar;
        }

        //public int WheelRotation { get; set; }

        public override void Process()
        {
            switch (this.steeringWheelPacket.IsBeingRotated)
            {
                case false: this.RotateWheelByInputRotation();break;
                case true: this.RotateWheelByInputRotation();break;
            }
            Steering();
        }

        public void RotateWheelByInputRotation()
        {
            // FOR FUTURE DEVELOPMENT. --> THE WHEEL SHOULD SLOWLS GET BACK TO ITS ORIGINAL POSITION IF NOT STEERED.
            //int newRotation = this.steeringWheelPacket.WheelRotation;
            //newRotation += this.WheelRotation;

            //int newRotation = this.WheelRotation;

            //if (newRotation > 60)
            //{
            //    this.steeringWheelPacket.WheelRotation = 60;
            //}
            //else if (newRotation < -60)
            //{
            //    this.steeringWheelPacket.WheelRotation = -60;
            //}
            //else
            //{
            //    this.steeringWheelPacket.WheelRotation = newRotation;
            //}

            switch (this.virtualFunctionBus.SteeringWheelPacket.WheelRotation)
            {
                case int n when (n < 0): this.RotateWheelByInputRotation(5); break;
                case int n when (n > 0): this.RotateWheelByInputRotation(-5); break;
            }
        }

        public void RotateWheelByInputRotation(int rotationSize)
        {
            int actualWheelRotation = steeringWheelPacket.WheelRotation;

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
            int steerAngle = steeringWheelPacket.WheelRotation;
            int wheelBase = 130;
            double dt = 1;
            int carLocationX = automatedCar.X;
            int carLocationY = automatedCar.Y;
            double carHeading = automatedCar.Rotation;
            int carSpeed = this.automatedCar.VirtualFunctionBus.PowerTrainPacket.Speed;

            if (automatedCar.VirtualFunctionBus.GearShiftPacket.CurrentGear == Helpers.Gear.Reverse)
            {
                carSpeed = carSpeed * (-1);
            }

            double valami = (carHeading * Math.PI) / 180;

            double matcos = Math.Cos(valami);
            double matsin = (Math.Sin((carHeading * Math.PI) / 180));

            double fele = (wheelBase / 2 * matcos);
            double fele2 = (wheelBase / 2 * matsin);

            double frontWheelX = carLocationX + fele2;
            double frontWheelY = carLocationY - fele;

            double backWheelX = carLocationX - wheelBase / 2 * matsin;
            double backWheelY = carLocationY + wheelBase / 2 * matcos;

            backWheelX += (int)(carSpeed * dt * matsin);
            backWheelY -= (int)(carSpeed * dt * matcos);

            double mat2sin = Math.Sin((carHeading + steerAngle) * Math.PI / 180);
            double mat2cos = Math.Cos((carHeading + steerAngle) * Math.PI / 180);

            frontWheelX += (carSpeed * dt * mat2sin);
            frontWheelY -= (carSpeed * dt * mat2cos);

            carLocationX = (int)Math.Round((frontWheelX + backWheelX) / 2);
            carLocationY = (int)Math.Round((frontWheelY + backWheelY) / 2);

            steeringWheelPacket.NextPositionX = carLocationX;
            steeringWheelPacket.NextPositionY = carLocationY;

            //automatedCar.X = carLocationX;
            //automatedCar.Y = carLocationY;

            /*carHeading = Math.Atan2(frontWheelY - backWheelY, frontWheelX - backWheelX) * (180 / Math.PI);*/    //////////////////////// új számítááááás
            automatedCar.Rotation = carHeading + steerAngle/20;

        }
    }
}
