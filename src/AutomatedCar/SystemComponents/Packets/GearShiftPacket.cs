namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Helpers;
    using ReactiveUI;

    public class GearShiftPacket: ReactiveObject, IGearShiftPacket
    {
        private Gear currentGear;
        private Shifts currentShift;

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
    }
}
