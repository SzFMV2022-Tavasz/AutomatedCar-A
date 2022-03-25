namespace AutomatedCar.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class NPCWaypointSerializer : ISerializer<string[][]>
    {
        private string[][] _data;

        public string[][] Data
        {
            get => this._data;
            private set => this._data = value;
        }

        public void ClearData()
        {
            this._data = null;
        }

        public void StoreData(string[][] data)
        {
            this.Data = data;
        }

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

                this._data = new string[waypoints.Count()][];
                for (int i = 0; i < waypoints.Count(); i++)
                {
                    this._data[i] = waypoints[i].Split(';');
                }
            }

            return this.Data;
        }

        public void Serialize(string fileName)
        {
            using (StreamWriter writer = new StreamWriter($"{fileName}.csv"))
            {
                writer.Write(this.ToString());
            }
        }

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