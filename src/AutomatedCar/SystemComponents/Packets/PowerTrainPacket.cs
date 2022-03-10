namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PowerTrainPacket: ReactiveObject
    {
        public int RPM { get; set; }
    }
}
