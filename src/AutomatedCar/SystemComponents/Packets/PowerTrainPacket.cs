﻿// <copyright file="PowerTrainPacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets

{
    using ReactiveUI;

    public class PowerTrainPacket: ReactiveObject, IPowerTrainPacket
    {
        private int rpm;
        private int speed; //közvetlenül összefügg az rpm el
        private int acceleration;

        public int RPM
        {
            get => this.rpm;
            set => this.RaiseAndSetIfChanged(ref this.rpm, value);
        }

        public int Speed
        {
            get => this.speed;
            set => this.RaiseAndSetIfChanged(ref this.speed, value);
        }

        public int Acceleration
        {
            get => this.acceleration;
            set => this.RaiseAndSetIfChanged(ref this.acceleration, value);
        }
    }
}
