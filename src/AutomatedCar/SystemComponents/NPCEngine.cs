namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using System;
    using System.Drawing;
    using System.Numerics;

    public class NPCEngine : GameBase
    {
        private Vector2[] positions;
        private float[] velocities;
        private double[] rotations;
        private int currentIdx = 0;
        private Vector2 currentPosition;

        public NPCEngine(Vector2[] positions, float[] velocities)
        {
            this.positions = positions;
            this.velocities = velocities;

            this.currentPosition = this.positions[0];

            this.rotations = new double[this.positions.Length];
            for (int i = 0; i < this.positions.Length; i++)
            {
                var vector = this.positions[(i + 1) % this.positions.Length] - this.positions[i];

                var degree = Math.Atan2(vector.Y, vector.X);
                this.rotations[i] = (degree * (180 / Math.PI)) + 90;
            }
        }

        public WorldObject Npc { get; private set; }

        public void SetNpc(WorldObject npc)
        {
            this.Npc = npc;
            this.Npc.X = (int)this.currentPosition.X;
            this.Npc.Y = (int)this.currentPosition.Y - 120;
        }

        protected Vector2 NextPositionPoint
        {
            get { return this.positions[(this.currentIdx + 1) % this.positions.Length]; }
        }

        protected Vector2 PreviousPositionPoint
        {
            get { return this.positions[this.currentIdx]; }
        }

        protected double PreviousRotationPoint
        {
            get { return this.rotations[this.currentIdx]; }
        }

        protected double NextRotationPoint
        {
            get { return this.rotations[(this.currentIdx + 1) % this.rotations.Length]; }
        }

        private void Move(int deltaTime)
        {
            var currentToNextPos = this.NextPositionPoint - this.currentPosition;
            var previousToCurrentPos = this.currentPosition - this.PreviousPositionPoint;

            var direction = Vector2.Normalize(currentToNextPos);
            var displacement = direction * this.velocities[this.currentIdx] * deltaTime;

            var currentToNextDistance = currentToNextPos.Length();
            var previousToCurrentDistance = previousToCurrentPos.Length();
            this.Npc.Rotation = ((this.PreviousRotationPoint * currentToNextDistance) + (this.NextRotationPoint * previousToCurrentDistance)) / (currentToNextDistance + previousToCurrentDistance);

            if (displacement.LengthSquared() >= currentToNextPos.LengthSquared())
            { // steps over next position
                this.currentIdx = (this.currentIdx + 1) % this.positions.Length;
            }

            this.currentPosition += displacement;
            this.Npc.X = (int)this.currentPosition.X;
            this.Npc.Y = (int)this.currentPosition.Y;
        }

        protected virtual void OnTick()
        {
            this.Move(1);
        }

        protected override void Tick()
        {
            this.OnTick();
        }
    }
}
