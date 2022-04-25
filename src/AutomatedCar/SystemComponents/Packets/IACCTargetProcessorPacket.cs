namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IACCTargetProcessorPacket
    {
        public int DriverTarget { get; set; }
        public int ActualTarget { get; }
    }
}
