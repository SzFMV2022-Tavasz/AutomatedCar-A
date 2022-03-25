namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.Models.NPC;
    using System;
    using System.Drawing;
    using System.Numerics;

    public class NPCEngine : GameBase
    {
        //private Vector2[] positions;
        //private float[] velocities;
        //private double[] rotations;
        //private int currentIdx = 0;
        //private Vector2 currentPosition;

        public NPCCar Car { get; set; }

        public NPCEngine(Vector2[] positions, float[] velocities, NPCCar car)
        {
            this.Car = car;

            //this.positions = positions;
            this.Car.NPCStatus.Positions = positions;
            //this.velocities = velocities;
            this.Car.NPCStatus.Velocities = velocities;

            //this.currentPosition = this.positions[0];
            this.Car.NPCStatus.CurrentPosition = this.Car.NPCStatus.Positions[0];

            //this.rotations = new double[this.positions.Length];
            this.Car.NPCStatus.Rotations = new double[this.Car.NPCStatus.Positions.Length];
            //for (int i = 0; i < this.positions.Length; i++)
            //{
            //    var vector = this.positions[(i + 1) % this.positions.Length] - this.positions[i];

            //    var degree = Math.Atan2(vector.Y, vector.X);
            //    this.rotations[i] = (degree * (180 / Math.PI)) + 90;
            //}
            for (int i = 0; i < this.Car.NPCStatus.Positions.Length; i++)
            {
                var vector = this.Car.NPCStatus.Positions[(i + 1) % this.Car.NPCStatus.Positions.Length] - this.Car.NPCStatus.Positions[i];

                var degree = Math.Atan2(vector.Y, vector.X);
                this.Car.NPCStatus.Rotations[i] = (degree * (180 / Math.PI)) + 90;
            }
        }

        //public WorldObject Npc { get; private set; }

        //public void SetNpc(WorldObject npc)
        //{
        //    this.Npc = npc;
        //    this.Npc.X = (int)this.currentPosition.X;
        //    this.Npc.Y = (int)this.currentPosition.Y - 120;
        //}

        //protected Vector2 NextPositionPoint
        //{
        //    get { return this.positions[(this.currentIdx + 1) % this.positions.Length]; }
        //}

        protected Vector2 NextPositionPoint
        {
            get { return this.Car.NPCStatus.Positions[(this.Car.NPCStatus.CurrentIdx + 1) % this.Car.NPCStatus.Positions.Length]; }
        }

        //protected Vector2 PreviousPositionPoint
        //{
        //    get { return this.positions[this.currentIdx]; }
        //}

        protected Vector2 PreviousPositionPoint
        {
            get { return this.Car.NPCStatus.Positions[this.Car.NPCStatus.CurrentIdx]; }
        }

        //protected double PreviousRotationPoint
        //{
        //    get { return this.rotations[this.currentIdx]; }
        //}

        protected double PreviousRotationPoint
        {
            get { return this.Car.NPCStatus.Rotations[this.Car.NPCStatus.CurrentIdx]; }
        }

        //protected double NextRotationPoint
        //{
        //    get { return this.rotations[(this.currentIdx + 1) % this.rotations.Length]; }
        //}

        protected double NextRotationPoint
        {
            get { return this.Car.NPCStatus.Rotations[(this.Car.NPCStatus.CurrentIdx + 1) % this.Car.NPCStatus.Rotations.Length]; }
        }

        private void Move(int deltaTime)
        {
            //var currentToNextPos = this.NextPositionPoint - this.currentPosition;
            var currentToNextPos = this.NextPositionPoint - this.Car.NPCStatus.CurrentPosition;
            //var previousToCurrentPos = this.currentPosition - this.PreviousPositionPoint;
            var previousToCurrentPos = this.Car.NPCStatus.CurrentPosition - this.PreviousPositionPoint;

            var direction = Vector2.Normalize(currentToNextPos);
            var displacement = direction * this.Car.NPCStatus.Velocities[this.Car.NPCStatus.CurrentIdx] * deltaTime;

            var currentToNextDistance = currentToNextPos.Length();
            var previousToCurrentDistance = previousToCurrentPos.Length();
            //this.Npc.Rotation = ((this.PreviousRotationPoint * currentToNextDistance) + (this.NextRotationPoint * previousToCurrentDistance)) / (currentToNextDistance + previousToCurrentDistance);
            this.Car.Rotation = ((this.PreviousRotationPoint * currentToNextDistance) + (this.NextRotationPoint * previousToCurrentDistance)) / (currentToNextDistance + previousToCurrentDistance);

            if (displacement.LengthSquared() >= currentToNextPos.LengthSquared())
            { // steps over next position
                this.Car.NPCStatus.CurrentIdx = (this.Car.NPCStatus.CurrentIdx + 1) % this.Car.NPCStatus.Positions.Length;
            }

            this.Car.NPCStatus.CurrentPosition += displacement;
            //this.Npc.X = (int)this.currentPosition.X;
            this.Car.X = (int)this.Car.NPCStatus.CurrentPosition.X;
            //this.Npc.Y = (int)this.currentPosition.Y;
            this.Car.Y = (int)this.Car.NPCStatus.CurrentPosition.Y;
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
