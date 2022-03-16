// <copyright file="PowerTrainPacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets

{
    using ReactiveUI;

    public class PowerTrainPacket: ReactiveObject,IPowerTrainPacket
    {
        private int rpm;

        public int RPM 
        {
            get => this.rpm;
            set => this.RaiseAndSetIfChanged(ref this.rpm, value);
        }
    }
}
