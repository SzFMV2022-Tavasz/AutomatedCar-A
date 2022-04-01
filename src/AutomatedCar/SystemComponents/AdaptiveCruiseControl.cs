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

    class AdaptiveCruiseControl : SystemComponent
    {
        static Vector2 invalidPosition = new Vector2(float.NaN, float.NaN);
        private WorldObject currentCar = null;
        private Vector2 carPreviousPosition; // set to invalidPosition when a new car is being tracked (it's previous poisiton is unkown)
        int targetSpeed;
        int userProvidedTargetSpeed;

        private ISensorPacket SensorPacket
        {
            get { return this.virtualFunctionBus.SensorPacket; }
        }

        private WorldObject currentRoadSign = null;

        private AutomatedCar controlledCar;
        public AdaptiveCruiseControl(VirtualFunctionBus virtualFunctionBus) 
            : base(virtualFunctionBus)
        {
        }

        public override void Process()
        {
            ProcessSensorInput();
            ClaculateTargetSpeed();
        }

        private void ClaculateTargetSpeed()
        {
            float deltaTime = 1;
            if (this.currentCar != null && !this.carPreviousPosition.Equals(invalidPosition))
            {
                Vector2 position = new Vector2(this.currentCar.X, this.currentCar.Y);
                float displacement = (this.carPreviousPosition - position).Length();
                float carSpeed = displacement / deltaTime; // conversion
                this.targetSpeed = (int)carSpeed;
                this.carPreviousPosition = position;
            }
            else if (this.currentRoadSign != null)
            {
                this.targetSpeed = GetSpeedLimitFromSign(this.currentRoadSign);
            }
            else
            {
                this.targetSpeed = this.userProvidedTargetSpeed;
            }
        }

        private void ProcessSensorInput()
        {
            this.currentRoadSign = null;
            foreach (var obj in this.SensorPacket.WorldObjectsInRange)
            {
                if (obj.WorldObjectType == WorldObjectType.RoadSign)
                {
                    if (IsRoadSignFrontFacing(obj) &&
                        (this.currentRoadSign == null || DistanceFrom(obj) < DistanceFrom(this.currentRoadSign)))
                    {
                        this.currentRoadSign = obj;
                    }
                }
                else if (obj.WorldObjectType == WorldObjectType.Car)
                {
                    if (IsCarInSameLine(obj)
                        && (this.currentCar == null || DistanceFrom(obj) < DistanceFrom(this.currentCar)))
                    {
                        this.currentCar = obj;
                        this.carPreviousPosition = invalidPosition;
                    }
                }
            }
        }

        private bool IsCarInSameLine(WorldObject car)
        {
            return this.controlledCar.Rotation - car.Rotation < 90;
        }

        private bool IsRoadSignFrontFacing(WorldObject sign)
        {
            return this.controlledCar.Rotation - sign.Rotation < 90;
        }

        private float DistanceFrom(WorldObject obj)
        {
            return new Vector2(this.controlledCar.X - obj.X, this.controlledCar.Y - obj.Y).Length();
        }

        private int GetSpeedLimitFromSign(WorldObject obj)
        {
            switch (obj.Filename)
            {
                case "roadsign_speed_40.png":
                    return 40;
                case "roadsign_speed_50.png":
                    return 50;
                case "roadsign_speed_60.png":
                    return 60;
                default:
                    throw new ApplicationException("Roadsign " + obj.Filename + " not handled");
            }
        }
    }
}
