namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPedalPacket
    {
        public int GasPedalLevel { get; }
        public int BreakPedalLevel { get; set; }

        public bool GasPressed { get; set; }
        public bool BreakPressed { get; set; }
    }
}
