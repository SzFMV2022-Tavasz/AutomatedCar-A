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

        /*
        public PolylineGeometry SensorGeometry{ get; set; }

        public int Range { get; set; }

        public double AngleSensor { get; set; }

        public Point RightEdge { get; private set; }

        public Point LeftEdge { get; private set; }

        public Point Position { get; private set; }

        public List<WorldObject> WorldObjects { get; set; }

        public Sensor(VirtualFunctionBus virtualFunctionBus, Car car, int range) : base(virtualFunctionBus)
        {
            
            this.Position = new Point(car.X + 10, car.Y + 10);



           
        }


        public override void Process()
        {
            throw new System.NotImplementedException();
        }

        protected void SetPosition(Point point)
        {
            this.Position = point;
        }

        /// <summary>
        /// Calculates the sensor area.
        /// </summary>
        /// <returns>Retrun to PolylineGeometry sensor.</returns>
        public abstract PolylineGeometry GetSensorArea();

        /// <summary>
        /// Rotates the angle around the point. (or something similar).
        /// </summary>
        public abstract void RotatetoSensor();*/
    }
}
