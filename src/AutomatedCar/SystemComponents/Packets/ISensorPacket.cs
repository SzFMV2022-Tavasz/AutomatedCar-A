﻿namespace AutomatedCar.SystemComponents.Packets
{
    using System.Collections.Generic;
    using AutomatedCar.Models;

    public interface ISensorPacket
    {
        ICollection<WorldObject> WorldObjectsInRange { get; set; }
    }
}
