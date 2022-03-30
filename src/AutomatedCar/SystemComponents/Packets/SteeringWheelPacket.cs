namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ReactiveUI;

    public class SteeringWheelPacket : ReactiveObject, ISteeringWheelPacket
    {
        private int wheelRotation;
        private int nextPositionX;
        private int nextPositionY;
        private bool isbeingRotated;
        private bool isLKAActive;

        public SteeringWheelPacket()
        {
            this.wheelRotation = 0;
        }

        public int WheelRotation
        {
            get => this.wheelRotation;
            set => this.RaiseAndSetIfChanged(ref this.wheelRotation, value);
        }

        public int NextPositionX 
        {
            get => this.nextPositionX;
            set => this.RaiseAndSetIfChanged(ref this.nextPositionX, value);
        }

        public int NextPositionY
        {
            get => this.nextPositionY;
            set => this.RaiseAndSetIfChanged(ref this.nextPositionY, value);
        }

        public bool IsBeingRotated
        {
            get => this.isbeingRotated;
            set => this.RaiseAndSetIfChanged(ref this.isbeingRotated, value);
        }

        public bool IsLKAActive
        {
            get => this.isLKAActive;
            set => this.RaiseAndSetIfChanged(ref this.isLKAActive, value);
        }
    }
}
