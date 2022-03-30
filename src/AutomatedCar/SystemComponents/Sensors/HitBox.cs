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
                    // foreach (var carGeometry in this.world.ControlledCar.Geometries)
                    {
                        if (objGeometry.Bounds.Intersects(this.world.ControlledCar.Geometry.Bounds))
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