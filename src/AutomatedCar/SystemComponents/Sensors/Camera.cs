namespace AutomatedCar.SystemComponents.Sensors
{
    using Avalonia;
    using Avalonia.Media;
    using System;
    using AutomatedCar.Models;
    using System.Collections.Generic;

    public class Camera : Sensor
    {
        public Camera(World world, VirtualFunctionBus virtualFunctionBus)
            : base(world, virtualFunctionBus, 80, 60)
        {
        }

        public override void Process()
        {
            this.UpdateSensorPosition();
            this.virtualFunctionBus.SensorPacket.WorldObjectsInRange = GetWorldObjectsInRange();
        }

        protected override PolylineGeometry CalculateSensorPolylineGeometry()
        {
            throw new NotImplementedException();
        }

        protected override ICollection<WorldObject> GetWorldObjectsInRange()
        {
            return this.world.WorldObjects.FindAll(IsInRange);
        }

        protected override bool IsInRange(WorldObject worldObject)
        {
            bool flag = false;
            foreach (var geometry in worldObject.Geometries)
            {
                if (geometry.Bounds.Intersects(this.FieldOfView.Bounds))
                {
                    flag = true;
                }
            }

            return flag;
        }
    }
}
