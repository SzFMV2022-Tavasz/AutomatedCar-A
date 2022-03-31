namespace AutomatedCar.SystemComponents.Packets
{
    using System.Collections.Generic;
    using AutomatedCar.Models;
    using ReactiveUI;

    public class SensorPacket : ReactiveObject, ISensorPacket
    {
        private ICollection<WorldObject> worldObjectsInRange;
        private int xCord;
        private int yCord;
        public int XCord
        {
            get => this.xCord;
            set => this.RaiseAndSetIfChanged(ref this.xCord, value);
        }
        public int YCord
        {
            get => this.yCord;
            set => this.RaiseAndSetIfChanged(ref this.yCord, value);
        }

        public ICollection<WorldObject> WorldObjectsInRange
        {
            get => this.worldObjectsInRange;
            set => this.worldObjectsInRange = value;
        }
    }
}
