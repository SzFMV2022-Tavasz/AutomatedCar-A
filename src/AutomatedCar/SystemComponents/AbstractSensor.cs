namespace AutomatedCar.SystemComponents
{
    using System.Collections.Generic;
    using AutomatedCar.Models;
    using Avalonia;

    abstract class AbstractSensor
    {
        private List<Point> points;

        public int Range { get; set; }

        public double AngleSensor { get; set; }

        public int RightEdge { get; set; }

        public int LeftEdge { get; set; }

        public int Position { get; set; }

        public List<WorldObject> WorldObjects { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractSensor"/> class.
        /// Default Constructor.
        /// </summary>
        /// <param name="range">Sensor range.</param>
        /// <param name="angleSensor">Sensor angle.</param>
        /// <param name="position">Sensor position.</param>
        public AbstractSensor(int range, double angleSensor, int position)
        {
            this.Range = range;
            this.AngleSensor = angleSensor;
            this.Position = position;
        }

        /// <summary>
        /// Calculates the sensor area.
        /// </summary>
        /// <returns>Retrun to list of sensor point.</returns>
        public abstract List<Point> GetSensorArea();

        /// <summary>
        /// Rotates the angle around the point. (or something similar).
        /// </summary>
        public abstract void RotatetoSensor();
    }
}
