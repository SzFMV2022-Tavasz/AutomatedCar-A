namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPowerTrainPacket
    {
        int RPM { get; set; }

        int Speed { get; set; }

        int CorrectedSpeed { get; set; }
    }
}
