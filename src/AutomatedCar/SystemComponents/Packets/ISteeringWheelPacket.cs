namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ISteeringWheelPacket
    {
        int WheelRotation { get; }

        int NextPositionX { get; }

        int NextPositionY { get; }

        bool IsBeingRotated { get; set; }

        bool IsLKAActive { get; set; }

        string LKAState { get; set; }
    }
}
