namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using Avalonia.Media;

    public abstract class Sensor : SystemComponent
    {

        public SensorPacket SensorPacket;
        protected World world;

        protected PolylineGeometry FieldOfView { get; private set; }

        public Sensor(ref World world, VirtualFunctionBus virtualFunctionBus, int range, double angleOfView)
            : base(virtualFunctionBus)
        {
            this.world = world;
            this.SensorPacket = new SensorPacket();
            this.virtualFunctionBus.SensorPacket = this.SensorPacket;
            this.InitSensor(range, angleOfView);
        }

        private void InitSensor(int range, double angleOfView)
        {
            this.FieldOfView = new PolylineGeometry(new List<Point>() { new Point(2, 3), new Point(4, 5), new Point(6, 7) }, true);
        }

        protected abstract ICollection<WorldObject> GetWorldObjectsInRange();

        protected abstract bool IsInRange(WorldObject worldObject);
    }
}
