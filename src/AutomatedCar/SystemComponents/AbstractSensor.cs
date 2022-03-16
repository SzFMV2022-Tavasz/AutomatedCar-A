namespace AutomatedCar.SystemComponents
{
    using System.Collections.Generic;
    using AutomatedCar.Models;
    using Avalonia;
    using Avalonia.Media;

    abstract class AbstractSensor
    {
        public PolylineGeometry SensorGeometry{ get; set; }

        public int Range { get; set; }

        public double AngleSensor { get; set; }

        public Point RightEdge { get; set; }

        public Point LeftEdge { get; set; }

        public Point Position { get; set; }

        public List<WorldObject> WorldObjects { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractSensor"/> class.
        /// Default Constructor.
        /// </summary>
        /// <param name="range">Sensor range.</param>
        /// <param name="angleSensor">Sensor angle.</param>
        /// <param name="position">Sensor position.</param>
        public AbstractSensor(int range, double angleSensor, Point position)
        {
            this.Range = range;
            this.AngleSensor = angleSensor;
            this.Position = position;
        }

        /// <summary>
        /// Calculates the sensor area.
        /// </summary>
        /// <returns>Retrun to PolylineGeometry sensor.</returns>
        public abstract PolylineGeometry GetSensorArea();

        /// <summary>
        /// Rotates the angle around the point. (or something similar).
        /// </summary>
        public abstract void RotatetoSensor();
    }
}
