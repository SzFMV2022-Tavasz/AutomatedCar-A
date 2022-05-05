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
        }

        public override void Process()
        {
            // TODO ki kell kapcsolnia hogyha hoszá érsz a kormányhoz 
            // most ha bevan nyomja és forgatod a kormányt akkor körbe körbe forog 

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
            // kikommenteztem mert NUllExceptionnal elszál 
           // int value = 0;
            //foreach(var line in lanes)
            //{
            //    Console.WriteLine(line.Filename);
            //}
            //this.world.ControlledCar.StreeringInputKey(value);
        }
    }
}
