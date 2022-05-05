namespace AutomatedCar.SystemComponents.Packets
{
    using System.Collections.Generic;
    using AutomatedCar.Models;
    using ReactiveUI;

    public class SensorPacket : ReactiveObject, ISensorPacket
    {
        private ICollection<WorldObject> worldObjectsInRange;
        private int xCord;
        private int yCord;
        private List<string> fileNamesRadar;
        private List<string> fileNamesCam;
        public int XCord
        {
            get => this.xCord;
            set => this.RaiseAndSetIfChanged(ref this.xCord, value);
        }

        public int YCord
        {
            get => this.yCord;
            set => this.RaiseAndSetIfChanged(ref this.yCord, value);
        }

        private int rightEdgeX;

        public int RightEdgeX
        {
            get => this.rightEdgeX;
            set => this.RaiseAndSetIfChanged(ref this.rightEdgeX, value);
        }

        private int rightEdgeY;

        public int RightEdgeY
        {
            get => this.rightEdgeY;
            set => this.RaiseAndSetIfChanged(ref this.rightEdgeY, value);
        }

        private int leftEdgeX;

        public int LeftEdgeX
        {
            get => this.leftEdgeX;
            set => this.RaiseAndSetIfChanged(ref this.leftEdgeX, value);
        }

        private int leftEdgeY;

        public int LeftEdgeY
        {
            get => this.leftEdgeY;
            set => this.RaiseAndSetIfChanged(ref this.leftEdgeY, value);
        }

        public ICollection<WorldObject> WorldObjectsInRange
        {
            get => this.worldObjectsInRange;
            set => this.worldObjectsInRange = value;
        }

        public List<string> FileNamesRadar
        {
            get => this.fileNamesRadar;
            set => this.RaiseAndSetIfChanged(ref this.fileNamesRadar, value);
        }

        public List<string> FileNamesCam
        {
            get => this.fileNamesCam;
            set => this.RaiseAndSetIfChanged(ref this.fileNamesCam, value);
        }
    }
}
