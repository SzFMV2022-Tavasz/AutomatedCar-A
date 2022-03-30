﻿namespace AutomatedCar.SystemComponents.Sensors
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
        //}

        //protected override PolylineGeometry CalculateSensorPolylineGeometry()
        //{               //          kezdö pont                                      jobb széle                                              balszéle
        //    Point[] p = { new Point(/*world.ControlledCar.X, world.ControlledCar.Y*/100, 100), new Point(/*Az a pont ahol a kamera van +*/Range, Range + 50), new Point(/*Az a pont ahol a kamera van +*/Range, Range - 50) };
        //    return new PolylineGeometry(p, false);
        //    // defaultnak azért is jo mert legalább nem száll el hibával az egész 
        //}

        protected override ICollection<WorldObject> GetWorldObjectsInRange()
        {
            return this.world.WorldObjects.FindAll(IsInRange);
        }

        protected override bool IsInRange(WorldObject worldObject)
        {
            if (worldObject == this.world.ControlledCar)
            {
                return false;
            }

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
