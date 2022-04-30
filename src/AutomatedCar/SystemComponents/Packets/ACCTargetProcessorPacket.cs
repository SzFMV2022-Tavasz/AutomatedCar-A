namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using ReactiveUI;

    public class ACCTargetProcessorPacket : ReactiveObject, IACCTargetProcessorPacket
    {
        private int driverTarget;
        private int actualTarget;
        private double targetDistanceCycle = 0.8;

        public double TargetDistanceCycle
        {
            get => this.targetDistanceCycle;
            set => this.RaiseAndSetIfChanged(ref this.targetDistanceCycle, value);
        }

        public void TargetDistanceCycleUp()
        {
            switch (this.targetDistanceCycle)
            {
                case 0.8:
                    this.TargetDistanceCycle = 1.0;
                    break;
                case 1.0:
                    this.TargetDistanceCycle = 1.2;
                    break;
                case 1.2:
                    this.TargetDistanceCycle = 1.4;
                    break;
                case 1.4:
                    this.TargetDistanceCycle = 0.8;
                    break;
            }
        }

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
