namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PedalPacket : ReactiveObject, IPedalPacket
    {
        private int gasLevel;
        private int breakLevel;

        public int GasPedalLevel { get => this.gasLevel; set => this.RaiseAndSetIfChanged(ref this.gasLevel, value); }
        public int BreakPedalLevel { get =>this.breakLevel; set => this.RaiseAndSetIfChanged(ref this.breakLevel, value); }
    }
}
