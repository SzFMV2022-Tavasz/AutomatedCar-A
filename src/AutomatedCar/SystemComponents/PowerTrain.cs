namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public delegate void ChangeShift(int rpm);

    public class PowerTrain : SystemComponent
    {
        public event ChangeShift changeShiftEvent;

        public PowerTrain(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }
    }
}
