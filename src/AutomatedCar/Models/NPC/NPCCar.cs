namespace AutomatedCar.Models.NPC
{
    using global::AutomatedCar.SystemComponents;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class NPCCar : Car
    {
        public NPCCar(NPCEngine engine, int x, int y, string filename)
            : base(x, y, filename)
        {
            this.Engine = engine;
            this.Engine.SetNpc(this);
            this.Engine.Start();
        }

        public NPCEngine Engine { get; set; }
    }
}
