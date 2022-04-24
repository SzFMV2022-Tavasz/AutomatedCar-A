namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;

    public class EmergencyBreak : SystemComponent
    {
        private AutomatedCar car;
        private EmergencyBrakePacket emergencyBrakePacket;

        //FELADATOK:
        // REVERSBEN NE MŰKÖDJÖN
        // SEBESSÉG SZERINT LEGYEN SZÁMOLVA A TÁVOLSÁG
        // FÉKEZÉS LOGIKA NE A PEDÁLT HASZNÁLJA
        // DASHBOARDRA KIÍRÁS
        // REFAKTORÁLÁS

        public EmergencyBreak(VirtualFunctionBus virtualFunctionBus, AutomatedCar car)
            : base(virtualFunctionBus)
        {
            this.car = car;
            this.virtualFunctionBus = virtualFunctionBus;
            this.emergencyBrakePacket = new EmergencyBrakePacket();
            this.virtualFunctionBus.EmergancyBrakePacket = this.emergencyBrakePacket;
        }

        /// <summary>
        /// Detektált objektumok vizsgálása a SensorPacketből.
        /// </summary>
        public override void Process()
        {
            if (this.emergencyBrakePacket.Activated && this.virtualFunctionBus.PowerTrainPacket.CorrectedSpeed >= 70)
            {
                this.DisableEBA();
            }
            else if (!this.emergencyBrakePacket.Activated && this.virtualFunctionBus.PowerTrainPacket.CorrectedSpeed < 70)
            {
                this.EnableEBA();
            }

            if (this.emergencyBrakePacket.Activated) // aktiválva van-e az EBA
            {
                ICollection<WorldObject> dangerObjects = this.CollisionObjects();
                if (dangerObjects.Count() > 0)
                {
                    this.ActivateBreak();
                }
            }
        }

        /// <summary>
        /// Visszaadja azokat az objektumokat, amikkel az ütközést nem lehetne elkerülni, és aktiválni kell az EBA-t.
        /// </summary>
        /// <returns>ICollection<WorldObject> objektumok</returns>
        private ICollection<WorldObject> CollisionObjects()
        {
            ICollection<WorldObject> objects = new List<WorldObject>();
            foreach (var item in this.virtualFunctionBus.SensorPacket.WorldObjectsInRange)
            {
                int distanceX = Math.Abs(this.car.X - item.X);
                int distanceY = Math.Abs(this.car.Y - item.Y);
                int distance = (int)Math.Sqrt((Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2)));

                if (distance < 1500) // dummy érték, csak tesztre
                {
                    objects.Add(item);
                }
            }

            return objects;
        }

        /// <summary>
        /// 70 km/h óra fölött inaktiválni kell.
        /// </summary>
        private void DisableEBA()
        {

            this.emergencyBrakePacket.Activated = false;

        }

        /// <summary>
        /// 70 km/h alatt pedig aktiválni kell.
        /// </summary>
        private void EnableEBA()
        {

            this.emergencyBrakePacket.Activated = true;

        }

        /// <summary>
        /// Ha van adott távolságon belül detektált objektum, akkor fékezzen.
        /// </summary>
        private void ActivateBreak()
        {
            if (this.virtualFunctionBus.PowerTrainPacket.Speed > 1)
            {
                this.virtualFunctionBus.PedalPacket.BreakPedalLevel = 100; // dummy logika, csak tesztre
            }
        }

    }
}
