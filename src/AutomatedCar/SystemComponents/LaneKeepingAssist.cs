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
        VirtualFunctionBus bus;

        public LaneKeepingAssist(VirtualFunctionBus bus) : base(bus)
        {}

        public override void Process()
        {
            if (this.bus.SteeringWheelPacket.IsLKAActive)
            {
                if (this.bus.SensorPacket.WorldObjectsInRange.Count > 0)
                {
                    ICollection<WorldObject> lanes = this.bus.SensorPacket.WorldObjectsInRange.Where(x => x.Filename.Contains("road_2lane")).ToList();

                    if (lanes.Count > 0)
                    {
                        this.SetWheelRotationByLanes(lanes);
                    }
                }
            }
        }

        private void SetWheelRotationByLanes(ICollection<WorldObject> lanes)
        {

        }
    }
}
