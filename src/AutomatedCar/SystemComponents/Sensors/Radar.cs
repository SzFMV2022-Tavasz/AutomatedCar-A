namespace AutomatedCar.SystemComponents.Sensors
{
    using Avalonia;
    using Avalonia.Media;
    using System;
    using AutomatedCar.Models;

    public class Radar : Sensor
    {

        public Radar(AutomatedCar car, int range) 
            : base(car, range)
        {

        }

        public override PolylineGeometry GetSensorArea()
        {
            throw new NotImplementedException();
        }

        public override void RotatetoSensor()
        {
            throw new NotImplementedException();
        }
    }
}
