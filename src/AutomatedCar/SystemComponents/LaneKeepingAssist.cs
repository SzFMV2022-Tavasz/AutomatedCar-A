namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutomatedCar.Models;

    public class LaneKeepingAssist : SystemComponent
    {
        private World world;

        public LaneKeepingAssist(VirtualFunctionBus bus, World world) : base(bus)
        {
            this.world = world;
        }

        public override void Process()
        {
            if (this.virtualFunctionBus.SteeringWheelPacket.IsLKAActive || true)
            {
                if (this.virtualFunctionBus.CameraPacket.WorldObjectsInRange.Count > 0)
                {
                    ICollection<WorldObject> lanes = this.virtualFunctionBus.CameraPacket.WorldObjectsInRange.Where(x => x.Filename.Contains("road_2lane")).ToList();

                    if (lanes.Count > 0)
                    {
                        this.SetWheelRotationByLanes(lanes);
                    }
                }
            }
        }

        private void SetWheelRotationByLanes(ICollection<WorldObject> lanes)
        {
            //foreach(var line in lanes)
            //{
            //    Console.WriteLine(line.Filename);
            //}
            //this.world.ControlledCar.StreeringInputKey();
        }
    }
}
