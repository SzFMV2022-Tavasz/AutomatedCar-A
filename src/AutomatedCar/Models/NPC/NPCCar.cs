namespace AutomatedCar.Models.NPC
{
    using global::AutomatedCar.SystemComponents;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Helpers;

    public class NPCCar : Car, INPC
    {
        public NPCCar(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.NPCStatus = new NPCStatus();
        }
        public NPCStatus NPCStatus { get; set; }
    }
}
