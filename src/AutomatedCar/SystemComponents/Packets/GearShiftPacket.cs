namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Helpers;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class GearShiftPacket: ReactiveObject
    {
        public Gear CurrentGear { get; set; }
        public Shifts CurrentShift { get; set; }
    }
}
