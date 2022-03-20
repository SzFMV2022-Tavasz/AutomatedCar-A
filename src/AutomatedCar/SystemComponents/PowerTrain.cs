namespace AutomatedCar.SystemComponents
{
    using System;
    using AutomatedCar.Helpers;
    using AutomatedCar.SystemComponents.Packets;
    using AutomatedCar.Models;
    using System.Timers;
    using System.Diagnostics;

    /// <summary>
    /// PowerTrain class, handles propulsion of automated car.
    /// </summary>
    public class PowerTrain : SystemComponent
    {
        public PowerTrainPacket PowerTrainPacket;

        private static int Friction = 1;
        private static double Acceleration = 1.5;
        private static double BrakePower = 1.5;
        private static int GasTemporary = 0; //ideiglenes
        private static int Brake = 0; //ideiglenes
        private static TimeSpan timeSpan;
        private Stopwatch stopwatch = new Stopwatch();
        private Stopwatch stopwatch2 = new Stopwatch();
        private AutomatedCar car;
        private int tick = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerTrain"/> class.
        /// </summary>
        /// <param name="virtualFunctionBus">comm object.</param>
        public PowerTrain(VirtualFunctionBus virtualFunctionBus, AutomatedCar car)
            : base(virtualFunctionBus)
        {
            this.car = car;
            this.PowerTrainPacket = new PowerTrainPacket();
            this.PowerTrainPacket.RPM = 1000; //üresjárat.
            //this.PowerTrainPacket.Speed = 20; //ideiglenes
            this.stopwatch.Start();
            this.stopwatch2.Start();
            this.virtualFunctionBus.PowerTrainPacket = this.PowerTrainPacket;
        }

        /// <summary>
        /// This method will handle speed, accel, throtle.
        /// </summary>
        public override void Process()
        {
            this.virtualFunctionBus.GearShiftPacket.CurrentGear = Gear.Drive;

            switch (this.virtualFunctionBus.GearShiftPacket.CurrentGear)
            {
                case Gear n when (n == Gear.Drive ): this.DriveGear(); break;
                case Gear n when (n == Gear.Neutral): this.NeutralGear(); break;
                case Gear n when (n == Gear.Reverse): this.ReverseGear(); break;
                case Gear n when (n == Gear.Park): this.ParkGear(); break;
                default:
                    this.virtualFunctionBus.GearShiftPacket.CurrentGear = Gear.Neutral;
                    break;
            }

            if (this.PowerTrainPacket.Speed != 0)
            {
                //RealPoz= this.PowerTrainPacket.Speed / 8;

                //if (stopwatch2.ElapsedMilliseconds > 1002 - (this.PowerTrainPacket.Speed*10))//)
                //{
                //    World.Instance.ControlledCar.Y -= (int)stopwatch2.ElapsedMilliseconds / 100;
                //    stopwatch2.Restart();
                //}

                this.car.Y -= ((int)this.stopwatch2.ElapsedMilliseconds * this.PowerTrainPacket.Speed) / 200;
                if (this.PowerTrainPacket.Speed<1)
                {
                    ;
                }
                this.stopwatch2.Restart();
            }

            /*Váltó figyelése, (gear) (D), N, P, R*/
            /*
            Sebesség figyelése mielőtt elegetteszünk a váltónak és pedáloknak (mi történjen ha) #másodlagos feladat.

            pedál figyelése, gyorsulás számítása,

            tick-ként x el megváltoztatni az rpm(sebesség mert úgy határoztunk hogy közvetlenül összefügg)
            állását

            akt sebesség(pixelben) számítása rpm és fokozat alapján

            irány >> kövi koordináta számítása sebesség és kormány poz alapján

            Autó mozgatása a világban

            Vészfékezés implementálása

            legellenállás állandó*/
        }

        public void DriveGear()
        {
            /*Gázpedál meghatározza az autó jelenlegi cél sebességét,*/
            if (Brake == 0 && GasTemporary > 0)
            {
                if ((this.stopwatch.Elapsed).TotalMilliseconds > (GasTemporary - this.PowerTrainPacket.Speed) * Acceleration && this.PowerTrainPacket.Speed < GasTemporary)
                {
                    this.PowerTrainPacket.Speed += 1;
                    this.PowerTrainPacket.RPM += 100;
                    this.stopwatch.Restart();
                }
            }
            else if (Brake == 0 && GasTemporary == 0)
            {
                if (/*(this.stopwatch.Elapsed - timeSpan).TotalMilliseconds > 1000*/ this.tick > 50)
                {
                    if (this.PowerTrainPacket.Speed > 0)
                    {
                        if ((this.PowerTrainPacket.Speed - Friction)<0)
                        {
                            this.PowerTrainPacket.Speed = 0;
                            this.PowerTrainPacket.RPM = 1000;
                        }
                        else
                        {
                            this.PowerTrainPacket.Speed -= Friction;
                            if (this.PowerTrainPacket.RPM > 1000)
                            {
                                this.PowerTrainPacket.RPM -= 50;
                            }
                        }
                    }

                    timeSpan = stopwatch.Elapsed;
                    this.tick = 0;
                }
            }

            tick++;
        }

        public void NeutralGear()
        {
            //rpm 
        }

        public void ReverseGear()
        {

        }

        public void ParkGear()
        {
            //fék
        }
    }
}
