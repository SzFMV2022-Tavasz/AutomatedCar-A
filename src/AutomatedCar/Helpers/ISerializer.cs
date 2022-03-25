namespace AutomatedCar.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ISerializer<T>
    {
        public T Data { get; }

        public void ClearData();

        public void StoreData(T data);

        /*Input*/
        public T Deserialize(string path);

        /*Output*/
        public void Serialize(string fileName);
    }
}