namespace AutomatedCar.SystemComponents.Packets
{
    using System.Collections.Generic;
    using AutomatedCar.Models;

    public class SensorPacket : ISensorPacket
    {

        private ICollection<WorldObject> worldObjectsInRange;

        public ICollection<WorldObject> WorldObjectsInRange
        {
            get => this.worldObjectsInRange;
            set => this.worldObjectsInRange = value;
        }
    }
}
