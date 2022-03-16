namespace AutomatedCar.SystemComponents.Sensors
{
    using Avalonia;
    using Avalonia.Media;
    using System;
    using AutomatedCar.Models;
    using System.Collections.Generic;

    public class Camera : Sensor
    {
        public Camera(ref World world, VirtualFunctionBus virtualFunctionBus, int range, double angleOfView)
            : base(ref world, virtualFunctionBus, range, angleOfView)
        {
        }

        public override void Process()
        {
            this.UpdateSensorPosition();
            this.virtualFunctionBus.SensorPacket.WorldObjectsInRange = GetWorldObjectsInRange();
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
