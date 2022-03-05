namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// AutomaticGearShift class, which handles the gear changes.
    /// </summary>
    public class AutomaticGearShift : SystemComponent
    {
        private enum Gear
        {
            Park,
            Reverse,
            Neutral,
            Drive
        }

        private enum Shifts
        {
            One,
            Two,
            Three,
            Four
        }

        private PowerTrain powerTrain;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomaticGearShift"/> class.
        /// </summary>
        /// <param name="virtualFunctionBus">Main communication object</param>
        /// <param name="powerTrain">Power train of the automated car</param>
        public AutomaticGearShift(VirtualFunctionBus virtualFunctionBus, PowerTrain powerTrain)
            : base(virtualFunctionBus)
        {
            this.powerTrain = powerTrain;
        }

        /// <summary>
        /// This function will change the shifts according to rpm.
        /// </summary>
        public override void Process()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Changes the Gears.
        /// </summary>
        public void ChangeShift()
        {

        }
    }
}
