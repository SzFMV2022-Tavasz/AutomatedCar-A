namespace AutomatedCar.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;

    public class NPCStatus
    {
        public Vector2[] Positions { get; set; }
        public float[] Velocities { get; set; }
        public double[] Rotations { get; set; }
        public int CurrentIdx { get; set; } = 0;
        public Vector2 CurrentPosition { get; set; }
    }
}
