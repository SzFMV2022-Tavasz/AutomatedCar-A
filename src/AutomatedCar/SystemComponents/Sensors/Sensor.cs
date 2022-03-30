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

        protected Point SensorPosition { get; set; }

        protected Point RightEdge { get; set; }

        protected Point LeftEdge { get; set; }

        protected PolylineGeometry FieldOfView { get; set; }

        protected int Range { get; set; }

        protected double AngleOfView { get; set; }

        protected TranslateTransform PositionUpdater;
        protected RotateTransform OrientationUpdater;
        protected TransformGroup TransformGroup;


        public Sensor(World world, VirtualFunctionBus virtualFunctionBus, int range, double angleOfView)
            : base(virtualFunctionBus)
        {
            this.world = world;
            this.SensorPacket = new SensorPacket();
            this.virtualFunctionBus.SensorPacket = this.SensorPacket;
            this.Range = range;
            this.AngleOfView = angleOfView;
            this.CalculateSensorPolylineGeometry();
        }

        protected void UpdateSensorPositionAndOrientation()
        {
            Matrix translation = Matrix.CreateTranslation(world.ControlledCar.X - SensorPosition.X, world.ControlledCar.Y - SensorPosition.Y);
            Matrix rotation = Matrix.CreateRotation(world.ControlledCar.Rotation);

            SensorPosition = SensorPosition.Transform(translation);
            RightEdge = RightEdge.Transform(translation);
            LeftEdge = LeftEdge.Transform(translation);
            SensorPosition = SensorPosition.Transform(rotation);
            RightEdge = RightEdge.Transform(rotation);
            LeftEdge = LeftEdge.Transform(rotation);

            this.FieldOfView = new PolylineGeometry(new List<Point> { this.SensorPosition, this.RightEdge, this.LeftEdge }, false);
        }

        protected void CalculateSensorPolylineGeometry()
        {
            this.SensorPosition = new Point(480, 1425);
            this.RightEdge = new Point(480 + 200, 1425 + 100);
            this.LeftEdge = new Point(480 + 200, 1425 - 100);
            this.FieldOfView = new PolylineGeometry(new List<Point> { SensorPosition, RightEdge, LeftEdge }, false);
        }

        protected abstract ICollection<WorldObject> GetWorldObjectsInRange();

        protected abstract bool IsInRange(WorldObject worldObject);
    }
}
