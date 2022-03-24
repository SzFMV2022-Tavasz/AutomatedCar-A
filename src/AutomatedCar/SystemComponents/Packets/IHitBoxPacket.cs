namespace AutomatedCar.SystemComponents.Packets
{
    using System;

    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public interface IHitBoxPacket
    {
        public bool Collided { get; set; }
    }
}
