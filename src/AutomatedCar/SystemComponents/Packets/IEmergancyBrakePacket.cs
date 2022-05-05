namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using AutomatedCar.Models;
    using AutomatedCar.Helpers;

    public interface IEmergancyBrakePacket
    {
        bool Activated { get; set; }

        BrakeStatus BrakeStatus { get; set; }

        string BrakeState { get; set; }
    }
}
