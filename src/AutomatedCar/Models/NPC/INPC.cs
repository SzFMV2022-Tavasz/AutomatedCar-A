namespace AutomatedCar.Models.NPC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    interface INPC
    {
        public void Step(int x, int y, double rotation);
    }
}
