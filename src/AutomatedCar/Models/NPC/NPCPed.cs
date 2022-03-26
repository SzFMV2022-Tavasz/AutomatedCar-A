namespace AutomatedCar.Models.NPC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Helpers;

    class NPCPed : Car, INPC
    {
        public NPCPed(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.NPCStatus = new NPCStatus();
        }
        public NPCStatus NPCStatus { get; set; }
    }
}
