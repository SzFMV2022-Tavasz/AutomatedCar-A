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
            Hp = new HitBoxPacket();
            Hp.Collided = false;
        }

        public override void Process()
        {
            this.Hp.Collided = CheckIfCollides();
            if (this.Hp.Collided) this.ObjectsInRange?.Invoke(this, EventArgs.Empty);
        }

        protected bool CheckIfCollides()
        {

            return this.world.WorldObjects.Find(this.IsInRange) != null ;
        }

        protected bool IsInRange(WorldObject worldObject)
        {
            // undorito de legalább müködik (kb)
            List<Point> cartemp = new List<Point>(); //az auto pontjai // ezeket a worldbe kellene lehet és nem it kiszámolni mindig 
            List<Point> objtemp = new List<Point>(); //az objekt pontjai
            // No collision with self.
            if (worldObject == this.world.ControlledCar)
            {
                return false;
            }

            if (worldObject.Collideable /*&& worldObject.Filename != "roadsign_parking_right.png"*/)
            {
                bool flag = false;
                {
                    foreach (var Point in world.ControlledCar.Geometry.Points)
                    {
                        // kigyüjtjük az auto pontjait 
                        cartemp.Add(new Point(Point.X + world.ControlledCar.X, Point.Y + world.ControlledCar.Y));
                    }
                }

                foreach (var oneGeometri in worldObject.Geometries)
                    {
                        foreach (var GeometriPoint in oneGeometri.Points)
                        {
                         // kigyüjtük az ütköztethetö tárgyak pontjait
                            objtemp.Add(new Point(GeometriPoint.X + worldObject.X, GeometriPoint.Y + worldObject.Y));
                        }
                    }

                //össze hasonlitjuk a pontokat
                foreach (var objpoint in objtemp)
                {
                    foreach (var carpoint in cartemp)
                    {
                        if (objpoint.Equals(carpoint))
                        {
                            flag = true;
                        }
                    }
                }

                return flag;
            }
            else
            {
                return false;
            }
            // ez azért is gázos mert a pontok valahogy nem jönnek ki de legalább valaminek lehet már ütközni hitboxal
        }
    }
}