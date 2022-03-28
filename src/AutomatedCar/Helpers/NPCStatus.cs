namespace AutomatedCar.Helpers
{
    using System.Numerics;

    /// <summary>
    /// Class containing all the information for the NPCEngine.
    /// /// </summary>
    public class NPCStatus
    {
        /// <summary>
        /// Gets or sets Positions/Waypoint of the npc.
        /// </summary>
        public Vector2[] Positions { get; set; }

        /// <summary>
        /// Gets or sets Velocities of the npc. 
        /// </summary>
        public float[] Velocities { get; set; }

        /// <summary>
        /// Gets or sets Rotations of the npc.
        /// </summary>
        public double[] Rotations { get; set; }

        /// <summary>
        /// Gets or sets the current index of the waypoints and rotations.
        /// </summary>
        public int CurrentIdx { get; set; } = 0;

        /// <summary>
        /// Gets or sets the position of the npc in float coordinates.
        /// </summary>
        public Vector2 CurrentPosition { get; set; }
    }
}
