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

        private Circle cPos;
        private Circle cRight;
        private Circle cLeft;

        public Sensor(World world, VirtualFunctionBus virtualFunctionBus, int range, double angleOfView)
            : base(virtualFunctionBus)
        {
            this.world = world;
            
            this.cPos = new Circle(200, 200, "circle.png", 20);

            cPos.Width = 40;
            cPos.Height = 40;
            cPos.ZIndex = 20;
            cPos.Rotation = 45;
            this.world.AddObject(this.cPos);
            this.cRight = new Circle(200, 200, "circle.png", 20);

            cRight.Width = 40;
            cRight.Height = 40;
            cRight.ZIndex = 20;
            cRight.Rotation = 45;
            this.world.AddObject(this.cRight);
            this.cLeft = new Circle(200, 200, "circle.png", 20);

            cLeft.Width = 40;
            cLeft.Height = 40;
            cLeft.ZIndex = 20;
            cLeft.Rotation = 45;
            this.world.AddObject(this.cLeft);

            this.SensorPacket = new SensorPacket();
            this.virtualFunctionBus.SensorPacket = this.SensorPacket;
            this.Range = range;
            this.AngleOfView = angleOfView;
            this.CalculateSensorPolylineGeometry();
        }

        protected void UpdateSensorPositionAndOrientation()
        {
            Matrix translation = Matrix.CreateTranslation(world.ControlledCar.X, world.ControlledCar.Y);
            Matrix rotation = Matrix.CreateRotation((float)(world.ControlledCar.Rotation * Math.PI / 180));

            // (double)this.Range * Math.Tan((double)this.AngleOfView / 2 * (Math.PI / 180)
            var p = SensorPosition.Transform(rotation).Transform(translation);
            var r = RightEdge.Transform(rotation).Transform(translation);
            var l = LeftEdge.Transform(rotation).Transform(translation);

            this.cPos.X = (int)p.X;
            this.cPos.Y = (int)p.Y;
            this.cLeft.X = (int)l.X;
            this.cLeft.Y = (int)l.Y;
            this.cRight.X = (int)r.X;
            this.cRight.Y = (int)r.Y;
            this.FieldOfView = new PolylineGeometry(new List<Point> { p, r, l }, false);
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
