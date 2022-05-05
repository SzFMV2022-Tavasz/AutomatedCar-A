namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;

    public class ACCTargetProcessor : SystemComponent
    {
        private static Vector2 invalidPosition = new Vector2(float.NaN, float.NaN);
        private Vector2 carPreviousPosition; // set to invalidPosition when a new car is being tracked (it's previous poisiton is unkown)
        private WorldObject currentCar = null;
        private WorldObject prevCar = null;
        private WorldObject currentRoadSign = null;

        private int targetSpeed;

        private AutomatedCar controlledCar;

        public ACCTargetProcessorPacket Packet { get; set; }

        private ISensorPacket SensorPacket
        {
            get { return this.virtualFunctionBus.RadarPacket; }
        }
        private IControllerPacket<double> ACCControllerPacket
        {
            get { return this.virtualFunctionBus.ControllerPacket; }
        }

        public ACCTargetProcessor(VirtualFunctionBus virtualFunctionBus, AutomatedCar controlledCar)
            : base(virtualFunctionBus)
        {
            this.controlledCar = controlledCar;
            this.Packet = new ACCTargetProcessorPacket();
            this.Packet.DriverTarget = 90;
            this.virtualFunctionBus.ACCTargetProcessorPacket = this.Packet;
        }

        public override void Process()
        {
            if (this.controlledCar.Filename.Contains("red"))
                return;
            ProcessSensorInput();
            CalculateTargetSpeed();
        }

        private void CalculateTargetSpeed()
        {
            if (this.currentCar != null)
            {
                Vector2 position = new Vector2(this.currentCar.X, this.currentCar.Y);
                if (!float.IsNaN(this.carPreviousPosition.X))
                {
                    float displacement = (this.carPreviousPosition - position).Length();
                    float carSpeed_mPs = displacement * (float)((6.0 / 5.0)); // px/tick -> m/s
                    float carSpeed = carSpeed_mPs * 3.6f; // m/s -> km/h

                    Vector2 thiscarposition = new Vector2(this.controlledCar.X, this.controlledCar.Y);
                    var realDistance = (thiscarposition - position).Length() / 50.0f; // pixel -> m
                    var distance = this.Packet.TargetDistanceCycle * carSpeed_mPs;

                    if (Math.Abs(realDistance - distance) <= 0.0001f)
                    {
                        this.targetSpeed = (int)carSpeed;
                    }
                    else if (realDistance < distance && carSpeed <= this.targetSpeed)
                    {
                        this.targetSpeed = Math.Max((int)carSpeed - 10, 5);
                    }
                    else if (realDistance > distance && carSpeed >= this.targetSpeed)
                    {
                        this.targetSpeed = (int)carSpeed + 10;
                    }
                }
                this.carPreviousPosition = position;
            }
            else if (this.Packet.DriverTarget != 0)
            {
                this.targetSpeed = this.Packet.DriverTarget;
            }
            if (this.currentRoadSign != null)
            {
                var rsTargetSpeed = GetSpeedLimitFromSign(this.currentRoadSign);
                if (rsTargetSpeed < this.targetSpeed)
                {
                    this.targetSpeed = rsTargetSpeed;
                }
            }

            this.Packet.ActualTarget = this.targetSpeed;
            this.ACCControllerPacket.Target = this.Packet.ActualTarget;
        }

        private void ProcessSensorInput()
        {
            this.prevCar = this.currentCar;
            this.currentCar = null;
            foreach (var obj in this.SensorPacket.WorldObjectsInRange)
            {
                if (obj.WorldObjectType == WorldObjectType.RoadSign)
                {
                    if (IsRoadSignFrontFacing(obj) && (this.currentRoadSign == null || DistanceFrom(obj) < DistanceFrom(this.currentRoadSign)))
                    {
                        this.currentRoadSign = obj;
                    }
                }
                else if (obj.WorldObjectType == WorldObjectType.Car)
                {
                    if (IsCarInSameLine(obj)
                        && (this.prevCar == null || obj == prevCar || DistanceFrom(obj) < DistanceFrom(this.prevCar)))
                    {
                        this.currentCar = obj;
                        if (obj != prevCar)
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
                case "roadsign_priority_stop.png":
                    return 0;
                default:
                    throw new ApplicationException("Roadsign " + obj.Filename + " not handled");
            }
        }
    }
}
