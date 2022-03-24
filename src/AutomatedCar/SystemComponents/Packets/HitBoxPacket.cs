namespace AutomatedCar.SystemComponents.Packets
{

    using System;

    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class HitBoxPacket : IHitBoxPacket
    {
        private bool collided { get; set; }

        public bool Collided
        {
            get => this.Collided;
            set => this.Collided = value;
        }
    }
}
