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
        }

        protected void UpdateSensorPositionAndOrientation()
        {
            this.PositionUpdater = new TranslateTransform(this.world.ControlledCar.X, this.world.ControlledCar.Y);
            this.OrientationUpdater = new RotateTransform(this.world.ControlledCar.Rotation);
            this.TransformGroup.Children.Add(this.PositionUpdater);
            this.TransformGroup.Children.Add(this.OrientationUpdater);
            this.FieldOfView.Transform = this.TransformGroup;
        }

        protected abstract PolylineGeometry CalculateSensorPolylineGeometry();

        protected abstract ICollection<WorldObject> GetWorldObjectsInRange();

        protected abstract bool IsInRange(WorldObject worldObject);
    }
}
