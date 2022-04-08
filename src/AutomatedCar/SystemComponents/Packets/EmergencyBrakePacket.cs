namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using AutomatedCar.Models;
    using ReactiveUI;
    using AutomatedCar.Helpers;

    class EmergencyBrakePacket : ReactiveObject, IEmergancyBrakePacket
    {

        private bool activated = false;
        private BrakeStatus currentstatus;
        private string currentstate;

        public bool Activated
        {
            get => this.activated;
            set => this.RaiseAndSetIfChanged(ref this.activated, value);
        }

        public BrakeStatus BrakeStatus
        {
            get => this.currentstatus;
            set => this.RaiseAndSetIfChanged(ref this.currentstatus, value);
        }

        public string BrakeState
        {
            get => this.currentstate;

            set => this.RaiseAndSetIfChanged(ref this.currentstate, value);
        }
    }
}
