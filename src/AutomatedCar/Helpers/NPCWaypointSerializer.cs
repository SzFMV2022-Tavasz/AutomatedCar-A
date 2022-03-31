namespace AutomatedCar.Helpers
{
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Class for read or write npc waypoint data.
    /// </summary>
    public class NPCWaypointSerializer : ISerializer<string[][]>
    {
        private string[][] _data;


        /// <inheritdoc/>
        public string[][] Data
        {
            get => this._data;
            private set => this._data = value;
        }

        /// <inheritdoc/>
        public void ClearData()
        {
            this.Data = null;
        }

        /// <inheritdoc/>
        public void StoreData(string[][] data)
        {
            this.Data = data;
        }

        /// <inheritdoc/>
        public string[][] Deserialize(string path)
        {
            if (this.Data == null)
            {
                string stringData;
                string[] waypoints;

                using (StreamReader reader = new StreamReader(path))
                {
                    stringData = reader.ReadToEnd();
                }

                waypoints = stringData.Split("\n");

                this.Data = new string[waypoints.Count()][];
                for (int i = 0; i < waypoints.Count(); i++)
                {
                    this.Data[i] = waypoints[i].Split(';');
                }
            }

            return this.Data;
        }

        /// <inheritdoc/>
        public void Serialize(string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(this.ToString());
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            string waypoints = string.Empty;
            for (int i = 0; i < this.Data?.Count(); i++)
            {
                for (int j = 0; j < this.Data[i].Count(); j++)
                {
                    waypoints += $"{this.Data[i][j]};";
                }

                waypoints = waypoints.Remove(waypoints.Count() - 1, 1) + "\n";
            }

            return waypoints;
        }
    }
}