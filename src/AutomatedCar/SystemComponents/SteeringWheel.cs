namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SteeringWheel : SystemComponent
    {
        public SteeringWheel(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
        }

        public override void Process()
        {
            var a = 5 + 5;
        }
    }
}
