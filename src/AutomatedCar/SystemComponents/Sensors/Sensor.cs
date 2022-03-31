namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using Avalonia.Media;

    //reaktiv obj 
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
            Matrix rotation = Matrix.CreateRotation((float)(world.ControlledCar.Rotation));
            Matrix translation = Matrix.CreateTranslation(world.ControlledCar.X - SensorPosition.X, world.ControlledCar.Y - SensorPosition.Y);

            SensorPosition = SensorPosition.Transform(rotation);
            RightEdge = RightEdge.Transform(rotation);
            LeftEdge = LeftEdge.Transform(rotation);
            // (double)this.Range * Math.Tan((double)this.AngleOfView / 2 * (Math.PI / 180)
            SensorPosition = SensorPosition.Transform(translation);
            RightEdge = RightEdge.Transform(translation);
            LeftEdge = LeftEdge.Transform(translation);

            this.SensorPacket.XCord = (int)this.SensorPosition.X;
            this.SensorPacket.YCord = (int)this.SensorPosition.Y;
            this.SensorPacket.LeftEdgeX = (int)this.LeftEdge.X;
            this.SensorPacket.LeftEdgeY = (int)this.LeftEdge.Y;
            this.SensorPacket.RightEdgeX = (int)this.RightEdge.X;
            this.SensorPacket.RightEdgeY = (int)this.RightEdge.Y;

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
