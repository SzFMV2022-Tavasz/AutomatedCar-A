namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal interface IPedalPacket
    {

        public int GasPedalLevel { get; set; }
        public int BreakPedalLevel { get; set; }
    }
}
