namespace AutomatedCar.Models.NPC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class NPCCar : Car, INPC
    {
        public NPCCar(int x, int y, string filename)
            : base(x, y, filename)
        {

        }
        public void Step(int x, int y, double rotation)
        {
            throw new NotImplementedException();
        }
    }
}
