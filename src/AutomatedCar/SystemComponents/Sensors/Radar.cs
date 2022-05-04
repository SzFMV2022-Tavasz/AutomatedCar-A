namespace AutomatedCar.SystemComponents.Sensors
{
    using Avalonia;
    using Avalonia.Media;
    using System;
    using AutomatedCar.Models;
    using System.Collections.Generic;

    public class Radar : Sensor
    {
        public event EventHandler ObjectsInRange;

        private List<string> fileNames;
        public Radar(World world, VirtualFunctionBus virtualFunctionBus)
            : base(world, virtualFunctionBus, 7, 60)
        {
            this.virtualFunctionBus.RadarPacket = this.SensorPacket;
        }

        public override void Process()
        {
            this.UpdateSensorPositionAndOrientation();
            this.virtualFunctionBus.RadarPacket.WorldObjectsInRange = GetWorldObjectsInRange();
            if (this.virtualFunctionBus.RadarPacket.WorldObjectsInRange.Count > 0) this.ObjectsInRange?.Invoke(this, EventArgs.Empty);
            if (this.fileNames != null)
            {
                this.SensorPacket.FileNamesRadar = this.fileNames;
            }
        }

        protected override ICollection<WorldObject> GetWorldObjectsInRange()
        {
            this.fileNames = new List<string>();
            return this.world.WorldObjects.FindAll(IsInRange);
        }

        protected override bool IsInRange(WorldObject worldObject)
        {
            if (worldObject == this.world.ControlledCar)
            {
                return false;
            }

            if (worldObject.Collideable)
            {
                Matrix preTanslation = Matrix.CreateTranslation(-worldObject.RotationPoint.X, -worldObject.RotationPoint.Y);
                Matrix translation = Matrix.CreateTranslation(worldObject.X, worldObject.Y);
                Matrix rotation = Matrix.CreateRotation((worldObject.Rotation * Math.PI) / 180.0);
                Point transformed;

                foreach (var point in worldObject.RawGeometries[0].Points)
                {
                    transformed = point.Transform(preTanslation).Transform(rotation).Transform(translation);
                    if (this.FieldOfView.FillContains(transformed))
                    {
                        this.fileNames.Add(worldObject.Filename);
                        return true;
                    }
                }

                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
