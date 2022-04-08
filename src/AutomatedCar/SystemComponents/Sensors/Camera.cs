namespace AutomatedCar.SystemComponents.Sensors
{
    using Avalonia;
    using Avalonia.Media;
    using System;
    using AutomatedCar.Models;
    using System.Collections.Generic;

    public class Camera : Sensor
    {
        public event EventHandler ObjectsInRange;
        public Camera(World world, VirtualFunctionBus virtualFunctionBus)
            : base(world, virtualFunctionBus, 80, 60)
        {
            //this.FieldOfView = CalculateSensorPolylineGeometry();
        }

        public override void Process()
        {
            this.UpdateSensorPositionAndOrientation();
            this.virtualFunctionBus.SensorPacket.WorldObjectsInRange = GetWorldObjectsInRange();
            if (this.virtualFunctionBus.SensorPacket.WorldObjectsInRange.Count > 0) this.ObjectsInRange?.Invoke(this, EventArgs.Empty);
        }

        protected override ICollection<WorldObject> GetWorldObjectsInRange()
        {
            return this.world.WorldObjects.FindAll(IsInRange);
        }

        protected override bool IsInRange(WorldObject worldObject)
        {
            //Not counting self, and if world object has no geometry
            if (worldObject == this.world.ControlledCar || worldObject.RawGeometries.Count == 0)
            {
                return false;
            }

            Matrix preTanslation = Matrix.CreateTranslation(-worldObject.RotationPoint.X, -worldObject.RotationPoint.Y);
            Matrix translation = Matrix.CreateTranslation(worldObject.X, worldObject.Y);
            Matrix rotation = Matrix.CreateRotation((worldObject.Rotation * Math.PI) / 180.0);
            Point transformed;

            foreach (var point in worldObject.RawGeometries[0].Points)
            {
                transformed = point.Transform(preTanslation).Transform(rotation).Transform(translation);
                if (this.FieldOfView.FillContains(transformed))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
