namespace AutomatedCar.SystemComponents.Packets
{

    using System;

    public class HitBoxPacket : IHitBoxPacket
    {
        private bool collided { get; set; }

        public bool Collided
        {
            get => this.collided;
            set => this.collided = value;
        }
    }
}
