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
            Matrix translation = Matrix.CreateTranslation(world.ControlledCar.X, world.ControlledCar.Y);
            Matrix rotation = Matrix.CreateRotation((world.ControlledCar.Rotation * Math.PI) / 180.0);
            var p = SensorPosition.Transform(rotation).Transform(translation);
            var r = RightEdge.Transform(rotation).Transform(translation);
            var l = LeftEdge.Transform(rotation).Transform(translation);
            this.FieldOfView = new PolylineGeometry(new List<Point> { p, r, l }, false);

            //DEBUG MODE
            this.SensorPacket.XCord = (int)p.X;
            this.SensorPacket.YCord = (int)p.Y;
            this.SensorPacket.LeftEdgeX = (int)l.X;
            this.SensorPacket.LeftEdgeY = (int)l.Y;
            this.SensorPacket.RightEdgeX = (int)r.X;
            this.SensorPacket.RightEdgeY = (int)r.Y;
            //this.SensorPacket.FileNames = new List<string> { "Anyuam "," apukam"}; // nem ide valo lol 
        }

        protected void CalculateSensorPolylineGeometry()
        {
            this.SensorPosition = new Point(0, 0);
            this.RightEdge = this.SensorPosition + new Point(100, -200);
            this.LeftEdge = this.SensorPosition + new Point(-100, -200);
            this.FieldOfView = new PolylineGeometry(new List<Point> { SensorPosition, RightEdge, LeftEdge }, false);
        }

        protected abstract ICollection<WorldObject> GetWorldObjectsInRange();

        protected abstract bool IsInRange(WorldObject worldObject);
    }
}
