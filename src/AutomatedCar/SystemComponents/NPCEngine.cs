namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.Models.NPC;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Numerics;

    public class NPCEngine : GameBase
    {
        public List<INPC> NPCs { get; set; }

        public NPCEngine(List<INPC> NPCs)
        {
            this.NPCs = NPCs;
        }

        private void Move(int deltaTime, INPC npc)
        {
            var currentToNextPos = npc.NPCStatus.Positions[(npc.NPCStatus.CurrentIdx + 1) % npc.NPCStatus.Positions.Length] - npc.NPCStatus.CurrentPosition;
            var previousToCurrentPos = npc.NPCStatus.CurrentPosition - npc.NPCStatus.Positions[npc.NPCStatus.CurrentIdx];

            var direction = Vector2.Normalize(currentToNextPos);
            var displacement = direction * npc.NPCStatus.Velocities[npc.NPCStatus.CurrentIdx] * deltaTime;

            var currentToNextDistance = currentToNextPos.Length();
            var previousToCurrentDistance = previousToCurrentPos.Length();
            (npc as WorldObject).Rotation = ((npc.NPCStatus.Rotations[npc.NPCStatus.CurrentIdx] * currentToNextDistance) + (npc.NPCStatus.Rotations[(npc.NPCStatus.CurrentIdx + 1) % npc.NPCStatus.Rotations.Length] * previousToCurrentDistance)) / (currentToNextDistance + previousToCurrentDistance);

            if (displacement.LengthSquared() >= currentToNextPos.LengthSquared())
            {
                displacement = currentToNextPos;
                npc.NPCStatus.CurrentIdx = (npc.NPCStatus.CurrentIdx + 1) % npc.NPCStatus.Positions.Length;
            }

            npc.NPCStatus.CurrentPosition += displacement;
            (npc as WorldObject).X = (int)npc.NPCStatus.CurrentPosition.X;
            (npc as WorldObject).Y = (int)npc.NPCStatus.CurrentPosition.Y;
        }

        protected virtual void OnTick()
        {
            foreach (var item in NPCs)
            {
                this.Move(1, item);
            }
        }

        protected override void Tick()
        {
            this.OnTick();
        }
    }
}
