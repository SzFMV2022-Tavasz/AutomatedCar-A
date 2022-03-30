namespace AutomatedCar.Helpers
{
    /// <summary>
    /// Interface for waypoint serializer class.
    /// </summary>
    /// <typeparam name="T">Waypoints.</typeparam>
    public interface ISerializer<T>
    {
        /// <summary>
        /// Gets waypoints.
        /// </summary>
        public T Data { get; }

        /// <summary>
        /// Remove saved wapoints.
        /// </summary>
        public void ClearData();

        /// <summary>
        /// Store waypoint data.
        /// </summary>
        /// <param name="data">Waypoints.</param>
        public void StoreData(T data);

        /// <summary>
        /// Read waypoints from file.
        /// </summary>
        /// <param name="path">Filepath.</param>
        /// <returns>Waypoints.</returns>
        public T Deserialize(string path);

        /// <summary>
        /// Write waypoints to file.
        /// </summary>
        /// <param name="path">Filepath.</param>
        public void Serialize(string path);
    }
}