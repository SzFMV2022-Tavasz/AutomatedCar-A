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
        private PolylineGeometry transformedCarGeo;

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
            Matrix preCarTranslation = Matrix.CreateTranslation(-this.world.ControlledCar.RotationPoint.X, -this.world.ControlledCar.RotationPoint.Y);
            Matrix carTranslation = Matrix.CreateTranslation(this.world.ControlledCar.X, this.world.ControlledCar.Y);
            Matrix carRotation = Matrix.CreateRotation((this.world.ControlledCar.Rotation * Math.PI) / 180.0);
            var carPoints = new List<Point>();
            foreach (var carPoint in this.world.ControlledCar.Geometry.Points)
            {
                Point transformedCarPoint = carPoint.Transform(preCarTranslation).Transform(carRotation).Transform(carTranslation);
                carPoints.Add(transformedCarPoint);
            }

            this.transformedCarGeo = new PolylineGeometry(carPoints, false);
            this.Hp.Collided = CheckIfCollides();
            //if (this.Hp.Collided)
            //{
            //    Console.WriteLine("Collision");
            //}
            //else
            //{
            //    Console.WriteLine("--");
            //}
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
                if (Math.Abs(worldObject.X - this.world.ControlledCar.X) <= 106 && Math.Abs(worldObject.Y - this.world.ControlledCar.Y) <= 238)
                {
                    Matrix preTranslation = Matrix.CreateTranslation(-worldObject.RotationPoint.X, -worldObject.RotationPoint.Y);
                    Matrix translation = Matrix.CreateTranslation(worldObject.X, worldObject.Y);
                    Matrix rotation = Matrix.CreateRotation((worldObject.Rotation * Math.PI) / 180.0);

                    foreach (var geometry in worldObject.Geometries)
                    {
                        foreach (var point in geometry.Points)
                        {
                            Point transformed = point.Transform(preTranslation).Transform(rotation).Transform(translation);

                            if (this.transformedCarGeo.FillContains(transformed))
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }

            return false;
        }
    }
}