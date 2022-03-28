namespace AutomatedCar.SystemComponents.Sensors
{
    using System;
    using System.Collections.Generic;
    using AutomatedCar.Models;
    using Avalonia;
    using Avalonia.Media;

    public class Radar : Sensor
    {

        public Radar(ref World world, VirtualFunctionBus virtualFunctionBus)
            : base(ref world, virtualFunctionBus, 200, 60)
        {
            //jelenelgnem használt ignore
        }

        public Radar( VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus, 200, 60)
        {
            // ezt használjuk.
            this.world = World.Instance;
            this.FieldOfView = this.GetRadarGeometry();

        }

          //Vagy valami hasonlóvalkéne megszerezni a wordOBJket  Pub fügvény ami az adot rangeböl kuszedni öket 
          //  public List<WorldObject> GetWorldObjectsInRange()
          //  => World.Instance.GetWorldObjectsInsideTriangle(??? );

        public override void Process()
        {
            this.UpdateSensorPosition();
            this.virtualFunctionBus.SensorPacket.WorldObjectsInRange = GetWorldObjectsInRange();
        }

        protected override ICollection<WorldObject> GetWorldObjectsInRange()
        {
            return this.world.WorldObjects.FindAll(IsInRange);
        }

        protected override bool IsInRange(WorldObject worldObject)
        {
            if (worldObject.Collideable)
            {
                bool flag = false;
                foreach (var geometry in worldObject.Geometries)
                {
                    if (geometry.Bounds.Intersects(this.FieldOfView.Bounds))
                    {
                        flag = true;
                    }
                }

                return flag;
            }
            else
            {
                return false;
            }
        }

        protected override PolylineGeometry GetRadarGeometry()
        {
            // start(honan tudom hol a senzor helye ? ) , jobb , bal ide valami szögszámolás kell hogy mekkora a héromzög
            Point[] p = { new Point(/*Aszenzorhelye*/100,/*Aszenzorhelye*/ 100), new Point(/*Aszenzorhelye+*/this.Range,
                /*Aszenzorhelye+*/this.Range - 50), new Point(/*Aszenzorhelye+*/this.Range,/*Aszenzorhelye+*/ this.Range + 50) };
            return new PolylineGeometry(p, false);
        }
    }
}
