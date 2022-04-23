namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.SystemComponents.Packets;
    using System.Collections.Generic;

    public class VirtualFunctionBus : GameBase
    {
        private List<SystemComponent> components = new List<SystemComponent>();

        public ISensorPacket RadarPacket { get; set; }

        public ISensorPacket CameraPacket { get; set; }

        public IHitBoxPacket HitBoxPacket { get; set; }

        public IGearShiftPacket GearShiftPacket { get; set; }

        public IPowerTrainPacket PowerTrainPacket { get; set; }

        public IPedalPacket PedalPacket { get; set; }

        public ISteeringWheelPacket SteeringWheelPacket { get; set; }

        public IControllerPacket<double> ControllerPacket { get; set; }

        public void RegisterComponent(SystemComponent component)
        {
            this.components.Add(component);
        }

        protected override void Tick()
        {
            foreach (SystemComponent component in this.components)
            {
                component.Process();
            }
        }
    }
}