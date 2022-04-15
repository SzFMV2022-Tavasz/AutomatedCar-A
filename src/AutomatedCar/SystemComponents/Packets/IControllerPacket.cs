﻿namespace AutomatedCar.SystemComponents.Packets
{
    public interface IControllerPacket<T>
    {
        public T Input { get; }
        public T Output { get; }
        public T Target { get; }
        public T Error { get; }
        public T LastError { get; }
    }
}
