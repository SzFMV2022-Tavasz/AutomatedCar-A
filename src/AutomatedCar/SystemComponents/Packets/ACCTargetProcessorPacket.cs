namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using ReactiveUI;

    public class ACCTargetProcessorPacket : ReactiveObject, IACCTargetProcessorPacket
    {
        private int driverTarget;
        private int actualTarget;

        public int DriverTarget
        {
            get => this.driverTarget;
            set => this.RaiseAndSetIfChanged(ref this.driverTarget, value);
        }

        public int ActualTarget
        {
            get => this.actualTarget;
            set => this.RaiseAndSetIfChanged(ref this.actualTarget, value);
        }
    }
}
