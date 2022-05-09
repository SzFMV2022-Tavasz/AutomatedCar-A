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

        private void DisableLKA()
        {
            ICollection<WorldObject> objects = this.virtualFunctionBus.CameraPacket.WorldObjectsInRange.ToList();

            if (this.virtualFunctionBus.SteeringWheelPacket.IsLKAActive == true)
            {
                foreach (var item in objects)
                {
                    if (item.Filename == "road_2lane_90left.png" || item.Filename == "road_2lane_90right.png" ||
                        item.Filename == "road_2lane_crossroad_1.png" || item.Filename == "road_2lane_crossroad_2.png" ||
                        item.Filename == "road_2lane_rotary.png" || item.Filename == "road_2lane_tjunctionleft.png" ||
                        item.Filename == "road_2lane_tjunctionright.png")
                    {
                        this.virtualFunctionBus.SteeringWheelPacket.IsLKAActive = false;
                        this.virtualFunctionBus.SteeringWheelPacket.LKAState = "Kezelhetetlen forgalmi szituáció!";
                    }
                }
            }
        }
    }
}
