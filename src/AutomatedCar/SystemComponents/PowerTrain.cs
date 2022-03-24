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
            //this.virtualFunctionBus.GearShiftPacket.CurrentGear = Gear.Drive;

            switch (this.car.VirtualFunctionBus.GearShiftPacket.CurrentGear)
            {
                case Gear n when (n == Gear.Drive ): this.DriveGear(30); break;
                case Gear n when (n == Gear.Neutral): this.NeutralGear(); break;
                case Gear n when (n == Gear.Reverse): this.ReverseGear(); break;
                case Gear n when (n == Gear.Park): this.ParkGear(); break;
                default:
                    this.car.VirtualFunctionBus.GearShiftPacket.CurrentGear = Gear.Neutral;
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

                //this.car.Y -= ((int)this.stopwatch2.ElapsedMilliseconds * this.PowerTrainPacket.Speed) / 200;
                //if (this.PowerTrainPacket.Speed<1)
                //{
                //    ;
                //}
                //this.stopwatch2.Restart();
                this.car.Y = this.car.VirtualFunctionBus.SteeringWheelPacket.NextPositionY;
                this.car.X = this.car.VirtualFunctionBus.SteeringWheelPacket.NextPositionX;
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

        public void DriveGear(int maxspeed)//for reverse
        {
            /*Gázpedál meghatározza az autó jelenlegi cél sebességét,*/
            if (this.car.Pedal.PedalPacket.BreakPedalLevel == 0 && this.car.Pedal.PedalPacket.GasPedalLevel > 0) //Gas gas gas
            {
                if (this.PowerTrainPacket.Speed < this.car.Pedal.PedalPacket.GasPedalLevel && this.PowerTrainPacket.Speed < maxspeed)
                {
                    this.PowerTrainPacket.RPM += 11;

                    int adjustedGasLevel = (int)Math.Round(maxspeed * ((float)this.car.Pedal.PedalPacket.GasPedalLevel / 80));

                    if (this.tick > ((maxspeed + 20) - adjustedGasLevel))//50 / (this.car.Pedal.PedalPacket.GasPedalLevel / 10)
                    {
                        this.PowerTrainPacket.Speed += 1;
                        this.tick = 0;
                    }
                }
                else if (this.PowerTrainPacket.Speed > this.car.Pedal.PedalPacket.GasPedalLevel) // RPM / TICK SPEED / 50Tick
                {
                    this.PowerTrainPacket.RPM -= 11;

                    if (this.tick > 10)/// (this.car.Pedal.PedalPacket.GasPedalLevel / 10)
                    {
                        this.PowerTrainPacket.Speed -= 1;
                        this.tick = 0;
                    }
                }
            }
            else if (this.car.Pedal.PedalPacket.BreakPedalLevel == 0 && this.car.Pedal.PedalPacket.GasPedalLevel == 0) //Lassulás
            {
                if (this.PowerTrainPacket.RPM > 1000)
                {
                    this.PowerTrainPacket.RPM -= 11;
                }

                if (this.tick > 50)// Dinamik TODO
                {
                    if (this.PowerTrainPacket.Speed > 0 && this.PowerTrainPacket.Speed > this.car.Pedal.PedalPacket.GasPedalLevel)
                    {

                        if ((this.PowerTrainPacket.Speed - Friction) < 0)
                        {
                            this.PowerTrainPacket.Speed = 0;
                            this.PowerTrainPacket.RPM = 1000;
                        }
                        else
                        {
                            this.PowerTrainPacket.Speed -= Friction;
                        }
                    }

                    this.tick = 0;
                }
            }
            else if (this.car.Pedal.PedalPacket.BreakPedalLevel > 0 && this.car.Pedal.PedalPacket.GasPedalLevel == 0) //Fékezés
            {
                int adjustedbreakLevel = (int)Math.Round(maxspeed * ((float)this.car.Pedal.PedalPacket.BreakPedalLevel / 80));

                RPMDecreaser(55);

                if (this.tick > ((maxspeed + 20) - adjustedbreakLevel)/2) // a pedaltol valtozzon TODO 
                {
                    if (this.PowerTrainPacket.Speed > 0)
                    {
                        if ((this.PowerTrainPacket.Speed - (1 + Friction)) < 0)
                        {
                            this.PowerTrainPacket.Speed = 0;
                            this.PowerTrainPacket.RPM = 1000;
                        }
                        else
                        {
                            this.PowerTrainPacket.Speed -= Friction + 1;
                        }
                    }

                    this.tick = 0;
                }
            }

            this.tick++;
        }

        public void NeutralGear()
        {
            this.car.Pedal.PedalPacket.GasPedalLevel = 0;
            this.car.Pedal.PedalPacket.BreakPedalLevel = 0;
            RPMDecreaser(110);
            SpeedDecreaser(1);
        }

        public void ReverseGear()
        {
            this.car.Pedal.PedalPacket.BreakPedalLevel = 0;
            DriveGear(40);
        }

        public void ParkGear()
        {
            this.car.Pedal.PedalPacket.BreakPedalLevel = 50;
        }

        private void RPMDecreaser(int value)
        {
            if (this.PowerTrainPacket.RPM > 1000) // Basic, can be modified
            {
                int newRPM = this.PowerTrainPacket.RPM -= value;
                if (newRPM >= 1000)
                {
                    this.PowerTrainPacket.RPM = newRPM;
                }
                else
                {
                    this.PowerTrainPacket.RPM = 1000;
                }
            }
        }

        private void SpeedDecreaser(int value)
        {
            if (this.PowerTrainPacket.Speed > 0 && this.tick > 10)/// (this.car.Pedal.PedalPacket.GasPedalLevel / 10)
            {
                this.PowerTrainPacket.Speed -= value;
                this.tick = 0;
            }

            tick++;
        }
    }
}
