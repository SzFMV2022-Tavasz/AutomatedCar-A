namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutomatedCar.SystemComponents.Packets;

    /// <summary>
    /// Pedal class, handles gas and break of automated car.
    /// </summary>
    public class Pedal : SystemComponent
    {
        public PedalPacket PedalPacket;

        public Pedal(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.PedalPacket = new PedalPacket();
            this.virtualFunctionBus.PedalPacket = this.PedalPacket;
        }

        public override void Process()
        {
            this.virtualFunctionBus.PedalPacket.GasPedalLevel = 20;
            this.virtualFunctionBus.PedalPacket.BreakPedalLevel = 0;
        }
    }
}
