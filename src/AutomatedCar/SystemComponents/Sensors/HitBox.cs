namespace AutomatedCar.SystemComponents.Sensors
{
    using Avalonia;
    using Avalonia.Media;
    using System;
    using AutomatedCar.Models;
    using System.Collections.Generic;

    public class HitBox : SystemComponent
    {
        private World world;
        private VirtualFunctionBus bus;

        public event EventHandler ObjectsInRange;

        public HitBox(World world, VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.world = world;
            this.bus = virtualFunctionBus;
        }

        public override void Process()
        {
            this.bus.HitBoxPacket.Collided = CheckIfCollides();
            if (this.bus.HitBoxPacket.Collided) this.ObjectsInRange?.Invoke(this, EventArgs.Empty);
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
                bool flag = false;
                foreach (var objGeometry in worldObject.Geometries)
                {
                    foreach (var carGeometry in this.world.ControlledCar.Geometries)
                    {
                        if (objGeometry.Bounds.Intersects(carGeometry.Bounds))
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
        }
    }
}