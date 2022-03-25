namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ISteeringWheelPacket
    {
        int WheelRotation { get; }      //THIS MAY NOT BE AN INT IN THE FUTURE!!!!!!

        int NextPositionX { get; }

        int NextPositionY { get; }

        bool IsBeingRotated { get; set; }
    }
}
