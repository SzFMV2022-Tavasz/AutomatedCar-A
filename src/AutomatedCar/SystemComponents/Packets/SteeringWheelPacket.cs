namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SteeringWheelPacket : ReactiveObject, ISteeringWheelPacket
    {
        private int wheelRotation;

        public SteeringWheelPacket()
        {
            this.wheelRotation = 0;
        }

        public int WheelRotation
        {
            get => this.wheelRotation;
            set => this.RaiseAndSetIfChanged(ref this.wheelRotation, value);
        }
    }
}
