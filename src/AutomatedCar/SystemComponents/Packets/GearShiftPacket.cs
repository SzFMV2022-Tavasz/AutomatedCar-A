namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Helpers;
    using ReactiveUI;

    public class GearShiftPacket: ReactiveObject, IGearShiftPacket
    {
        private Gear currentGear;
        private Shifts currentShift;
        private string currentState;

        private Gear prevGear;

        public Gear PrevGear
        {
            get => this.prevGear;
            set => this.RaiseAndSetIfChanged(ref this.prevGear, value);
        }

        public Gear CurrentGear
        {
            get => this.currentGear;
            set => this.RaiseAndSetIfChanged(ref this.currentGear, value);
        }

        public Shifts CurrentShift
        {
            get => this.currentShift;
            set => this.RaiseAndSetIfChanged(ref this.currentShift, value);
        }

        public string GearState
        {
            get => this.currentState;
   
            set => this.RaiseAndSetIfChanged(ref this.currentState, value);
        }
    }
}
