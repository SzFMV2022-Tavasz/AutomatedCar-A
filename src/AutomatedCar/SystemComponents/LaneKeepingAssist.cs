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
            if (this.virtualFunctionBus.SteeringWheelPacket.IsLKAActive == true)
            {
                this.DisableLKA();
            }
            else if (this.virtualFunctionBus.SteeringWheelPacket.IsLKAActive == false)
            {
                this.EnableLKA();
            }

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
            WorldObject[] objects = this.virtualFunctionBus.CameraPacket.WorldObjectsInRange.ToArray();
            bool result = true;
            int i = 0;

            do
            {
                if (objects[i].Filename == "road_2lane_90left.png" || objects[i].Filename == "road_2lane_90right.png" ||
                        objects[i].Filename == "road_2lane_crossroad_1.png" || objects[i].Filename == "road_2lane_crossroad_2.png" ||
                        objects[i].Filename == "road_2lane_rotary.png" || objects[i].Filename == "road_2lane_tjunctionleft.png" ||
                        objects[i].Filename == "road_2lane_tjunctionright.png")
                {
                    result = false;
                }

                i++;
            } while (result == true && i < objects.Length);

            if (result == true)
            {
            }
            else
            {
                this.virtualFunctionBus.SteeringWheelPacket.IsLKAActive = false;
                this.virtualFunctionBus.SteeringWheelPacket.LKAState = "Kezelhetetlen!";
            }
        }

        private void EnableLKA()
        {
            WorldObject[] objects = this.virtualFunctionBus.CameraPacket.WorldObjectsInRange.ToArray();

            foreach (var item in objects)
            {
                if (item.Filename != "road_2lane_90left.png" && item.Filename != "road_2lane_90right.png" &&
                        item.Filename != "road_2lane_crossroad_1.png" && item.Filename != "road_2lane_crossroad_2.png" &&
                        item.Filename != "road_2lane_rotary.png" && item.Filename != "road_2lane_tjunctionleft.png" &&
                        item.Filename != "road_2lane_tjunctionright.png")
                {
                    this.virtualFunctionBus.SteeringWheelPacket.IsLKAActive = true;
                    this.virtualFunctionBus.SteeringWheelPacket.LKAState = "Elérhető a sávtartó funkció!";
                }
            }
        }
    }
}
