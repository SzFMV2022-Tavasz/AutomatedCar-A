namespace AutomatedCar.SystemComponents.Sensors
{
    using Avalonia;
    using Avalonia.Media;
    using System;
    using AutomatedCar.Models;
    using System.Collections.Generic;

    public class Radar : Sensor
    {

        public Radar(ref World world, VirtualFunctionBus virtualFunctionBus, int range, double angleOfView) 
            : base(ref world, virtualFunctionBus, range, angleOfView)
        {

        }

        public override void Process()
        {
            throw new NotImplementedException();
        }

        protected override ICollection<WorldObject> GetWorldObjectsInRange()
        {
            throw new NotImplementedException();
        }

        protected override bool IsInRange(WorldObject worldObject)
        {
            throw new NotImplementedException();
        }
    }
}
