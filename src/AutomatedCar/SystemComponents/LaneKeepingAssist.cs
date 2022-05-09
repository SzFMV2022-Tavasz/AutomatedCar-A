namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;

    public class LaneKeepingAssist : SystemComponent
    {
        private World world;

        public LaneKeepingAssist(VirtualFunctionBus bus, World world) : base(bus)
        {
            this.world = world;
            this.virtualFunctionBus.SteeringWheelPacket.IsLKAActive = false;
            this.virtualFunctionBus.SteeringWheelPacket.LKAState = "";
        }

        public override void Process()
        {

            if (this.virtualFunctionBus.SteeringWheelPacket.IsLKAActive)
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
            //Lane keeping calculations
        }
    }
}
