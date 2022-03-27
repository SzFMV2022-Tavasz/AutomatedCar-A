namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutomatedCar.SystemComponents.Packets;
    using AutomatedCar.Models;
    using System.Diagnostics;

    /// <summary>
    /// Pedal class, handles gas and break of automated car.
    /// </summary>
    public class Pedal : SystemComponent
    {
        public PedalPacket PedalPacket;
        private AutomatedCar car;
        private bool throttleUp;
        private bool pressedPedal;
        private bool brakeUp;
        private Stopwatch timer = new Stopwatch();

        public Pedal(VirtualFunctionBus virtualFunctionBus, AutomatedCar car)
            : base(virtualFunctionBus)
        {
            this.PedalPacket = new PedalPacket();
            this.virtualFunctionBus.PedalPacket = this.PedalPacket;
            timer.Start();
        }

        public void ToggleUp()
        {
            if (brakeUp)
            {
                brakeUp = false;
            }

            pressedPedal = true;

            if (PedalPacket.BreakPedalLevel == 0 && !throttleUp)
            {
                throttleUp = true;
            }
        }

        public void ToggleDown()
        {
            if (throttleUp)
            {
                throttleUp = false;
            }

            pressedPedal = true;

            if (PedalPacket.GasPedalLevel == 0 && !brakeUp)
            {
                brakeUp = true;
            }
        }

        public override void Process()
        {
            //if (this.throttleUp && this.pressedPedal)
            //{
            //    this.ThrottleUp();
            //    this.pressedPedal = false;
            //}
            //else if (!this.throttleUp && !this.brakeUp && this.pressedPedal)
            //{
            //    this.ThrottleDown();
            //    this.BrakeDown();
            //    this.pressedPedal = false;
            //}
            //else if (this.brakeUp && this.pressedPedal)
            //{
            //    this.BrakeUp();
            //    this.pressedPedal = false;
            //}

            if (this.PedalPacket.GasPressed)
            {
                this.ThrottleUp();
            }
            else if (this.PedalPacket.GasPressed == false)
            {
                this.ThrottleDown();
            }

            if (this.PedalPacket.BreakPressed)
            {
                this.BrakeUp();
            }
            else if (this.PedalPacket.BreakPressed == false)
            {
                this.BrakeDown();
            }


            //this.RPMCalculator();
            //this.SpeedCalculator();
        }

        private void ThrottleUp()
        {
            if ((this.PedalPacket.GasPedalLevel + 10) > 80)
            {
                this.PedalPacket.GasPedalLevel = 80;
            }
            else
            {
                this.PedalPacket.GasPedalLevel += 5;
            }

        }

        private void ThrottleDown()
        {
            if ((this.PedalPacket.GasPedalLevel - 10) < 0)
            {
                this.PedalPacket.GasPedalLevel = 0;
            }
            else
            {
                this.PedalPacket.GasPedalLevel -= 10;
            }
        }

        private void BrakeUp()
        {
            if ((this.PedalPacket.BreakPedalLevel + 10) > 80)
            {
                this.PedalPacket.BreakPedalLevel = 80;
            }
            else
            {
                this.PedalPacket.BreakPedalLevel += 5;
            }

        }

        private void BrakeDown()
        {
            //if ((this.PedalPacket.BreakPedalLevel - 10) < 0)
            //{
            //    this.PedalPacket.BreakPedalLevel = 0;
            //}
            //else
            //{
                this.PedalPacket.BreakPedalLevel = 0;
            //}

        }

        //Ideiglenes, powertrainnél kell majd ez is.
        private void RPMCalculator()
        {
            var value = this.virtualFunctionBus.PowerTrainPacket.RPM + (int)(this.PedalPacket.GasPedalLevel * 0.25) - (int)(this.PedalPacket.BreakPedalLevel * 0.85);

            if (value < 0)
            {
                this.virtualFunctionBus.PowerTrainPacket.RPM = 0;
            }
            else
            {
                this.virtualFunctionBus.PowerTrainPacket.RPM = value;
            }
        }

        // Idegilenes, elv nem is itt kell majd hanem powertrainben
        //private void SpeedCalculator()
        //{
        //    if (timer.ElapsedMilliseconds > 850)
        //    {
        //        this.virtualFunctionBus.PowerTrainPacket.Speed += (this.virtualFunctionBus.PowerTrainPacket.RPM / 500);
        //        timer.Restart();
        //    }
        //}
    }
}
