namespace AutomatedCar.Models
{
    using Helpers;

    /// <summary>
    /// NPC object. WorldObject with NPCStatus property.
    /// </summary>
    public class NPC : WorldObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NPC"/> class.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="filename">Image name.</param>
        public NPC(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.NPCStatus = new NPCStatus();
            this.ZIndex = 10;
        }

        /// <summary>
        /// Gets or sets npc's status.
        /// </summary>
        public NPCStatus NPCStatus { get; set; }
    }
}
