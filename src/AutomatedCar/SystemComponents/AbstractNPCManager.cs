namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models.NPC;
    using Avalonia;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    abstract class AbstractNPCManager : GameBase
    {
        public List<INPC> NPCs { get; set; }
        public List<Point> carPoints { get; set; }

        public AbstractNPCManager()
        {

        }

        public abstract void LoadPoints();

        public abstract void ClaculatePosition();
    }
}
