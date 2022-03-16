namespace AutomatedCar.SystemComponents
{
    using System;
    using AutomatedCar.Helpers;
    using System.Collections.Generic;
    using AutomatedCar.SystemComponents.Packets;

    /// <summary>
    /// PowerTrain class, handles propulsion of automated car.
    /// </summary>
    public class PowerTrain : SystemComponent
    {
        public PowerTrainPacket PowerTrainPacket;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerTrain"/> class.
        /// </summary>
        /// <param name="virtualFunctionBus">comm object.</param>
        public PowerTrain(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.PowerTrainPacket = new PowerTrainPacket();
            this.virtualFunctionBus.PowerTrainPacket = this.PowerTrainPacket;
        }

        /// <summary>
        /// This method will handle speed, accel, throtle.
        /// </summary>
        public override void Process()
        {
            this.virtualFunctionBus.PowerTrainPacket.RPM = 1000; //üresjárat.

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
    }
}
