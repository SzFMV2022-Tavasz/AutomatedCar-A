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

        private bool gaspressed;
        private bool breakpressed;
        public int GasPedalLevel { get => this.gasLevel; set => this.RaiseAndSetIfChanged(ref this.gasLevel, value); }
        public int BreakPedalLevel { get =>this.breakLevel; set => this.RaiseAndSetIfChanged(ref this.breakLevel, value); }

        public bool GasPressed { get => this.gaspressed; set => this.RaiseAndSetIfChanged(ref this.gaspressed, value); }
        public bool BreakPressed { get => this.breakpressed; set => this.RaiseAndSetIfChanged(ref this.breakpressed, value); }
    }
}
