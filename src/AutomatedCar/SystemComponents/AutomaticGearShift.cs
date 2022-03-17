namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Helpers;
    using AutomatedCar.SystemComponents.Packets;
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
        public GearShiftPacket ShiftPacket;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomaticGearShift"/> class.
        /// </summary>
        /// <param name="virtualFunctionBus">Main communication object</param>
        public AutomaticGearShift(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.ShiftPacket = new GearShiftPacket();
            this.virtualFunctionBus.GearShiftPacket = this.ShiftPacket;
        }

        /// <summary>
        /// This function will change the shifts according to rpm.
        /// </summary>
        public override void Process()
        {
            switch (this.virtualFunctionBus.PowerTrainPacket.RPM)
            {
                case int n when (n >= 1000 && n < 2500) && this.virtualFunctionBus.GearShiftPacket.CurrentGear==Gear.Drive: this.ChangeShift(Shifts.One);break;
                case int n when (n >= 2500 && n < 4500) && this.virtualFunctionBus.GearShiftPacket.CurrentGear == Gear.Drive: this.ChangeShift(Shifts.Two);break;
                case int n when (n >= 4500 && n < 6000) && this.virtualFunctionBus.GearShiftPacket.CurrentGear == Gear.Drive: this.ChangeShift(Shifts.Three);break;
                case int n when (n >= 6000 && n < 8000) && this.virtualFunctionBus.GearShiftPacket.CurrentGear == Gear.Drive: this.ChangeShift(Shifts.Four);break;
                default: this.virtualFunctionBus.GearShiftPacket.CurrentGear = Gear.Neutral;
                    break;
            }

        }

        /// <summary>
        /// Changes the Gears. Esetleges sorrend kényszerítés D>N>P>R.
        /// </summary>
        private void ChangeShift(Shifts shift)
        {
            this.virtualFunctionBus.GearShiftPacket.CurrentShift = shift;
            this.virtualFunctionBus.GearShiftPacket.GearState = shift.ToString();
        }
    }
}
