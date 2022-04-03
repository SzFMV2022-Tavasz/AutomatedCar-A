namespace AutomatedCar.SystemComponents.Sensors
{
    using Avalonia;
    using Avalonia.Media;
    using System;
    using AutomatedCar.Models;
    using System.Collections.Generic;
    using AutomatedCar.SystemComponents.Packets;

    public class HitBox : SystemComponent
    {
        private World world;
        private VirtualFunctionBus bus;
        HitBoxPacket Hp;

        public event EventHandler ObjectsInRange;

        public HitBox(World world, VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.world = world;
            this.bus = virtualFunctionBus;
            this.Hp = new HitBoxPacket();
            this.Hp.Collided = false;
        }

        public override void Process()
        {
            this.Hp.Collided = CheckIfCollides();
            if (this.Hp.Collided) this.ObjectsInRange?.Invoke(this, EventArgs.Empty);
        }

        protected bool CheckIfCollides()
        {
            return this.world.WorldObjects.Find(this.IsInRange) != null;
        }

        protected bool IsInRange(WorldObject worldObject)
        {
            // No collision with self.
            if (worldObject == this.world.ControlledCar)
            {
                return false;
            }

            if (worldObject.Collideable)
            {
                Matrix carTranslation = Matrix.CreateTranslation(this.world.ControlledCar.X, this.world.ControlledCar.Y);
                Matrix carRotation = Matrix.CreateRotation((this.world.ControlledCar.Rotation * Math.PI) / 180.0);

                Matrix translation = Matrix.CreateTranslation(worldObject.X, worldObject.Y);
                Matrix rotation = Matrix.CreateRotation((worldObject.Rotation * Math.PI) / 180.0);

                var carPoints = new List<Point>();
                foreach (var carPoint in this.world.ControlledCar.Geometry.Points)
                {
                    Point transformedCarPoint = carPoint.Transform(carRotation).Transform(carTranslation);
                    carPoints.Add(transformedCarPoint);
                }

                PolylineGeometry realGeo = new PolylineGeometry(carPoints, false);

                foreach (var geometry in worldObject.Geometries)
                {
                    foreach (var point in geometry.Points)
                    {
                        Point transformed = point.Transform(rotation).Transform(translation);

                        if (realGeo.FillContains(transformed))
                        {
                            return true;
                        }

                    }

                    return false;
                }

                return false;
            }

            return false;
        }
    }
}