namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IControllerPacket<T>
    {
        public T Input { get; }
        public T Output { get; }
        public T Target { get; }
        public T Error { get; }
        public T LastError { get; }

        public abstract T Transfer(T input);
    }
}
