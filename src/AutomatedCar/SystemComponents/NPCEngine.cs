namespace AutomatedCar.SystemComponents
{
    using System.Collections.Generic;
    using System.Numerics;
    using AutomatedCar.Models;

    /// <summary>
    /// Logic responsible for the movement of the npcs.
    /// </summary>
    public class NPCEngine : GameBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCEngine"/> class.
        /// </summary>
        /// <param name="NPCs">List of npcs.</param>
        public NPCEngine(List<NPC> npcs)
        {
            this.NPCs = npcs;
        }

        /// <summary>
        /// Gets or sets the list of the npcs.
        /// </summary>
        public List<NPC> NPCs { get; set; }

        /// <summary>
        /// GameBase Tick method.
        /// </summary>
        protected override void Tick()
        {
            foreach (var npc in this.NPCs)
            {
                this.Move(1, npc);
            }
        }

        /// <summary>
        /// Calculating the next position and rotation of the npc.
        /// </summary>
        /// <param name="deltaTime">Delta time.</param>
        /// <param name="npc">Npc to be moved.</param>
        private void Move(int deltaTime, NPC npc)
        {
            var currentToNextPos = npc.NPCStatus.Positions[(npc.NPCStatus.CurrentIdx + 1) % npc.NPCStatus.Positions.Length] - npc.NPCStatus.CurrentPosition;
            var previousToCurrentPos = npc.NPCStatus.CurrentPosition - npc.NPCStatus.Positions[npc.NPCStatus.CurrentIdx];

            var direction = Vector2.Normalize(currentToNextPos);
            var displacement = direction * npc.NPCStatus.Velocities[npc.NPCStatus.CurrentIdx] * deltaTime;

            var currentToNextDistance = currentToNextPos.Length();
            var previousToCurrentDistance = previousToCurrentPos.Length();
            npc.Rotation = ((npc.NPCStatus.Rotations[npc.NPCStatus.CurrentIdx] * currentToNextDistance) + (npc.NPCStatus.Rotations[(npc.NPCStatus.CurrentIdx + 1) % npc.NPCStatus.Rotations.Length] * previousToCurrentDistance)) / (currentToNextDistance + previousToCurrentDistance);

            if (displacement.LengthSquared() >= currentToNextPos.LengthSquared())
            {
                displacement = currentToNextPos;
                npc.NPCStatus.CurrentIdx = (npc.NPCStatus.CurrentIdx + 1) % npc.NPCStatus.Positions.Length;
            }

            npc.NPCStatus.CurrentPosition += displacement;
            npc.X = (int)npc.NPCStatus.CurrentPosition.X;
            npc.Y = (int)npc.NPCStatus.CurrentPosition.Y;
        }
    }
}
